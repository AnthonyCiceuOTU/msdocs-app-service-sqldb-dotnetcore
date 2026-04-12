import { execBlock } from './executionEngine.js';

export function runCode() {
    const lines = document.getElementById('code').value.split('\n');

    const vars = {};
    const funcs = {};

    try {
        const result = execBlock(lines, vars, funcs);
        document.getElementById('output').textContent =
            Array.isArray(result) ? result.join('\n') : result;
    } catch (e) {
        document.getElementById('output').textContent = 'Error: ' + e.message;
    }
}