#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include "string.h"
#include "windows.h"

#define mark_num 5

typedef struct semester
{
    char* name;
    int group;
    int marks[mark_num];
} Sem;

Sem inputStruct()
{
    Sem sem;
    char ch;
    printf("Введите имя\n");
    while((ch = fgetc(stdin)) != '\n' && ch != EOF);
    sem.name = lineRead();

    printf("Введите группу\n");
    scanf("%d", &sem.group);

    printf("Введите оценки 5 штук\n");
    for (int i = 0; i < mark_num; i++) {
        printf("Введите оценку %d: ", i + 1);
        scanf("%d", &sem.marks[i]);
    }
    return sem;
}

void changeStruct(Sem* arr, int size)
{
    if (size == 0) {
        printf("У вас нет данных\n");
        return;
    }
    char ch;
    printf("Введите номер студента, которого хотите изменить: \n");
    int student_num;
    scanf("%d", &student_num);
    if (student_num < 0 || student_num >= size)
        printf("Такого студента не существует\n");
    else
        arr[student_num] = inputStruct();
}

void deleteStruct(Sem* arr, int* size) {
    if (*size == 0) {
        printf("У вас нет данных\n");
        return;
    }
    printf("Введите номер студента, которого хотите удалить: \n");
    int student_num;
    scanf("%d", &student_num);

    while (getchar() != '\n');

    if (student_num < 0 || student_num >= *size)
        printf("Такого студента не существует\n");
    else {
        for (int i = student_num; i < *size - 1; i++)
            arr[i] = arr[i + 1];
        (*size)--;
        printf("Данные успешно удалены\n");
    }
}

float calculateAverage(Sem* arr, int index)
{
    int sum = 0;
    for (int j = 0; j < mark_num; j++) {
            sum += arr[index].marks[j];
    }

    return (float)sum / mark_num;
}

void sortByGroup(Sem* arr, int size) {
    for (int i = 0; i < size - 1; i++) {
        for (int j = 0; j < size - i - 1; j++) {
            if (arr[j].group > arr[j + 1].group) {
                Sem temp = arr[j];
                arr[j] = arr[j + 1];
                arr[j + 1] = temp;
            }
        }
    }
}


void outputTable(Sem* arr, int size)
{
    printf("Таблица:\n");
    printf("------------------------------------------------------------------\n");
    printf("| %-4s | %-20s | %-6s | %-40s | %-20s\n", "ID", "Имя", "Группа", "Оценки", "Средний балл");
    printf("------------------------------------------------------------------\n");
    for (int i = 0; i < size; i++)
    {
        printf("| %-4d | %s | %-6d |", i, arr[i].name, arr[i].group);
        for (int j = 0; j < mark_num; j++) {
            printf(" %-4d", arr[i].marks[j]);
        }
        printf(" | %.2f\n", calculateAverage(arr, i));
    }
    printf("------------------------------------------------------------\n");
}

void outputTable2(Sem* arr, int size, int groupNum)
{
    printf("Таблица:\n");
    printf("------------------------------------------------------------------\n");
    printf("| %-4s | %-20s | %-6s | %-40s | %-20s\n", "ID", "Имя", "Группа", "Оценки", "Средний балл");
    printf("------------------------------------------------------------------\n");
    for (int i = 0; i < size; i++)
    {
        if (groupNum == arr[i].group) {
            printf("| %-4d | %s | %-6d |", i, arr[i].name, arr[i].group);
            for (int j = 0; j < mark_num; j++) {
                printf(" %-4d", arr[i].marks[j]);
            }
            printf(" | %.2f\n", calculateAverage(arr, i));
        }
    }
    printf("------------------------------------------------------------\n");
}

void outputTableFile(FILE* file, Sem* arr, int size)
{
    fprintf(file, "Таблица:\n");
    fprintf(file, "------------------------------------------------------------------\n");
    fprintf(file, "| %-4s | %-20s | %-6s | %-40s | %-20s\n", "ID", "Имя", "Группа", "Оценки", "Средний балл");
    fprintf(file, "------------------------------------------------------------------\n");
    for (int i = 0; i < size; i++)
    {
        fprintf(file, "| %-4d | %s | %-6d |", i, arr[i].name, arr[i].group);
        for (int j = 0; j < mark_num; j++) {
            fprintf(file, " %-4d", arr[i].marks[j]);
        }
        fprintf(file, " | %.2f\n", calculateAverage(arr, i));
    }
    fprintf(file, "------------------------------------------------------------\n");
    fflush(file);
}

int main() {
    SetConsoleCP(CP_UTF8);
    SetConsoleOutputCP(CP_UTF8);
    FILE* file;
    int amount;
    char ch;
    printf("Введите число студентов: \n");
    scanf("%d", &amount);

    printf("Введите путь к файлу: \n");
    while((ch = fgetc(stdin)) != '\n' && ch != EOF);
    char* path = lineRead();

    file = fopen(path, "w");
    if (file == NULL) {
        printf("%s данный путь не существует или же проверьте файл", path);
        return 1;
    }
    int choose = 0;

    Sem* arr = (Sem*) malloc(sizeof(Sem) * amount);
    int size = 0;

    while (choose != 6)
    {
        printf("Введите 0 для добавления\nВведите 1 для изменения\n Введите 2 для удаления\n Введите 3 для распечатки таблицы студентов\nВведите 4 для сохранения в файл\nВведите 5 для поиска по группе\nВведите 6 для выхода\n");
        scanf("%d", &choose);
        switch (choose)
        {
            case 0:
            {
                arr[size++] = inputStruct();
                break;
            }
            case 1:
            {
                changeStruct(arr, size);
                break;
            }
            case 2:
            {
                deleteStruct(arr, &size);
                break;
            }
            case 3:
            {
                outputTable(arr, size);
                break;
            }
            case 4:
            {
                outputTableFile(file, arr, size);
                break;
            }
            case 5:
            {
                int groupNum;
                scanf("%d", &groupNum);
                outputTable2(arr, size, groupNum);
            }
        }
    }
    fclose(file);

    return 0;
}
