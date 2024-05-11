#include <string.h>
#include <windows.h>
#include <stdio.h>
#include "queue.h"

int main ()
{
    SetConsoleCP(CP_UTF8);
    SetConsoleOutputCP(CP_UTF8);
    int number;
    scanf("%d", &number);
    int* arr = (int*) malloc(sizeof(int) * number);
    for (int i = 0; i < number; i++)
        arr[i] = rand() % 2;

    for (int i = 0; i < number; i++)
        printf(arr[i] == 0 ? "Б шар | " : "Ч шар | ");
    printf("\n");
    struct Queue* white = createQueue();
    struct Queue* black = createQueue();
    for (int i = 0; i < number; i++)
        enqueue(arr[i] == 0 ? white : black, arr[i]);

    while (!isEmptyQueue(white)) {
        dequeue(white);
        printf("Б шар | ");
    }

    while (!isEmptyQueue(black)) {
        dequeue(black);
        printf("Ч шар | ");
    }

	return 0;
}