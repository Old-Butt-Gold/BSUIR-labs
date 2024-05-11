#include <iostream>  
using namespace std;

int numberIn(const int MIN_NUM)
{
    int numb;
    bool isIncorrect;
    do
    {
        isIncorrect = false;
        cout << "Введите натуральное P: ";
        cin >> numb;
        if (cin.fail())
        {
            cin.clear();
            while (cin.get() != '\n');
            isIncorrect = true;
            cout << "Ошибка, введите целочисленное число!" << endl << "Повторите ввод натуральной переменной";
        }
        if (!isIncorrect && cin.get() != '\n')
        {
            cin.clear();
            while (cin.get() != '\n');
            cout << "Ошибка, введите целочисленное число!" << endl << "Повторите ввод натуральной переменной" << endl;
            isIncorrect = true;
        }
        if (!isIncorrect && numb < MIN_NUM)
        {
            isIncorrect = true;
            cout << "Введите натуральное число, большее 1: " << endl;
        }
    } while (isIncorrect);
    return numb;
}

bool* zanulenie(bool* A, int n)
{
    for (int i = 1; i <= n; i++)
    {
        A[i] = 0;
    }
    return A;
}

bool* findProst(bool* A, int n)
{
    A = zanulenie(A, n);
    int j, i = 2;
    while (i <= sqrt(n))
    {
        j = i * i;
        while (j <= n)
        {
            A[j] = 1;
            j += i;
        }
        do
        {
            i++;
        } while (A[i]);
    }
    return A;
}

void writeProst(bool* A, int n)
{
    int i = 2;
    while (i <= n)
    {
        if (!(A[i]))
        {
            cout << i << endl;
        }
        i++;
    }
}

int main()
{
    setlocale(LC_ALL, "Russian");
    int p;
    bool* arr;
    const int MIN_NUM = 3;
    cout << "Данная программа находит все простые числа, не превосходящие P" << endl << "Введите натуральное число P: ";
    p = numberIn(MIN_NUM);
    arr = new bool[p + 1];
    arr = findProst(arr, p);
    cout << "Простые числа" << endl;
    writeProst(arr, p);
    return 0;
}