#include <stdio.h>
#include <fcntl.h>
#include <sys/stat.h>
#include <sys/types.h>
#include <dirent.h>
#include <sys/time.h>
#include <stdlib.h>
#include <string.h>

typedef struct stat STAT;
typedef struct dirent DIRENT;

int searchFiles(const char* catalogPath, int minSize, int maxSize, FILE* outputFile) {

    int totalFiles = 0;
    DIRENT* entry;
    DIR* currentDir = opendir(catalogPath);
    if (currentDir == NULL) {
        fprintf(outputFile, "Error opening directory %s\n\n", catalogPath);
        return totalFiles;
    }

    while (entry = readdir(currentDir)) {
        if (strcmp(entry->d_name, ".") != 0 && strcmp(entry->d_name, "..") != 0) {
            char* path = (char*)malloc(strlen(catalogPath) + NAME_MAX + 2);  // +2 для '/' и '\0'
            strcpy(path, catalogPath);
            strcat(path, "/");
            strcat(path, entry->d_name);

            STAT buf;
            if (stat(path, &buf) == -1) {
                fprintf(outputFile, "Error reading file information %s\n\n", path);
            }

            if (S_ISDIR(buf.st_mode) != 0)
                totalFiles += searchFiles(path, minSize, maxSize, outputFile);
            else if (S_ISREG(buf.st_mode)) {
                off_t fileSize = buf.st_size;
                if (fileSize >= minSize && fileSize <= maxSize) {
                    fprintf(outputFile, "Path: %s,\nName: %s,\nSize: %ld\n\n", path, entry->d_name, (long)fileSize);
                    totalFiles++;
                }
            }
            free(path);
        }
    }

    closedir(currentDir);
    return totalFiles;
}

int main(int argc, char* argv[]) {
    if (argc != 5) {
        fprintf(stderr, "Usage: %s <directory> <min_size> <max_size> <output_file>\n", argv[0]);
        return 1;
    }
    const char* catalogPath = argv[1];
    int minSize = atoi(argv[2]);
    int maxSize = atoi(argv[3]);
    const char* outputFilePath = argv[4];

    FILE *outputFile = fopen(outputFilePath, "w");
    if (outputFile == NULL) {
        fprintf(stderr, "Error opening outputFile");
        exit(1);
    }
    chmod(outputFilePath, 0666);
    printf("Total files found: %d\n", searchFiles(catalogPath, minSize, maxSize, outputFile));
    fclose(outputFile);
    return 0;
}