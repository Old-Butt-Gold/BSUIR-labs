#include <stdio.h>
#include <stdlib.h>
#include "linkedlist.h"

struct Node* createNode(int data) {
    struct Node* newNode = (struct Node*)malloc(sizeof(struct Node));
    if (newNode == NULL) {
        printf("Ошибка при выделении памяти для узла связного списка.\n");
        exit(1);
    }
    newNode->data = data;
    newNode->next = NULL;
    return newNode;
}

struct LinkedList* createLinkedList() {
    struct LinkedList* list = (struct LinkedList*)malloc(sizeof(struct LinkedList));
    if (list == NULL) {
        printf("Ошибка при выделении памяти для связного списка.\n");
        exit(1);
    }
    list->head = NULL;
    return list;
}

int isEmpty(struct LinkedList* list) {
    return (list->head == NULL);
}

void append(struct LinkedList* list, int data) {
    struct Node* newNode = createNode(data);
    if (list->head == NULL) {
        list->head = newNode;
    } else {
        struct Node* current = list->head;
        while (current->next != NULL) {
            current = current->next;
        }
        current->next = newNode;
    }
}

void printList(struct LinkedList* list) {
    struct Node* current = list->head;
    while (current != NULL) {
        printf("%d -> ", current->data);
        current = current->next;
    }
    printf("NULL\n");
}

void deleteNode(struct LinkedList* list, int data) {
    struct Node* current = list->head;
    struct Node* prev = NULL;

    while (current != NULL && current->data != data) {
        prev = current;
        current = current->next;
    }

    if (current == NULL) {
        printf("Узел с данными %d не найден.\n", data);
        return;
    }

    if (prev == NULL) {
        list->head = current->next;
    } else {
        prev->next = current->next;
    }

    free(current);
}

struct Node* findNode(struct LinkedList* list, int data) {
    struct Node* current = list->head;

    while (current != NULL) {
        if (current->data == data) {
            return current;
        }
        current = current->next;
    }

    return NULL;
}

struct Node* tailNode(struct LinkedList* list) {
    if (list->head == NULL) {
        return NULL;
    }

    struct Node* current = list->head;
    while (current->next != NULL) {
        current = current->next;
    }

    return current;
}

int countNodes(struct LinkedList* list) {
    int count = 0;
    struct Node* current = list->head;

    while (current != NULL) {
        count++;
        current = current->next;
    }

    return count;
}
