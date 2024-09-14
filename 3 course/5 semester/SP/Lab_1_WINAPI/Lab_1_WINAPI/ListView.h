#ifndef LISTVIEW_H
#define LISTVIEW_H

#include <windows.h>

#define WC_CUSTOM_LIST_VIEW L"CustomListView"
#define SCREEN_WIDTH  GetSystemMetrics(SM_CXSCREEN)
#define SCREEN_HEIGHT GetSystemMetrics(SM_CYSCREEN)

#define COLUMN_HEADER_HEIGHT 30
#define ITEM_HEIGHT 25
#define RESIZE_MARGIN 5
#define HEADER_PADDING 5
#define MARGIN_LEFT 0

bool RegisterCustomListClass(HINSTANCE hInstance);
void AddColumn(HWND hwnd, int column, const wchar_t* header);
void AddItem(HWND hwnd, int row, int column, const wchar_t* item);
void ClearItems(HWND hwnd);

//ID FOR HMENU COMPONENTS

#define EnterButtonClicked 0x1c

#define hEditX 1
#define hEditY 3

#endif // LISTVIEW_H