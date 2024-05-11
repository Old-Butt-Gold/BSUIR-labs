#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include "linkedlist.h"
#include <windows.h>

int main ()
{
    SetConsoleCP(CP_UTF8);
    SetConsoleOutputCP(CP_UTF8);
    int count;
    struct LinkedList* list = createLinkedList();
    printf("Введите количество элементов: \n");
    scanf("%d", &count);

    for (int i = 0; i < count; i++) {
        printf("Введите элемент %d: ", i + 1);
        int num;
        scanf("%d", &num);
        append(list, num);
    }

    printf("Ваш список: \n");
    printList(list);


    struct Node* temp = list->head;
    int index = -1;
    int counter = 0;
    while (temp != NULL) {
        if (temp->data % 2 == 0 && counter % 2 == 0) {
            index = temp->data;
            break;
        }
        counter++;
        temp = temp->next;
    }
    if (index == -1)
        printf("Такого элемента нет\n");
    else {
        deleteNode(list, index);
        printf("Ваш измененный список: \n");
        printList(list);
    }


	return 0;
}