#include <stdio.h>
#include "windows.h"

typedef struct student STUDENT;
#define MAX_AMOUNT 10


struct student {
    char FIO[255];
    int groupNumber;
    double avgMark;
    int income;
};

void sortStruct(STUDENT group[], int size) {
    for (int i = 1; i < size; i++) {
        STUDENT tempGroup = group[i];
        int key = group[i].income;
        int j = i - 1;
        while (j > -1 && key > group[j].income) {
            group[j + 1] = group[j];
            j--;
        }
        group[j + 1] = tempGroup;
    }
}

int main() {
    SetConsoleCP(CP_UTF8);
    SetConsoleOutputCP(CP_UTF8);
    STUDENT group[MAX_AMOUNT] = {
            {.FIO = "Кумичова Дарья", .avgMark = 10.1, .groupNumber = 251004, .income = 1000},
            {.FIO = "Кирлица Дарья", .avgMark = 1.1, .groupNumber = 251004, .income = 100},
            {.FIO = "Заяц Александра", .avgMark = 90.1, .groupNumber = 251004, .income = 2110},
            {.FIO = "Крутько Андрей", .avgMark = 10.0, .groupNumber = 251004, .income = 51000},
            {.FIO = "Данила Асепков", .avgMark = 0.1, .groupNumber = 251004, .income = 10000},
            {.FIO = "Карась Андрей", .avgMark = 10.1, .groupNumber = 251004, .income = 1000},
            {.FIO = "Влад Арефин", .avgMark = 5.5, .groupNumber = 251004, .income = 10040},
            {.FIO = "Иванов Андрей", .avgMark = 6.1, .groupNumber = 251004, .income = 12000},
            {.FIO = "Матвей Елькин", .avgMark = 8.1, .groupNumber = 251004, .income = 1600},
            {.FIO = "Шмаргун Александр", .avgMark = 2.1, .groupNumber = 251004, .income = 1000},
    };
    for (int i = 0; i < MAX_AMOUNT; i++)
        printf("ФИО: %s; Средний балл: %f2; Номер группы: %d; Доход: %d$\n",
               group[i].FIO, group[i].avgMark, group[i].groupNumber, group[i].income);
    printf("\n\n");
    sortStruct(group, sizeof(group) / sizeof(group[0]));
    for (int i = 0; i < MAX_AMOUNT; i++)
        printf("ФИО: %s; Средний балл: %f2; Номер группы: %d; Доход: %d$\n",
               group[i].FIO, group[i].avgMark, group[i].groupNumber, group[i].income);
    return 0;
}
