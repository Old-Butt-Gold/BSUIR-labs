#ifndef UNICODE
#define UNICODE
#endif 

#include "Sort.h"
#include "Constants.h"

#include <Windows.h>
#include <iomanip>
#include <sstream>
#include <CommCtrl.h>
#include <windowsx.h>
#include <wingdi.h>

//g for global
std::vector<FileData> gFilesVector;
std::vector<std::wstring> gFontPaths;

FontSettings gFontSettings = {
    //file Names
    DEFAULT_FONT, DEFAULT_FONT, DEFAULT_FONT_SIZE, nullptr,

    //file Headers
    DEFAULT_FONT, DEFAULT_FONT, DEFAULT_FONT_SIZE, nullptr,

    //file Sizes
    DEFAULT_FONT, DEFAULT_FONT, DEFAULT_FONT_SIZE, nullptr,
};

void* hCustomListView = nullptr; //��������� �� CreateCustomTableFunc
HWND hFontChildWindow = NULL;
HWND hMainHandle;

typedef void* (*CreateCustomTableFunc)(HWND, int, int, HFONT, HFONT);
typedef void (*DestroyCustomTableFunc)(void*);
typedef HFONT (*CreateFontWithParamsFunc)(std::wstring, std::wstring, int);
typedef void (*UnloadCustomFontsFunc)();
typedef void (*AddColumnFunc)(void*, int, const wchar_t*);
typedef void (*AddItemFunc)(void*, int, int, const wchar_t*);
typedef void (*ClearItemsFunc)(void*);
typedef void (*ShowTableFunc)(void*);
typedef void (*UnShowTableFunc)(void*);
typedef HWND (*GetTableHWNDFunc)(void*);
typedef int (*GetRowsFunc)();
typedef int (*GetColsFunc)();
typedef bool (*RegisterCustomListClassFunc)(HINSTANCE, const wchar_t*);
typedef HFONT (*CreateDefaultFontFunc)();

RegisterCustomListClassFunc register_custom_list_class_func = nullptr;
CreateDefaultFontFunc create_default_font_func = nullptr;
CreateCustomTableFunc create_custom_table_func = nullptr;
DestroyCustomTableFunc destroy_custom_table_func = nullptr;
CreateFontWithParamsFunc create_font_with_params_func = nullptr;
UnloadCustomFontsFunc unload_custom_fonts_func = nullptr;
AddColumnFunc add_column_func = nullptr;
AddItemFunc add_item_func = nullptr;
ClearItemsFunc clear_items_func = nullptr;
ShowTableFunc show_table_func = nullptr;
UnShowTableFunc unshow_table_func = nullptr;
GetTableHWNDFunc get_table_hwnd_func = nullptr;
GetRowsFunc get_rows_func = nullptr;
GetColsFunc get_cols_func = nullptr;

HINSTANCE hInstLibrary = nullptr;

void PrepareListView(void* hListView, HWND hEdit)
{
    wchar_t directoryPath[MAX_PATH] = { 0 };
    GetWindowTextW(hEdit, directoryPath, MAX_PATH);

    WIN32_FIND_DATA data;

    std::wstring searchPath = std::wstring(directoryPath) + L"\\*";

    HANDLE hFind = FindFirstFile(searchPath.c_str(), &data);

    if (hFind == INVALID_HANDLE_VALUE || searchPath == L"\\*") {
        MessageBox(get_table_hwnd_func(hListView), L"Enter existing directory path!", L"Error", MB_ICONERROR);
        return;
    }

    clear_items_func(hListView);

    std::wofstream errorLogFile(L"D:/errors.log.txt", std::ios::app);

    gFilesVector = { };
    std::unordered_map<std::wstring, int> gNameCount = { };

    LoadFilesData(gFilesVector, directoryPath, gFilesVector, gNameCount, errorLogFile);

    SendMessage(get_table_hwnd_func(hListView), WM_SETREDRAW, FALSE, 0);
    InvalidateRect(get_table_hwnd_func(hListView), NULL, TRUE);

    int size = static_cast<int>(gFilesVector.size());

    add_item_func(hCustomListView, 0, 0, L"Count");
    add_item_func(hCustomListView, 0, 1, std::to_wstring(size).c_str());
    add_item_func(hCustomListView, 0, 2, L"Count");
    add_item_func(hCustomListView, 0, 3, std::to_wstring(size).c_str());

    for (int i = 0; i < size; i++) {
        int row = i + 1;
        add_item_func(hCustomListView, row, 0, gFilesVector[i].fileName.c_str());
        add_item_func(hCustomListView, row, 1, gFilesVector[i].fileSize.c_str());
    }

    SendMessage(get_table_hwnd_func(hListView), WM_SETREDRAW, TRUE, 0);
    InvalidateRect(get_table_hwnd_func(hListView), NULL, TRUE);
}

void SortListView(void* hListView, bool isAscending)
{
    if (!gFilesVector.empty()) {
        QSort(gFilesVector, 0, static_cast<int>(gFilesVector.size()) - 1, isAscending);
        SendMessage(get_table_hwnd_func(hListView), WM_SETREDRAW, FALSE, 0);
        InvalidateRect(get_table_hwnd_func(hListView), NULL, TRUE);

        auto size = static_cast<int>(gFilesVector.size());

        for (auto i = 0; i < size; i++) {
            int row = i + 1;
            add_item_func(hCustomListView, row, 2, gFilesVector[i].fileName.c_str());
            add_item_func(hCustomListView, row, 3, gFilesVector[i].fileSize.c_str());
        }

        SendMessage(get_table_hwnd_func(hListView), WM_SETREDRAW, TRUE, 0);
        InvalidateRect(get_table_hwnd_func(hListView), NULL, TRUE);
    }
}

WNDPROC oldEditProc;

LRESULT CALLBACK SubEditProc(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam)
{
    switch (msg)
    {
        case WM_KEYDOWN:
            switch (wParam)
            {
                case VK_RETURN:
                    PrepareListView(hCustomListView, hwnd);
                    SortListView(hCustomListView, true);
                    return 0;

                case VK_F1:
                {
                    PrepareListView(hCustomListView, hwnd);
                    SortListView(hCustomListView, false);
                    return 0;
                }
                default: ;
            }
            break;

        default:
            return CallWindowProc(oldEditProc, hwnd, msg, wParam, lParam);
        }
    return 0;
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

        gFontSettings.headerFontHandle = create_font_with_params_func(gFontSettings.headerFontPath, gFontSettings.headerFontFamily, gFontSettings.headerFontSize);

        SendMessage(get_table_hwnd_func(hCustomListView), WM_SETFONT, (WPARAM)gFontSettings.headerFontHandle, (LPARAM)ID_COMBOBOX_HEADER);
    }

    else if ((HMENU)ID_COMBOBOX_HEADER_SIZES == hMenu) {
        gFontSettings.headerFontSize = _wtoi(text.c_str());
        DeleteFont(gFontSettings.headerFontHandle);
        
        gFontSettings.headerFontHandle = create_font_with_params_func(gFontSettings.headerFontPath, gFontSettings.headerFontFamily, gFontSettings.headerFontSize);
        SendMessage(get_table_hwnd_func(hCustomListView), WM_SETFONT, (WPARAM)gFontSettings.headerFontHandle, (LPARAM)ID_COMBOBOX_HEADER_SIZES);
    }
    else if ((HMENU)ID_COMBOBOX_FILENAMES == hMenu) {
        gFontSettings.fileNameFontFamily = text;

        gFontSettings.fileNameFontPath = SearchForFont(gFontPaths, text);
        DeleteFont(gFontSettings.fileNameFontHandle);

        gFontSettings.fileNameFontHandle = create_font_with_params_func(gFontSettings.fileNameFontPath, gFontSettings.fileNameFontFamily, gFontSettings.fileNameFontSize);

        SendMessage(get_table_hwnd_func(hCustomListView), WM_SETFONT, (WPARAM)gFontSettings.fileNameFontHandle, (LPARAM)ID_COMBOBOX_FILENAMES);
    }
    else if ((HMENU)ID_COMBOBOX_FILENAMES_SIZES == hMenu) {
        gFontSettings.fileNameFontSize = _wtoi(text.c_str());
        DeleteFont(gFontSettings.fileNameFontHandle);
        
        gFontSettings.fileNameFontHandle = create_font_with_params_func(gFontSettings.fileNameFontFamily, gFontSettings.fileNameFontFamily, gFontSettings.fileNameFontSize);
        SendMessage(get_table_hwnd_func(hCustomListView), WM_SETFONT, (WPARAM)gFontSettings.fileNameFontHandle, (LPARAM)ID_COMBOBOX_FILENAMES_SIZES);
    }
    else if ((HMENU)ID_COMBOBOX_FILESIZES == hMenu) {
        gFontSettings.fileSizeFontFamily = text;

        gFontSettings.fileSizeFontPath = SearchForFont(gFontPaths, text);
        DeleteFont(gFontSettings.fileSizeFontHandle);

        gFontSettings.fileSizeFontHandle = create_font_with_params_func(gFontSettings.fileSizeFontPath, gFontSettings.fileSizeFontFamily, gFontSettings.fileSizeFontSize);
        SendMessage(get_table_hwnd_func(hCustomListView), WM_SETFONT, (WPARAM)gFontSettings.fileSizeFontHandle, (LPARAM)ID_COMBOBOX_FILESIZES);
    }
    else if ((HMENU)ID_COMBOBOX_FILESIZES_SIZES == hMenu) {
        gFontSettings.fileSizeFontSize = _wtoi(text.c_str());
        DeleteFont(gFontSettings.fileSizeFontHandle);
        gFontSettings.fileSizeFontHandle = create_font_with_params_func(gFontSettings.fileSizeFontPath, gFontSettings.fileSizeFontFamily, gFontSettings.fileSizeFontSize);
        
        SendMessage(get_table_hwnd_func(hCustomListView), WM_SETFONT, (WPARAM)gFontSettings.fileSizeFontHandle, (LPARAM)ID_COMBOBOX_FILESIZES_SIZES);
    }

    InvalidateRect(get_table_hwnd_func(hCustomListView), NULL, TRUE);
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

                HWND hEdit = GetDlgItem(hFontChildWindow, ID_FONTS_LOAD_EDIT); // �������� ID_EDIT �� ��� ID
                SetFocus(hEdit); // ���������� ����� �� hEdit

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

std::wstring GetCurrentTimeNew() {
    SYSTEMTIME st;
    GetLocalTime(&st);  // �������� ��������� �����
    
    std::wstringstream wss;
    wss << std::setfill(L'0') << std::setw(2) << st.wHour << L":"
        << std::setfill(L'0') << std::setw(2) << st.wMinute << L":"
        << std::setfill(L'0') << std::setw(2) << st.wSecond << L"."
        << std::setfill(L'0') << std::setw(3) << st.wMilliseconds;

    return wss.str();
}

LRESULT CALLBACK WindowProc(HWND hwnd, UINT uMsg, WPARAM wParam, LPARAM lParam)
{
    static HWND hFontButton;
    static HWND hEdit;

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
        SendMessage(get_table_hwnd_func(hCustomListView), WM_SIZE, wParam, lParam); 
    }
    return 0;
    
    case WM_CREATE:
    {
        gFontPaths = GetFontsFromFolder(FONTS_DIRECTORY);

        HFONT hHeaderFont, hCellFont;

        //������ ��� �������!!!
        /*hFontButton = CreateWindowEx(0, L"BUTTON", L"Font Settings",
            WS_CHILD | WS_VISIBLE | BS_PUSHBOX | BS_OWNERDRAW,
            1, SCREEN_HEIGHT - 28, 200, 25, hwnd, (HMENU)ID_SELECT_FONT_BTN, NULL, NULL);*/

        hHeaderFont = create_font_with_params_func(gFontPaths[0], GetFileNameWithoutExtension(gFontPaths[0]), DEFAULT_FONT_SIZE * 2);
        hCellFont = create_font_with_params_func(gFontPaths[3], GetFileNameWithoutExtension(gFontPaths[3]), DEFAULT_FONT_SIZE);

            
        register_custom_list_class_func((HINSTANCE)GetWindowLongPtr(hwnd, GWLP_HINSTANCE), WC_CUSTOM_LIST_VIEW);
        hCustomListView = create_custom_table_func(hwnd, get_rows_func(), get_cols_func(), hHeaderFont, hCellFont);
        show_table_func(hCustomListView);

        add_column_func(hCustomListView, 1, L"File Name");
        add_column_func(hCustomListView, 3, L"File Size");
        add_column_func(hCustomListView, 5, L"Sorted File Size");

        // ������������� ���������� ��������� �����
        std::srand((unsigned int)std::time(nullptr));

        // ��������� ��������� ������ � ���������� �������
        for (int i = 0; i < get_rows_func(); ++i) {
            // ��������� ��� �����
            std::wstring fileName = L"File_" + std::to_wstring(i) + L".txt";

            // ��������� ������ �����
            std::wstring fileSize = std::to_wstring(std::rand() % 10000 + 1) + L" bytes"; // ��������� ������ �� 1 �� 10000 ����

            // ���������� ������ � ������ �������
            add_item_func(hCustomListView, i, 0, fileName.c_str());
            add_item_func(hCustomListView, i, 1, fileSize.c_str());
            add_item_func(hCustomListView, i, 2, fileSize.c_str());
            add_item_func(hCustomListView, i, 3, fileName.c_str());
            add_item_func(hCustomListView, i, 4, fileName.c_str());
            add_item_func(hCustomListView, i, 5, fileSize.c_str());
        }

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

    case WM_DRAWITEM:
    {
        LPDRAWITEMSTRUCT lpDrawItem = (LPDRAWITEMSTRUCT)lParam;
        if (lpDrawItem->CtlID == ID_SELECT_FONT_BTN) {
            HDC hdc = lpDrawItem->hDC;

            // ������ ���
            HBRUSH hBrush = CreateSolidBrush(RGB(30, 144, 255)); // ���� ����
            FillRect(hdc, &lpDrawItem->rcItem, hBrush);
            DeleteObject(hBrush);

            // ������ �����
            SetTextColor(hdc, RGB(255, 255, 255)); // ����� ���� ������
            SetBkMode(hdc, TRANSPARENT);

            RECT textRect = lpDrawItem->rcItem;
            DrawText(hdc, L"Font Settings", -1, &textRect, DT_CENTER | DT_VCENTER | DT_SINGLELINE);

            // ���� ������ ������
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
        
    case WM_CELL_CLICK: 
    {
        int itemIndex = HIWORD(wParam);
        int columnIndex = LOWORD(wParam);

        // ������ ��������� ����� �� ������
        std::wstring message = L"WM_CELL_CLICK - Item: " + std::to_wstring(itemIndex) +
            L", Column: " + std::to_wstring(columnIndex) +
            L"\nTime: " + GetCurrentTimeNew();
        MessageBox(hwnd, message.c_str(), L"Cell Click", MB_OK);
    }
    break;

    case WM_CELL_RCLICK: 
    {
        int itemIndex = HIWORD(wParam);
        int columnIndex = LOWORD(wParam);

        // ������ ��������� ������� ����� �� ������
        std::wstring message = L"WM_CELL_RCLICK Clicked - Item: " + std::to_wstring(itemIndex) +
            L", Column: " + std::to_wstring(columnIndex) +
            L"\nTime: " + GetCurrentTimeNew();
        MessageBox(hwnd, message.c_str(), L"Cell Right Click", MB_OK);
    }
    break;

    case WM_CELL_DBLCLICK: 
    {
        int itemIndex = HIWORD(wParam);
        int columnIndex = LOWORD(wParam);

        // ������ ��������� �������� ����� �� ������
        std::wstring message = L"WM_CELL_DBLCLICK Clicked - Item: " + std::to_wstring(itemIndex) +
            L", Column: " + std::to_wstring(columnIndex) +
            L"\nTime: " + GetCurrentTimeNew();
        MessageBox(hwnd, message.c_str(), L"Cell Double Click", MB_OK);
    }
    break;

    case WM_CELL_RDBLCLICK: 
    {
        int itemIndex = HIWORD(wParam);
        int columnIndex = LOWORD(wParam);

        // ������ ��������� �������� ������� �����
        std::wstring message = L"WM_CELL_RDBLCLICK Clicked - Item: " + std::to_wstring(itemIndex) +
            L", Column: " + std::to_wstring(columnIndex) +
            L"\nTime: " + GetCurrentTimeNew();
        MessageBox(hwnd, message.c_str(), L"Cell Right Double Click", MB_OK);
    }
    break;

    case WM_HEADER_CLICK: 
    {
        int columnIndex = wParam; 

        std::wstring message = L"WM_HEADER_CLICK - Column: " + std::to_wstring(columnIndex) +
            L"\nTime: " + GetCurrentTimeNew();
        MessageBox(hwnd, message.c_str(), L"Header Click", MB_OK);
    }
    break;

    case WM_HEADER_RCLICK: 
    {
        int columnIndex = wParam; // ������ �������

        std::wstring message = L"WM_HEADER_RCLICK - Column: " + std::to_wstring(columnIndex) +
            L"\nTime: " + GetCurrentTimeNew();
        MessageBox(hwnd, message.c_str(), L"Header Right Click", MB_OK);
    }
    break;

    case WM_COLUMN_RESIZE: 
    {
        int resizedColumn = HIWORD(wParam);
        int newWidth = LOWORD(wParam);

        std::wstring message = L"WM_LISTVIEW_RESIZE by column: " + std::to_wstring(resizedColumn) +
            L", New Width: " + std::to_wstring(newWidth) +
            L"\nTime: " + GetCurrentTimeNew();
        MessageBox(hwnd, message.c_str(), L"Warning", MB_OK);
    }
    break;

    case WM_LISTVIEW_SCROLL:
    {
        int scrollPos = wParam;

        std::wstring message = L"WM_LISTVIEW_SCROLL - Scroll Position: " + std::to_wstring(scrollPos) +
            L"\nTime: " + GetCurrentTimeNew();
        MessageBox(hwnd, message.c_str(), L"Scroll Info", MB_OK);
    }
    break;

    case WM_HEADER_DBLCLICK:
    {
        int columnIndex = static_cast<int>(wParam);
        MessageBox(hwnd, (L"WM_HEADER_DBLCLICK on Header " + std::to_wstring(columnIndex) +
            L"\nTime: " + GetCurrentTimeNew()).c_str(), L"Header Event", MB_OK);
    }
    break;

    case WM_HEADER_RDBLCLICK:
    {
        int columnIndex = static_cast<int>(wParam);
        MessageBox(hwnd, (L"WM_HEADER_RDBLCLICK on Header " + std::to_wstring(columnIndex) +
            L"\nTime: " + GetCurrentTimeNew()).c_str(), L"Header Event", MB_OK);
    }
    break;

    case WM_DESTROY:
        {
            FreeLibrary(hInstLibrary);
            unload_custom_fonts_func();
            DeleteFont(gFontSettings.fileNameFontHandle);
            DeleteFont(gFontSettings.headerFontHandle);
            DeleteFont(gFontSettings.fileSizeFontHandle);
            destroy_custom_table_func(hCustomListView);

            PostQuitMessage(0);
        }
        return 0;

    default:
        return DefWindowProc(hwnd, uMsg, wParam, lParam);
    }
    return 0;
}

int WINAPI wWinMain(_In_ HINSTANCE hInstance, _In_opt_ HINSTANCE hPrevInstance,
    _In_ PWSTR pCmdLine, _In_ int nCmdShow)
{
    hInstLibrary = LoadLibrary(L"C:\\Users\\Lenovo\\source\\repos\\TABLE_DLL.dll");
    if (hInstLibrary == nullptr) {
        return 1;
    }

    create_font_with_params_func = (CreateFontWithParamsFunc)GetProcAddress(hInstLibrary, "?CreateFontWithParams@@YAPEAUHFONT__@@V?$basic_string@_WU?$char_traits@_W@std@@V?$allocator@_W@2@@std@@0H@Z");
    unload_custom_fonts_func = (UnloadCustomFontsFunc)GetProcAddress(hInstLibrary, "?UnloadCustomFonts@@YAXXZ");

    add_column_func = (AddColumnFunc)GetProcAddress(hInstLibrary, "AddColumn");
    add_item_func = (AddItemFunc)GetProcAddress(hInstLibrary, "AddItem");
    clear_items_func = (ClearItemsFunc)GetProcAddress(hInstLibrary, "ClearItems");
    show_table_func = (ShowTableFunc)GetProcAddress(hInstLibrary, "ShowTable");
    unshow_table_func = (UnShowTableFunc)GetProcAddress(hInstLibrary, "UnShowTable");
    get_table_hwnd_func = (GetTableHWNDFunc)GetProcAddress(hInstLibrary, "GetTableHWND");
    get_rows_func = (GetRowsFunc)GetProcAddress(hInstLibrary, "GetRows");
    get_cols_func = (GetColsFunc)GetProcAddress(hInstLibrary, "GetCols");
    register_custom_list_class_func = (RegisterCustomListClassFunc)GetProcAddress(hInstLibrary, "RegisterCustomListClass");
    create_default_font_func = (CreateDefaultFontFunc) GetProcAddress(hInstLibrary, "CreateDefaultFont");
    
    create_custom_table_func = (CreateCustomTableFunc)GetProcAddress(hInstLibrary, "CreateCustomTable");
    destroy_custom_table_func = (DestroyCustomTableFunc)GetProcAddress(hInstLibrary, "DestroyCustomTable");

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