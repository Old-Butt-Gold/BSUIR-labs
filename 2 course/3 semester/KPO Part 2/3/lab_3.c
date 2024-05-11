#include <stdio.h>
#include "stdlib.h"
#include "time.h"
#include "windows.h"

const int ARRAY_ROW = 5;
const int ARRAY_COL = 10;

void fillMatrix(int arr[][ARRAY_COL], int rows, int cols) {
    for (int i = 0; i < rows; i++) {
        for (int j = 0; j < cols; j++) {
            arr[i][j] = rand() % 200 + 1;
        }
    }
}

void printMatrix(int arr[][ARRAY_COL], int rows, int cols) {
    for (int i = 0; i < rows; i++) {
        for (int j = 0; j < cols; j++) {
            printf("%d\t", arr[i][j]);
        }
        printf("\n");
    }
}

void sortMatrixBySum(int arr[][ARRAY_COL], int sum[], int rows, int cols)
{
    int temp;
    for (int i = 0; i < rows - 1; i++) {
        for (int j = i + 1; j < rows; j++) {
            if (sum[i] < sum[j]) {
                // Обмениваем местами суммы строк
                temp = sum[i];
                sum[i] = sum[j];
                sum[j] = temp;

                // Обмениваем местами строки в матрице
                for (int k = 0; k < cols; k++) {
                    temp = arr[i][k];
                    arr[i][k] = arr[j][k];
                    arr[j][k] = temp;
                }
            }
        }
    }
    for (int i = 0; i < ARRAY_ROW; i++)
        printf("%d ", sum[i]);
    printf("\n");
}

int main() {
    SetConsoleCP(CP_UTF8);
    SetConsoleOutputCP(CP_UTF8);
    srand(time(NULL));

    printf("Задание 29. Расположить элементы строк матрицы A(5,5)\nв порядке возрастания, если номера строк четные и в порядке убывания, если номера строк нечетные\n");

    int arr[ARRAY_ROW][ARRAY_COL];
    int sum[ARRAY_ROW];
    fillMatrix(arr, sizeof(arr) / sizeof(arr[0]), sizeof(arr[0]) / sizeof(arr[0][0]));
    for (int i = 0; i < ARRAY_ROW; i++)
        sum[i] = 0;

    for (int i = 0; i < ARRAY_ROW; i++)
        for (int j = 0; j < ARRAY_COL; j++)
            sum[i] += arr[i][j];

    for (int i = 0; i < ARRAY_ROW; i++) {
        for (int j = 0; j < ARRAY_COL; j++) {
            printf("%d\t", arr[i][j]);
        }
        printf("Сумма чисел равна: %d\n", sum[i]);
    }

    printf("\n После сортировки по убыванию:\n");
    
    for (int i = 0; i < ARRAY_ROW; i++) {
        for (int j = 1; j < ARRAY_COL; j++) {
            int key = arr[i][j];
            int index = j - 1;
            while (index > -1 && key > arr[i][index])
            {
                arr[i][index + 1] = arr[i][index];
                index--;
            }

            arr[i][index + 1] = key;
        }
    }

    for (int i = 0; i < ARRAY_ROW; i++) {
        for (int j = 0; j < ARRAY_COL; j++) {
            printf("%d\t", arr[i][j]);
        }
        printf("Сумма чисел равна: %d\n", sum[i]);
    }

    printf("\n После сортировки по сумме в строках\n");

    sortMatrixBySum(arr, sum, sizeof(arr) / sizeof(arr[0]), sizeof(arr[0]) / sizeof(arr[0][0]));
    for (int i = 0; i < ARRAY_ROW; i++) {
        for (int j = 0; j < ARRAY_COL; j++) {
            printf("%d\t", arr[i][j]);
        }
        printf("Сумма чисел равна: %d\n", sum[i]);
    }


    return 0;
}



