import './style.css';
import { Halstead } from "./Halstead.ts";

document.querySelector<HTMLDivElement>('#app')!.innerHTML = `
    <div class="container mt-5">
        <div class="row justify-content-center">
            <div class="col-md-6 text-center">
                <input class="form-control-lg" type="file" accept=".ts" id="formFile">
                <button type="button" class="btn-convert btn btn-outline-primary btn-lg">Получить Результат</button>
            </div>
        </div>
        <div class="div-result mt-5"></div>
    </div>
`;

document.querySelector<HTMLButtonElement>('.btn-convert')!.addEventListener('click', () => {
    const fileInput = document.querySelector<HTMLInputElement>('#formFile')!;
    const file = fileInput.files?.[0];

    if (file) {
        const reader = new FileReader();
        reader.readAsText(file);

        reader.onload = function () {
            let halstead = new Halstead();
            let text = reader.result as string;
            const fileContent = halstead.getString(text);
            let operatorsMap = fileContent[0];
            let operandsMap = fileContent[1];

            operatorsMap.set('{}', operatorsMap.get('{') + operatorsMap.get('}'));
            operatorsMap.set('[]', operatorsMap.get('[') + operatorsMap.get(']'));
            operatorsMap.set('()', operatorsMap.get('(') + operatorsMap.get(')'));

            operatorsMap.set('{', 0);
            operatorsMap.set('}', 0);
            operatorsMap.set('[', 0);
            operatorsMap.set(']', 0);
            operatorsMap.set('(', 0);
            operatorsMap.set(')', 0);

            let elseCounter = operatorsMap.get('else');
            if (elseCounter && elseCounter > 0) {
                operatorsMap.set('if ... else', elseCounter);
                operatorsMap.set('if', operatorsMap.get('if') - elseCounter);
                operatorsMap.set('else', 0);
            }

            let operators = [...operatorsMap.entries()].filter(item => item[1] > 0).sort((a, b) => a[1] - b[1]);
            let operands = [...operandsMap.entries()].filter(item => item[1] > 0).sort((a, b) => a[1] - b[1]);

            let sb = '<div class="table-responsive"><table class="table table-bordered table-hover"><thead class="thead-dark">\n';
            sb += '<tr><th scope="col">Индекс</th><th scope="col">Оператор</th><th scope="col">Количество</th></tr>\n';
            sb += '</thead><tbody>\n';
            let allOperators = 0;
            let counter = 1;

            for (let [key, value] of operators) {
                if (key === '\r\n') key = '\\r\\n';
                if (key === ' ') key = 'пробел';

                sb += `<tr><td>${counter++}</td><td>${key}</td><td>${value}</td></tr>\n`;
                allOperators += value;
            }
            sb += `<tr class="table-info"><th scope="row">Всего</th><th>Уникальных операторов: ${operators.length}</th><th>Всего операторов: ${allOperators}</th></tr>\n`;
            sb += "</tbody></table></div>\n";

            let allOperands = 0;
            counter = 1;

            sb += '<div class="table-responsive"><table class="table table-bordered table-hover"><thead class="thead-dark">\n';
            sb += '<tr><th scope="col">Индекс</th><th scope="col">Операнд</th><th scope="col">Количество</th></tr>\n';
            sb += '</thead><tbody>\n';

            for (let [key, value] of operands) {
                sb += `<tr><td>${counter++}</td><td>${key}</td><td>${value}</td></tr>\n`;
                allOperands += value;
            }
            sb += `<tr class="table-info"><th scope="row">Всего</th><th>Уникальных операндов: ${operands.length}</th><th>Всего операндов: ${allOperands}</th></tr>\n`;
            sb += "</tbody></table></div>\n";

            sb += `<div class="mt-4"><h5>Метрики Холстеда:</h5>`;
            sb += `<div class="alert alert-success" role="alert">Словарь программы (n): ${operands.length + operators.length}</div>`;
            sb += `<div class="alert alert-success" role="alert">Длина программы (N): ${allOperands + allOperators}</div>`;
            sb += `<div class="alert alert-success" role="alert">Объем программы (V): ${((allOperators + allOperands) * Math.log2(operators.length + operands.length)).toFixed(2)}</div>
            </div>`;

            document.querySelector<HTMLDivElement>('.div-result')!.innerHTML = sb;

        };

    } else {
        alert('Please select a file.');
    }
});
