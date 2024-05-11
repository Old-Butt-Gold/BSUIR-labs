#include <iostream>

using namespace std;

int main()
{
    setlocale(LC_ALL, "Russian");
    cout << "Изменяя х от а с шагом h, определить, при каком значении х, SIN(x) станет больше COS(x)." << endl;
    float a, h;
    bool isIncorrect;
    do
    {
        isIncorrect = false;
        cout << "Введите a — начальное значение x: ";
        cin >> a;
        if (cin.fail())
        {
            cout << "Проверьте правильность ввода" << endl;
            isIncorrect = true;
            cin.clear();
            while (cin.get() != '\n');
        }
    } while (isIncorrect);

    do
    {
        isIncorrect = false;
        cout << "Введите шаг изменения x — h: ";
        cin >> h;
        if (cin.fail())
        {
            cout << "Проверьте правильность ввода" << endl;
            isIncorrect = true;
            cin.clear();
            while (cin.get() != '\n');
        }
    } while (isIncorrect);

    while (!(sin(a) > cos(a)))
    {
        a += h;
    }
    cout << "sin(x) больше cos(x) при x, равном: " << a << endl <<
            "sin(x) = " << sin(a) << "; cos(x) = " << cos(a);
    return 0;
}




// Последовательно выберите пункты меню "Проект" > "Добавить новый элемент", чтобы создать файлы кода, или "Проект" > "Добавить существующий элемент", чтобы добавить в проект существующие файлы кода.
