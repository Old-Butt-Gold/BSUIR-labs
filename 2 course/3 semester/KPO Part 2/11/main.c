#include <string.h>
#include <windows.h>
#include <stdio.h>
#include "bst.h"

int main ()
{
    SetConsoleCP(CP_UTF8);
    SetConsoleOutputCP(CP_UTF8);

    printf("Программа подсчитывает число неполных узлов заданного бинарного дерева\n");
    int stop = 1;
    struct BinarySearchTree* tree = createBinarySearchTree();
    while (stop) {
        printf("Введите 0, чтобы добавить элемент\n1, чтобы удалить элемент\n2, чтобы найти элемент\n3, для прямого обхода\n4, для симметричного обхода\n5, для обратного обхода\n6, для подсчета числа неполных узлов заданного бинарного дерева\n7, для вывода дерева\n8, выйти\n");
        int num;
        scanf("%d", &num);
        switch (num) {
            case 0:
                printf("Введите число\n");
                int data;
                scanf("%d", &data);
                tree->root = insert(tree->root, data);
                break;
            case 1:
                printf("Введите число\n");
                int data1;
                scanf("%d", &data1);
                tree->root = deleteTreeNode(tree, data1);
                break;
            case 2:
                printf("Введите число\n");
                int data2;
                scanf("%d", &data2);
                struct TreeNode* temp = search(tree->root, data2);
                printf(temp == NULL ? "не найдено\n" : "найдено\n");
                break;
            case 3:
                preOrderTraversal(tree->root);
                break;
            case 4:
                inOrderTraversal(tree->root);
                break;
            case 5:
                postOrderTraversal(tree->root);
                break;
            case 6:
                printf("Ваше число неполных вершин: %d\n", countIncompleteNodes(tree->root));
                break;
            case 7:
                printTree(tree->root, 4);
                printf("\n");
                break;
            case 8:
                stop = 0;
                break;
        }
    }



	return 0;
}