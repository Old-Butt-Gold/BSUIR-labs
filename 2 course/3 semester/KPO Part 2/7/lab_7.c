#include <stdio.h>
#include "mystring.h"

char doContinue() {
    printf("Do you wish to continue? Y/N (default Yes)\n> ");
    char ch;
    scanf("%c", &ch);
    return ch == 'N' || ch == 'n' ? 0 : 1;
}

char mergeArrayFile() {
    char ch;
    printf("Merge 2 files:\n");
    printf("Read name of first file:\n> ");
    while((ch = fgetc(stdin)) != '\n' && ch != EOF);
    char* fileName1 = "D:\\C\\1.txt";
    printf("Read name of second file:\n> ");
    char* fileName2 = "D:\\C\\2.txt";
    printf("Read name of result file:\n> ");
    char* fileNameResult = "D:\\C\\final.txt";

    FILE *file1 = fopen(fileName1, "r");
    FILE *file2 = fopen(fileName2, "r");
    FILE *fileResult = fopen(fileNameResult, "w");

    if (file1 == NULL || file2 == NULL || fileResult == NULL) {
        printf("Could not open a file.\n");
        return doContinue();
    }

    int num1, num2;

    int eof1 = fscanf(file1, "%d", &num1);
    int eof2 = fscanf(file2, "%d", &num2);

    while (eof1 > 0 && eof2 > 0) {
        if (num1 > num2) {
            fprintf(fileResult, "%d\t", num1);
            eof1 = fscanf(file1, "%d\t", &num1);
        } else {
            fprintf(fileResult, "%d\t", num2);
            eof2 = fscanf(file2, "%d\t", &num2);
        }
    }

    while (eof1 > 0) {
        fprintf(fileResult, "%d\t", num1);
        eof1 = fscanf(file1, "%d\t", &num1);
    }

    while (eof2 > 0) {
        fprintf(fileResult, "%d\t", num2);
        eof2 = fscanf(file2, "%d\t", &num2);
    }

    fclose(file1);
    fclose(file2);
    fclose(fileResult);

    printf("Sucessuly merged %s and %s into %s\n", fileName1, fileName2, fileNameResult);

    return doContinue();
}

int compStrNew(const void* a, const void* b) {
    return compStr(*(char**)a, *(char**)b);
}

char mergeStudentListsFile() {
    char ch;
    printf("Merge 2 sorted files:\n");
    printf("Read name of first file:\n> ");
    while((ch = fgetc(stdin)) != '\n' && ch != EOF);
    char* fileName1 = "D:\\C\\3.txt";
    printf("Read name of second file:\n> ");
    char* fileName2 = "D:\\C\\4.txt";
    printf("Read name of result file:\n> ");
    char* fileNameResult = "D:\\C\\final.txt";

    FILE *file1 = fopen(fileName1, "r");
    FILE *file2 = fopen(fileName2, "r");
    FILE *fileResult = fopen(fileNameResult, "w");

    if (file1 == NULL || file2 == NULL || fileResult == NULL) {
        printf("Could not open a file.\n");
        return doContinue();
    }

    char** studentArray1 = NULL;
    char** studentArray2 = NULL;
    int size1 = 0, size2 = 0;

    char* studentName1 = lineFileRead(file1);
    char* studentName2 = lineFileRead(file2);

    while (studentName1 != NULL) {
        size1++;
        studentArray1 = (char**)realloc(studentArray1, size1 * sizeof(char*));
        studentArray1[size1 - 1] = studentName1;
        studentName1 = lineFileRead(file1);
    }

    while (studentName2 != NULL) {
        size2++;
        studentArray2 = (char**)realloc(studentArray2, size2 * sizeof(char*));
        studentArray2[size2 - 1] = studentName2;
        studentName2 = lineFileRead(file2);
    }

    fclose(file1);
    fclose(file2);

    char** mergedArray = (char**)malloc((size1 + size2) * sizeof(char*));

    int i = 0, j = 0, k = 0;
    while (i < size1 && j < size2) {
        if (compStr(studentArray1[i], studentArray2[j]) <= 0)
            mergedArray[k++] = studentArray1[i++];
        else
            mergedArray[k++] = studentArray2[j++];
    }

    while (i < size1)
        mergedArray[k++] = studentArray1[i++];

    while (j < size2)
        mergedArray[k++] = studentArray2[j++];

    qsort(mergedArray, size1 + size2, sizeof(char*), compStrNew);

    for (i = 0; i < size1 + size2; i++)
        writeString(mergedArray[i], fileResult);

    fclose(fileResult);

    printf("Successfully merged %s and %s into %s\n", fileName1, fileName2, fileNameResult);

    return doContinue();
}

int main() {
    int i = 0;
    char isContinue = 1;
    while(isContinue) {
        printf("Choose what to do:\n");
        printf("1. Merge 2 arrays and sort from min to max;\n");
        printf("2. Merge 2 student files with alphabetically sort;\n");
        printf("3. Exit.\n");
        fflush(stdin);
        scanf("%d", &i);
        switch (i) {
            case 1: {
                isContinue = mergeArrayFile();
                break;
            }
            case 2: {
                isContinue = mergeStudentListsFile();
                break;
            }
            default: {
                isContinue = 0;
                break;
            }
        }
    }

    return 0;
}