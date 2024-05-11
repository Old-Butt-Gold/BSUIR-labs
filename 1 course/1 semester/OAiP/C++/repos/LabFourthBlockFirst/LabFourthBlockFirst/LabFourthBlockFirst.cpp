#include <iostream>
using namespace std;
    
int main()
{
    setlocale(LC_ALL, "Russian");
    int n;
    float perimetr = 0;
    float *abscisses, *ordinates;
    const int MIN_NUM = 3;
    bool isIncorrect;
    do {
        isIncorrect = false;
        cout << "Введите число сторон многоугольника N, N > 2: " << endl;
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
    abscisses = new float[n];
    ordinates = new float[n];
    for (int i = 0; i < n; ++i)
    {
        do {
            isIncorrect = false;
            cout << "Abscissa[" << (i + 1) << "]:";
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
    for (int i = 0; i < n; ++i)
    {
        do {
            isIncorrect = false;
            cout << "Ordinate[" << (i + 1) << "]:";
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
    for (int i = 0; i < n - 1; ++i)
    {
        perimetr += sqrt((abscisses[i] - abscisses[i + 1]) * (abscisses[i] - abscisses[i + 1]) + (ordinates[i] - ordinates[i + 1]) * (ordinates[i] - ordinates[i + 1]));
    }
    perimetr += sqrt((abscisses[0] - abscisses[n - 1]) * (abscisses[0] - abscisses[n - 1]) + (ordinates[0] - ordinates[n - 1]) * (ordinates[0] - ordinates[n - 1]));
    delete[] abscisses, ordinates;
    cout << "Периметр равен: " << perimetr;
    return 0;
}


