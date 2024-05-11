#include <iostream>
using namespace std;

int main()
{
    setlocale(LC_ALL, "Russian");
    int n, k;
    float crossLine1, crossLine2;
    float *abscisses, *ordinates;
    bool isIncorrect, isTrue;
    const int MIN_NUM = 3;
    do
    {
        isIncorrect = false;
        cout << "Введите число сторон многоугольника N, N > 2: ";
        cin >> n;
        if (cin.fail())
        {
            cin.clear();
            while (cin.get() != '\n');
            isIncorrect = true;
            cout << "Проверьте правильность ввода" << endl;
        }
        if (!isIncorrect && cin.get() != '\n')
        {
            cin.clear();
            while (cin.get() != '\n');
            cout << "Проверьте правильность ввода" << endl;
            isIncorrect = true;
        }
        if (!isIncorrect && n < MIN_NUM)
        {
            isIncorrect = true;
            cout << "Проверьте правильность ввода" << endl;
        }
    } while (isIncorrect);
    abscisses = new float[n+3];
    ordinates = new float[n+3];
    for (int i = 1; i <= n; ++i)
    {
        do {
            isIncorrect = false;
            cout << "Abscissa[" << (i) << "]: ";
            cin >> abscisses[i];
            if (cin.fail())
            {
                cin.clear();
                while (cin.get() != '\n');
                isIncorrect = true;
                cout << "Проверьте правильность ввода" << endl;
            }
            if (!isIncorrect && cin.get() != '\n')
            {
                cin.clear();
                while (cin.get() != '\n');
                cout << "Проверьте правильность ввода" << endl;
                isIncorrect = true;
            }
        } while (isIncorrect);
    }
    for (int i = 1; i <= n; ++i)
    {
        do {
            isIncorrect = false;
            cout << "Ordinate[" << (i) << "]: ";
            cin >> ordinates[i];
            if (cin.fail())
            {
                cin.clear();
                while (cin.get() != '\n');
                isIncorrect = true;
                cout << "Проверьте правильность ввода" << endl;
            }
            if (!isIncorrect && cin.get() != '\n')
            {
                cin.clear();
                while (cin.get() != '\n');
                cout << "Проверьте правильность ввода" << endl;
                isIncorrect = true;
            }
        } while (isIncorrect);
    }
    abscisses[0] = abscisses[n];
    ordinates[0] = abscisses[n];
    abscisses[n + 1] = abscisses[1];
    abscisses[n + 2] = abscisses[2];
    ordinates[n + 1] = ordinates[1];
    ordinates[n + 2] = ordinates[2];
    isTrue = true;
    k = 1;
    while (k <= n && isTrue)
    {
        crossLine1 = (abscisses[k - 1] - abscisses[k]) * (ordinates[k + 1] - ordinates[k]) - (ordinates[k - 1] - ordinates[k]) * (abscisses[k + 1] - abscisses[k]);
        crossLine2 = (abscisses[k] - abscisses[k + 1]) * (ordinates[k + 2] - ordinates[k + 1]) - (ordinates[k] - ordinates[k + 1]) * (abscisses[k + 2] - abscisses[k + 1]);
        if (crossLine1 * crossLine2 < 0)
            isTrue = false;
        else
            k++;
    }
    delete[] abscisses, ordinates;
    if (isTrue)
        cout << "Выпуклый";
    else
        cout << "Не выпуклый";
    return 0;
}
