#pragma once

#include <vector>
#include <unordered_map>
#include "string"
#include <Windows.h>
#include "../TABLE_DLL/Constants.h"
#include <fstream>

std::pair<std::wstring, std::wstring> SplitFileNameAndExtension(const std::wstring& fileName);

std::wstring GenerateUniqueFileName(const std::wstring& baseName, std::unordered_map<std::wstring, int>& gNameCount);

void LogError(std::wofstream& logFile, const std::wstring& message);

void LoadFilesData(std::vector<FileData>& gFilesVector, const std::wstring directoryPath, std::vector<FileData>& files, std::unordered_map<std::wstring, int>& gNameCount,
    std::wofstream& errorLogFile);

void Swap(std::vector<FileData>& gFilesVector, int i, int j);

int Partition(std::vector<FileData>& gFilesVector, int left, int right, bool isAscending);

void QSort(std::vector<FileData>& gFilesVector, int left, int right, bool isAscending);