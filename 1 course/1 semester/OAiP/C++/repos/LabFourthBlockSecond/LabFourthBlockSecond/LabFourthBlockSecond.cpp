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

int** createMatrix(int n)
{
    int** arr;
    int doubleN = 2 * n;
    arr = new int* [doubleN];
    for (int i = 0; i < doubleN; i++)
    {
        arr[i] = new int[doubleN];
    }
    return arr;
}

int** fillMatrix(int** arr, int n)
{
    int doubleN = 2 * n;
    for (int i = 0; i < doubleN; i++)
    {
        for (int j = 0; j < doubleN; j++)
        {
            cout << "Элемент " << (i + 1) << " строки " << (j + 1) << " столбца = ";
            arr[i][j] = takeCell();
        }
    }
    return arr;
}

bool isFileCorrect(string way)
{
    int n, size, i = 0, elem;
    bool isIncorrect = false;
    ifstream fin(way);
    if (!fin.eof())
    {
        fin >> n;
        if (fin.fail())
        {
            isIncorrect = true;
            cout << "Проверьте правильность введенной размерности матрицы" << endl;
        }
        else if (!fin.fail() and n < 2)
        {
            isIncorrect = true;
            cout << "Размерность > 1" << endl;
        }
    }
    else
    {
        isIncorrect = true;
        cout << "Нет данных для размерности" << endl;
    }
    size = 4 * n * n;
    while (i < size && !isIncorrect)
    {
        if (!fin.eof())
        {
            fin >> elem;
            i++;
            /*if (!fin.fail())
            {
                isIncorrect = true;
                cout << "Проверьте правильность введенной матрицы" << endl;
            }*/
        }
        else
        {
            isIncorrect = true;
            cout << "Данные при считывании матрицы закончились" << endl;
        }
    }
    if (!fin.eof() && !isIncorrect)
    {
        isIncorrect = true;
        cout << "Уберите лишние данные" << endl;
    }
    fin.close();
    return isIncorrect;
}

string wayToFile(int p)
{
    string way;
    bool isIncorrect;
    do
    {
        isIncorrect = false;
        cout << "Введите путь к файлу" << endl;
        cin >> way;
        if (p == 1)
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
            fstream fout(way, ios::in);
            if (!fout.is_open())
            {
                cout << "Проверьт правильность и нахождение вашего файла по заданному пути" << endl;
                isIncorrect = true;
            }
            fout.close();
        }
    } while (isIncorrect);
    return way;
}

int parameterInF(string way)
{
    int n;
    ifstream fin(way);
    fin.seekg(ios::beg); //перевод курсора в начало файла
    fin >> n;
    fin.close();
    return n;
}

int** takeArrFromFile(string way, int** arr, int n)
{
    int doubleN = 2 * n;
    ifstream fin(way);
    fin.seekg(ios::cur); //смещение относительно текущего расположения указателя в файле
    arr = createMatrix(n);
    for (int i = 0; i < doubleN; i++)
    {
        for (int j = 0; j < doubleN; j++)
            fin >> arr[i][j];
    }
    fin.close();
    return arr;
}

void writeFirstMatrix(int** arr, int n)
{
    int doubleN = 2 * n;
    for (int i = 0; i < doubleN; i++)
    {
        for (int j = 0; j < doubleN; j++)
        {
            cout << setw(5) << arr[i][j];
        }
        cout << endl;
    }
}

int** convertMatrix(int** arr, int n)
{
    int x;
    for (int i = 0; i < n; i++) {
        for (int j = 0; j < n; j++) {
            x = arr[i][j];
            arr[i][j] = arr[i + n][j];
            arr[i + n][j] = arr[i][j + n];
            arr[i][j + n] = arr[i + n][j + n];
            arr[i + n][j + n] = x;
        }
    }
    return arr;
}

void writeNewMatrix(int** arr, int n)
{
    int doubleN = 2 * n;
    arr = convertMatrix(arr, n);
    for (int i = 0; i < doubleN; i++)
    {
        for (int j = 0; j < doubleN; j++)
        {
            cout << setw(5) << arr[i][j];
        }
        cout << endl;
    }
}

void writeMatrixF(string way, int** arr, int n)
{
    int doubleN = 2 * n;
    ofstream fout(way);
    fout << "Ваша матрица:" << endl;
    for (int i = 0; i < doubleN; i++)
    {
        for (int j = 0; j < doubleN; j++)
        {
            fout << setw(5) << arr[i][j];
        }
        fout << endl;
    }
    fout.close();
}

void writeNewMatrixF(string way, int** arr, int n)
{
    int doubleN = 2 * n;
    arr = convertMatrix(arr, n);
    ofstream fout(way, ios::app);
    fout << "Ваша преобразованная матрица:" << endl;
    for (int i = 0; i < doubleN; i++)
    {
        for (int j = 0; j < doubleN; j++)
        {
            fout << setw(5) << arr[i][j];
        }
        fout << endl;
    }
    fout.close();
}

void clearMemory(int** arr, int n)
{
    int doubleN = 2 * n;
    for (int i = 0; i < doubleN; i++)
    {
        delete[] arr[i];
    }
    delete[] arr;
}

int** finalMatrixIn(const int MIN_NUM, bool isInput, int& n) //& изменится значение в возвращаемом методе, и вернется в функцию, откуда было вызвано (main)
{
    int** arr = nullptr;
    string way;
    if (isInput)
    {
        cout << "Введите N: ";
        n = parameterIn(MIN_NUM);
        arr = createMatrix(n);
        arr = fillMatrix(arr, n);
    }
    else
    {
        way = wayToFile(1);
        n = parameterInF(way);
        arr = takeArrFromFile(way, arr, n);
    }
    return arr;
}

void finalMatrixOut(bool isOutput, int** arr, int n)
{
    string way;
    if (isOutput)
    {
        cout << "Ваша матрица:" << endl;
        writeFirstMatrix(arr, n);
        cout << "Преобразованная матрица:" << endl;
        writeNewMatrix(arr, n);
    }
    else
    {
        way = wayToFile(0);
        writeMatrixF(way, arr, n);
        writeNewMatrixF(way, arr, n);
    }
}

int main()
{
    setlocale(LC_ALL, "Russian");
    const int MIN_NUM = 2;
    bool isInput, isOutput;
    int n;
    int** arr = nullptr;
    string way;
    cout << "Данная программа меняет подматрицы N вашей матрицы 2N местами" << endl;
    cout << "Введите 0, если хотите ввод на консоль, 1 — ввод из файла: ";
    isInput = consoleChoice();
    arr = finalMatrixIn(MIN_NUM, isInput, n);
    cout << "Введите 0, если хотите ввод на консоль, 1 — ввод из файла: ";
    isOutput = consoleChoice();
    finalMatrixOut(isOutput, arr, n);
    clearMemory(arr, n);
    return 0;
}