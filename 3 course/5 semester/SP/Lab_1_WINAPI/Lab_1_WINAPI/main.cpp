#ifndef UNICODE
#define UNICODE
#endif 

#include <Windows.h>
#include <CommCtrl.h>
#include <string>
#include <vector>
#include <unordered_map>
#include <fstream>
#include <ctime>
#include "ListView.h"

struct FileData {
    std::wstring fileName;
    std::wstring fileSize;
};

//g for global
std::vector<FileData> gFilesVector;

HWND hEdit, hCustomListView;

LRESULT CALLBACK WindowProc(HWND hwnd, UINT uMsg, WPARAM wParam, LPARAM lParam);

std::pair<std::wstring, std::wstring> SplitFileNameAndExtension(const std::wstring& fileName) {
    size_t pos = fileName.find_last_of(L'.');
    if (pos != std::wstring::npos) {
        return { fileName.substr(0, pos), fileName.substr(pos) };
    }
    return { fileName, L"" }; // ≈сли расширени€ нет, возвращаем пустую строку дл€ расширени€
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

void LoadFilesData(HWND hListView, const std::wstring directoryPath, std::vector<FileData>& files, std::unordered_map<std::wstring, int>& gNameCount,
    std::wofstream& errorLogFile)
{
    WIN32_FIND_DATA data;

    std::wstring searchPath = directoryPath + L"//*";

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
                const std::wstring newDirPath = directoryPath + L"//" + data.cFileName;
                LoadFilesData(hListView, newDirPath, files, gNameCount, errorLogFile);
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

void PrepareListView(HWND hListView)
{
    wchar_t directoryPath[MAX_PATH] = { 0 };
    GetWindowTextW(hEdit, directoryPath, MAX_PATH);

    WIN32_FIND_DATA data;

    std::wstring searchPath = std::wstring(directoryPath) + L"\\*";

    HANDLE hFind = FindFirstFile(searchPath.c_str(), &data);

    if (hFind == INVALID_HANDLE_VALUE || searchPath == L"\\*") {
        MessageBox(hListView, L"Enter existing directory path!", L"Error", MB_ICONERROR);
        return;
    }

    ClearItems(hListView);

    std::wofstream errorLogFile(L"D://errors.log.txt", std::ios::app);

    gFilesVector = { };
    std::unordered_map<std::wstring, int> gNameCount = { };

    LoadFilesData(hListView, directoryPath, gFilesVector, gNameCount, errorLogFile);

    SendMessage(hListView, WM_SETREDRAW, FALSE, 0);
    InvalidateRect(hListView, NULL, TRUE);

    int size = static_cast<int>(gFilesVector.size());

    AddItem(hListView, 0, 0, L"Count:");
    AddItem(hListView, 0, 2, L"Count:");
    AddItem(hListView, 0, 1, std::to_wstring(size).c_str());
    AddItem(hListView, 0, 3, std::to_wstring(size).c_str());

    for (int i = 0; i < size; i++) {
        int row = i + 1;
        AddItem(hListView, row, 0, gFilesVector[i].fileName.c_str());
        AddItem(hListView, row, 1, gFilesVector[i].fileSize.c_str());
    }

    SendMessage(hListView, WM_SETREDRAW, TRUE, 0);
    InvalidateRect(hListView, NULL, TRUE);
}

void Swap(int i, int j)
{
    auto temp = gFilesVector[i];
    gFilesVector[i] = gFilesVector[j];
    gFilesVector[j] = temp;
}

int Partition(int left, int right, bool isAscending)
{
    auto pivot = gFilesVector[left].fileSize.c_str();
    int j = left;

    for (int i = left + 1; i <= right; i++)
    {
        if (isAscending) {
            if (wcscmp(gFilesVector[i].fileSize.c_str(), pivot) <= 0)
            {
                j++;
                Swap(i, j);
            }
        }
        else {
            if (wcscmp(pivot, gFilesVector[i].fileSize.c_str()) <= 0)
            {
                j++;
                Swap(i, j);
            }
        }
    }

    Swap(left, j);
    return j;
}

void QSort(int left, int right, bool isAscending)
{
    while (left < right)
    {
        int pivotIndex = Partition(left, right, isAscending);
        if (pivotIndex - left <= right - pivotIndex)
        {
            QSort(left, pivotIndex - 1, isAscending);
            left = pivotIndex + 1;
        }
        else
        {
            QSort(pivotIndex + 1, right, isAscending);
            right = pivotIndex - 1;
        }
    }
}

void SortListView(HWND hListView, bool isAscending)
{
    if (gFilesVector.size() > 0) {
        QSort(0, gFilesVector.size() - 1, isAscending);
        SendMessage(hListView, WM_SETREDRAW, FALSE, 0);
        InvalidateRect(hListView, NULL, TRUE);

        size_t size = gFilesVector.size();

        for (auto i = 0; i < size; i++) {
            int row = i + 1;
            AddItem(hListView, row, 2, gFilesVector[i].fileName.c_str());
            AddItem(hListView, row, 3, gFilesVector[i].fileSize.c_str());
        }

        SendMessage(hListView, WM_SETREDRAW, TRUE, 0);
        InvalidateRect(hListView, NULL, TRUE);
    }
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

    RegisterClass(&wc);
    RegisterCustomListClass(hInstance);

    HWND hwnd = CreateWindowEx(0, CLASS_NAME, L"WINAPI_LAB1", WS_OVERLAPPEDWINDOW,
        CW_USEDEFAULT, CW_USEDEFAULT, SCREEN_WIDTH, SCREEN_HEIGHT,
        NULL, NULL, hInstance, NULL
    );

    if (hwnd == NULL)
    {
        return 0;
    }

    LONG style = GetWindowLong(hwnd, GWL_STYLE);
    style &= ~(WS_CAPTION | WS_THICKFRAME);
    SetWindowLong(hwnd, GWL_STYLE, style);

    LONG exStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
    exStyle &= ~(WS_EX_DLGMODALFRAME | WS_EX_CLIENTEDGE | WS_EX_STATICEDGE);
    SetWindowLong(hwnd, GWL_EXSTYLE, exStyle);

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

WNDPROC oldEditProc;

LRESULT CALLBACK subEditProc(HWND wnd, UINT msg, WPARAM wParam, LPARAM lParam)
{
    switch (msg)
    {
        case WM_KEYDOWN:
            switch (wParam)
            {
                case VK_RETURN:
                    PrepareListView(hCustomListView);
                    SortListView(hCustomListView, true);
                    return 0;

                case VK_F1:
                {
                    PrepareListView(hCustomListView);
                    SortListView(hCustomListView, false);
                    return 0;
                }
            }
            break;

        default:
            return CallWindowProc(oldEditProc, wnd, msg, wParam, lParam);
        }
    return 0;
}

LRESULT CALLBACK WindowProc(HWND hwnd, UINT uMsg, WPARAM wParam, LPARAM lParam)
{
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

    case WM_CREATE:
    {
        hCustomListView = CreateWindowEx(0, WC_CUSTOM_LIST_VIEW, L"",
            WS_CHILD | WS_VISIBLE | WS_BORDER | LVS_REPORT,
            5, 1, SCREEN_WIDTH - 10, SCREEN_HEIGHT - 5,
            hwnd, NULL, NULL, NULL);

        AddColumn(hCustomListView, 0, L"");
        AddColumn(hCustomListView, 2, L"");
        AddColumn(hCustomListView, 1, L"File Size");
        AddColumn(hCustomListView, 3, L"Sorted File Size");

        hEdit = CreateWindowEx(0, WC_EDIT, L"",
            WS_CHILD | WS_VISIBLE | WS_DLGFRAME,
            hEditX, hEditY, SCREEN_WIDTH / 4 - 2, COLUMN_HEADER_HEIGHT - 5,
            hCustomListView, (HMENU)EnterButtonClicked, NULL, NULL);

        oldEditProc = (WNDPROC)SetWindowLongPtr(hEdit, GWLP_WNDPROC, (LONG_PTR)subEditProc);
    }
    break;

    case WM_KEYDOWN:
    {
        int wmId = LOWORD(wParam);
        int wmEvent = HIWORD(wParam);

        if (wParam == VK_RETURN) {
            PrepareListView(hCustomListView);
            SortListView(hCustomListView, true);
            return 0;
            //GetDlgItem(hwnd, LoadFilesButtonClicked); Ц GETS HWND OF Window element
        }

        if (wParam == VK_F1) {
            PrepareListView(hCustomListView);
            SortListView(hCustomListView, false);
            return 0;
        }
    }
    break;

    /*case WM_COMMAND:
    {
        int wmId = LOWORD(wParam);
        int wmEvent = HIWORD(wParam);

        switch (wmId)
        {
            case EnterButtonClicked:
            {
                if (wmEvent == BN_CLICKED) {
                    PrepareListView(hCustomListView);
                    SortListView(hCustomListView);
                    //GetDlgItem(hwnd, LoadFilesButtonClicked); Ц GETS HWND OF Window element
                }
                break;
            }
        }
    }
    break;*/

    /*case WM_SYSCOMMAND:
        // Ѕлокируем команду перемещени€
        if ((wParam & 0xFFF0) == SC_MOVE) 
        {
            return 0;
        }
        break; */

    case WM_CLOSE:
        if (MessageBox(hwnd, L"Really quit?", L"My application", MB_OKCANCEL) == IDOK) 
        {
            DestroyWindow(hwnd);
        }
        break;

    case WM_DESTROY:
        PostQuitMessage(0);
        return 0;

    default:
        return DefWindowProc(hwnd, uMsg, wParam, lParam);
    }
    return 0;
}