#include <stdio.h>
#include <stdlib.h>
#include <windows.h>

char* lineRead() {
    char* str = (char*) malloc(sizeof(char));
    char ch;
    int i = 0;
    while(ch = getc(stdin), ch != '\n') {
        str[i] = ch;
        i++;
        str = (char*) realloc(str, (i + 1) * sizeof(char));
    }
    str[i] = '\0';
    return str[0] == '\0' ? NULL : str;
}

int GCD(int a, int b) {
    return b == 0 ? a : GCD(b, a % b);
}

int Task12() {
    int num1, num2;
    scanf("%d %d", &num1, &num2);
    int result = GCD(num1, num2);
    printf("%d\n", result);
    return result;
}

int IsPowerOfTwo(int number) {
    return (number & number - 1) == 0 ? 1 : 0;
}

void Task21() {
    int number;
    scanf("%d", &number);
    printf(IsPowerOfTwo(number) ? "Power of 2\n" : "Not a power of 2\n");
}

int Task22() {
    char* str = lineRead();
    int count = 0;
    for (int i = 0; str[i]; i++)
        count += (str[i] >= 'A' && str[i] <= 'Z');
    printf("%d\n", count);
    return count;
}

void Task6() {
    int len;
    scanf("%d", &len);
    if (len < 2) return;
    int* arr = (int *) malloc(sizeof(int) * len);
    if (arr == NULL) return;
    for (int i = 0; i < len; i++)
        scanf("%d", &arr[i]);
    int num1, num2;

    if (arr[0] < arr[1]) {
        num1 = arr[0];
        num2 = arr[1];
    } else {
        num1 = arr[1];
        num2 = arr[0];
    }

    for (int i = 2; i < len; i++) {
        if (arr[i] < num1) {
            num2 = num1;
            num1 = arr[i];
        } else if (arr[i] < num2) {
            num2 = arr[i];
        }
    }

    printf("2 min: %d and %d\n", num1, num2);
    free(arr);
}

void Task5() {
    int number;
    scanf("%d", &number);
    for (int i = 1; i <= number; i++) {
        printf("2^%d = %d\n", i, 1 << i);
    }
}

void Task1() {
    printf("Введите строку: ");
    char* str = lineRead();
    printf("Введите число: ");
    int num1;
    scanf("%d", &num1);
    int temp = num1;
    int numLen = 0;
    while (temp > 0) {
        numLen++;
        temp /= 10;
    }
    char *num = (char *) malloc((len_num + 1) * sizeof(char));
    sprintf(num, "%d", num1);
    num[len_num] = '\0';
    int count = 0;
    int index = 0;

    for (int i = 0; str[i]; i++)
    {
        if (str[i] == num[index++]) {
            if (index == numLen) {
                count++;
                index = 0;
            }
        } else {
            index = 0;
        }
    }
    printf("Общее число вхождений: %d\n", count);
    free(str);
}

void Task3() {
    int N;
    int K;
    scanf("%d %d", &N, &K);
    if (K < N) {
        int* arr = (int*) malloc(sizeof(int) * N);
        for (int i = 0; i < N; i++)
            scanf("%d", &arr[i]);
        for (int i = K; i < N - 1; i++)
            arr[i] = arr[i] + 1;
        arr = (int*) realloc(arr, (N - 1) * sizeof(int));
        for (int i = 0; i < N - 1; i++)
            printf("%7d", arr[i]);
        printf("\n");
    }
}

void Task8() {
    char* str = lineRead();
    char symbol;
    scanf("%c", &symbol);
    int count = 0;
    int len = 0;
    for (int i = 0; str[i]; i++) {
        count += symbol == str[i];
        len++;
    }

    double percentage = (double) count / len;
    percentage *= 100;
    printf("%.2f%%", percentage);

}

void Task14() {
    char* str = lineRead();
    int isTrue = 1;
    for (int i = 0; str[i]; i++)
        isTrue &= (str[i] <= '9' && str[i] >= '0') || str[i] == ' ' || str[i] == '+' || str[i] == '-' || str[i] == ':' || str[i] == '*';
    printf((isTrue ? "True" : "False"));
}

void Task17() {
    char* str = lineRead();
    char symbol;
    scanf("%c", &symbol);
    int len = 0;
    for (int i = 0; str[i]; i++)
        len++;
    for (int i = 0; str[i]; i++)
        if (str[i] == symbol) {
            for (int j = i; j < len - 1; j++)
                str[j] = str[j + 1];
            str[len - 1] = '\0';
            len--;
            str = (char*) realloc(str, sizeof(char) * len);
        }

    printf("%s", str);
}

void Task4() {
    char* str = lineRead();
    char* newStr = (char*) malloc(sizeof(char));
    int index = 0;
    for (int i = 0; str[i]; i++) {
        int isTrue = 1;
        int j = i;
        while (str[j] != ' ' && str[j + 1] != '\0') {
            isTrue &= str[j] >= '0' && str[j] <= '9';
            j++;
        }
        if (str[j + 1] == '\0')
            isTrue &= str[j] >= '0' && str[j] <= '9';

        if (isTrue) {
            newStr = (char*) realloc(newStr, sizeof(char) * j - i + index + 1);
            for (int k = i; k <= j; k++)
                newStr[index++] = str[k];
            if (str[j + 1] != '\0')
                newStr[index - 1] = ',';
        }
        i = j;
    }
    newStr[(newStr[index - 1] == ',' ? index - 1 : index)] = '\0';
    printf("%s\n", newStr);
    free(str);
}

void Task15() {
    int N;
    int K;
    scanf("%d %d", &N, &K);
    int** matrix = (int**) malloc(sizeof(int*) * N);
    for (int i = 0; i < N; i++)
        matrix[i] = (int*) malloc(sizeof(int) * K);
    for (int i = 0; i < N; i++)
        for (int j = 0; j < K; j++)
            scanf("%d", &matrix[i][j]);
    int sum = 0;
    for (int i = 0; i < N; i++)
        sum += matrix[i][0];
    int index = 0;
    for (int j = 1; j < K; j++) {
        int tempSum = 0;
        for (int i = 0; i < N; i++)
            tempSum += matrix[i][j];
        if (sum < tempSum) {
            sum = tempSum;
            index = j;
        }
    }
    printf("%d\n", index);
}

void Task18() {
    int N;
    int K;
    scanf("%d %d", &N, &K);
    int** matrix = (int**) malloc(sizeof(int*) * N);
    for (int i = 0; i < N; i++)
        matrix[i] = (int*) malloc(sizeof(int) * K);
    for (int i = 0; i < N; i++)
        for (int j = 0; j < K; j++)
            scanf("%d", &matrix[i][j]);
    int sum = 0;
    for (int i = 0; i < N; i++)
        sum += matrix[0][i];
    int index = 0;
    for (int i = 1; i < N; i++) {
        int tempSum = 0;
        for (int j = 0; j < K; j++)
            tempSum += matrix[i][j];
        if (tempSum < sum) {
            sum = tempSum;
            index = i;
        }
    }
    printf("%d\n", index);
}

void Task19() {
    char* str = lineRead();
    char symbol;
    scanf("%c", &symbol);
    int count = 0;
    for (int i = 0; str[i]; i++) {
        if (str[i] == symbol) {
            while (str[i] != ' ' && str[i + 1] != '\0')
                i++;
            count++;
        }
    }
    printf("%d\n", count);
}

void Task20() {
    int N;
    scanf("%d", &N);
    int* arr = (int*) malloc(sizeof(int) * N);
    for (int i = 0; i < N; i++)
        scanf("%d", &arr[i]);
    int num = 0x7FFFFFFF;
    int isExisting = 0;
    for (int i = 0; i < N; i++)
        if (arr[i] % 2 == 0 && arr[i] < num) {
            num = arr[i];
            isExisting = 1;
        }
    !isExisting ? printf("no") : printf("%d\n", num);
}

void Task2() {
    int N;
    scanf("%d", &N);
    int* arr = (int*) malloc(sizeof(int) * N);
    int* newArr = (int*) malloc(sizeof(int) * N);
    for (int i = 0; i < N; i++)
        scanf("%d", &arr[i]);
    int index = 0;
    for (int i = 0; i < N; i++)
        if (arr[i] % 2 == 1)
            newArr[index++] = arr[i];
    for (int i = N - 1; i > -1; i--)
        if (arr[i] % 2 == 0)
            newArr[index++] = arr[i];
    for (int i = 0; i < N; i++)
        printf("%d ", newArr[i]);
    printf("\n");
}

void Task10() {
    char* str = lineRead();
    char symbol;
    scanf("%c", &symbol);

    char* newStr = (char*) malloc(sizeof(char));
    int index = 0;

    for (int i = 0; str[i]; i++)
        if (str[i] == symbol) {
            printf("%d ", i);
        } else {
            newStr[index] = str[i];
            index++;
            newStr = (char*) realloc(newStr, (index + 1) * sizeof(char));
        }
    newStr[index] = '\0';
    printf("\n");
    printf("%s\n", newStr);
}

void Task11() {
    int num;
    scanf("%d", &num);
    int base;
    scanf("%d", &base);
    int *result = (int *) malloc(sizeof(int));
    int index = 0;

    while (num > 0) {
        result[index++] = num % base;
        num /= base;
        result = (int *) realloc(result, sizeof(int) * (index + 1));
    }

    for (int i = index - 1; i > -1; i--)
        result[i] < 10 ? printf("%d", result[i]) : printf("%c", 'A' + result[i] - 10);
    printf("\n");

}

void Task16() {
    int num;
    scanf("%d", &num);
    int base;
    scanf("%d", &base);
    if (base < 10 && base > 1) {
        int *result = (int *) malloc(sizeof(int));
        int index = 0;

        while (num > 0) {
            result[index++] = num % base;
            num /= base;
            result = (int *) realloc(result, sizeof(int) * (index + 1));
        }

        for (int i = index - 1; i > -1; i--)
            printf("%d", result[i]);
        printf("\n");
    }
};

void Task13() {

}

int main() {
    SetConsoleCP(CP_UTF8);
    SetConsoleOutputCP(CP_UTF8);
    int choice;
    int loop = 1;
    while (loop) {
        scanf("%d", &choice);
        fflush(stdin);
        switch (choice) {
            case 0:
                loop = 0;
                break;
            case 1:
                //Найти в строке, содержащей слова и числа, разделенные пробелами, количество вхождений числа N
                Task1();
                break;
            case 2:
                //есть массив 987654321. Сделать 975312468
                Task2();
                break;
            case 3:
                //Ввод чисел N и K, K < N. Создать массив из N элементов и удалить из массива K-й элемент.
                Task3();
                break;
            case 4:
                //Дана строка, состоящая из идентификаторов и чисел, разделенных пробелами. Необходимо сформировать новую строку, состоящую из чисел, разделенных запятыми.
                Task4();
                break;
            case 5:
                //Дано число, вывести степени двойки от первой до N-ой
                Task5();
                break;
            case 6:
                //За один проход найти два минимальных числа
                Task6();
                break;
            case 7:
                //Ввод чисел N и K, K < N. Создать массив из N элементов и удалить из массива K-й элемент.
                Task3();
                break;
            case 8:
                //Найти процентное соотношение в строке символа.
                Task8();
                break;
            case 9:
                //Удалить из массива K-й элемент
                Task3();
                break;
            case 10:
                //Найти все вхождения символа в строку и сформировать новую без данного символа.
                Task10();
                break;
            case 11:
                //Перевод в n-ую СС из 10-тичной
                Task11();
                break;
            //НОД методом Эвклида
            case 12:
                Task12();
                break;
            case 13:
                //В строке S могут быть комментарии. Удалить их
                Task13();
                break;
            case 14:
                //Проверить, является ли строка математическим выражением
                Task14();
                break;
            case 15:
                //найти индекс столбца, в котором сумма elements max
                Task15();
                break;
            case 16:
                //Дано два положительных целочисленных значения A и D. Вывести число A в СС (2 <= D <= 9), при этом нельзя, чтобы вначале были нули.
                Task16();
                break;
            case 17:
                //Дана строка S. Удалить все символы C из строки S.
                Task17();
                break;
            case 18:
                //найти индекс строки, в которых сумма elements min
                Task18();
                break;
            case 19:
                //Дана строка S и символ C. Слова в строке разделены одним пробелом. Найти количество слов с символов C.
                Task19();
                break;
            case 20:
                //Дано N и массив целых чисел размерности N, найти минимальный четный элемент, если их нет, вывести "no"
                Task20();
                break;
            case 21:
                //определить, является ли введенное с клавиатуры число степенью двойки
                Task21();
                break;
            case 22:
                //Подсчитать количество латинских букв в строке
                Task22();
                break;
        }
    }
    return 0;
}
