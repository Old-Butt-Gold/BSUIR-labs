#pragma once

#include <vector>
#include <unordered_map>
#include "string"
#include <iomanip>
#include <sstream>
#include <Windows.h>
#include "../TABLE_DLL/Constants.h"
#include <fstream>

struct FileData {
    std::wstring fileName;
    std::wstring fileSize;
    std::wstring fileDate;
};

std::pair<std::wstring, std::wstring> SplitFileNameAndExtension(const std::wstring& fileName);

std::wstring GenerateUniqueFileName(const std::wstring& baseName, std::unordered_map<std::wstring, int>& gNameCount);

void LoadFilesData(std::vector<FileData>& gFilesVector, std::wstring directoryPath, std::unordered_map<std::wstring, int>& gNameCount);

void Swap(std::vector<FileData>& gFilesVector, int i, int j);

int Partition(std::vector<FileData>& gFilesVector, int left, int right);

void QSort(std::vector<FileData>& gFilesVector, int left, int right);