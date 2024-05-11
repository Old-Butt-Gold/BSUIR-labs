#ifndef STACK_H
#define STACK_H

struct StackNode {
    int data;
    struct StackNode* next;
};

struct Stack {
    struct StackNode* top;
};

struct StackNode* createStackNode(int data);
struct Stack* createStack();
int isEmptyStack(struct Stack* stack);
void push(struct Stack* stack, int data);
int pop(struct Stack* stack);
int peek(struct Stack* stack);

#endif
