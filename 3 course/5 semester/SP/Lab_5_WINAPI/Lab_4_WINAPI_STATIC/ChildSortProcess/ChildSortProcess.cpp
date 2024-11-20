#include <vector>
#include <iostream>
#include <string>
#include <windows.h>
#include "../Lab_1_WINAPI/Sort.h"

int columnIndex;

bool Compare(const FileData& a, const FileData& b) {
    switch (columnIndex) {
    case 0:  // Сортировка по имени файла
        return a.fileName < b.fileName;
    case 1:  // Сортировка по размеру файла
        return a.fileSize < b.fileSize;
    case 2:  // Сортировка по дате
        return a.fileDate < b.fileDate;
    default:
        return false;
    }
}

void Merge(std::vector<FileData>& data, int left, int mid, int right) {
    std::vector<FileData> temp(right - left + 1); // Временный массив
    int i = left, j = mid + 1, k = 0;

    while (i <= mid && j <= right) {
        if (Compare(data[i], data[j])) {
            temp[k++] = data[i++];
        } else {
            temp[k++] = data[j++];
        }
    }

    while (i <= mid) {
        temp[k++] = data[i++];
    }

    while (j <= right) {
        temp[k++] = data[j++];
    }

    for (k = 0; k < temp.size(); ++k) {
        data[left + k] = temp[k];
    }
}

void MergeSort(std::vector<FileData>& data, int left, int right) {
    if (left < right) {
        int mid = left + (right - left) / 2;

        MergeSort(data, left, mid);    
        MergeSort(data, mid + 1, right);

        Merge(data, left, mid, right);
    }
}

std::vector<FileData> ReadFromFile(const std::wstring& fileName) {
    std::wifstream file(fileName);
    std::vector<FileData> data;
    std::wstring line;

    while (std::getline(file, line)) {
        std::wstringstream ss(line);
        FileData entry;
        std::wstring token;
        
        if (std::getline(ss, token, L'|')) {
            entry.fileName = token;
        }
        if (std::getline(ss, token, L'|')) {
            entry.fileSize = token;
        }
        if (std::getline(ss, token, L'|')) {
            entry.fileDate = token;
        }
        data.push_back(entry);
    }

    file.close();
    return data;
}

int wmain(int argc, wchar_t* argv[])
{
    if (argc < 5) {
        std::wcout << "Usage: ChildSortProcess <start> <end> <fileName> <indexSort>" << std::endl;
        Sleep(1000);
        return 1;
    }

    int start = std::stoi(argv[1]);
    int end = std::stoi(argv[2]);
    std::wstring fileName = argv[3]; 
    columnIndex = std::stoi(argv[4]);
    
    std::vector<FileData> data = ReadFromFile(fileName);
    MergeSort(data, 0, data.size() - 1);
    
    std::wofstream outFile(fileName);
    for (int j = 0; j < data.size(); j++) {
        outFile << data[j].fileName << L"|" << data[j].fileSize << L"|" << data[j].fileDate << L"|" << "\n";
    }
    outFile.close();
    
    std::wcout << "Sorting is done between " << start << " and " << end ;
    Sleep(1000);
    
    return 0;
}