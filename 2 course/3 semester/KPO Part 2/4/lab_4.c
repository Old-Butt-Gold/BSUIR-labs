#include "stdio.h"
#include "stdlib.h"
#include "time.h"
#include "windows.h"

void Task13()
{
    char str[255];
    printf("Задание 13. Подсчитать количество букв в строке\n");

    printf("Введите строку: ");
    gets(str);

    for (int i = 0; str[i]; i++) {
        if (str[i] >= 'a' && str[i] <= 'z') {
            str[i] -= 32;
        }
    }

    for (int i = 0; str[i]; i++)
        str[i] = str[i] == ' ' ? '_' : str[i];
    //puts(str);
    printf("Вы ввели: %s\n", str);

    int letters[26] = {};
    for (int i = 0; str[i]; i++)
        if (str[i] != '_')
            letters[str[i] % 'A']++;

    for (int i = 0; i < 26; i++)
        if (letters[i] != 0)
        {
            putchar((char) i + 'A');
            printf("%d", letters[i]);
        }
}

void Task1() {
    char str[255];
    printf("\nЗадание 1. С клавиатуры вводится строка. Выбрать все буквы от Q до Z. Строчные преобразовать в прописные. Отсортировать в алфавитном порядке\n");

    printf("Введите строку: ");
    gets(str);

    for (int i = 0; str[i]; i++) {
        if (str[i] >= 'a' && str[i] <= 'z') {
            str[i] -= 32;
        }
    }

    printf("Вы ввели: %s\n", str);

    char newStr[255];
    int counter = 0;
    for (int i = 0; str[i]; i++)
        if (str[i] >= 'Q' && str[i] <= 'Z')
            newStr[counter++] = str[i];

    printf("Ваша строка без других букв имеет вид: %s\n", newStr);

    int lettersNew[10] = {0};

    for (int i = 0; newStr[i]; i++)
        lettersNew[newStr[i] % 'Q']++;

    printf("Ваша отсортированная строка имеет вид: ");

    for (int i = 0; i < 10; i++)
        while (lettersNew[i]-- > 0)
            putchar((char) i + 'Q');
}

void Task26() {
    printf("\nЗадание 26. Имеются две строки A и B длиной по 30 символов и переменная K. Необходимо заменить в строке А символы, начиная с номера K, на первые (30-K) символов строки B");

    char strA[30], strB[30];

    printf("Введите строку A: ");
    gets(strA);

    printf("Введите строку B: ");
    gets(strB);

    printf("Введите число K:");
    int K;
    scanf("%d", &K);

    int i = K;
    int j = 0;
    while (strB[j] && i < 30) {
        strA[i++] = strB[j++];
    }
    printf("\nВаша строка A: %s", strA);
}

int main() {
    SetConsoleCP(CP_UTF8);
    SetConsoleOutputCP(CP_UTF8);
    srand(time(NULL));
    Task13();
    Task1();
    Task26();

    return 0;
}



