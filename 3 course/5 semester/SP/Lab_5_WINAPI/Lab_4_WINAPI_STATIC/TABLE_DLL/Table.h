#ifndef LISTVIEW_H
#define LISTVIEW_H

#include <windows.h>
#include <vector>
#include <string>
#include <windowsx.h>
#include <stdlib.h>
#include <string.h>
#include <CommCtrl.h>

#include "Constants.h"
#include "Fonts.h"
#include "HeaderTable.h"
#include "Cell.h"

#define CUSTOM_LIST_VIEW_ROW_COUNT 100
#define CUSTOM_LIST_VIEW_COL_COUNT 6

#define ITEM_HEIGHT 20
#define HEADER_PADDING 5
#define RESIZE_MARGIN 5
#define MARGIN_LEFT 0

typedef struct {
    std::vector<Cell> columns;
} CustomListItem;

class __declspec(dllexport) Table {
public:
    Table(HWND parent, int rows, int cols, HFONT hHeaderFont, HFONT hCellFont);
    ~Table();

    void AddColumn(int column, const std::wstring& header);
    void AddItem(int row, int column, const std::wstring& item);

    void ClearItems();
    
    void Show();
    void UnShow();

    HWND GetHWND();

    static bool RegisterCustomListClass(HINSTANCE hInstance, std::wstring className);

    std::vector<CustomListItem>& GetItems();
    int GetSelectedColumn();
    int GetSelectedItem();

    void SetHEdit(HWND value);
    HWND GetHEdit();

    HeaderTable* GetHeaderClass();

    static HFONT CreateDefaultFont();

    int GetItemHeight();
    void SetItemHeight(int item_height);

protected:
    static LRESULT CALLBACK CustomListWndProc(HWND hwnd, UINT message, WPARAM wParam, LPARAM lParam);

private:
    HeaderTable* header;
    std::vector<CustomListItem> items;

    HWND hEdit;

    HWND hwndParent;
    HWND hwnd;
    int itemHeight;

    int selectedColumn;
    int selectedItem;

    int scrollPos;
    int resizingColumn;
    int resizeStartX;
    int resizeStartWidth;

    HFONT hHeaderFont;
    HFONT hCellFont;

    int rowCount;

    bool ignoreNextLButtonDown;
    bool ignoreNextLButtonUp;

    bool ignoreNextRButtonDown;
    bool ignoreNextRButtonUp;

    void UpdateVerticalScrollBar();
    void UpdateItemHeight();

    void OnSize();
    void OnVScroll(WPARAM wParam);
    void OnMouseWheel(WPARAM wParam);
    void DestroyHEdit();

    bool HandleColumnResize(int xPos, int yPos);
    bool HandleHeaderClick(int xPos, int yPos, UINT uMsg, bool useSelection);
    void HandleCellClick(int xPos, int yPos, UINT uMsg, bool useSelection);
    int GetColumnIndexAtX(int xPos);

    POINT CalculateEditPosition(int itemIndex, int columnIndex);
    SIZE CalculateEditSize(int itemIndex, int columnIndex);
    void CreateEditControl(int itemIndex, int columnIndex, POINT editPosition, SIZE editSize);

    void OnLButtonDown(LPARAM lParam);
    void OnRButtonDown(LPARAM lParam);
    void OnLButtonDblClk(LPARAM lParam);
    void OnRButtonDblClk(LPARAM lParam);

    void OnMouseMove(WPARAM wParam, LPARAM lParam);
    void OnLButtonUp(LPARAM lParam);
    void OnSetFont(WPARAM wParam, LPARAM lParam);
    void OnPaint();
};

// Фабрика для создания и уничтожения объекта Table
extern "C" __declspec(dllexport) void* CreateCustomTable(HWND parent, int rows, int cols, HFONT hHeaderFont, HFONT hCellFont) {
    return new Table(parent, rows, cols, hHeaderFont, hCellFont);
}

extern "C" __declspec(dllexport) void DestroyCustomTable(void* table) {
    delete static_cast<Table*>(table);
}

// Обертки для публичных методов Table
extern "C" __declspec(dllexport) void AddColumn(void* table, int column, const wchar_t* header) {
    static_cast<Table*>(table)->AddColumn(column, header);
}

extern "C" __declspec(dllexport) void AddItem(void* table, int row, int column, const wchar_t* item) {
    static_cast<Table*>(table)->AddItem(row, column, item);
}

extern "C" __declspec(dllexport) void ClearItems(void* table) {
    static_cast<Table*>(table)->ClearItems();
}

extern "C" __declspec(dllexport) void ShowTable(void* table) {
    static_cast<Table*>(table)->Show();
}

extern "C" __declspec(dllexport) void UnShowTable(void* table) {
    static_cast<Table*>(table)->UnShow();
}

extern "C" __declspec(dllexport) HWND GetTableHWND(void* table) {
    return static_cast<Table*>(table)->GetHWND();
}

extern "C" __declspec(dllexport) int GetRows()
{
    return CUSTOM_LIST_VIEW_ROW_COUNT;
}

extern "C" __declspec(dllexport) int GetCols()
{
    return CUSTOM_LIST_VIEW_COL_COUNT;
}

extern "C" __declspec(dllexport) bool RegisterCustomListClass(HINSTANCE hInstance, const wchar_t* className) {
    return Table::RegisterCustomListClass(hInstance, std::wstring(className));
}

extern "C" __declspec(dllexport) HFONT CreateDefaultFont() {
    return Table::CreateDefaultFont();
}



#define WM_CELL_CLICK (WM_USER + 1)

#define WM_CELL_RCLICK (WM_USER + 2)

#define WM_CELL_DBLCLICK (WM_USER + 3)

#define WM_CELL_RDBLCLICK (WM_USER + 4)

#define WM_HEADER_CLICK (WM_USER + 5)

#define WM_HEADER_RCLICK (WM_USER + 6)

#define WM_COLUMN_RESIZE (WM_USER + 7)

#define WM_LISTVIEW_SCROLL (WM_USER + 8)

#define WM_HEADER_DBLCLICK (WM_USER + 9)

#define WM_HEADER_RDBLCLICK (WM_USER + 10)

#endif
