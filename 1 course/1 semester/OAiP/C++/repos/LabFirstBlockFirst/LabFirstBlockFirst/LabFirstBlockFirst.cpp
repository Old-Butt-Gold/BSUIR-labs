#include <iostream>
using namespace std;
int main()
{   
    setlocale(LC_ALL, "Russian");
    cout << "Даны координаты точки М(х,у). Определите, принадлежит ли данная точка замкнутому множеству D." << endl;
    float abscissa, ordinate;
    bool isIncorrect;
    do 
    {
        isIncorrect = false;
        cout << "Введите ось абсцисс: ";
        cin >> abscissa;
        if (cin.fail()) 
        {
            cout << "Проверьте правильность ввода" << endl;
            isIncorrect = true;
            cin.clear();
            while (cin.get() != '\n');
        }
    } 
    while (isIncorrect);

    do 
    {
        isIncorrect = false;
        cout << "Введите ось ординат: ";
        cin >> ordinate;
        if (cin.fail()) 
        {
            cout << "Проверьте правильность ввода" << endl;
            isIncorrect = true;
            cin.clear();
            while (cin.get() != '\n');
        }      
    }
    while (isIncorrect);

    if ((ordinate > -abscissa / 2 + 1) || (ordinate < 0) || (abscissa > 2) || (abscissa < 0))
        cout << "Точка M не принадлежит замкнутому множеству D.";
    else 
        cout << "Точка M принадлежит замкнутому множеству D.";
    return 0;
}




// Последовательно выберите пункты меню "Проект" > "Добавить новый элемент", чтобы создать файлы кода, или "Проект" > "Добавить существующий элемент", чтобы добавить в проект существующие файлы кода.
