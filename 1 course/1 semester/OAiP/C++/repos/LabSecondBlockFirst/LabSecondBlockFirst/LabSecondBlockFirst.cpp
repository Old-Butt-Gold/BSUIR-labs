#include <iostream>
#include <iomanip>
using namespace std;

int main()
{
    setlocale(LC_ALL, "Russian");
    float sum, numb;
    int i, n;
    bool isIncorrect;
    const int MIN_NUM = 1, MAX_NUM = 24;
    cout << "Программа выводит на экран сумму." << endl <<
            "Диапазон значений для ввода числа N : 1..24" << endl;
    do 
    {
        isIncorrect = false;
        cout << "Введите целое число N: ";
        cin >> n;
        if (cin.fail())
        {
            cout << "Проверьте правильность ввода" << endl;
            isIncorrect = true;
            cin.clear();
            while (cin.get() != '\n');
        }
        if (!isIncorrect && (n < MIN_NUM || n > MAX_NUM))
        {
            isIncorrect = true;
            cout << "Проверьте правильность ввода" << endl;
        }
    }
    while (isIncorrect);
    sum = 0;
    numb = 1;
    for (i = 1; i < n + 1; ++i)
    {
        numb = numb * 2;
        sum += (1 / numb);
    }
    cout << setprecision(15) << "Сумма: " << sum;
    return 0;
}




// Последовательно выберите пункты меню "Проект" > "Добавить новый элемент", чтобы создать файлы кода, или "Проект" > "Добавить существующий элемент", чтобы добавить в проект существующие файлы кода.
