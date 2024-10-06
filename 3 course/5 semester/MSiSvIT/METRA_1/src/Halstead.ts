export class Halstead {
    private operators: Map<string, number>;
    private operands: Map<string, number>;
    private ignore: Set<string>

    private refresh() {
        this.operators = new Map();
        this.ignore = new Set();
        let items: string[] = ['const', 'in', 'of', 'function', 'let', 'async', ' ', '\r\n'];
        items.forEach(item => this.ignore.add(item));

        this.operators.set('in', 0);
        this.operators.set('async', 0);
        this.operators.set('of', 0);
        this.operators.set('function', 0);
        this.operators.set('let', 0);
        this.operators.set('const', 0);
        this.operators.set('=', 0);
        this.operators.set('*', 0);
        this.operators.set('**', 0);
        this.operators.set('/', 0);
        this.operators.set('+', 0);
        this.operators.set('-', 0);
        this.operators.set('(', 0);
        this.operators.set(')', 0);
        this.operators.set('[', 0);
        this.operators.set(']', 0);
        this.operators.set('{', 0);
        this.operators.set('}', 0);
        this.operators.set('.', 0);
        this.operators.set('\r\n', 0);
        this.operators.set('=>', 0);
        this.operators.set(',', 0);
        this.operators.set(' ', 0);
        this.operators.set('>', 0);
        this.operators.set('>=', 0);
        this.operators.set('<', 0);
        this.operators.set('<=', 0);
        this.operators.set('%', 0);
        this.operators.set('return', 0);
        this.operators.set('let', 0);
        this.operators.set(';', 0);
        this.operators.set('+=', 0);
        this.operators.set('-=', 0);
        this.operators.set('*=', 0);
        this.operators.set('/=', 0);
        this.operators.set('?', 0);
        this.operators.set('as', 0);
        this.operators.set('!=', 0);
        this.operators.set('!==', 0);
        this.operators.set('==', 0);
        this.operators.set('===', 0);
        this.operators.set(':', 0);
        this.operators.set('for', 0);
        this.operators.set('if', 0);
        this.operators.set('else', 0);
        this.operators.set('while', 0);
        this.operators.set('do', 0);
        this.operators.set('break', 0);
        this.operators.set('continue', 0);

        this.operands = new Map();
    }

    private calculate(text: string) : void {
        let prevToken;
        let currentToken = '';

        let dot = false;

        let gravis = false;

        for (const symbol of text) {
            prevToken = currentToken;
            currentToken += symbol;

            if (this.operators.has(currentToken)) {
                continue;
            }

            if (['`', '\'', '"'].includes(prevToken[prevToken.length - 1])) {
                gravis = true;
            }

            if (['`', '\'', '"'].includes(prevToken[prevToken.length - 1]) && [' ', ';', '\r', '\n', ']', ',', ')', '}'].includes(symbol)) {
                gravis = false;
            }

            if (gravis) {
                continue;
            }

            if (this.operators.has(prevToken)) {

                if (symbol !== ' ' && ['do', 'as', 'in', 'of'].includes(prevToken)) {
                    continue;
                }

                this.operators.set(prevToken, (this.operators.get(prevToken) ?? 0) + 1);
                currentToken = symbol;

                if (prevToken !== ".") {
                    dot = false;
                }

            } else if (this.operators.has(symbol)) {
                if (dot || symbol === '(') {

                    if (dot) {
                        this.operands.set(prevToken, (this.operands.get(prevToken) ?? 0) + 1);
                    }

                    if (symbol === '(') {
                        this.operators.set(prevToken + "()", (this.operators.get(prevToken) ?? 0) + 1);
                        this.operators.set("(", (this.operators.get("(") ?? 0) - 1);
                        this.operators.set(")", (this.operators.get(")") ?? 0) - 1);
                    }

                } else {
                    this.operands.set(prevToken, (this.operands.get(prevToken) ?? 0) + 1);
                }

                currentToken = symbol;
                dot = false;
            }
        }

        if (this.operators.has(currentToken)) {
            this.operators.set(currentToken, (this.operators.get(currentToken) ?? 0) + 1);
        } else if (dot) {
            this.operators.set(currentToken, (this.operators.get(currentToken) ?? 0) + 1);
        } else {
            this.operands.set(currentToken, (this.operands.get(currentToken) ?? 0) + 1);
        }
    }

    public getString(text: string) : [Map<string, number>, Map<string, number>] {
        this.refresh();
        this.calculate(text);

        let newOperatorsMap = [...this.operators.entries()].filter(item => !this.ignore.has(item[0]));

        this.operators = new Map(newOperatorsMap);

        return [this.operators, this.operands];
    }
}