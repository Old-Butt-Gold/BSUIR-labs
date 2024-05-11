#include <stdio.h>
#include <stdlib.h>
#include "queue.h"

struct QueueNode* createQueueNode(int data) {
    struct QueueNode* newNode = (struct QueueNode*)malloc(sizeof(struct QueueNode));
    if (newNode == NULL) {
        printf("Ошибка при выделении памяти для узла очереди.\n");
        exit(1);
    }
    newNode->data = data;
    newNode->next = NULL;
    return newNode;
}

struct Queue* createQueue() {
    struct Queue* queue = (struct Queue*)malloc(sizeof(struct Queue));
    if (queue == NULL) {
        printf("Ошибка при выделении памяти для очереди.\n");
        exit(1);
    }
    queue->front = queue->rear = NULL;
    return queue;
}

int isEmptyQueue(struct Queue* queue) {
    return (queue->front == NULL);
}

void enqueue(struct Queue* queue, int data) {
    struct QueueNode* newNode = createQueueNode(data);
    if (isEmptyQueue(queue)) {
        queue->front = queue->rear = newNode;
    } else {
        queue->rear->next = newNode;
        queue->rear = newNode;
    }
}

int dequeue(struct Queue* queue) {
    if (isEmptyQueue(queue)) {
        printf("Очередь пуста. Невозможно выполнить операцию dequeue.\n");
        exit(1);
    }
    struct QueueNode* temp = queue->front;
    int data = temp->data;
    queue->front = temp->next;
    free(temp);
    if (queue->front == NULL) {
        queue->rear = NULL;
    }
    return data;
}

int front(struct Queue* queue) {
    if (isEmptyQueue(queue)) {
        printf("Очередь пуста. Невозможно выполнить операцию front.\n");
        exit(1);
    }
    return queue->front->data;
}

int rear(struct Queue* queue) {
    if (isEmptyQueue(queue)) {
        printf("Очередь пуста. Невозможно выполнить операцию rear.\n");
        exit(1);
    }
    return queue->rear->data;
}
