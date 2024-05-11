#include "stdlib.h"
#include "stdio.h"

struct TreeNode {
    int data;
    struct TreeNode* left;
    struct TreeNode* right;
};

struct BinarySearchTree {
    struct TreeNode* root;
};

struct TreeNode* createTreeNode(int data) {
    struct TreeNode* newNode = (struct TreeNode*)malloc(sizeof(struct TreeNode));
    if (newNode == NULL) {
        printf("Ошибка при выделении памяти для узла дерева.\n");
        exit(1);
    }
    newNode->data = data;
    newNode->left = NULL;
    newNode->right = NULL;
    return newNode;
}

struct TreeNode* insert(struct TreeNode* root, int data) {
    if (root == NULL)
        return createTreeNode(data);

    if (data < root->data) {
        root->left = insert(root->left, data);
    } else if (data > root->data) {
        root->right = insert(root->right, data);
    }

    return root;
}

struct TreeNode* search(struct TreeNode* root, int data) {
    if (root == NULL || root->data == data)
        return root;

    return search(data < root->data ? root->left : root->right, data);
}

void inOrderTraversal(struct TreeNode* root) {
    if (root != NULL) {
        inOrderTraversal(root->left);
        printf("%d ", root->data);
        inOrderTraversal(root->right);
    }
}

void preOrderTraversal(struct TreeNode* root) {
    if (root != NULL) {
        printf("%d ", root->data);
        preOrderTraversal(root->left);
        preOrderTraversal(root->right);
    }
}

void postOrderTraversal(struct TreeNode* root) {
    if (root != NULL) {
        postOrderTraversal(root->left);
        postOrderTraversal(root->right);
        printf("%d ", root->data);
    }
}

struct TreeNode* findMin(struct TreeNode* root) {
    while (root->left != NULL)
        root = root->left;
    return root;
}

struct TreeNode* deleteTreeNode(struct TreeNode* root, int data) {
    if (root == NULL)
        return root;

    if (data < root->data) {
        root->left = deleteTreeNode(root->left, data);
    } else if (data > root->data) {
        root->right = deleteTreeNode(root->right, data);
    } else {
        if (root->left == NULL) {
            struct TreeNode* temp = root->right;
            free(root);
            return temp;
        } else if (root->right == NULL) {
            struct TreeNode* temp = root->left;
            free(root);
            return temp;
        }

        struct TreeNode* temp = findMin(root->right);
        root->data = temp->data;
        root->right = deleteTreeNode(root->right, temp->data);
    }

    return root;
}

struct BinarySearchTree* createBinarySearchTree() {
    struct BinarySearchTree *tree = (struct BinarySearchTree*) malloc(sizeof(struct BinarySearchTree));
    if (tree == NULL) {
        printf("Ошибка при выделении памяти для бинарного дерева поиска.\n");
        exit(1);
    }
    tree->root = NULL;
    return tree;
}

void printTree(struct TreeNode* root, int space) {
    if (root == NULL)
        return;

    space += 5;

    printTree(root->right, space);

    printf("\n");
    for (int i = 5; i < space; i++)
        printf(" ");
    printf("%d\n", root->data);

    printTree(root->left, space);
}

int countIncompleteNodes(struct TreeNode* root) {
    if (root == NULL)
        return 0;

    int count = 0;

    if ((root->left == NULL && root->right != NULL) ||
        (root->left != NULL && root->right == NULL)) {
        count++;
    }

    count += countIncompleteNodes(root->left);
    count += countIncompleteNodes(root->right);

    return count;
}