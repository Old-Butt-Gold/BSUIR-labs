#include "stdio.h"
#include "windows.h"

int main() {
    SetConsoleCP(CP_UTF8);
    SetConsoleOutputCP(CP_UTF8);
    char str[255];

    printf("Введите строку: ");
    gets(str);

    printf("Исходное слово: %s\n", str);

    printf("\"Протокол\" просмотра: ");
    int left = 0;
    int right = 0;
    for (int i = 0; str[i + 1]; i++)
        right++;
    while (left <= right) {
        while (left <= right && str[left] != str[left + 1])
            putchar(str[left++]);
        while (left <= right && str[left] == str[left + 1])
            left++;
        if (left <= right)
            putchar('_');
        while (left <= right && str[right] != str[right - 1])
            putchar(str[right--]);
        while (left <= right && str[right] == str[right - 1])
            right--;
        if (left <= right)
            putchar('_');
        left++;
        right--;
    }
    printf("\n");

    return 0;
}



