#ifndef LISTVIEW_H
#define LISTVIEW_H

#include <windows.h>

#define COLUMN_HEADER_HEIGHT 30
#define ITEM_HEIGHT 20
#define HEADER_PADDING 5
#define RESIZE_MARGIN 5
#define MARGIN_LEFT 0

void AddColumn(HWND hwnd, int column, const wchar_t* header);
void AddItem(HWND hwnd, int row, int column, const wchar_t* item);
void ClearItems(HWND hwnd);
void UpdateItemHeight(HWND hwnd, HFONT hHeaderFont, HFONT hItemFont);
bool RegisterCustomListClass(HINSTANCE hInstance);

#endif