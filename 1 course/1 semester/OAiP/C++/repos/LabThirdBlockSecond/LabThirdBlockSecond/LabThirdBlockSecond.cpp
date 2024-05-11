#include <iostream>
#include <fstream>
#include <string>
#include <iomanip>

using namespace std;

bool consoleChoice() 
{
    bool isIncorrect, isTrue;
    int k;
    const int MIN_NUM = 0, MAX_NUM = 1;
    do {
        isIncorrect = false;
        cin >> k;
        if (cin.fail()) 
        {
            cin.clear();
            while (cin.get() != '\n');
            cout << "Проверьте правильность ввода данных: " << endl;
            isIncorrect = true;
        }
        if (!isIncorrect && cin.get() != '\n')
        {
            cin.clear();
            while (cin.get() != '\n');
            cout << "Проверьте правильность ввода данных: " << endl;
                isIncorrect = true;
        }
        if (!isIncorrect && (k < MIN_NUM || k > MAX_NUM)) {
            cout << "Ваше число долнно быть или 1, или 0: " << endl;
            isIncorrect = true;
        }
    } while (isIncorrect);
    if (k == 0)
        isTrue = true;
    else
        isTrue = false;
    //isTrue = k == 0;
    // return isTrue == 0;?
    return isTrue;
}

int parameterIn(const int MIN_NUM) {
    bool isIncorrect;
    int n;
    do {
        isIncorrect = false;
        cin >> n;
        if (cin.fail()) 
        {
            cin.clear();
            while (cin.get() != '\n');
            cout << "Проверьте правильность ввода данных: " << endl;
            isIncorrect = true;
        }
        if (!isIncorrect && cin.get() != '\n')
        {
            cin.clear();
            while (cin.get() != '\n');
            cout << "Проверьте правильность ввода данных: " << endl;
            isIncorrect = true;
        }
        if (!isIncorrect && n < MIN_NUM) {
            cout << "Должно быть > 1" << endl;
            isIncorrect = true;
        }
    } while (isIncorrect);
    return n;
}

int takeCell()
{
    bool isIncorrect;
    int n;
    do {
        isIncorrect = false;
        cin >> n;
        if (cin.fail())
        {
            cin.clear();
            while (cin.get() != '\n');
            cout << "Проверьте правильность ввода данных: " << endl;
            isIncorrect = true;
        }
        if (!isIncorrect && cin.get() != '\n')
        {
            cin.clear();
            while (cin.get() != '\n');
            cout << "Проверьте правильность ввода данных: " << endl;
            isIncorrect = true;
        }
    } while (isIncorrect);
    return n;
}

int** fillMatrix(int** arr, int nRow, int nCol)
{
    for (int i = 0; i < nRow; i++)
    {
        for (int j = 0; j < nCol; j++)
        {
            cout << "Элемент " << (i+1) << " строки " << (j+1) << " столбца = ";
            arr[i][j] = takeCell();
        }
    }
    return arr;
}

void writeMatrix(int** arr, int nRow, int nCol)
{
    for (int i = 0; i < nRow; i++)
    {
        for (int j = 0; j < nCol; j++)
        {
            cout << setw(5) << arr[i][j];
        }
        cout << endl;
    }
}

int minRow(int** arr, int i, int nCol)
{
    int rowMin;
    rowMin = arr[i][0];
    for (int j = 1; j < nCol; j++) 
    {
        if (arr[i][j] < rowMin)
            rowMin = arr[i][j];
    }
    return rowMin;
}

int maxColumn(int** arr, int rowMin, int i, int nCol)
{
    int colMax;
    for (int j = 0; j < nCol; j++) {
        if (arr[i][j] == rowMin)
        {
            colMax = j;
        }    
    }
    return colMax;
}

bool sedlovayaTochka(int** arr, int rowMin, int colMax, int i, int nRow)
{
    bool isTrue;
    int m = 0;
    isTrue = true;
    while (m < nRow && isTrue) {
        if (rowMin < arr[m][colMax]) {
            isTrue = false;
        }
        else {
            m++;
        }
    }
    return isTrue;
}

void writeSedlovaya(bool isTrue, int colMax, int rowMin, int i)
{
    if (isTrue)
        cout << "Седловая точка в " << (i + 1) << " строке и " << (colMax + 1) << " столбце = " << rowMin;
}

bool isFileCorrect(string way)
{
    int nRow, nCol, size, i = 0;
    bool isIncorrect = false;
    ifstream fin(way);
    fin >> nRow;
    if (nRow < 2)
    {
        isIncorrect = true;
        cout << "Число строк в файле должно быть > 2" << endl;
    }
    fin >> nCol;
    if (nCol < 2)
    {
        isIncorrect = true;
        cout << "Число строк в файле должно быть > 2" << endl;
    }
    size = nRow * nCol;
    while (i < size && !isIncorrect)
    {
        fin >> i; //сделать проверку и выше тоже
        i++;
    }
    /*fin >> size;
    if (size != NULL)
    {
        isIncorrect = true;
        cout << "Уберите лишние данные" << endl;
    }*/
    fin.close();
    return isIncorrect;
}

string wayToFile(int n)
{
    string way;
    bool isIncorrect;
    do
    {
        isIncorrect = false;
        cout << "Введите путь к файлу" << endl;
        cin >> way;
        if (n == 1)
        {
            ifstream fin(way);
            if (!fin.is_open())  //Функция is_open() вернет true, если поток в данный момент открыт, и false в противном случае.
            {
                cout << "Проверьте правильность и нахождение вашего файла по заданному пути" << endl;
                isIncorrect = true;
            }
            else
            {
                isIncorrect = isFileCorrect(way);
            }
            fin.close();
        }
        else
        {
            ofstream fout(way);
            if (!fout.is_open())
            {
                cout << "Проверьте правильность и нахождение вашего файла по заданному пути" << endl;
                isIncorrect = true;
            }
            fout.close();
        }
    } while (isIncorrect);
    return way;
}

int** createMatrix(int nRow, int nCol)
{
    int** arr;
    arr = new int* [nRow];
    for (int i = 0; i < nRow; i++)
    {
        arr[i] = new int[nCol];
    }
    return arr;
}

int** takeArrFromFile(string way, int** arr, int nRow, int nCol)
{
    ifstream fin(way);
    fin.seekg(4, ios::beg);
    arr = createMatrix(nRow, nCol);
    for (int i = 0; i < nRow; i++)
    {
        for (int j = 0; j < nCol; j++)
            fin >> arr[i][j];
    }
    fin.close();
    return arr;
}

void writeMatrixF(string way, int** arr, int nRow, int nCol)
{
    ofstream fout(way); //стирает данные при перезаписи программы, если нет ios::app;
    for (int i = 0; i < nRow; i++)
    {
        for (int j = 0; j < nCol; j++)
        {
            fout << setw(5) << arr[i][j];
        }
        fout << endl;
    }
    fout.close();
}

void writeSedlovayaF(string way, bool isTrue, int colMax, int rowMin, int i)
{
    ofstream fout(way, ios::app); //открывает файл для дозаписи, старые данные не стирает
    if (isTrue)
        fout << "Седловая точка в " << (i + 1) << " строке и " << (colMax + 1) << " столбце = " << rowMin;
    fout.close();
}

int parameterInF(string way, int p)
{
    int n;
    ifstream fin(way);
    if (p == 1)
    {
        fin.seekg(0, ios::beg); //перевод курсора в начало файла
        fin >> n;
    }
    else
    {
        fin.seekg(2, ios::beg); // перевод курсора с начала файла на 2 позиции вправо (1 за число, 2 на след строку)
        fin >> n;
    }
    fin.close();
    return n;
}

int** finalMatrixIn(const int MIN_NUM, bool isInput, int &nRow, int &nCol) //& изменится значение в возвращаемом методе, и вернется в функцию, откуда было вызвано (main)
{
    int** arr = nullptr;
    string way;
    if (isInput)
    {
        cout << "Введите количество строк: ";
        nRow = parameterIn(MIN_NUM);
        cout << "Введите количество столбцов: ";
        nCol = parameterIn(MIN_NUM);
        arr = createMatrix(nRow, nCol);
        arr = fillMatrix(arr, nRow, nCol);
    }
    else
    {
        way = wayToFile(1);
        nRow = parameterInF(way, 1);
        nCol = parameterInF(way, 0);
        arr = takeArrFromFile(way, arr, nRow, nCol);
    }
    return arr;
}

void finalMatrixOutput(bool isOutput, int** arr, int nRow, int nCol)
{
    int rowMin, colMax;
    bool isTrue;
    string way;
    if (isOutput)
    {
        writeMatrix(arr, nRow, nCol);
        for (int i = 0; i < nRow; i++)
        {
            rowMin = minRow(arr, i, nCol);
            colMax = maxColumn(arr, rowMin, i, nCol);
            isTrue = sedlovayaTochka(arr, rowMin, colMax, i, nRow);
            writeSedlovaya(isTrue, colMax, rowMin, i);
        }
        if (!(isTrue))
        {
            cout << "Седловых точек нет";
        }
    }
    else
    {
        way = wayToFile(11);
        writeMatrixF(way, arr, nRow, nCol);
        for (int i = 0; i < nRow; i++)
        {
            rowMin = minRow(arr, i, nCol);
            colMax = maxColumn(arr, rowMin, i, nCol);
            isTrue = sedlovayaTochka(arr, rowMin, colMax, i, nRow);
            writeSedlovayaF(way, isTrue, colMax, rowMin, i);
        }
        ofstream fout(way, ios::app);
        if (!(isTrue))
        {
            fout << "Седловых точек нет";
        }
        fout.close();
    }
}

void clearMemory(int** arr, int nRow)
{
    for (int i = 0; i < nRow; i++)
    {
        delete[] arr[i];
    }
    delete[] arr;
}

int main()
{
    setlocale(LC_ALL, "Russian");
    const int MIN_NUM = 2;
    bool isInput, isOutput;
    int nRow = NULL, nCol = NULL;
    int** arr = nullptr; //??? 
    cout << "Данная программа находит седловую точку в матрице" << endl;
    cout << "Введите 0, если хотите ввод на консоль, 1 — ввод из файла: ";
    isInput = consoleChoice();
    arr = finalMatrixIn(MIN_NUM, isInput, nRow, nCol);
    cout << "Введите 0, если хотите ввод на консоль, 1 — ввод из файла: ";
    isOutput = consoleChoice();
    finalMatrixOutput(isOutput, arr, nRow, nCol);
    clearMemory(arr, nRow);
    return 0;
}