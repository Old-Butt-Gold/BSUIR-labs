#include <stdio.h>
#include <stdlib.h>
#include "stack.h"

struct StackNode* createStackNode(int data) {
    struct StackNode* newNode = (struct StackNode*)malloc(sizeof(struct StackNode));
    if (newNode == NULL) {
        printf("Ошибка при выделении памяти для узла стека.\n");
        exit(1);
    }
    newNode->data = data;
    newNode->next = NULL;
    return newNode;
}

struct Stack* createStack() {
    struct Stack* stack = (struct Stack*)malloc(sizeof(struct Stack));
    if (stack == NULL) {
        printf("Ошибка при выделении памяти для стека.\n");
        exit(1);
    }
    stack->top = NULL;
    return stack;
}

int isEmptyStack(struct Stack* stack) {
    return (stack->top == NULL);
}

void push(struct Stack* stack, int data) {
    struct StackNode* newNode = createStackNode(data);
    newNode->next = stack->top;
    stack->top = newNode;
}

int pop(struct Stack* stack) {
    if (isEmptyStack(stack)) {
        printf("Стек пуст. Невозможно выполнить операцию pop.\n");
        exit(1);
    }
    struct StackNode* temp = stack->top;
    int data = temp->data;
    stack->top = temp->next;
    free(temp);
    return data;
}

int peek(struct Stack* stack) {
    if (isEmptyStack(stack)) {
        printf("Стек пуст. Невозможно выполнить операцию peek.\n");
        exit(1);
    }
    return stack->top->data;
}
