#ifndef UNICODE
#define UNICODE
#endif 

#include "Sort.h"
#include "../TABLE_DLL/Table.h"
#include "../TABLE_DLL/Constants.h"
#include "../TABLE_DLL/Fonts.h"

#include <Windows.h>
#include <iomanip>
#include <sstream>
#include <CommCtrl.h>
#include <windowsx.h>
#include <wingdi.h>
#include <codecvt>

//g for global
std::vector<std::wstring> gFontPaths;

FontSettings gFontSettings = {
    //file Names
    DEFAULT_FONT, DEFAULT_FONT, DEFAULT_FONT_SIZE, nullptr,

    //file Headers
    DEFAULT_FONT, DEFAULT_FONT, DEFAULT_FONT_SIZE, nullptr,

    //file Sizes
    DEFAULT_FONT, DEFAULT_FONT, DEFAULT_FONT_SIZE, nullptr,
};

Table* hCustomListView = nullptr;
HWND hFontChildWindow = NULL;
HWND hMainHandle;

std::wstring GetFileNameWithoutExtension(const std::wstring& filePath) 
{
    size_t lastSlashPos = filePath.find_last_of(L"/\\");
    std::wstring fileName = (lastSlashPos == std::wstring::npos) ? filePath : filePath.substr(lastSlashPos + 1);

    size_t lastDotPos = fileName.find_last_of(L'.');

    return (lastDotPos == std::wstring::npos) ? fileName : fileName.substr(0, lastDotPos);
}

void PopulateFontsComboBox(HWND hComboBox, const std::vector<std::wstring> fontPaths) 
{
    for (const auto& font : fontPaths) 
    {
        auto fontName = GetFileNameWithoutExtension(font);
        SendMessage(hComboBox, CB_ADDSTRING, 0, (LPARAM)fontName.c_str());
    }
}

std::wstring SearchForFont(std::vector<std::wstring> fontPaths, std::wstring fontFamily) {
    std::wstring result = DEFAULT_FONT;
    for (const auto& font : gFontPaths) 
    {
        auto fontName = GetFileNameWithoutExtension(font);
        if (fontName == fontFamily) {
            return font;
        }
    }
    return result;
}

void HandleComboBoxChange(HWND hComboBox) {
    int index = SendMessage(hComboBox, CB_GETCURSEL, 0, 0);

    wchar_t buffer[256] = { 0 };
    SendMessage(hComboBox, CB_GETLBTEXT, index, (LPARAM)buffer);
    std::wstring text(buffer);

    HMENU hMenu = GetMenu(hComboBox);

    if ((HMENU)ID_COMBOBOX_HEADER == hMenu) {
        gFontSettings.headerFontFamily = text;
        gFontSettings.headerFontPath = SearchForFont(gFontPaths, text);
        DeleteFont(gFontSettings.headerFontHandle);

        gFontSettings.headerFontHandle = CreateFontWithParams(gFontSettings.headerFontPath, gFontSettings.headerFontFamily, gFontSettings.headerFontSize);

        SendMessage(hCustomListView->GetHWND(), WM_SETFONT, (WPARAM)gFontSettings.headerFontHandle, (LPARAM)ID_COMBOBOX_HEADER);
    }

    else if ((HMENU)ID_COMBOBOX_HEADER_SIZES == hMenu) {
        gFontSettings.headerFontSize = _wtoi(text.c_str());
        DeleteFont(gFontSettings.headerFontHandle);
        
        gFontSettings.headerFontHandle = CreateFontWithParams(gFontSettings.headerFontPath, gFontSettings.headerFontFamily, gFontSettings.headerFontSize);
        SendMessage(hCustomListView->GetHWND(), WM_SETFONT, (WPARAM)gFontSettings.headerFontHandle, (LPARAM)ID_COMBOBOX_HEADER_SIZES);
    }
    else if ((HMENU)ID_COMBOBOX_FILENAMES == hMenu) {
        gFontSettings.fileNameFontFamily = text;

        gFontSettings.fileNameFontPath = SearchForFont(gFontPaths, text);
        DeleteFont(gFontSettings.fileNameFontHandle);

        gFontSettings.fileNameFontHandle = CreateFontWithParams(gFontSettings.fileNameFontPath, gFontSettings.fileNameFontFamily, gFontSettings.fileNameFontSize);

        SendMessage(hCustomListView->GetHWND(), WM_SETFONT, (WPARAM)gFontSettings.fileNameFontHandle, (LPARAM)ID_COMBOBOX_FILENAMES);
    }
    else if ((HMENU)ID_COMBOBOX_FILENAMES_SIZES == hMenu) {
        gFontSettings.fileNameFontSize = _wtoi(text.c_str());
        DeleteFont(gFontSettings.fileNameFontHandle);
        
        gFontSettings.fileNameFontHandle = CreateFontWithParams(gFontSettings.fileNameFontFamily, gFontSettings.fileNameFontFamily, gFontSettings.fileNameFontSize);
        SendMessage(hCustomListView->GetHWND(), WM_SETFONT, (WPARAM)gFontSettings.fileNameFontHandle, (LPARAM)ID_COMBOBOX_FILENAMES_SIZES);
    }
    else if ((HMENU)ID_COMBOBOX_FILESIZES == hMenu) {
        gFontSettings.fileSizeFontFamily = text;

        gFontSettings.fileSizeFontPath = SearchForFont(gFontPaths, text);
        DeleteFont(gFontSettings.fileSizeFontHandle);

        gFontSettings.fileSizeFontHandle = CreateFontWithParams(gFontSettings.fileSizeFontPath, gFontSettings.fileSizeFontFamily, gFontSettings.fileSizeFontSize);
        SendMessage(hCustomListView->GetHWND(), WM_SETFONT, (WPARAM)gFontSettings.fileSizeFontHandle, (LPARAM)ID_COMBOBOX_FILESIZES);
    }
    else if ((HMENU)ID_COMBOBOX_FILESIZES_SIZES == hMenu) {
        gFontSettings.fileSizeFontSize = _wtoi(text.c_str());
        DeleteFont(gFontSettings.fileSizeFontHandle);
        gFontSettings.fileSizeFontHandle = CreateFontWithParams(gFontSettings.fileSizeFontPath, gFontSettings.fileSizeFontFamily, gFontSettings.fileSizeFontSize);
        
        SendMessage(hCustomListView->GetHWND(), WM_SETFONT, (WPARAM)gFontSettings.fileSizeFontHandle, (LPARAM)ID_COMBOBOX_FILESIZES_SIZES);
    }

    InvalidateRect(hCustomListView->GetHWND(), NULL, TRUE);
}

std::vector<std::wstring> GetFontsFromFolder(const std::wstring& folderPath) {
    std::vector<std::wstring> fonts = { };
    WIN32_FIND_DATA findFileData;
    HANDLE hFind;

    if (folderPath.empty()) {
        return fonts;
    }

    std::wstring folder = folderPath;

    if (folder.back() != L'\\' && folder.back() != L'/') {
        folder += L'\\';
    }

    std::wstring searchPatterns[] = { L"*.ttf", L"*.otf" };

    for (const auto& pattern : searchPatterns) {
        hFind = FindFirstFile((folder + pattern).c_str(), &findFileData);

        if (hFind == INVALID_HANDLE_VALUE) {
            continue;
        }

        do {
            if (!(findFileData.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY)) {
                fonts.push_back(folder + findFileData.cFileName);
            }
        } while (FindNextFile(hFind, &findFileData) != 0);

        FindClose(hFind);
    }

    return fonts;
}

WNDPROC oldFontEditProc;

LRESULT CALLBACK FontEditProc(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam)
{
    switch (msg)
    {
    case WM_KEYDOWN:
        switch (wParam)
        {
            case VK_RETURN: 
            {
                wchar_t directoryPath[MAX_PATH] = { 0 };
                GetWindowTextW(hwnd, directoryPath, MAX_PATH);

                std::wstring dirPath(directoryPath);

                auto fontsPaths = GetFontsFromFolder(dirPath);

                std::vector<std::wstring> filterPaths = { };

                for (const auto& font : fontsPaths)
                {
                    bool wasTaken = false;
                    const std::wstring fontFamily = GetFileNameWithoutExtension(font);

                    for (const auto& fontOld : gFontPaths) {
                        const std::wstring fontOldFamily = GetFileNameWithoutExtension(fontOld);

                        if (fontFamily == fontOldFamily) {
                            wasTaken = true;
                        }

                        if (wasTaken) break;
                    }

                    if (wasTaken) continue;

                    gFontPaths.push_back(font);
                    filterPaths.push_back(font);
                }  

                MessageBox(hwnd, (L"Amount of loaded Fonts: " + std::to_wstring(filterPaths.size())).c_str(), L"Attention", MB_OK);

                HWND hEdit = GetDlgItem(hFontChildWindow, ID_FONTS_LOAD_EDIT); // ???????? ID_EDIT ?? ??? ID
                SetFocus(hEdit); // ?????????? ????? ?? hEdit

                if (filterPaths.empty()) {
                    return 0;
                }

                HWND hHeaderCB = GetDlgItem(hFontChildWindow, ID_COMBOBOX_HEADER);
                PopulateFontsComboBox(hHeaderCB, filterPaths);

                HWND hFileNameCB = GetDlgItem(hFontChildWindow, ID_COMBOBOX_FILENAMES);
                PopulateFontsComboBox(hFileNameCB, filterPaths);

                HWND hFileSizeCB = GetDlgItem(hFontChildWindow, ID_COMBOBOX_FILESIZES);
                PopulateFontsComboBox(hFileSizeCB, filterPaths);
            }
            return 0;
        default: ;
        }
        break;

    default:
        return CallWindowProc(oldFontEditProc, hwnd, msg, wParam, lParam);
    }
    return 0;
}

LRESULT CALLBACK ChildWindowProc(HWND hwnd, UINT uMsg, WPARAM wParam, LPARAM lParam) 
{
    static HWND hComboHeader, hComboHeaderSizes, hComboFileNames, hComboFileNamesSizes, hComboFileSizes, hComboFileSizesSizes;
    static HWND h_edit;

    switch (uMsg)
    {
    case WM_PAINT:
    {
        PAINTSTRUCT ps;
        HDC hdc = BeginPaint(hwnd, &ps);

        FillRect(hdc, &ps.rcPaint, (HBRUSH)CreateSolidBrush(RGB(114, 188, 219)));
        EndPaint(hwnd, &ps);
    }
    return 0;

    case WM_CREATE:
    {
        h_edit = CreateWindowEx(0, WC_EDIT, L"",
            WS_CHILD | WS_VISIBLE | WS_DLGFRAME,
            10, 10, 350, 40,
            hwnd, (HMENU)ID_FONTS_LOAD_EDIT, NULL, NULL);

        oldFontEditProc = (WNDPROC)SetWindowLongPtr(h_edit, GWLP_WNDPROC, (LONG_PTR)FontEditProc);

        CreateWindowEx(0, WC_STATIC, L"Header:", WS_CHILD | WS_VISIBLE,
            10, 90, 100, 20, hwnd, NULL, NULL, NULL);
        hComboHeader = CreateWindowEx(0, WC_COMBOBOX, NULL, WS_CHILD | WS_VISIBLE | CBS_DROPDOWNLIST | WS_VSCROLL,
            120, 90, 150, 120, hwnd, (HMENU)ID_COMBOBOX_HEADER, NULL, NULL);
        SendMessage(hComboHeader, CB_ADDSTRING, 0, (LPARAM)DEFAULT_FONT);
        PopulateFontsComboBox(hComboHeader, gFontPaths);

        hComboHeaderSizes = CreateWindowEx(0, WC_COMBOBOX, NULL, WS_CHILD | WS_VISIBLE | CBS_DROPDOWNLIST | WS_VSCROLL,
            280, 90, 80, 120, hwnd, (HMENU)ID_COMBOBOX_HEADER_SIZES, NULL, NULL);
        for (int i = 1; i <= COMBOBOX_FONT_SIZES_COUNT; ++i) {
            wchar_t buffer[3];
            swprintf(buffer, sizeof(buffer) / sizeof(wchar_t), L"%d", i);
            SendMessage(hComboHeaderSizes, CB_ADDSTRING, 0, (LPARAM)buffer);
        }

        CreateWindowEx(0, WC_STATIC, L"File names:", WS_CHILD | WS_VISIBLE,
            10, 145, 100, 20, hwnd, NULL, NULL, NULL);
        hComboFileNames = CreateWindowEx(0, WC_COMBOBOX, NULL, WS_CHILD | WS_VISIBLE | CBS_DROPDOWNLIST | WS_VSCROLL,
            120, 145, 150, 120, hwnd, (HMENU)ID_COMBOBOX_FILENAMES, NULL, NULL);
        SendMessage(hComboFileNames, CB_ADDSTRING, 0, (LPARAM)DEFAULT_FONT);
        PopulateFontsComboBox(hComboFileNames, gFontPaths);

        hComboFileNamesSizes = CreateWindowEx(0, WC_COMBOBOX, NULL, WS_CHILD | WS_VISIBLE | CBS_DROPDOWNLIST | WS_VSCROLL,
            280, 145, 80, 120, hwnd, (HMENU)ID_COMBOBOX_FILENAMES_SIZES, NULL, NULL);
        for (int i = 1; i <= COMBOBOX_FONT_SIZES_COUNT; ++i) {
            wchar_t buffer[3];
            swprintf(buffer, sizeof(buffer) / sizeof(wchar_t), L"%d", i);
            SendMessage(hComboFileNamesSizes, CB_ADDSTRING, 0, (LPARAM)buffer);
        }
        SendMessage(hComboFileNamesSizes, CB_SETMINVISIBLE, (WPARAM)6, 0);

        CreateWindowEx(0, WC_STATIC, L"File Sizes:", WS_CHILD | WS_VISIBLE,
            10, 190, 100, 20, hwnd, NULL, NULL, NULL);
        hComboFileSizes = CreateWindowEx(0, WC_COMBOBOX, NULL, WS_CHILD | WS_VISIBLE | CBS_DROPDOWNLIST | WS_VSCROLL,
            120, 190, 150, 120, hwnd, (HMENU)ID_COMBOBOX_FILESIZES, NULL, NULL);
        SendMessage(hComboFileSizes, CB_ADDSTRING, 0, (LPARAM)DEFAULT_FONT);
        PopulateFontsComboBox(hComboFileSizes, gFontPaths);

        hComboFileSizesSizes = CreateWindowEx(0, WC_COMBOBOX, NULL, WS_CHILD | WS_VISIBLE | CBS_DROPDOWNLIST | WS_VSCROLL,
            280, 190, 80, 120, hwnd, (HMENU)ID_COMBOBOX_FILESIZES_SIZES, NULL, NULL);
        for (int i = 1; i <= COMBOBOX_FONT_SIZES_COUNT; ++i) {
            wchar_t buffer[3];
            swprintf(buffer, sizeof(buffer) / sizeof(wchar_t), L"%d", i);
            SendMessage(hComboFileSizesSizes, CB_ADDSTRING, 0, (LPARAM)buffer);
        }

        // Set default selections based on gFontSettings

    // Set default font for headers
        int fontIndex = SendMessage(hComboHeader, CB_FINDSTRINGEXACT, -1, (LPARAM)gFontSettings.headerFontFamily.c_str());
        if (fontIndex != CB_ERR) {
            SendMessage(hComboHeader, CB_SETCURSEL, fontIndex, 0);
        }
        SendMessage(hComboHeaderSizes, CB_SETCURSEL, gFontSettings.headerFontSize - 1, 0);

        // Set default font for file names
        fontIndex = SendMessage(hComboFileNames, CB_FINDSTRINGEXACT, -1, (LPARAM)gFontSettings.fileNameFontFamily.c_str());
        if (fontIndex != CB_ERR) {
            SendMessage(hComboFileNames, CB_SETCURSEL, fontIndex, 0);
        }
        SendMessage(hComboFileNamesSizes, CB_SETCURSEL, gFontSettings.fileNameFontSize - 1, 0);

        // Set default font for file sizes
        fontIndex = SendMessage(hComboFileSizes, CB_FINDSTRINGEXACT, -1, (LPARAM)gFontSettings.fileSizeFontFamily.c_str());
        if (fontIndex != CB_ERR) {
            SendMessage(hComboFileSizes, CB_SETCURSEL, fontIndex, 0);
        }
        SendMessage(hComboFileSizesSizes, CB_SETCURSEL, gFontSettings.fileSizeFontSize - 1, 0);

        CreateWindowEx(0, WC_BUTTON, L"OK", WS_CHILD | WS_VISIBLE | BS_DEFPUSHBUTTON,
            280, 230, 80, 30, hwnd, (HMENU)IDOK, NULL, NULL);
    }
    break;

    case WM_COMMAND: 
    {
        int wmId = LOWORD(wParam);
        
        int wmEvent = HIWORD(wParam);
        //caused when combobox is changed
        if (wmEvent == CBN_SELCHANGE)
        {
            HWND hCombo = (HWND)lParam;
            if (hCombo) {
                HandleComboBoxChange(hCombo);
            }
        }

        if (wmId == IDOK) {
            SetFocus(hMainHandle);
            SendMessage(hwnd, WM_CLOSE, 0, 0);
        }
    }
    break;

    case WM_CLOSE:
        hFontChildWindow = NULL;
        DestroyWindow(hwnd); 
        break;

    default:
        return DefWindowProc(hwnd, uMsg, wParam, lParam);
    }
    return 0;
}

std::vector<FileData> PrepareListView(HWND parent, std::wstring directory)
{
    std::vector<FileData> result = { };
    WIN32_FIND_DATA data;

    HANDLE hFind = FindFirstFile((directory + L"\\*").c_str(), &data);

    if (hFind == INVALID_HANDLE_VALUE || directory == L"\\*") {
        MessageBox(parent, L"Enter existing directory path!", L"Error", MB_ICONERROR);
        return result;
    }

    std::unordered_map<std::wstring, int> gNameCount = { };

    LoadFilesData(result, directory, gNameCount);
    return result;
}

std::vector<FileData> ReadFromFile(const std::wstring& fileName) {
    std::wifstream file(fileName);
    file.imbue(std::locale(file.getloc(), new std::codecvt_utf8<wchar_t>));  // Устанавливаем UTF-8
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

bool Compare(const FileData& a, const FileData& b, int columnIndex) {
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

std::vector<FileData> MergeTwoSortedLists(const std::vector<FileData>& list1, const std::vector<FileData>& list2, int columnIndex) 
{
    std::vector<FileData> merged;
    size_t i = 0, j = 0;

    while (i < list1.size() && j < list2.size()) {
        if (Compare(list1[i], list2[j], columnIndex)) {
            merged.push_back(list1[i++]);
        } else {
            merged.push_back(list2[j++]);
        }
    }

    while (i < list1.size()) {
        merged.push_back(list1[i++]);
    }

    while (j < list2.size()) {
        merged.push_back(list2[j++]);
    }

    return merged;
}

std::vector<FileData> MergeKSortedLists(const std::vector<std::vector<FileData>>& sortedParts, int columnIndex) {
    if (sortedParts.empty()) return {};

    std::vector<std::vector<FileData>> lists = sortedParts;
    int interval = 1;
    int length = static_cast<int>(lists.size());

    while (interval < length) {
        for (int i = 0; i < length - interval; i += interval * 2) {
            lists[i] = MergeTwoSortedLists(lists[i], lists[i + interval], columnIndex);
        }
        interval *= 2;
    }

    return lists[0];
}

void CreateSortProcess(const std::wstring& processName, const std::wstring fileName, int start, int end, int columnIndex, PROCESS_INFORMATION& pi) {
    std::wstring params = L" " + std::to_wstring(start) + L" " + std::to_wstring(end) + L" " + fileName + L" " + std::to_wstring(columnIndex);
    
    STARTUPINFO si;
    ZeroMemory(&si, sizeof(si));
    si.cb = sizeof(si);
    ZeroMemory(&pi, sizeof(pi));

    if (!CreateProcess(processName.c_str(), &params[0], NULL, NULL, FALSE, 0, NULL, NULL, &si, &pi)) {
        MessageBox(NULL, (L"CreateProcess failed (" + std::to_wstring(GetLastError()) + L").").c_str(), L"", MB_OK);
    }

    // Создание процесса
    /*if (!CreateProcess(processName.c_str(),   // Имя исполняемого файла
                       &params[0],            // Аргументы
                       NULL,                  // Атрибуты процесса
                       NULL,                  // Атрибуты потока
                       FALSE,                 // Наследование дескрипторов
                       0,                     // Флаги создания
                       NULL,                  // Указатель на переменные окружения
                       NULL,                  // Текущий каталог
                       &si,                   // Указатель на STARTUPINFO
                       &pi))                  // Указатель на PROCESS_INFORMATION
                       */
}

std::vector<FileData> SortInParallelProcesses(std::vector<FileData> unsortedData, int columnIndex, const int K)
{
    int dataSize = static_cast<int>(unsortedData.size());
    int partSize = dataSize / K;

    std::wstring processName = L"C:\\Users\\Lenovo\\source\\repos\\ChildSortProcess.exe";
    std::vector<std::wstring> fileNames(K);
    
    std::vector<PROCESS_INFORMATION> processInfos(K);

    for (int i = 0; i < K; i++) {
        fileNames[i] = L"C:\\Users\\Lenovo\\source\\repos\\temp_" + std::to_wstring(i) + L".txt";
        
        std::wofstream outFile(fileNames[i]);
        outFile.imbue(std::locale(outFile.getloc(), new std::codecvt_utf8<wchar_t>));
        int start = i * partSize;
        int end = (i == K - 1) ? dataSize : (start + partSize);
        for (int j = start; j < end; ++j) {
            outFile << unsortedData[j].fileName << L"|" << unsortedData[j].fileSize << L"|" << unsortedData[j].fileDate << L"|" << "\n";
        }
        outFile.close();

        CreateSortProcess(processName, fileNames[i], start, end - 1, columnIndex, processInfos[i]);
    }

    for (auto& pi : processInfos) {
        WaitForSingleObject(pi.hProcess, INFINITE);
        CloseHandle(pi.hProcess);
        CloseHandle(pi.hThread);
    }

    std::vector<std::vector<FileData>> sortedParts(K);
    for (int i = 0; i < K; i++) {
        sortedParts[i] = ReadFromFile(fileNames[i]);
        _wremove(fileNames[i].c_str());
    }

    return MergeKSortedLists(sortedParts, columnIndex);
}

WNDPROC oldEditProc;

LRESULT CALLBACK SubEditProc(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam)
{
    switch (msg)
    {
        case WM_KEYDOWN:
            switch (wParam)
            {
                /*case VK_RETURN:
                    PrepareListView(hCustomListView, hwnd);
                    SortListView(hCustomListView, true);
                    return 0;

                case VK_F1:
                {
                    PrepareListView(hCustomListView, hwnd);
                    SortListView(hCustomListView, false);
                    return 0;
                }*/
                default: ;
            }
            break;

        default:
            return CallWindowProc(oldEditProc, hwnd, msg, wParam, lParam);
        }
    return 0;
}

std::wstring GetCurrentTimeNew() {
    SYSTEMTIME st;
    GetLocalTime(&st);
    std::wstringstream wss;
    wss << std::setfill(L'0') << std::setw(2) << st.wHour << L":"
        << std::setfill(L'0') << std::setw(2) << st.wMinute << L":"
        << std::setfill(L'0') << std::setw(2) << st.wSecond << L"."
        << std::setfill(L'0') << std::setw(3) << st.wMilliseconds;

    return wss.str();
}

constexpr int K = 4;  // Количество процессов (должно быть степенью двойки).

LRESULT CALLBACK WindowProc(HWND hwnd, UINT uMsg, WPARAM wParam, LPARAM lParam)
{
    static HWND hFontButton;
    static HWND hEdit;

    static std::vector<FileData> unsortedData;

    switch (uMsg)
    {
    case WM_PAINT:
    {
        PAINTSTRUCT ps;
        HDC hdc = BeginPaint(hwnd, &ps);

        FillRect(hdc, &ps.rcPaint, (HBRUSH)(COLOR_WINDOW + 1));
        EndPaint(hwnd, &ps);
    }
    return 0;
    
    case WM_SIZE:
        {
            SendMessage(hCustomListView->GetHWND(), WM_SIZE, wParam, lParam);
        }
        return 0;
    
    case WM_CREATE:
    {
        gFontPaths = GetFontsFromFolder(FONTS_DIRECTORY);

        HFONT hHeaderFont, hCellFont;

        /*hFontButton = CreateWindowEx(0, L"BUTTON", L"Font Settings",
            WS_CHILD | WS_VISIBLE | BS_PUSHBOX | BS_OWNERDRAW,
            1, SCREEN_HEIGHT - 28, 200, 25, hwnd, (HMENU)ID_SELECT_FONT_BTN, NULL, NULL);*/

        hHeaderFont = nullptr;
        hCellFont = nullptr;

        Table::RegisterCustomListClass((HINSTANCE)GetWindowLongPtr(hwnd, GWLP_HINSTANCE), WC_CUSTOM_LIST_VIEW);
        std::wstring directory = L"D:\\Зачет по кпо";
        unsortedData = PrepareListView(hwnd, directory);
            
        hCustomListView = new Table(hwnd, unsortedData.size(), CUSTOM_LIST_VIEW_COL_COUNT, hHeaderFont, hCellFont);
        hCustomListView->Show();

        hCustomListView->AddColumn(0, L"Имя файла");
        hCustomListView->AddColumn(3, L"Имя файла");
        hCustomListView->AddColumn(1, L"Размер файла");
        hCustomListView->AddColumn(4, L"Размер файла");
        hCustomListView->AddColumn(2, L"Дата создания файла");
        hCustomListView->AddColumn(5, L"Дата создания файла");

        SendMessage(hCustomListView->GetHWND(), WM_SETREDRAW, FALSE, 0);
    
        int size = static_cast<int>(unsortedData.size());

        hCustomListView->AddItem(0, 0, L"Всего: ");
        hCustomListView->AddItem(0, 1, std::to_wstring(size));    
        for (int i = 0; i < size; i++) {
            int row = i + 1;
            hCustomListView->AddItem(row, 0, unsortedData[i].fileName);
            hCustomListView->AddItem(row, 3, unsortedData[i].fileName);
            hCustomListView->AddItem(row, 1, unsortedData[i].fileSize);
            hCustomListView->AddItem(row, 4, unsortedData[i].fileSize);
            hCustomListView->AddItem(row, 2, unsortedData[i].fileDate);
            hCustomListView->AddItem(row, 5, unsortedData[i].fileDate);
        }
    
        SendMessage(hCustomListView->GetHWND(), WM_SETREDRAW, TRUE, 0);
        InvalidateRect(hCustomListView->GetHWND(), NULL, TRUE);

        /*hEdit = CreateWindowEx(0, WC_EDIT, L"",
            WS_CHILD | WS_VISIBLE | WS_DLGFRAME | ES_AUTOHSCROLL,
            hEditX, hEditY, SCREEN_WIDTH / CUSTOM_LIST_VIEW_COL_COUNT - 2, COLUMN_HEADER_HEIGHT - 5,
            hCustomListView->GetHWND(), (HMENU)ID_ENTER_BTN_CLICKED, NULL, NULL);*/

        //oldEditProc = (WNDPROC)SetWindowLongPtr(hEdit, GWLP_WNDPROC, (LONG_PTR)SubEditProc);

        WNDCLASS wc = { 0 };
        wc.lpfnWndProc = ChildWindowProc;
        wc.hInstance = (HINSTANCE)GetWindowLongPtr(hwnd, GWLP_HINSTANCE);
        wc.lpszClassName = FontWindowClass;
        RegisterClass(&wc);
    }
    break;

    case WM_HEADER_CLICK: 
        {
            int columnIndex = wParam; 

            std::wstring message = L"WM_HEADER_CLICK - Column: " + std::to_wstring(columnIndex) +
                L"\nTime: " + GetCurrentTimeNew();
            MessageBox(hwnd, message.c_str(), L"Header Click", MB_OK);

            if (columnIndex != 0 && columnIndex != 1 && columnIndex != 2)
            {
                break;
            }

            auto sortedData = SortInParallelProcesses(unsortedData, columnIndex, K);
            SendMessage(hCustomListView->GetHWND(), WM_SETREDRAW, FALSE, 0);

            auto size = static_cast<int>(sortedData.size());
            hCustomListView->AddItem(0, 3, L"Всего: ");
            hCustomListView->AddItem(0, 4, std::to_wstring(size));    
            for (auto i = 0; i < size; i++) {
                int row = i + 1;
                hCustomListView->AddItem(row, 3, sortedData[i].fileName);
                hCustomListView->AddItem(row, 4, sortedData[i].fileSize);
                hCustomListView->AddItem(row, 5, sortedData[i].fileDate);
            }

            SendMessage(hCustomListView->GetHWND(), WM_SETREDRAW, TRUE, 0);
            InvalidateRect(hCustomListView->GetHWND(), NULL, TRUE);
    }
    break;

    case WM_DRAWITEM:
    {
        LPDRAWITEMSTRUCT lpDrawItem = (LPDRAWITEMSTRUCT)lParam;
        if (lpDrawItem->CtlID == ID_SELECT_FONT_BTN) {
            HDC hdc = lpDrawItem->hDC;

            // ?????? ???
            HBRUSH hBrush = CreateSolidBrush(RGB(30, 144, 255)); // ???? ????
            FillRect(hdc, &lpDrawItem->rcItem, hBrush);
            DeleteObject(hBrush);

            // ?????? ?????
            SetTextColor(hdc, RGB(255, 255, 255)); // ????? ???? ??????
            SetBkMode(hdc, TRANSPARENT);

            RECT textRect = lpDrawItem->rcItem;
            DrawText(hdc, L"Font Settings", -1, &textRect, DT_CENTER | DT_VCENTER | DT_SINGLELINE);

            // ???? ?????? ??????
            if (lpDrawItem->itemState & ODS_SELECTED) {
                FrameRect(hdc, &lpDrawItem->rcItem, (HBRUSH)GetStockObject(BLACK_BRUSH));
            }

            return TRUE;
        }
        break;
    }

    case WM_KEYDOWN:
    {
        int wmId = LOWORD(wParam);
        int wmEvent = HIWORD(wParam);
        
        /*if (wParam == VK_RETURN) {
            PrepareListView(hCustomListView, hEdit);
            SortListView(hCustomListView, true);
            return 0;
        }

        if (wParam == VK_F1) {
            PrepareListView(hCustomListView, hEdit);
            SortListView(hCustomListView, false);
            return 0;
        }*/
    }
    break;

    case WM_COMMAND:
    {
        int wmId = LOWORD(wParam);
        int wmEvent = HIWORD(wParam);

        switch (wmId) {
            case ID_SELECT_FONT_BTN: 
            {
                if (hFontChildWindow == NULL) {
                    const int childWidth = 400;
                    const int childHeight = 300;

                    int screenWidth = SCREEN_WIDTH;
                    int screenHeight = SCREEN_HEIGHT;

                    int posX = (screenWidth - childWidth) / 2;
                    int posY = (screenHeight - childHeight) / 2;

                    hFontChildWindow = CreateWindowEx(
                        0, FontWindowClass, L"Font Window", WS_CAPTION | WS_SYSMENU | WS_VISIBLE,
                        posX, posY, childWidth, childHeight, hwnd, NULL, (HINSTANCE)GetWindowLongPtr(hwnd, GWLP_HINSTANCE), NULL);
                }
            }
            break;
        }
    }
    break;

    case WM_CLOSE:
        if (MessageBox(hwnd, L"Really quit?", L"My application", MB_OKCANCEL) == IDOK) 
        {
            DestroyWindow(hwnd);
        }
        break;

    case WM_DESTROY:
    {
        UnloadCustomFonts();
        DeleteFont(gFontSettings.fileNameFontHandle);
        DeleteFont(gFontSettings.headerFontHandle);
        DeleteFont(gFontSettings.fileSizeFontHandle);
        delete hCustomListView;

        PostQuitMessage(0);
    }
    return 0;
        
    case WM_CELL_CLICK: 
    {
        int itemIndex = HIWORD(wParam);
        int columnIndex = LOWORD(wParam);

        std::wstring message = L"WM_CELL_CLICK - Item: " + std::to_wstring(itemIndex) +
            L", Column: " + std::to_wstring(columnIndex) +
            L"\nTime: " + GetCurrentTimeNew();
        MessageBox(NULL, message.c_str(), L"Cell Click", MB_OK | MB_TASKMODAL);
    }
    break;

    case WM_CELL_RCLICK: 
    {
        int itemIndex = HIWORD(wParam);
        int columnIndex = LOWORD(wParam);

        std::wstring message = L"WM_CELL_RCLICK Clicked - Item: " + std::to_wstring(itemIndex) +
            L", Column: " + std::to_wstring(columnIndex) +
            L"\nTime: " + GetCurrentTimeNew();
        MessageBox(NULL, message.c_str(), L"Cell Right Click", MB_OK | MB_TASKMODAL);
    }
    break;

    case WM_CELL_DBLCLICK: 
    {
        int itemIndex = HIWORD(wParam);
        int columnIndex = LOWORD(wParam);

        std::wstring message = L"WM_CELL_DBLCLICK Clicked - Item: " + std::to_wstring(itemIndex) +
            L", Column: " + std::to_wstring(columnIndex) +
            L"\nTime: " + GetCurrentTimeNew();
        MessageBox(NULL, message.c_str(), L"Cell Double Click", MB_OK | MB_TASKMODAL);
    }
    break;

    case WM_CELL_RDBLCLICK: 
    {
        int itemIndex = HIWORD(wParam);
        int columnIndex = LOWORD(wParam);

        std::wstring message = L"WM_CELL_RDBLCLICK Clicked - Item: " + std::to_wstring(itemIndex) +
            L", Column: " + std::to_wstring(columnIndex) +
            L"\nTime: " + GetCurrentTimeNew();
        MessageBox(NULL, message.c_str(), L"Cell Right Double Click", MB_OK | MB_TASKMODAL);
    }
    break;

    case WM_HEADER_RCLICK: 
    {
        int columnIndex = wParam; // ?????? ???????

        std::wstring message = L"WM_HEADER_RCLICK - Column: " + std::to_wstring(columnIndex) +
            L"\nTime: " + GetCurrentTimeNew();
        MessageBox(NULL, message.c_str(), L"Header Right Click", MB_OK | MB_TASKMODAL);
    }
    break;

    case WM_COLUMN_RESIZE: 
    {
        int resizedColumn = HIWORD(wParam);
        int newWidth = LOWORD(wParam);

        std::wstring message = L"WM_LISTVIEW_RESIZE by column: " + std::to_wstring(resizedColumn) +
            L", New Width: " + std::to_wstring(newWidth) +
            L"\nTime: " + GetCurrentTimeNew();
        MessageBox(NULL, message.c_str(), L"Warning", MB_OK | MB_TASKMODAL);
    }
    break;

    case WM_LISTVIEW_SCROLL:
    {
        int scrollPos = wParam;

        std::wstring message = L"WM_LISTVIEW_SCROLL - Scroll Position: " + std::to_wstring(scrollPos) +
            L"\nTime: " + GetCurrentTimeNew();
        //MessageBox(NULL, message.c_str(), L"Scroll Info", MB_OK | MB_TASKMODAL);
    }
    break;

    case WM_HEADER_DBLCLICK:
    {
        int columnIndex = static_cast<int>(wParam);
        MessageBox(NULL, (L"WM_HEADER_DBLCLICK on Header " + std::to_wstring(columnIndex) +
            L"\nTime: " + GetCurrentTimeNew()).c_str(), L"Header Event", MB_OK | MB_TASKMODAL);
    }
    break;

    case WM_HEADER_RDBLCLICK:
    {
        int columnIndex = static_cast<int>(wParam);
        MessageBox(NULL, (L"WM_HEADER_RDBLCLICK on Header " + std::to_wstring(columnIndex) +
            L"\nTime: " + GetCurrentTimeNew()).c_str(), L"Header Event", MB_OK | MB_TASKMODAL);
    }
    break;


    default:
        return DefWindowProc(hwnd, uMsg, wParam, lParam);
    }
    return 0;
}

int WINAPI wWinMain(_In_ HINSTANCE hInstance, _In_opt_ HINSTANCE hPrevInstance,
    _In_ PWSTR pCmdLine, _In_ int nCmdShow)
{
    const wchar_t CLASS_NAME[] = L"WINAPI_LAB1";

    WNDCLASS wc = { 0 };
    wc.lpfnWndProc = WindowProc;
    wc.hInstance = hInstance;
    wc.lpszClassName = CLASS_NAME;
    wc.hCursor = LoadCursor(nullptr, IDC_ARROW);
    wc.hbrBackground = (HBRUSH)(COLOR_WINDOW + 1);
    wc.style = CS_HREDRAW | CS_VREDRAW;

    RegisterClass(&wc);

    HWND hwnd = CreateWindowEx(0, CLASS_NAME, L"WINAPI_LAB1", WS_OVERLAPPEDWINDOW,
        CW_USEDEFAULT, CW_USEDEFAULT, SCREEN_WIDTH, SCREEN_HEIGHT,
        NULL, NULL, hInstance, NULL
    );

    if (hwnd == NULL)
    {
        return 0;
    }
    
    hMainHandle = hwnd;

    /*LONG style = GetWindowLong(hwnd, GWL_STYLE);
    style &= ~(WS_CAPTION | WS_THICKFRAME);
    SetWindowLong(hwnd, GWL_STYLE, style);

    LONG exStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
    exStyle &= ~(WS_EX_DLGMODALFRAME | WS_EX_CLIENTEDGE | WS_EX_STATICEDGE);
    SetWindowLong(hwnd, GWL_EXSTYLE, exStyle);*/

    MONITORINFO mi = { sizeof(mi) };
    if (GetMonitorInfo(MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST), &mi)) {
        SetWindowPos(hwnd, HWND_TOP,
            mi.rcMonitor.left, mi.rcMonitor.top,
            mi.rcMonitor.right - mi.rcMonitor.left,
            mi.rcMonitor.bottom - mi.rcMonitor.top,
            SWP_NOZORDER | SWP_FRAMECHANGED);
    }

    ShowWindow(hwnd, SW_SHOWMAXIMIZED);

    MSG msg;
    while (GetMessage(&msg, NULL, 0, 0))
    {
        TranslateMessage(&msg);
        DispatchMessage(&msg);
    }

    return 0;
}