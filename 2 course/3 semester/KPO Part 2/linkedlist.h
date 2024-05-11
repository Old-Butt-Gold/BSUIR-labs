#ifndef LINKEDLIST_H
#define LINKEDLIST_H

struct Node {
    int data;
    struct Node* next;
};

struct LinkedList {
    struct Node* head;
};

struct Node* createNode(int data);
struct LinkedList* createLinkedList();
int isEmpty(struct LinkedList* list);
void append(struct LinkedList* list, int data);
void printList(struct LinkedList* list);
void deleteNode(struct LinkedList* list, int data);
struct Node* findNode(struct LinkedList* list, int data);
struct Node* tailNode(struct LinkedList* list);
int countNodes(struct LinkedList* list);

#endif
