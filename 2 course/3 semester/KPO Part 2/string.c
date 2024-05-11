#include "mystring.h"

char* lineRead() {
    char* str = (char*) malloc(sizeof(char));
    char ch;
    int i = 0;
    while(ch = fgetc(stdin), ch != '\n') {
        str[i] = ch;
        i++;
        str = (char*) realloc(str, (i + 1)*sizeof(char));
    }
    str[i] = '\0';
    return str[0] == '\0' ? NULL : str;
}

char* lineFileRead(FILE* file) {
    char* str = (char*) malloc(sizeof(char));
    char ch;
    int i = 0;
    while(ch = fgetc(file), ch != '\n' && ch != EOF) {
        str[i] = ch;
        i++;
        str = (char*) realloc(str, (i + 1) * sizeof(char));
    }
    str[i] = '\0';
    return str[0] == '\0' ? NULL : str;
}

char* inputString(FILE *fp, size_t size) {
    char* str;
    int ch;
    size_t len = 0;
    str = (char*)realloc(NULL, sizeof(*str)*size);
    if(!str) return str;
    while(EOF!=(ch=fgetc(fp)) && ch != '\n') {
        str[len++] = ch;
        if(len==size) {
            str = (char*)realloc(str, sizeof(*str)*(size+=16));
            if(!str) return str;
        }
    }

    str[len++] = '\0';

    return (char*)realloc(str, sizeof(*str)*len);
}

void stringPrint(char* str) {
    printf("%s", str);
    printf("\n");
}

void writeString(char* str, FILE* file) {
    fputs(str, file);
    fputc('\n', file);
}

int getLen(char* str) {
    int len = 0;
    for(int i = 0; str[i]; i++)
        len++;
    return len;
}

void linePrintReverse(char* str) {
    for (int i = getLen(str) - 1; i > -1; i--)
        printf("%c", str[i]);
    printf("\n");
}

char* lineGetReverse(char* str) {
    char* strNew = (char*) malloc(getLen(str) * sizeof(char));
    for (int i = getLen(str) - 1, j = 0; i > -1; i--, j++)
        strNew[j] = str[i];
    strNew[getLen(str) + 1] = 0;
    return strNew;
}

void strSort(char* str, char (*comparator)(char a, char b)) {

    for(int i = 0; str[i + 1]; i++) {
        char minCharInd = i;
        for(int j = i + 1; str[j] != 0; j++) {
            if(comparator(str[j], str[minCharInd]) > 0) minCharInd = j;
        }

        if (minCharInd != i) {
            char temp = str[i];
            str[i] = str[minCharInd];
            str[minCharInd] = temp;
        }

    }
}

char** strDiv(char* str, char div) {
    char** result = (char**) malloc(2*sizeof(char*));
    result[0] = (char*) malloc(1*sizeof(char));
    int rowInd = 0, colInd = 0;
    for (int i = 0; str[i] != 0; i++) {
        if(str[i] == div) {
            result[rowInd][colInd] = 0;
            rowInd++;
            colInd = 0;
            result = (char**) realloc(result, (rowInd+1)*sizeof(char*));
            result[rowInd] = (char*) realloc(result[rowInd], 1*sizeof(char));
        } else {
            result[rowInd][colInd] = str[i];
            colInd++;
            result[rowInd] = (char*) realloc(result[rowInd], (colInd+2)*sizeof(char));
        }
    }

    result[rowInd][colInd] = 0;
    result[rowInd+1] = NULL;

    return result;
}

char compStr(char* str1, char* str2) {
    int i = 0;
    while (str1[i] != '\0' && str2[i] != '\0') {
        if (str1[i] < str2[i]) {
            return -1;
        } else if (str1[i] > str2[i]) {
            return 1;
        }
        i++;
    }

    if (str1[i] == '\0' && str2[i] == '\0') {
        return 0; // Строки эквивалентны
    } else if (str1[i] == '\0') {
        return -1; // Первая строка короче
    } else {
        return 1; // Вторая строка короче
    }
}