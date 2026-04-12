export function evalExpr(expr, vars, funcs) {
    expr = expr.trim();

    // Strings
    if ((expr.startsWith('"') && expr.endsWith('"')) || (expr.startsWith("'") && expr.endsWith("'"))) {
        return expr.slice(1, -1);
    }

    // List literal [1,2,3]
    if (expr.startsWith('[') && expr.endsWith(']')) {
        let inner = expr.slice(1, -1);
        if (inner.trim() === '') return [];
        return inner.split(',').map(x => evalExpr(x, vars, funcs));
    }

    // Number
    if (!isNaN(expr)) return Number(expr);

    // Boolean literals
    if (expr === 'TRUE') return true;
    if (expr === 'FALSE') return false;

    // Indexing: arr[i]
    let idxMatch = expr.match(/^(\w+)\[(.+)\]$/);
    if (idxMatch) {
        let arr = vars[idxMatch[1]];
        let index = evalExpr(idxMatch[2], vars, funcs);
        return arr[index];
    }

    // Function call: name(a,b)
    let call = expr.match(/^(\w+)\((.*)\)$/);
    if (call && funcs[call[1]]) {
        let args = call[2].split(',').map(a => a.trim()).filter(a => a.length > 0);
        let values = args.map(a => evalExpr(a, vars, funcs));
        return runFunction(call[1], values, funcs);
    }

    // Variable
    if (vars.hasOwnProperty(expr)) return vars[expr];

    // Boolean operators
    let bool = expr.match(/^(.*)\s+(AND|OR)\s+(.*)$/);
    if (bool) {
        let left = evalExpr(bool[1], vars, funcs);
        let right = evalExpr(bool[3], vars, funcs);
        return bool[2] === 'AND' ? (left && right) : (left || right);
    }

    if (expr.startsWith('NOT ')) {
        return !evalExpr(expr.substring(4), vars, funcs);
    }

    // Comparison
    let cmp = expr.match(/^(.*?)(==|!=|<=|>=|<|>)(.*)$/);
    if (cmp) {
        let left = evalExpr(cmp[1], vars, funcs);
        let right = evalExpr(cmp[3], vars, funcs);
        switch (cmp[2]) {
            case '==': return left == right;
            case '!=': return left != right;
            case '<': return left < right;
            case '>': return left > right;
            case '<=': return left <= right;
            case '>=': return left >= right;
        }
    }

    // Math
    let math = expr.match(/^(.*?)([+\-*/])(.*)$/);
    if (math) {
        let left = evalExpr(math[1], vars, funcs);
        let right = evalExpr(math[3], vars, funcs);
        switch (math[2]) {
            case '+': return left + right;
            case '-': return left - right;
            case '*': return left * right;
            case '/': return left / right;
        }
    }

    throw new Error('Invalid expression: ' + expr);
}
