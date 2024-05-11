#ifndef BST_H
#define BST_H

struct TreeNode {
    int data;
    struct TreeNode* left;
    struct TreeNode* right;
};

struct BinarySearchTree {
    struct TreeNode* root;
};

struct TreeNode* createTreeNode(int data);
struct BinarySearchTree* createBinarySearchTree();
struct TreeNode* insert(struct TreeNode* tree, int data);
struct TreeNode* search(struct TreeNode* tree, int data);
void inOrderTraversal(struct TreeNode* node);
void preOrderTraversal(struct TreeNode* node);
void postOrderTraversal(struct TreeNode* node);
struct TreeNode* findMin(struct TreeNode* node);
struct TreeNode* deleteTreeNode(struct BinarySearchTree* tree, int data);
void printTree(struct TreeNode* root, int space);
int countIncompleteNodes(struct TreeNode* root);

#endif
