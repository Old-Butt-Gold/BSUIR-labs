#include <stdio.h>
#include "stdlib.h"
#include "time.h"

const int ARRAY_SIZE = 6;

void fillMatrix(int arr[][6], int rows, int cols) {
    for (int i = 0; i < rows; i++) {
        for (int j = 0; j < cols; j++) {
            arr[i][j] = rand() % 100 + 1;
        }
    }
}

void printMatrix(int arr[][6], int rows, int cols) {
    for (int i = 0; i < rows; i++) {
        for (int j = 0; j < cols; j++) {
            printf("%d ", arr[i][j]);
        }
        printf("\n");
    }
}

int main() {
    srand(time(NULL));

    printf("Task 25\n");

    int arr[ARRAY_SIZE][ARRAY_SIZE];
    fillMatrix(arr, sizeof(arr) / sizeof(arr[0]), sizeof(arr[0]) / sizeof(arr[0][0]));
    printMatrix(arr, sizeof(arr) / sizeof(arr[0]), sizeof(arr[0]) / sizeof(arr[0][0]));

    int sumMain = 0;
    for (int i = 0; i < 6; i++)
        sumMain += arr[i][i];
    printf("Main diag = %d, Arithmetic average = %.3f\n", sumMain, (float) sumMain / 6);

    int sumSide = 0;
    for (int i = 0; i < 6; i++)
        sumSide += arr[i][6 - 1 - i];
    printf("Side diag = %d, Arithmetic average = %.3f\n", sumSide, (float) sumSide / 6);

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    printf("Task17\n");

    fillMatrix(arr, sizeof(arr) / sizeof(arr[0]), sizeof(arr[0]) / sizeof(arr[0][0]));
    printMatrix(arr, sizeof(arr) / sizeof(arr[0]), sizeof(arr[0]) / sizeof(arr[0][0]));

    for (int i = 0; i < ARRAY_SIZE; i++) {
        for (int j = 0; j < ARRAY_SIZE; j++) {
            int counter = 0;
            if (arr[i][j] != -1) {
                int elem = arr[i][j];
                for (int i = 0; i < ARRAY_SIZE; i++)
                    for (int j = 0; j < ARRAY_SIZE; j++)
                        if (arr[i][j] == elem) {
                            counter++;
                            arr[i][j] = -1;
                        }
                printf("Counter = %d, Element = %d\n", counter, elem);
            }
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    printf("Task21\n");

    fillMatrix(arr, sizeof(arr) / sizeof(arr[0]), sizeof(arr[0]) / sizeof(arr[0][0]));
    printMatrix(arr, sizeof(arr) / sizeof(arr[0]), sizeof(arr[0]) / sizeof(arr[0][0]));
    int sum = 0;
    for (int i = 0; i < ARRAY_SIZE; i++) {
        int temp = arr[i][0];
        if (i % 2 == 0) {
            for (int j = 1; j < ARRAY_SIZE; j++)
                if (temp > arr[i][j])
                    temp = arr[i][j];
        } else {
            for (int j = 1; j < ARRAY_SIZE; j++)
                if (temp < arr[i][j])
                    temp = arr[i][j];
        }
        sum += temp;
    }

    printf("Sum of Min and Max elements = %d", sum);

    return 0;
}



