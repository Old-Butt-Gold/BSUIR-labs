#include <iostream>
#include <set>
#include <string>
#include <fstream>
#include <algorithm>

using namespace std;

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

string inputStrConsole()
{
    string str;
    bool isIncorrect;
    do
    {
        isIncorrect = false;
        getline(cin, str);
        if (str.empty() || str[0] == ' ')
        {
            isIncorrect = true;
            cerr << "Повторите ввод" << endl;
        }
    } while (isIncorrect);
    return str;
}

set<char> findAnswer(string str)
{
    set<char> finalSet, tempSet, mySet{ '0','1','2','3','4','5','6','7','8','9','+','*',':','-', '<', '>', '=', '(', ')' };
    for (int i = 0; i < str.length(); i++)
        if (mySet.find(str[i]) != mySet.end())
            finalSet.insert(str[i]);
    /*tempSet.insert(str[i]);
set_intersection(mySet.begin(), mySet.end(), tempSet.begin(), tempSet.end(), inserter(finalSet, finalSet.begin()));*/
    return finalSet;
}

bool checkExtension(string path)
{
    return (path[path.length() - 1] == 't') && (path[path.length() - 2] == 'x') && (path[path.length() - 3] == 't') && (path[path.length() - 4] == '.');
}

string takeFileWay()
{
    string way;
    bool isIncorrect;
    do
    {
        isIncorrect = false;
        cout << "Введите путь к файлу" << endl;
        cin >> way;
        fstream fin(way);
        if (!checkExtension || !fin.is_open())
        {
            cout << "Проверьте правильность или нахождение вашего файла по заданному пути" << endl;
            isIncorrect = true;
        }
        fin.close();
    } while (isIncorrect);
    return way;
}

string takeStrFromFile(string way)
{
    string str;
    bool isIncorrect;
    ifstream fin(way);
    do {
        isIncorrect = false;
        if (!fin.eof())
        {
            getline(fin, str);
        }
        else
        {
            cerr << "В файле нет данных для строки" << endl;
            isIncorrect = true;
            way = takeFileWay();
        }
        if (!isIncorrect && !fin.eof())
        {
            cerr << "Уберите лишние данные" << endl;
            isIncorrect = true;
            way = takeFileWay();
        }
    } while (isIncorrect);
    fin.close();
    return str;
}

set<char> takeFinalInformation(string input)
{
    set<char> finalSet;
    string str, way;
    if (input == "console")
    {
        str = inputStrConsole();
    }
    else
    {
        way = takeFileWay();
        str = takeStrFromFile(way);
    }
    finalSet = findAnswer(str);
    return finalSet;
}

void outputAnswerInFile(string way, set<char> finalSet)
{
    bool isIncorrect;
    ofstream fout(way);
    do
    {
        isIncorrect = false;
        if (finalSet.empty())
            fout << "Нет нужных символов";
        else
            for (auto st : finalSet)
                fout << st << '\t';
    } while (isIncorrect);
    fout.close();
}

void outputFinalInformation(set<char> finalSet, string outputChoice)
{
    string way;
    if (outputChoice == "console")
    {
        if (finalSet.empty())
            cout << "Нет нужных символов";
        else
            for (auto i = finalSet.begin(); i != finalSet.end(); i++)
                cout << *i << '\t'; //auto st : finalSet cout << st << '\t'
    }
    else
    {
        way = takeFileWay();
        outputAnswerInFile(way, finalSet);
    }
}

int main()
{
    setlocale(LC_ALL, "RUSSIAN");
    set<char> finalSet;
    string inputChoice, outputChoice;
    cout << "Данная программа выводит множество арифметических знаков и цифр" << endl;
    inputChoice = chooseOption();
    finalSet = takeFinalInformation(inputChoice);
    outputChoice = chooseOption();
    outputFinalInformation(finalSet, outputChoice);
}

