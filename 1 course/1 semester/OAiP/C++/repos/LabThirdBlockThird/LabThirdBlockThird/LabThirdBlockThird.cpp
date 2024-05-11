#include <iostream>
#include <iomanip>
#include <string>
#include <fstream>
#include <algorithm>

using namespace std;

int inputSizeOfArray() {
    int n;
    bool isIncorrect;
    const int MIN_NUM = 2;
    cout << "Введите размерность массива" << endl;
    do {
        isIncorrect = false;
        cin >> n;
        if (cin.fail())
        {
            cin.clear();
            while (cin.get() != '\n');
            cerr << "Проверьте правильность ввода данных: " << endl;
            isIncorrect = true;
        }
        if (!isIncorrect && cin.get() != '\n')
        {
            cin.clear();
            while (cin.get() != '\n');
            cerr << "Проверьте правильность ввода данных: " << endl;
            isIncorrect = true;
        }
        if (!isIncorrect && n < MIN_NUM) {
            cerr << "Число > 1" << endl;
            isIncorrect = true;
        }
    } while (isIncorrect);
    return n;
}

string chooseOptionToInput()
{
    bool isIncorrect;
    string input;
    cout << "Введите console, если хотите использовать консоль, file, если файл, random, чтобы сгенерировать случайные значения" << endl;
    do
    {
        isIncorrect = false;
        cin >> input;
        transform(input.begin(), input.end(), input.begin(), ::tolower);
        if (input != "console" && input != "file" && input != "random")
        {
            cerr << "Повторите ввод" << endl;
            isIncorrect = true;
        }
    } while (isIncorrect);
    return input;
}

string chooseOptionToOutput()
{
    bool isIncorrect;
    string input;
    cout << "Введите console, если хотите вывести данные в консоль, file, если вывести в файл" << endl;
    do
    {
        isIncorrect = false;
        cin >> input;
        transform(input.begin(), input.end(), input.begin(), ::tolower);
        if (input != "console" && input != "file")
        {
            cerr << "Повторите ввод" << endl;
            isIncorrect = true;
        }
    } while (isIncorrect);
    return input;
}

int takeCellIntoArray()
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
            cerr << "Проверьте правильность ввода данных: " << endl;
            isIncorrect = true;
        }
        if (!isIncorrect && cin.get() != '\n')
        {
            cin.clear();
            while (cin.get() != '\n');
            cerr << "Проверьте правильность ввода данных: " << endl;
            isIncorrect = true;
        }
    } while (isIncorrect);
    return n;
}

int* fillArray(int& n)
{
    n = inputSizeOfArray();
    int* arr = new int[n];
    for (int i = 0; i < n; i++)
    {
        cout << "Введите элемент " << (i + 1) << endl;
        arr[i] = takeCellIntoArray();
    }
    return arr;
}


int takeRandomSizeOfArray()
{
    int n;
    n = rand() % 200 + 2;
    return n;
}

int* takeRandomElementsInArray(int& n)
{
    n = takeRandomSizeOfArray();
    int* arr = new int[n];
    for (int i = 0; i < n; i++)
    {
        arr[i] = rand() % 1000 - rand() % 1000;
    }
    return arr;
}

int* shellsSort(int* arr, int n)
{
    int gap = n / 2;
    int temp, j;
    while (gap > 0) {
        for (int i = 0; i < n; i++)
        {
            cout << setw(5) << arr[i];
        }
        cout << '\n';
        for (int i = 0; i < n - gap; i++) {
            j = i;
            temp = arr[j + gap];
            while (j >= 0 && arr[j] > temp) {
                arr[j + gap] = arr[j];
                arr[j] = temp;
                j -= gap;
            }
        }
        for (int i = 0; i < n; i++)
        {
            cout << setw(5) << arr[i];
        }
        cout << '\n';
        cout << "————————————————————————————————————————————————————————————————————————————————————" << endl;
        gap /= 2;
    }
    return arr;
}

bool checkExtension(string path)
{
    return (path[path.length() - 1] == 't') && (path[path.length() - 2] == 'x') && (path[path.length() - 3] == 't') && (path[path.length() - 4] == '.');
}

bool isFileCorrect(string way, int& n, int*& arr)
{
    const int MIN_NUM = 2;
    bool isIncorrect = false;
    ifstream fin(way, ios::beg);
    if (!fin.eof())
    {
        fin >> n;
        if (fin.fail())
        {
            isIncorrect = true;
            cerr << "Проверьте правильность ввода размерности массива" << endl;
        }
        if (!isIncorrect && n < MIN_NUM)
        {
            isIncorrect = true;
            cerr << "Число > 1" << endl;
        }
    }
    else
    {
        isIncorrect = true;
        cerr << "Нет данных для размерности" << endl;
    }
    arr = new int[n];
    for (int i = 0; i < n && !isIncorrect && !fin.eof(); i++)
    {
        fin >> arr[i];
        if (fin.fail())
        {
            isIncorrect = true;
            cerr << "Проверьте ваши данные на " << (i + 1) << " элементе массива" << endl;
        }
    }
    if (!fin.eof() && !isIncorrect)
    {
        isIncorrect = true;
        cerr << "Уберите лишние данные" << endl;
    }
    fin.close();
    return isIncorrect;
}


string takeFileWay(string temp, int& n, int*& arr)
{
    string way;
    bool isIncorrect;
    do
    {
        isIncorrect = false;
        cout << "Введите путь к файлу" << endl;
        cin >> way;
        if (temp == "In")
        {
            fstream fin(way);
            if (fin.is_open() && checkExtension)
            {
                isIncorrect = isFileCorrect(way, n, arr);
            }
            else
            {
                cerr << "Проверьте параметры и нахождение вашего файла по заданному пути" << endl;
                isIncorrect = true;
            }
            fin.close();
        }
        else
        {
            fstream fout(way);
            if (!fout.is_open() || !checkExtension)
            {
                cerr << "Проверьте параметры и нахождение вашего файла по заданному пути" << endl;
                isIncorrect = true;
            }
            fout.close();
        }
    } while (isIncorrect);
    return way;
}

int* takeFinalInformation(string inputChoice, int& n)
{
    string way;
    int* arr;
    if (inputChoice == "console")
    {
        arr = fillArray(n);
    }
    else if (inputChoice == "random")
    {
        arr = takeRandomElementsInArray(n);
    }
    else
    {
        way = takeFileWay("In", n, arr);
    }
    return arr;
}

void outputArray(int* arr, int n) {
    cout << "Первоначальный массив: " << endl;
    for (int i = 0; i < n; i++)
    {
        cout << arr[i] << '\t';
    }
    cout << '\n';
    arr = shellsSort(arr, n);
    cout << "Новый массив:" << endl;
    for (int i = 0; i < n; i++)
    {
        cout << arr[i] << '\t';
    }
}

void outputArrayInFile(int* arr, string way, int n)
{
    ofstream fout(way);
    fout << "Первоначальный массив: " << endl;
    for (int i = 0; i < n; i++)
    {
        fout << arr[i] << '\t';
    }
    fout << '\n';
    arr = shellsSort(arr, n);
    fout << "Новый массив: " << endl;
    for (int i = 0; i < n; i++)
    {
        fout << arr[i] << '\t';
    }
    fout.close();
}

void outputFinalInformation(int* arr, string outputChoice, int n) {
    string way;
    if (outputChoice == "console")
    {
        outputArray(arr, n);
    }
    else
    {
        way = takeFileWay("Out", n, arr);
        outputArrayInFile(arr, way, n);
    }
}

int main()
{
    setlocale(LC_ALL, "RUSSIAN");
    int* arr;
    int n;
    string inputChoice, outputChoice;
    cout << "Данная программа сортирует массив методом Шелла" << endl;
    inputChoice = chooseOptionToInput();
    arr = takeFinalInformation(inputChoice, n);
    outputChoice = chooseOptionToOutput();
    outputFinalInformation(arr, outputChoice, n);
    delete[] arr;
}

