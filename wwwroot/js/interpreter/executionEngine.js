import { evalExpr } from './expressionEvaluator.js';
function runFunction(name, args, funcs) {
    let func = funcs[name];
    let localVars = {};

    func.params.forEach((p, i) => {
        localVars[p] = args[i];
    });

    let result = execBlock(func.body, localVars, funcs);
    return result;
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
            return evalExpr(t.substring(6).trim(), vars, funcs);
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

                if (condLine.startsWith('IF')) cond = evalExpr(condLine.replace('IF','').replace('THEN',''), vars, funcs);
                else if (condLine.startsWith('ELSEIF')) cond = evalExpr(condLine.replace('ELSEIF','').replace('THEN',''), vars, funcs);
                else if (condLine === 'ELSE') cond = true;

                if (cond) {
                    let start = blocks[b].start;
                    let end = (b+1 < blocks.length) ? blocks[b+1].start - 1 : j;
                    let res = execBlock(lines.slice(start, end), vars, funcs);
                    if (res !== undefined) return res;
                    break;
                }
            }
            i = j;
        }

        else if (t.startsWith('WHILE')) {
            let cond = t.replace('WHILE','');
            let block = [];
            let j = i+1;
            while (lines[j].trim() !== 'ENDWHILE') block.push(lines[j++]);
            while (evalExpr(cond, vars, funcs)) {
                let res = execBlock(block, vars, funcs);
                if (res !== undefined) return res;
            }
            i = j;
        }

        else if (t.startsWith('FOR')) {
            let [left,right] = t.replace('FOR','').split('TO');
            let [name,start] = left.split('=');
            let end = evalExpr(right, vars, funcs);
            let block = [];
            let j = i+1;
            while (lines[j].trim() !== 'ENDFOR') block.push(lines[j++]);

            for (vars[name.trim()] = evalExpr(start, vars, funcs);
                 vars[name.trim()] <= end;
                 vars[name.trim()]++) {
                let res = execBlock(block, vars, funcs);
                if (res !== undefined) return res;
            }
            i = j;
        }

        // FUNCTION DEF
        else if (t.startsWith('FUNCTION')) {
            let name = t.match(/FUNCTION\s+(\w+)/)[1];
            let params = t.match(/\((.*)\)/)[1].split(',').map(x=>x.trim()).filter(x=>x);
            let body = [];
            let j = i+1;
            while (lines[j].trim() !== 'ENDFUNCTION') body.push(lines[j++]);
            funcs[name] = { params, body };
            i = j;
        }
    }

    return output;
}
