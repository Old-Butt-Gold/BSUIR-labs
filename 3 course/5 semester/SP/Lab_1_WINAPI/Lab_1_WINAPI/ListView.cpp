#define _CRT_SECURE_NO_WARNINGS

#include "ListView.h"
#include <windowsx.h>
#include <stdlib.h>
#include <string.h>

LRESULT CALLBACK CustomListWndProc(HWND hwnd, UINT message, WPARAM wParam, LPARAM lParam);

typedef struct {
    wchar_t** columns;
} CustomListItem;

typedef struct {
    int itemCount;
    int selectedItem;
    int scrollPos;
    HWND hwndParent;
    int columnCount;
    int* columnWidths;
    CustomListItem* items;
    wchar_t** headers;
    int resizingColumn;
    int resizeStartX;
    int resizeStartWidth;
} CustomListData;

void UpdateVerticalScrollBar(HWND hwnd) {
    CustomListData* pData = (CustomListData*)GetWindowLongPtr(hwnd, GWLP_USERDATA);
    RECT clientRect;
    GetClientRect(hwnd, &clientRect);

    SCROLLINFO si = { sizeof(SCROLLINFO), SIF_RANGE | SIF_PAGE | SIF_POS, 0, pData->itemCount - 1,
                      static_cast<UINT>((clientRect.bottom - COLUMN_HEADER_HEIGHT) / ITEM_HEIGHT), pData->scrollPos, 0 };
    SetScrollInfo(hwnd, SB_VERT, &si, TRUE);
}

void AddColumn(HWND hwnd, int column, const wchar_t* header) {
    CustomListData* pData = (CustomListData*)GetWindowLongPtr(hwnd, GWLP_USERDATA);

    // Reallocate memory for new column
    if (column >= pData->columnCount) {
        pData->columnWidths = (int*)realloc(pData->columnWidths, (column + 1) * sizeof(int));
        pData->headers = (wchar_t**)realloc(pData->headers, (column + 1) * sizeof(wchar_t*));
        for (int i = pData->columnCount; i <= column; i++) {
            pData->columnWidths[i] = (SCREEN_WIDTH - MARGIN_LEFT) / 4; // Default width
            pData->headers[i] = NULL;
        }
        pData->columnCount = column + 1;
    }

    // Free and allocate memory for the new header
    free(pData->headers[column]);
    pData->headers[column] = (wchar_t*)calloc(wcslen(header) + 1, sizeof(wchar_t));
    wcscpy(pData->headers[column], header);
}

// Add item to ListView
void AddItem(HWND hwnd, int row, int column, const wchar_t* item) {
    CustomListData* pData = (CustomListData*)GetWindowLongPtr(hwnd, GWLP_USERDATA);

    // Reallocate memory for new row
    if (row >= pData->itemCount) {
        pData->items = (CustomListItem*)realloc(pData->items, (row + 1) * sizeof(CustomListItem));
        for (int i = pData->itemCount; i <= row; i++) {
            pData->items[i].columns = (wchar_t**)calloc(pData->columnCount, sizeof(wchar_t*));
        }
        pData->itemCount = row + 1;
    }

    // Reallocate memory for new column
    if (column >= pData->columnCount) {
        pData->columnWidths = (int*)realloc(pData->columnWidths, (column + 1) * sizeof(int));
        pData->headers = (wchar_t**)realloc(pData->headers, (column + 1) * sizeof(wchar_t*));
        for (int i = pData->columnCount; i <= column; i++) {
            pData->columnWidths[i] = 100;
            pData->headers[i] = NULL;
        }
        for (int i = 0; i < pData->itemCount; i++) {
            pData->items[i].columns = (wchar_t**)realloc(pData->items[i].columns, (column + 1) * sizeof(wchar_t*));
            for (int j = pData->columnCount; j <= column; j++) {
                pData->items[i].columns[j] = NULL;
            }
        }
        pData->columnCount = column + 1;
    }

    // Free and allocate memory for new item
    free(pData->items[row].columns[column]);
    pData->items[row].columns[column] = (wchar_t*)calloc(wcslen(item) + 1, sizeof(wchar_t));
    wcscpy(pData->items[row].columns[column], item);

    UpdateVerticalScrollBar(hwnd);
}

// Clear all items in ListView
void ClearItems(HWND hwnd) {
    CustomListData* pData = (CustomListData*)GetWindowLongPtr(hwnd, GWLP_USERDATA);

    // Free memory for each item and its columns
    for (int i = 0; i < pData->itemCount; ++i) {
        for (int j = 0; j < pData->columnCount; ++j) {
            free(pData->items[i].columns[j]);
        }
        free(pData->items[i].columns);
    }
    free(pData->items);

    // Reset structure
    pData->items = NULL;
    pData->itemCount = 0;
    pData->selectedItem = -1;
    pData->scrollPos = 0;

    // Update scrollbar and repaint
    UpdateVerticalScrollBar(hwnd);
}

// Custom ListView window procedure
LRESULT CALLBACK CustomListWndProc(HWND hwnd, UINT message, WPARAM wParam, LPARAM lParam) {
    CustomListData* pData = (CustomListData*)GetWindowLongPtr(hwnd, GWLP_USERDATA);
    SCROLLINFO si;
    switch (message) {
    case WM_CREATE:
        pData = (CustomListData*)calloc(1, sizeof(CustomListData));
        pData->itemCount = 0;
        pData->selectedItem = -1;
        pData->scrollPos = 0;
        pData->hwndParent = ((LPCREATESTRUCT)lParam)->hwndParent;
        pData->columnCount = 0;
        pData->columnWidths = NULL;
        pData->items = NULL;
        pData->headers = NULL;
        pData->resizingColumn = -1;
        SetWindowLongPtr(hwnd, GWLP_USERDATA, (LONG_PTR)pData);

        // Initialize vertical scrollbar
        si = { sizeof(SCROLLINFO), SIF_PAGE | SIF_RANGE, 0, 0, 10, 0 };
        SetScrollInfo(hwnd, SB_VERT, &si, TRUE);
        return 0;

    case WM_DESTROY:
        ClearItems(hwnd);
        free(pData->columnWidths);
        for (int i = 0; i < pData->columnCount; i++) {
            free(pData->headers[i]);
        }
        free(pData->headers);
        free(pData);
        return 0;

    case WM_SIZE:
        UpdateVerticalScrollBar(hwnd);
        return 0;

    case WM_VSCROLL: {
        SCROLLINFO si = { sizeof(SCROLLINFO), SIF_ALL };
        GetScrollInfo(hwnd, SB_VERT, &si);
        int pos = si.nPos;
        switch (LOWORD(wParam)) {
        case SB_LINEUP: pos -= 1; break;
        case SB_LINEDOWN: pos += 1; break;
        case SB_PAGEUP: pos -= si.nPage; break;
        case SB_PAGEDOWN: pos += si.nPage; break;
        case SB_THUMBTRACK: pos = si.nTrackPos; break;
        }
        pos = max(0, min(pos, si.nMax - (int)si.nPage + 1));
        if (pos != si.nPos) {
            si.nPos = pos;
            SetScrollInfo(hwnd, SB_VERT, &si, TRUE);
            ScrollWindow(hwnd, 0, (pData->scrollPos - pos) * ITEM_HEIGHT, NULL, NULL);
            pData->scrollPos = pos;
            InvalidateRect(hwnd, NULL, TRUE);

            //MAKE FOR hEDIT COMING DOWN!!!
            HWND hEdit = GetDlgItem(hwnd, EnterButtonClicked);
            RECT editRect;
            GetWindowRect(hEdit, &editRect);
            int newYPos = editRect.top - (pData->scrollPos - pos) * ITEM_HEIGHT;
            SetWindowPos(hEdit, NULL, hEditX, hEditY, newYPos, COLUMN_HEADER_HEIGHT - 5, SWP_NOSIZE | SWP_NOZORDER);
        }
        
        return 0;
    }

    case WM_LBUTTONDOWN: {
        int xPos = GET_X_LPARAM(lParam);
        int yPos = GET_Y_LPARAM(lParam);
        int x = MARGIN_LEFT;
        for (int j = 0; j < pData->columnCount; j++) {
            if (abs(x + pData->columnWidths[j] - xPos) < RESIZE_MARGIN && yPos < COLUMN_HEADER_HEIGHT) {
                pData->resizingColumn = j;
                pData->resizeStartX = xPos;
                pData->resizeStartWidth = pData->columnWidths[j];
                SetCapture(hwnd);
                return 0;
            }
            x += pData->columnWidths[j];
        }
        int itemIndex = (yPos - COLUMN_HEADER_HEIGHT) / ITEM_HEIGHT + pData->scrollPos;
        if (itemIndex >= 0 && itemIndex < pData->itemCount) {
            pData->selectedItem = itemIndex;
            InvalidateRect(hwnd, NULL, TRUE);
        }
        return 0;
    }

    case WM_MOUSEMOVE: {
        if (wParam == MK_LBUTTON && pData->resizingColumn != -1) {
            int deltaX = GET_X_LPARAM(lParam) - pData->resizeStartX;
            pData->columnWidths[pData->resizingColumn] = max(10, pData->resizeStartWidth + deltaX);
            InvalidateRect(hwnd, NULL, TRUE);
            return 0;
        }
        int xPos = GET_X_LPARAM(lParam);
        int yPos = GET_Y_LPARAM(lParam);
        int x = MARGIN_LEFT;
        for (int j = 0; j < pData->columnCount; j++) {
            if (abs(x + pData->columnWidths[j] - xPos) < RESIZE_MARGIN && yPos < COLUMN_HEADER_HEIGHT) {
                SetCursor(LoadCursor(NULL, IDC_SIZEWE));
                return 0;
            }
            x += pData->columnWidths[j];
        }
        SetCursor(LoadCursor(NULL, IDC_ARROW));
        return 0;
    }

    case WM_LBUTTONUP:
        ReleaseCapture();
        pData->resizingColumn = -1;
        return 0;

    case WM_ERASEBKGND:
        return 1;

    case WM_PAINT: {
        PAINTSTRUCT ps;
        HDC hdc = BeginPaint(hwnd, &ps);
        

        RECT clientRect;
        GetClientRect(hwnd, &clientRect);

        int clientWidth = clientRect.right - clientRect.left;
        FillRect(hdc, &clientRect, (HBRUSH)(COLOR_WINDOW + 1));

        // Отрисовка заголовков
        RECT headerRect = { MARGIN_LEFT, 0, clientWidth, COLUMN_HEADER_HEIGHT };
        HBRUSH headerBrush = CreateSolidBrush(RGB(191, 166, 42)); // Мягкий светло-серый цвет
        FillRect(hdc, &headerRect, headerBrush);
        DeleteObject(headerBrush);

        // Отрисовка текста заголовков
        SetBkMode(hdc, TRANSPARENT); // Убираем фон для текста
        SetTextColor(hdc, RGB(50, 50, 50)); // Тёмно-серый цвет для текста

        // Отрисовка текста заголовков
        int x = MARGIN_LEFT;  // Добавлен MARGIN_LEFT
        for (int j = 0; j < pData->columnCount; j++) {
            RECT textRect = { x + HEADER_PADDING, 0, x + pData->columnWidths[j] - HEADER_PADDING, COLUMN_HEADER_HEIGHT };
            DrawText(hdc, pData->headers[j], -1, &textRect, DT_LEFT | DT_VCENTER | DT_SINGLELINE | DT_END_ELLIPSIS);
            MoveToEx(hdc, x + pData->columnWidths[j] - 1, 0, NULL);
            LineTo(hdc, x + pData->columnWidths[j] - 1, COLUMN_HEADER_HEIGHT + 0);
            x += pData->columnWidths[j];
        }
        MoveToEx(hdc, MARGIN_LEFT, COLUMN_HEADER_HEIGHT - 1, NULL);  // Добавлен MARGIN_LEFT
        LineTo(hdc, clientRect.right, COLUMN_HEADER_HEIGHT - 1);
        MoveToEx(hdc, headerRect.left + MARGIN_LEFT, headerRect.top, NULL);
        LineTo(hdc, headerRect.left + MARGIN_LEFT, headerRect.bottom);

        // Отрисовка строк
        /*int y = COLUMN_HEADER_HEIGHT - pData->scrollPos * ITEM_HEIGHT;
        for (int i = 0; i < pData->itemCount; i++) {
            if (y + ITEM_HEIGHT > 0 && y < clientRect.bottom) {
                RECT itemRect = { MARGIN_LEFT, y, clientWidth, y + ITEM_HEIGHT };  // Добавлен MARGIN_LEFT
                if (i == pData->selectedItem) {
                    HBRUSH selectionBrush = CreateSolidBrush(RGB(180, 220, 255));
                    FillRect(hdc, &itemRect, selectionBrush);
                    DeleteObject(selectionBrush);
                }
                else {
                    SetBkColor(hdc, RGB(255, 255, 255));
                    FillRect(hdc, &itemRect, (HBRUSH)GetStockObject(WHITE_BRUSH));
                }

                int x = MARGIN_LEFT;  // Добавлен MARGIN_LEFT
                for (int j = 0; j < pData->columnCount; j++) {
                    RECT textRect = { x + HEADER_PADDING, y, x + pData->columnWidths[j] - HEADER_PADDING, y + ITEM_HEIGHT };
                    DrawText(hdc, pData->items[i].columns[j], -1, &textRect, DT_LEFT | DT_VCENTER | DT_SINGLELINE | DT_END_ELLIPSIS);
                    HPEN hPen = CreatePen(PS_DASH, 2, RGB(82, 46, 48));
                    SelectObject(hdc, hPen);
                    MoveToEx(hdc, x + pData->columnWidths[j] - 1, y, NULL);
                    LineTo(hdc, x + pData->columnWidths[j] - 1, y + ITEM_HEIGHT);
                    DeleteObject(hPen);

                    x += pData->columnWidths[j];
                }

                HPEN hPen = CreatePen(PS_DASH, 2, RGB(82, 46, 48));
                SelectObject(hdc, hPen);
                //MoveToEx(hdc, MARGIN_LEFT, y + ITEM_HEIGHT - 1, NULL);
                //LineTo(hdc, clientWidth, y + ITEM_HEIGHT - 1);

                MoveToEx(hdc, MARGIN_LEFT, y, NULL);
                LineTo(hdc, MARGIN_LEFT, y + ITEM_HEIGHT - 1);

                DeleteObject(hPen);
            }
            y += ITEM_HEIGHT;
        }*/

        for (int i = pData->scrollPos; i < pData->itemCount && (i - pData->scrollPos + 1) * ITEM_HEIGHT < clientRect.bottom; ++i) {
            RECT itemRect = { 0, (i - pData->scrollPos) * ITEM_HEIGHT + COLUMN_HEADER_HEIGHT, clientRect.right, (i - pData->scrollPos + 1) * ITEM_HEIGHT + COLUMN_HEADER_HEIGHT };

            if (i == pData->selectedItem) {
                HBRUSH selectionBrush = CreateSolidBrush(RGB(180, 220, 255));
                FillRect(hdc, &itemRect, selectionBrush);
                DeleteObject(selectionBrush);
            }
            else {
                SetBkColor(hdc, RGB(255, 255, 255));
                FillRect(hdc, &itemRect, (HBRUSH)GetStockObject(WHITE_BRUSH));
            }

            int x = 0;
            for (int j = 0; j < pData->columnCount; j++) {
                RECT columnRect = { x + HEADER_PADDING, itemRect.top, x + pData->columnWidths[j] - HEADER_PADDING, itemRect.bottom };
                if (pData->items[i].columns[j] != NULL) {
                    DrawText(hdc, pData->items[i].columns[j], -1, &columnRect, DT_LEFT | DT_VCENTER | DT_SINGLELINE);
                }
                MoveToEx(hdc, x + pData->columnWidths[j], itemRect.top, NULL);
                LineTo(hdc, x + pData->columnWidths[j], itemRect.bottom);
                x += pData->columnWidths[j];
            }
            MoveToEx(hdc, 0, itemRect.bottom - 1, NULL);
            //LineTo(hdc, rect.right, itemRect.bottom - 1);
        }

        EndPaint(hwnd, &ps);
        return 0;
    }

    default:
        return DefWindowProc(hwnd, message, wParam, lParam);
    }
}

bool RegisterCustomListClass(HINSTANCE hInstance) {
    WNDCLASS wc = {};
    wc.lpfnWndProc = CustomListWndProc;
    wc.hInstance = hInstance;
    wc.lpszClassName = WC_CUSTOM_LIST_VIEW;
    wc.hbrBackground = (HBRUSH)(COLOR_WINDOW + 1);

    return RegisterClass(&wc);
}