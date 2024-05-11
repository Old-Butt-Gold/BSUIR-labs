#ifndef MY_STRING
#define MY_STRING

#include <stdio.h>
#include "stdlib.h"

char* lineRead();

char* lineFileRead(FILE* file);

char* inputString(FILE *fp, size_t size);

void stringPrint(char* str);

void writeString(char* str, FILE* file);

int getLen(char* str);

void linePrintReverse(char* str);

char* lineGetReverse(char* str);

void strSort(char* str, char (*comparator)(char a, char b));

char** strDiv(char* str, char div);

char compStr(char* str1, char* str2);

#endif