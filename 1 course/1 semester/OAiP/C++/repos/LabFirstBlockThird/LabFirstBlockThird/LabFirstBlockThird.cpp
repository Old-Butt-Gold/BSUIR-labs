#include <iostream>
#include <iomanip>
#include <string>
#include <fstream>
#include <algorithm>

using namespace std;

int inputAmountOfOccurences() {
    int n;
    bool isIncorrect;
    const int MIN_NUM = 1;
    cout << "Введите число, количество вхождений строки 1 в строку 2, k = " << endl;
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
            cout << "Число > 0 " << endl;
            isIncorrect = true;
        }
    } while (isIncorrect);
    return n;
}

string chooseOption()
{
    bool isIncorrect;
    string input;
    cout << "Введите console, если хотите использовать консоль, file, если файл" << endl;
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

string chooseConsoleWayToFill()
{
    bool isIncorrect;
    string input;
    cout << "Введите console, если хотите ввести данные сами, random, если хотите сгенерировать рандомные строки" << endl;
    do
    {
        isIncorrect = false;
        cin >> input;
        transform(input.begin(), input.end(), input.begin(), ::tolower);
        if (input != "console" && input != "random")
        {
            cerr << "Повторите ввод" << endl;
            isIncorrect = true;
        }
    } while (isIncorrect);
    return input;
}

string inputStrConsole()
{
    string str;
    bool isIncorrect;
    do
    {
        isIncorrect = false;
        cin >> str;
        if (str.empty() || str[0] == ' ')
        {
            isIncorrect = true;
            cerr << "Повторите ввод" << endl;
        }
    } while (isIncorrect);
    return str;
}

int calculateAnswer(string str1, string str2, int k)
{
    int countN = 0, n = 0;
    for (int i = 0; i <= str2.length() - str1.length(); i++)
    {
        string str = str2.substr(i, str1.length());
        if (str == str1) // (int j = str2.find(str1, i) == i)
        {
            ++countN;
            if (countN == k)
            {
                n = i + 1;
            }
        }
    }
    return n;
}

string randomStrConsole()
{
    int n;
    string str = "";
    string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    string lower = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    transform(lower.begin(), lower.end(), lower.begin(), ::tolower);
    string numbers = "0123456789";
    string allCurrent = upper + lower + numbers;
    n = rand() % 256;
    for (int i = 0; i < n; i++)
    {
        str += allCurrent[rand() % 62];
    }
    return str;
}

int consoleInput(string input)
{
    bool isLengthRight;
    int k, n;
    string str1, str2;
    if (input == "console")
    {
        do {
            cerr << "Введите строку 1(первый символ не должен быть пробелом)" << endl;
            str1 = inputStrConsole();
            cout << "Введите строку 2(первый символ не должен быть пробелом)" << endl;
            str2 = inputStrConsole();
            isLengthRight = (str1.length() > str2.length()) ? true : false;
            if (isLengthRight)
            {
                cerr << "Повторите ввод строк, чтобы размер вашей первой строки был МЕНЬШЕ размера второй строки" << endl;
            }
        } while (isLengthRight);
    }
    else
    {
        do {
            str1 = randomStrConsole();
            str2 = randomStrConsole();
            isLengthRight = (str1.length() > str2.length()) ? true : false;
        } while (isLengthRight);
        cout << "Строки сгенерированы" << endl;
        cout << str1 << endl;
        cout << str2 << endl;
    }
    k = inputAmountOfOccurences();
    n = calculateAnswer(str1, str2, k);
    return n;
}

bool checkExtension(string path)
{
    bool isInCorrect = true;
    if ((path[path.length() - 1] == 't') && (path[path.length() - 2] == 'x') && (path[path.length() - 3] == 't') && (path[path.length() - 4] == '.'))
        isInCorrect = false;
    return isInCorrect;
}

bool isFileCorrect(string way)
{
    int k;
    bool isIncorrect = false;
    string str1, str2;
    ifstream fin(way);
    if (!fin.eof())
    {
        getline(fin, str1);
        if (str1.empty() || str1[0] == ' ')
        {
            isIncorrect = true;
            cerr << "Проверьте правильность первой строки" << endl;
        }
    }
    else
    {
        isIncorrect = true;
        cerr << "Нет данных для первой строки" << endl;
    }
    if (!fin.eof())
    {
        if (!isIncorrect)
        {
            getline(fin, str2);
            if (str2.empty() || str2[0] == ' ')
            {
                isIncorrect = true;
                cerr << "Проверьте правильность второй строки" << endl;
            }
        }
    }
    else
    {
        cerr << "Нет данных для второй строки" << endl;
        isIncorrect = true;
    }
    if (!fin.eof())
    {
        if (!isIncorrect)
        {
            fin >> k;
            if (fin.fail())
            {
                isIncorrect = true;
                cerr << "Число вхождений не является числом" << endl;
            }
            if (!isIncorrect && k < 1)
            {
                isIncorrect = true;
                cerr << "Число вхождений > 1" << endl;
            }
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


string takeFileWay(string temp)
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
            ifstream fin(way);
            if (!fin.is_open() && checkExtension)
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
            ifstream fout(way);
            if (!fout.is_open() && checkExtension)
            {
                cout << "Проверьте правильность и нахождение вашего файла по заданному пути" << endl;
                    isIncorrect = true;
            }
            fout.close();
        }
    } while (isIncorrect);
    return way;
}

int fileInput(string way)
{
    int n, k;
    string str1, str2;
    ifstream fin(way, ios::beg);
    getline(fin, str1);
    getline(fin, str2);
    fin >> k;
    fin.close();
    n = calculateAnswer(str1, str2, k);
    return n;
}

int takeFinalInformation(string input)
{
    string way, str1, str2;
    int n;
    if (input == "console")
    {
        input = chooseConsoleWayToFill();
        n = consoleInput(input);
    }
    else
    {
        way = takeFileWay("In");
        n = fileInput(way);
    }
    return n;
}

void outputFinalInformation(int n, string outputChoice)
{
    string way;
    if (outputChoice == "console")
    {
        cout << "Программа завершилась успешно\n" << "номер позиции k-го вхождения: " << n;
    }
    else
    {
        way = takeFileWay("Out");
        ofstream fout(way);
        fout << "Программа завершилась успешно\n" << "номер позиции k-го вхождения: " << n;
        fout.close();
    }
}

int main()
{
    setlocale(LC_ALL, "RUSSIAN");
    int n;
    string inputChoice, outputChoice;
    cout << "Данная программа находит количество вхождений k 1-ой строки во 2-ую строку" << endl;
    inputChoice = chooseOption();
    n = takeFinalInformation(inputChoice);
    outputChoice = chooseOption();
    outputFinalInformation(n, outputChoice);
}

