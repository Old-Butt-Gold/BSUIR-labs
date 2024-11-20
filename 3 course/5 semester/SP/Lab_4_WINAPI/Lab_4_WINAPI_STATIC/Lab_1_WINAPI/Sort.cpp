#include "Sort.h"

std::pair<std::wstring, std::wstring> SplitFileNameAndExtension(const std::wstring& fileName) {
    size_t pos = fileName.find_last_of(L'.');
    if (pos != std::wstring::npos) {
        return { fileName.substr(0, pos), fileName.substr(pos) };
    }
    return { fileName, L"" }; // Если расширения нет, возвращаем пустую строку для расширения
}

std::wstring GenerateUniqueFileName(const std::wstring& baseName, std::unordered_map<std::wstring, int>& gNameCount)
{
    auto item = SplitFileNameAndExtension(baseName);
    std::wstring uniqueName = item.first + item.second;
    int count = gNameCount[baseName];

    if (count > 0) {
        uniqueName = item.first + L"[" + std::to_wstring(count) + L"]" + item.second;
    }

    gNameCount[baseName]++;

    return uniqueName;
}

void LogError(std::wofstream& logFile, const std::wstring& message) {
    std::time_t currentTime = std::time(nullptr);
    std::tm localTime;
    localtime_s(&localTime, &currentTime);

    wchar_t timeBuffer[80];
    wcsftime(timeBuffer, sizeof(timeBuffer) / sizeof(wchar_t), L"%Y-%m-%d %H:%M:%S", &localTime);

    logFile << L"[" << timeBuffer << L"] " << message << std::endl;
}

void LoadFilesData(std::vector<FileData>& gFilesVector, const std::wstring directoryPath, std::vector<FileData>& files, std::unordered_map<std::wstring, int>& gNameCount,
    std::wofstream& errorLogFile)
{
    WIN32_FIND_DATA data;

    std::wstring searchPath = directoryPath + L"\\*";

    HANDLE hFind = FindFirstFile(searchPath.c_str(), &data);

    if (hFind == INVALID_HANDLE_VALUE) {
        DWORD errorCode = GetLastError();
        std::wstring errorMessage = L"Error when getting files from directory: " + directoryPath +
            L". Error Code: " + std::to_wstring(errorCode);
        LogError(errorLogFile, errorMessage);
        return;
    }

    do {
        if (data.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY) {
            if (wcscmp(data.cFileName, L".") != 0 && wcscmp(data.cFileName, L"..") != 0) {
                const std::wstring newDirPath = directoryPath + L"\\" + data.cFileName;
                LoadFilesData(gFilesVector, newDirPath, files, gNameCount, errorLogFile);
            }
        }
        else {
            FileData fileData;
            std::wstring baseName = data.cFileName;

            fileData.fileName = GenerateUniqueFileName(baseName, gNameCount);

            ULONGLONG fileSize = ((ULONGLONG)data.nFileSizeHigh << 32) + data.nFileSizeLow;
            fileData.fileSize = std::to_wstring(fileSize) + L" bytes";

            gFilesVector.push_back(fileData);
        }
    } while (FindNextFile(hFind, &data) != 0);

    FindClose(hFind);
}

void Swap(std::vector<FileData>& gFilesVector, int i, int j)
{
    auto temp = gFilesVector[i];
    gFilesVector[i] = gFilesVector[j];
    gFilesVector[j] = temp;
}

int Partition(std::vector<FileData>& gFilesVector, int left, int right, bool isAscending)
{
    auto pivot = gFilesVector[left].fileSize.c_str();
    int j = left;

    for (int i = left + 1; i <= right; i++)
    {
        if (isAscending) {
            if (wcscmp(gFilesVector[i].fileSize.c_str(), pivot) <= 0)
            {
                j++;
                Swap(gFilesVector, i, j);
            }
        }
        else {
            if (wcscmp(pivot, gFilesVector[i].fileSize.c_str()) <= 0)
            {
                j++;
                Swap(gFilesVector, i, j);
            }
        }
    }

    Swap(gFilesVector, left, j);
    return j;
}

void QSort(std::vector<FileData>& gFilesVector, int left, int right, bool isAscending)
{
    while (left < right)
    {
        int pivotIndex = Partition(gFilesVector, left, right, isAscending);
        if (pivotIndex - left <= right - pivotIndex)
        {
            QSort(gFilesVector, left, pivotIndex - 1, isAscending);
            left = pivotIndex + 1;
        }
        else
        {
            QSort(gFilesVector, pivotIndex + 1, right, isAscending);
            right = pivotIndex - 1;
        }
    }
}