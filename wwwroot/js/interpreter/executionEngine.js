import { evalExpr } from './expressionEvaluator.js';

function runFunction(name, args, funcs) {
    let func = funcs[name];
    let localVars = {};

    func.params.forEach((p, i) => {
        localVars[p] = args[i];
    });

    let result = execBlock(func.body, localVars, funcs);

    // unwrap return if present
    if (result && result.__return !== undefined) {
        return result.__return;
    }

    return result.output;
}


// INTERPRETER 

export function execBlock(lines, vars, funcs) {
    let output = [];

    for (let i = 0; i < lines.length; i++) {
        let t = lines[i].trim();
        if (!t || t.startsWith('//')) continue;

        if (t.startsWith('SET')) {
            let [n, v] = t.substring(3).split('=');
            vars[n.trim()] = evalExpr(v.trim(), vars, funcs);
        }

        else if (t.startsWith('PRINT')) {
            output.push(evalExpr(t.substring(5).trim(), vars, funcs));
        }

        else if (t.startsWith('RETURN')) {
            return { __return: evalExpr(t.substring(6).trim(), vars, funcs) };
        }

        // IF / ELSEIF / ELSE
        else if (t.startsWith('IF')) {
            let blocks = [];
            let j = i;

            while (j < lines.length) {
                let line = lines[j].trim();

                if (line.startsWith('IF') || line.startsWith('ELSEIF') || line === 'ELSE') {
                    blocks.push({ cond: line, start: j + 1 });
                }

                if (line === 'ENDIF') break;
                j++;
            }

            for (let b = 0; b < blocks.length; b++) {
                let condLine = blocks[b].cond;
                let cond = true;

                if (condLine.startsWith('IF'))
                    cond = evalExpr(condLine.replace('IF','').replace('THEN','').trim(), vars, funcs);
                else if (condLine.startsWith('ELSEIF'))
                    cond = evalExpr(condLine.replace('ELSEIF','').replace('THEN','').trim(), vars, funcs);
                else if (condLine === 'ELSE')
                    cond = true;

                if (cond) {
                    let start = blocks[b].start;
                    let end = (b+1 < blocks.length) ? blocks[b+1].start - 1 : j;

                    let res = execBlock(lines.slice(start, end), vars, funcs);

                    if (res && res.__return !== undefined) return res;

                    // merge output
                    if (res && res.output) output.push(...res.output);

                    break;
                }
            }

            i = j;
        }

        else if (t.startsWith('WHILE')) {
            let cond = t.replace('WHILE','').replace('DO','').trim();

            let block = [];
            let j = i + 1;

            // support nesting
            let depth = 1;
            while (j < lines.length && depth > 0) {
                let line = lines[j].trim();

                if (line.startsWith('WHILE')) depth++;
                if (line === 'ENDWHILE') depth--;

                if (depth > 0) block.push(lines[j]);
                j++;
            }

            while (evalExpr(cond, vars, funcs)) {
                let res = execBlock(block, vars, funcs);

                if (res && res.__return !== undefined) return res;

                if (res && res.output) output.push(...res.output);
            }

            i = j - 1;
        }

        else if (t.startsWith('FOR')) {
            let match = t.match(/FOR\s+(\w+)\s*=\s*(.*?)\s+TO\s+(.*)/);
            if (!match) throw new Error("Invalid FOR syntax");

            let name = match[1];
            let start = match[2];
            let end = evalExpr(match[3], vars, funcs);

            let block = [];
            let j = i + 1;

            // support nesting
            let depth = 1;
            while (j < lines.length && depth > 0) {
                let line = lines[j].trim();

                if (line.startsWith('FOR')) depth++;
                if (line === 'ENDFOR') depth--;

                if (depth > 0) block.push(lines[j]);
                j++;
            }

            for (
                vars[name] = evalExpr(start, vars, funcs);
                vars[name] <= end;
                vars[name]++
            ) {
                let res = execBlock(block, vars, funcs);

                if (res && res.__return !== undefined) return res;

                if (res && res.output) output.push(...res.output);
            }

            i = j - 1;
        }

        // FUNCTION DEF
        else if (t.startsWith('FUNCTION')) {
            let name = t.match(/FUNCTION\s+(\w+)/)[1];
            let params = t.match(/\((.*)\)/)[1]
                .split(',')
                .map(x => x.trim())
                .filter(x => x);

            let body = [];
            let j = i + 1;

            while (lines[j].trim() !== 'ENDFUNCTION') {
                body.push(lines[j++]);
            }

            funcs[name] = { params, body };
            i = j;
        }
    }

    return { output };
}
