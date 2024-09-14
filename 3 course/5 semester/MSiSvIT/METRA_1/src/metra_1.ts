const gridSize = 9;

document.addEventListener('DOMContentLoaded', () => {
    const solveButton = document.getElementById('solve-btn');
    solveButton.addEventListener('click', solveSudoku);

    const clearButton = document.getElementById('clear-btn');
    clearButton.addEventListener('click', clearGrid);

    const sudokuGrid = document.getElementById('sudoku-grid');

    for (let row = 0; row < gridSize; row += 1) {
        const newRow = document.createElement('tr');
        for (let col = 0; col < gridSize; col += 1) {
            const cell = document.createElement('td');
            const input = document.createElement('input');
            input.type = 'number';
            input.className = 'cell';
            input.id = `cell-${row}-${col}`;
            cell.appendChild(input);
            newRow.appendChild(cell);
        }
        sudokuGrid?.appendChild(newRow);
    }
});

async function solveSudoku() {
    const clearButton = document.getElementById('clear-btn') as HTMLButtonElement;
    clearButton.disabled = true;
    const sudokuArray : number[][] = [];

    for (let row = 0; row < gridSize; row += 1) {
        sudokuArray[row] = [];
        for (let col = 0; col < gridSize; col += 1) {
            const cellId = `cell-${row}-${col}`;
            const cell = document.getElementById(cellId) as HTMLInputElement;
            const cellValue = cell.value;
            sudokuArray[row][col] = cellValue !== '' ? parseInt(cellValue) : 0;

            if (sudokuArray[row][col] !== 0) {
                cell.classList.add('user-input');
            }
        }
    }

    if (helper(sudokuArray, 0, 0)) {
        alert('Solution exists for the given Sudoku.');
    } else {
        alert('No solution exists for the given Sudoku.');
    }
    clearButton.disabled = false;
}

function helper(board : number[][], row : number, col : number) : boolean {
    if (col === 9) {
        col = 0;
        row += 1;
        if (row === 9) {
            return true;
        }
    }

    if (board[row][col] !== 0) {
        return helper(board, row, col + 1);
    }

    for (let c = 1; c <= 9; c += 1) {
        if (isValid(board, row, col, c)) {
            board[row][col] = c;

            if (helper(board, row, col + 1)) {
                return true;
            }

            board[row][col] = 0;
        }
    }

    return false;

    function isValid(board : number[][], row : number, col : number, c : number) : boolean {

        for (let i = 0; i < 9; i += 1) {
            if (board[row][i] === c) {
                return false;
            }
            if (board[i][col] === c) {
                return false;
            }
        }

        let startRow = Math.floor(row / 3) * 3;
        let startCol = Math.floor(col / 3) * 3;
        for (let i = startRow; i < startRow + 3; i += 1) {
            for (let j = startCol; j < startCol + 3; j += 1) {
                if (board[i][j] === c) {
                    return false;
                }
            }
        }

        return true;
    }
}

function clearGrid() {
    for (let row = 0; row < gridSize; row += 1) {
        for (let col = 0; col < gridSize; col += 1) {
            const cellId = `cell-${row}-${col}`;
            const cell = document.getElementById(cellId) as HTMLInputElement;
            cell.value = '';
            cell.classList.remove('user-input', 'solved');
        }
    }
}
