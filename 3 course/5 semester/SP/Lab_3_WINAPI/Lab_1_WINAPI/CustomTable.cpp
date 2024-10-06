#define _CRT_SECURE_NO_WARNINGS

#include "CustomTable.h"

bool CustomTable::RegisterCustomListClass(HINSTANCE hInstance, std::wstring className) {
    WNDCLASS wc = {};
    wc.lpfnWndProc = CustomTable::CustomListWndProc;
    wc.hInstance = hInstance;
    wc.style = CS_DBLCLKS;
    wc.lpszClassName = className.c_str();
    wc.hbrBackground = (HBRUSH)(COLOR_WINDOW + 1);

    return RegisterClass(&wc);
}

CustomTable::CustomTable(HWND parent, int rows, int cols)
    : hwndParent(parent), rowCount(rows) {
    this->itemHeight = ITEM_HEIGHT;
    this->selectedItem = -1;
    this->selectedColumn = -1;
    this->scrollPos = 0;
    this->resizingColumn = -1;
    this->resizeStartX = 0;
    this->resizeStartWidth = 0;

    this->ignoreNextLButtonDown = false;
    this->ignoreNextLButtonUp = false;

    this->ignoreNextRButtonDown = false;
    this->ignoreNextRButtonUp = false;

    this->hHeaderFont = CustomTable::CreateDefaultFont();
    this->hFileNamesFont = CustomTable::CreateDefaultFont();
    this->hFileSizesFont = CustomTable::CreateDefaultFont();

    this->header = new CustomHeader(cols, SCREEN_WIDTH);

    this->items.resize(rows, CustomListItem());
    for (int i = 0; i < rows; i++) {
        this->items[i].columns.resize(cols, L"");
    }

    this->hwnd = CreateWindowEx(0, WC_CUSTOM_LIST_VIEW, L"",
        WS_CHILD | LVS_REPORT,
        1, 1, SCREEN_WIDTH - 2, SCREEN_HEIGHT,
        this->hwndParent, (HMENU)ID_LIST_VIEW, (HINSTANCE)GetWindowLongPtr(this->hwndParent, GWLP_HINSTANCE), this);

    this->UpdateVerticalScrollBar();
}

CustomTable::~CustomTable() {
    DeleteObject(this->hHeaderFont);
    DeleteObject(this->hFileNamesFont);
    DeleteObject(this->hFileSizesFont);
    DestroyWindow(this->hwnd);
    ClearItems();

    if (this->hEdit) {
        DestroyWindow(this->hEdit);
    }

    delete header;
}

void CustomTable::ClearItems() {
    for (int i = 0; i < this->items.size(); i++) {
        this->items[i].columns.clear();
    }
    this->items.clear();

    this->scrollPos = 0;
    this->selectedItem = -1;

    this->UpdateVerticalScrollBar();

    RedrawWindow(this->hwnd, NULL, NULL, RDW_INVALIDATE | RDW_UPDATENOW);
}

void CustomTable::Show() {
    ShowWindow(this->hwnd, SW_SHOW);
}

void CustomTable::UnShow() {
    ShowWindow(this->hwnd, SW_HIDE);
}

HWND CustomTable::GetHWND() {
    return this->hwnd;
}

HFONT CustomTable::CreateDefaultFont() {
    LOGFONT logFont;
    ZeroMemory(&logFont, sizeof(LOGFONT));
    HFONT defaultFont = (HFONT)GetStockObject(DEFAULT_GUI_FONT);

    GetObject(defaultFont, sizeof(LOGFONT), &logFont);
    logFont.lfHeight = DEFAULT_FONT_SIZE;
    return CreateFontIndirect(&logFont);
}

void CustomTable::UpdateVerticalScrollBar() {
    RECT clientRect;
    GetClientRect(this->hwnd, &clientRect);

    int visibleRows = (clientRect.bottom - this->header->GetColumnHeight()) / this->itemHeight;

    SCROLLINFO si = {};
    si.cbSize = sizeof(SCROLLINFO);
    si.fMask = SIF_RANGE | SIF_PAGE | SIF_POS;
    si.nMin = 0;
    si.nMax = static_cast<int>(this->items.size()) - 1;
    si.nPage = visibleRows > 0 ? static_cast<UINT>(visibleRows) : 1; 
    si.nPos = this->scrollPos;  

    SetScrollInfo(this->hwnd, SB_VERT, &si, TRUE);
}

void CustomTable::UpdateItemHeight() {
    HDC hdc = GetDC(this->hwnd);

    this->header->SetColumnHeight(GetTextHeightInPixels(hdc, FONTS_TEXT_SIZE_UPDATE, this->hHeaderFont) + HEADER_PADDING * 2);

    int maxTextHeight = max(GetTextHeightInPixels(hdc, FONTS_TEXT_SIZE_UPDATE, this->hFileNamesFont), GetTextHeightInPixels(hdc, FONTS_TEXT_SIZE_UPDATE, this->hFileSizesFont));

    this->itemHeight = maxTextHeight + HEADER_PADDING * 2;
    
    HWND hEdit = GetDlgItem(this->hwnd, ID_ENTER_BTN_CLICKED);
    SetWindowPos(hEdit, NULL, hEditX, hEditY, this->header->GetColumnWidths()[0] - 2, this->header->GetColumnHeight() - 5, NULL);

    ReleaseDC(this->hwnd, hdc);

    this->UpdateVerticalScrollBar();

    SendMessage(this->hwnd, WM_SETREDRAW, TRUE, 0);
    SendMessage(hEdit, WM_SETREDRAW, TRUE, 0);
    InvalidateRect(this->hwnd, NULL, TRUE);
    InvalidateRect(hEdit, NULL, TRUE);
}

void CustomTable::AddColumn(int column, const std::wstring& header) {
    if (column < 0 || column >= this->header->GetColumnCount()) {
        return; 
    }

    this->header->GetColumnHeaders()[column] = header;
}

void CustomTable::AddItem(int row, int column, const std::wstring& item) {
    if (row < 0 || row >= this->rowCount || column < 0 || column >= this->header->GetColumnCount()) {
        return; 
    }

    if (row >= this->items.size()) {
        int size = this->items.size();
        this->items.resize(row + 1);

        for (int i = size; i <= row; i++) {
            this->items[i].columns.resize(this->header->GetColumnCount());
        }
    }

    this->items[row].columns[column] = item;

    this->UpdateVerticalScrollBar();
}

//

void CustomTable::OnSize() {
    this->UpdateVerticalScrollBar();
}

void CustomTable::OnVScroll(WPARAM wParam) {
    SCROLLINFO si = { sizeof(SCROLLINFO), SIF_ALL };
    GetScrollInfo(this->hwnd, SB_VERT, &si);
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
        SetScrollInfo(this->hwnd, SB_VERT, &si, TRUE);
        ScrollWindow(this->hwnd, 0, (this->scrollPos - pos) * this->itemHeight, NULL, NULL);
        scrollPos = pos;
        InvalidateRect(this->hwnd, NULL, TRUE);

        HWND hEdit = GetDlgItem(hwnd, ID_ENTER_BTN_CLICKED);
        RECT editRect;
        GetWindowRect(hEdit, &editRect);
        int newYPos = editRect.top - (this->scrollPos - pos) * this->itemHeight;
        SetWindowPos(hEdit, NULL, hEditX, hEditY, newYPos, this->header->GetColumnHeight() - 5, SWP_NOSIZE | SWP_NOZORDER);

        SendMessage(this->hwndParent, WM_LISTVIEW_SCROLL, static_cast<WPARAM>(pos), 0);
    }
}

void CustomTable::OnMouseWheel(WPARAM wParam) {
    int zDelta = GET_WHEEL_DELTA_WPARAM(wParam);

    SCROLLINFO si = { sizeof(SCROLLINFO), SIF_ALL };
    GetScrollInfo(this->hwnd, SB_VERT, &si);

    int pos = si.nPos;
    int scrollAmount = -zDelta / WHEEL_DELTA * this->itemHeight;

    pos += scrollAmount;

    pos = max(0, min(pos, si.nMax - (int)si.nPage + 1));

    if (pos != si.nPos) {
        si.nPos = pos;
        SetScrollInfo(this->hwnd, SB_VERT, &si, TRUE);
        ScrollWindow(this->hwnd, 0, (this->scrollPos - pos) * this->itemHeight, NULL, NULL);
        scrollPos = pos;
        InvalidateRect(this->hwnd, NULL, TRUE);

        HWND hEdit = GetDlgItem(hwnd, ID_ENTER_BTN_CLICKED);
        if (hEdit) {
            RECT editRect;
            GetWindowRect(hEdit, &editRect);
            int newYPos = editRect.top - (this->scrollPos - pos) * this->itemHeight;
            SetWindowPos(hEdit, NULL, hEditX, hEditY, newYPos, this->header->GetColumnHeight() - 5, SWP_NOSIZE | SWP_NOZORDER);
        }

        // Посылаем сообщение родительскому окну о прокрутке
        SendMessage(this->hwndParent, WM_LISTVIEW_SCROLL, static_cast<WPARAM>(pos), 0);
    }
}

void CustomTable::OnLButtonUp(LPARAM lParam) {
    if (this->resizingColumn != -1) {
        WPARAM wParam = (static_cast<WPARAM>(this->resizingColumn) << 16) | (this->header->GetColumnWidths()[this->resizingColumn] & 0xFFFF);

        SendMessage(this->hwndParent, WM_COLUMN_RESIZE, wParam, 0);

        this->resizingColumn = -1;
    }
}

std::vector<CustomListItem>& CustomTable::GetItems() {
    return this->items;
}

int CustomTable::GetSelectedColumn() {
    return this->selectedColumn;
}

int CustomTable::GetSelectedItem() {
    return this->selectedItem;
}

HWND CustomTable::GetHEdit() {
    return this->hEdit;
}

void CustomTable::SetHEdit(HWND value) {
    this->hEdit = value;
}

bool CustomTable::HandleColumnResize(int xPos, int yPos) {
    int x = MARGIN_LEFT;
    for (int j = 0; j < this->header->GetColumnCount(); j++) {
        if (abs(x + this->header->GetColumnWidths()[j] - xPos) < RESIZE_MARGIN && yPos < this->header->GetColumnHeight()) {
            this->resizingColumn = j;
            this->resizeStartX = xPos;
            this->resizeStartWidth = this->header->GetColumnWidths()[j];
            SetCapture(hwnd);
            return true;
        }
        x += this->header->GetColumnWidths()[j];
    }
    return false;
}

bool CustomTable::HandleHeaderClick(int xPos, int yPos, UINT uMsg, bool useSelection) {
    int x = MARGIN_LEFT;
    for (int j = 0; j < this->header->GetColumnCount(); j++) {
        if (xPos > x && xPos < x + this->header->GetColumnWidths()[j] && yPos < this->header->GetColumnHeight()) {
            if (useSelection) {
                selectedColumn = j;
                selectedItem = -1;  // Сбрасываем выделение строки
            }

            SendMessage(this->hwndParent, uMsg, (WPARAM)(j), 0);
            InvalidateRect(this->hwnd, NULL, TRUE);
            return true;
        }
        x += this->header->GetColumnWidths()[j];
    }
    return false;
}

void CustomTable::HandleCellClick(int xPos, int yPos, UINT uMsg, bool useSelection) {
    int x = MARGIN_LEFT;
    for (int i = 0; i < this->header->GetColumnCount(); i++) {
        x += this->header->GetColumnWidths()[i];
    }

    if (xPos > x) {
        return;
    }


    int itemIndex = (yPos - this->header->GetColumnHeight()) / this->itemHeight + this->scrollPos;
    if (itemIndex >= 0 && itemIndex < this->items.size()) {
        int columnIndex = GetColumnIndexAtX(xPos);

        if (useSelection) {
            this->selectedItem = itemIndex;
            this->selectedColumn = columnIndex;
        }

        WPARAM wParam = (static_cast<WPARAM>(itemIndex) << 16) | (columnIndex & 0xFFFF);
        SendMessage(this->hwndParent, uMsg, wParam, 0);

        InvalidateRect(this->hwnd, NULL, TRUE);
    }
}

int CustomTable::GetColumnIndexAtX(int xPos) {
    int columnIndex = 0;
    int xOffset = MARGIN_LEFT;
    for (int col = 0; col < this->header->GetColumnCount(); col++) {
        if (xPos >= xOffset && xPos < xOffset + this->header->GetColumnWidths()[col]) {
            columnIndex = col;
            break;
        }
        xOffset += this->header->GetColumnWidths()[col];
    }
    return columnIndex;
}

void CustomTable::OnLButtonDown(LPARAM lParam) {
    int xPos = GET_X_LPARAM(lParam);
    int yPos = GET_Y_LPARAM(lParam);

    this->selectedColumn = -1;
    this->selectedItem = -1;

    DestroyHEdit();

    if (HandleColumnResize(xPos, yPos)) {
        return;
    }

    if (HandleHeaderClick(xPos, yPos, WM_HEADER_CLICK, true)) {
        return;
    }

    HandleCellClick(xPos, yPos, WM_CELL_CLICK, true);
}

void CustomTable::OnRButtonDown(LPARAM lParam) {
    int xPos = GET_X_LPARAM(lParam);
    int yPos = GET_Y_LPARAM(lParam);

    DestroyHEdit();

    if (HandleHeaderClick(xPos, yPos, WM_HEADER_RCLICK, false)) {
        return;
    }

    HandleCellClick(xPos, yPos, WM_CELL_RCLICK, false);
}

POINT CustomTable::CalculateEditPosition(int itemIndex, int columnIndex) {
    int editX = MARGIN_LEFT;
    for (int i = 0; i < columnIndex; i++) {
        editX += this->header->GetColumnWidths()[i];
    }

    int editY = (itemIndex != -1) ? (itemIndex - this->scrollPos) * this->itemHeight + this->header->GetColumnHeight() : 0;

    return { editX, editY };
}

SIZE CustomTable::CalculateEditSize(int itemIndex, int columnIndex) {
    int editWidth = this->header->GetColumnWidths()[columnIndex];
    int editHeight = (itemIndex != -1) ? this->itemHeight : this->header->GetColumnHeight();

    return { editWidth, editHeight };
}

CustomHeader* CustomTable::GetHeaderClass() {
    return this->header;
}

WNDPROC currentEditCellProc;

LRESULT CALLBACK EditCellProc(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam) {
    if (msg == WM_KEYDOWN && wParam == VK_RETURN) {
        CustomTable* table = (CustomTable*)GetWindowLongPtr(hwnd, GWLP_USERDATA);
        if (table) {
            // Retrieve the input text
            wchar_t directoryPath[MAX_PATH] = { 0 };
            GetWindowTextW(hwnd, directoryPath, MAX_PATH);

            std::wstring text(directoryPath);
            if (table->GetSelectedItem() != -1) {
                table->GetItems()[table->GetSelectedItem()].columns[table->GetSelectedColumn()] = text;
            }
            else {
                table->GetHeaderClass()->GetColumnHeaders()[table->GetSelectedColumn()] = text;
            }

            DestroyWindow(hwnd);
            table->SetHEdit(nullptr); // Clear the edit handle
            InvalidateRect(table->GetHWND(), NULL, TRUE); // Redraw the table to show updated data
        }
        return 0;
    }
    return CallWindowProc(currentEditCellProc, hwnd, msg, wParam, lParam);
}

void CustomTable::CreateEditControl(int itemIndex, int columnIndex, POINT editPosition, SIZE editSize) {
    std::wstring initialText;

    if (itemIndex != -1) {
        initialText = this->items[itemIndex].columns[columnIndex].c_str();
    }
    else {
        initialText = this->header->GetColumnHeaders()[columnIndex].c_str();
    }

    hEdit = CreateWindowEx(WS_EX_TOOLWINDOW, WC_EDIT, initialText.c_str(),
        WS_CHILD | WS_VISIBLE | ES_AUTOHSCROLL | WS_BORDER,
        editPosition.x, editPosition.y, editSize.cx, editSize.cy, this->hwnd, NULL, NULL, NULL);

    // Set font for the edit control based on item index
    if (itemIndex != -1) {
        HFONT hFont = itemIndex % 2 == 0 ? this->hFileNamesFont : this->hFileSizesFont;
        SendMessage(hEdit, WM_SETFONT, (WPARAM)hFont, TRUE);
    }
    else {
        SendMessage(hEdit, WM_SETFONT, (WPARAM)this->hHeaderFont, TRUE);
    }

    SetFocus(hEdit);
    SetWindowLongPtr(hEdit, GWLP_USERDATA, (LONG_PTR)this);
    currentEditCellProc = (WNDPROC)SetWindowLongPtr(hEdit, GWLP_WNDPROC, (LONG_PTR)EditCellProc);
}

void CustomTable::DestroyHEdit() {
    if (hEdit) {
        DestroyWindow(hEdit);
    }
}

void CustomTable::OnLButtonDblClk(LPARAM lParam) {
    int xPos = GET_X_LPARAM(lParam);
    int yPos = GET_Y_LPARAM(lParam);

    this->selectedColumn = -1;
    this->selectedItem = -1;

    if (!HandleHeaderClick(xPos, yPos, WM_HEADER_DBLCLICK, true)) {
        HandleCellClick(xPos, yPos, WM_CELL_DBLCLICK, true);
    }

    int itemIndex = this->selectedItem;
    int columnIndex = this->selectedColumn;

    DestroyHEdit();

    InvalidateRect(this->hwnd, NULL, TRUE);
    if (itemIndex != -1 || columnIndex != -1) {
        // Calculate the position and size of the edit control
        POINT editPosition = CalculateEditPosition(itemIndex, columnIndex);
        SIZE editSize = CalculateEditSize(itemIndex, columnIndex);

        // Create the edit control
        CreateEditControl(itemIndex, columnIndex, editPosition, editSize);
    }
}

void CustomTable::OnRButtonDblClk(LPARAM lParam) {
    int xPos = GET_X_LPARAM(lParam);
    int yPos = GET_Y_LPARAM(lParam);

    DestroyHEdit();

    if (HandleHeaderClick(xPos, yPos, WM_HEADER_RDBLCLICK, false)) {
        return;
    }

    HandleCellClick(xPos, yPos, WM_CELL_RDBLCLICK, false);
}

void CustomTable::OnMouseMove(WPARAM wParam, LPARAM lParam) {
    if (wParam == MK_LBUTTON && this->resizingColumn != -1) {
        int deltaX = GET_X_LPARAM(lParam) - this->resizeStartX;
        
        HWND hEdit = GetDlgItem(this->hwnd, ID_ENTER_BTN_CLICKED);
        SetWindowPos(hEdit, NULL, hEditX, hEditY, this->header->GetColumnWidths()[0] - 2, this->header->GetColumnHeight() - 5, NULL);

        this->header->GetColumnWidths()[this->resizingColumn] = max(10, this->resizeStartWidth + deltaX);
        InvalidateRect(this->hwnd, NULL, TRUE);
        return;
    }
    int xPos = GET_X_LPARAM(lParam);
    int yPos = GET_Y_LPARAM(lParam);
    int x = MARGIN_LEFT;
    for (int j = 0; j < this->header->GetColumnHeaders().size(); j++) {
        if (abs(x + this->header->GetColumnWidths()[j] - xPos) < RESIZE_MARGIN && yPos < COLUMN_HEADER_HEIGHT) {
            SetCursor(LoadCursor(NULL, IDC_SIZEWE));
            return;
        }
        x += this->header->GetColumnWidths()[j];
    }
    SetCursor(LoadCursor(NULL, IDC_ARROW));
}

void CustomTable::OnSetFont(WPARAM wParam, LPARAM lParam) {
    if (lParam == ID_COMBOBOX_HEADER || lParam == ID_COMBOBOX_HEADER_SIZES) {
        this->hHeaderFont = (HFONT)wParam;
        SendMessage(GetDlgItem(this->hwnd, ID_ENTER_BTN_CLICKED), WM_SETFONT, (WPARAM)hHeaderFont, TRUE);
    }
    else if (lParam == ID_COMBOBOX_FILENAMES || lParam == ID_COMBOBOX_FILENAMES_SIZES) {
        this->hFileNamesFont = (HFONT)wParam;
    }
    else if (lParam == ID_COMBOBOX_FILESIZES || lParam == ID_COMBOBOX_FILESIZES_SIZES) {
        this->hFileSizesFont = (HFONT)wParam;
    }
    //Work with Font and send Params LPARAM and WPARAM
    this->UpdateItemHeight();
}

void CustomTable::OnPaint() {
    PAINTSTRUCT ps;
    HDC hdc = BeginPaint(hwnd, &ps);

    // 1. Создаем внеэкранный буфер (контекст рисования)
    HDC memDC = CreateCompatibleDC(hdc);

    RECT clientRect;
    GetClientRect(hwnd, &clientRect);

    //Отрисовка белым правой части таблицы, которая уходит далеко

    /*RECT originalRect = clientRect;
    int right = 0;
    for (int i = 0; i < this->header->GetColumnCount(); i++) {
        right += this->header->GetColumnWidths()[i];
    }
    originalRect.left = right;
    FillRect(hdc, &originalRect, (HBRUSH)GetStockObject(WHITE_BRUSH));

    clientRect.right = right;*/

    /////////////////////////////

    // Создаем совместимую поверхность (буфер) для рисования
    HBITMAP hbmMem = CreateCompatibleBitmap(hdc, clientRect.right, clientRect.bottom);
    HGDIOBJ hOldBitmap = SelectObject(memDC, hbmMem);

    // 2. Заполняем внеэкранный буфер фоновым цветом
    HBRUSH hBrush = CreateSolidBrush(RGB(30, 30, 30)); // Пример: темный фон
    FillRect(memDC, &clientRect, hBrush);
    DeleteObject(hBrush);

    // 3. Выполняем операции рисования на буфере (memDC)

    // Получаем ширину вертикального скроллбара
    int scrollBarWidth = GetSystemMetrics(SM_CXVSCROLL);

    // Уменьшаем ширину клиентской области на ширину скроллбара
    int clientWidth = (clientRect.right - clientRect.left) - 2;

    FillRect(memDC, &clientRect, (HBRUSH)(COLOR_WINDOW + 1));

    // Отрисовка заголовков
    RECT headerRect = { MARGIN_LEFT, 0, clientWidth, this->header->GetColumnHeight() };
    HBRUSH headerBrush = CreateSolidBrush(RGB(191, 166, 42)); // Мягкий светло-серый цвет
    FillRect(memDC, &headerRect, headerBrush);
    DeleteObject(headerBrush);

    // Отрисовка текста заголовков
    SetBkMode(memDC, TRANSPARENT);
    SetTextColor(memDC, RGB(50, 50, 50));

    HPEN hThinPen = CreatePen(PS_SOLID, 1, RGB(217, 37, 187));
    HPEN hOldPen = (HPEN)SelectObject(memDC, hThinPen);

    //Вертикальная линия справа
    MoveToEx(memDC, headerRect.right, headerRect.top, NULL);
    LineTo(memDC, headerRect.right, headerRect.bottom);

    // Горизонтальная линия над заголовками
    MoveToEx(memDC, MARGIN_LEFT, 1, NULL);
    LineTo(memDC, clientWidth, 1);

    //Вертикальная линия слева заголовков
    MoveToEx(memDC, headerRect.left + MARGIN_LEFT, headerRect.top, NULL);
    LineTo(memDC, headerRect.left + MARGIN_LEFT, headerRect.bottom);

    hThinPen = CreatePen(PS_SOLID, 1, RGB(50, 50, 50));
    hOldPen = (HPEN)SelectObject(memDC, hThinPen);

    // Горизонтальная линия под заголовками
    MoveToEx(memDC, MARGIN_LEFT, this->header->GetColumnHeight() - 1, NULL);
    LineTo(memDC, clientWidth, this->header->GetColumnHeight() - 1);

    // Отрисовка текста заголовков
    int x = MARGIN_LEFT;
    for (int j = 0; j < this->header->GetColumnCount(); j++) {
        RECT textRect = { x + HEADER_PADDING, 0, x + this->header->GetColumnWidths()[j] - HEADER_PADDING, this->header->GetColumnHeight() };
        RECT highLightRect = { x + 1, 2, x + this->header->GetColumnWidths()[j], this->header->GetColumnHeight() - 1 };

        if (j == selectedColumn && selectedItem == -1) {
            HBRUSH selectionBrush = CreateSolidBrush(RGB(180, 220, 255));
            FillRect(memDC, &highLightRect, selectionBrush);
            DeleteObject(selectionBrush);
        }

        TextOutWithFont(memDC, &textRect, this->header->GetColumnHeaders()[j].c_str(), hHeaderFont, DT_LEFT | DT_VCENTER | DT_SINGLELINE | DT_END_ELLIPSIS);

        MoveToEx(memDC, x + this->header->GetColumnWidths()[j], 1, NULL);
        LineTo(memDC, x + this->header->GetColumnWidths()[j], this->header->GetColumnHeight());
        x += this->header->GetColumnWidths()[j];
    }

    // Вертикальная линия в начале таблицы
    // Отрисовка элементов списка

    for (int i = scrollPos; i < items.size() && (i - scrollPos + 1) * itemHeight < clientRect.bottom; ++i) {
        RECT itemRect = { 0, (i - scrollPos) * itemHeight + this->header->GetColumnHeight(), clientWidth, (i - scrollPos + 1) * itemHeight + this->header->GetColumnHeight() };

        SetBkColor(memDC, RGB(255, 255, 255));
        FillRect(memDC, &itemRect, (HBRUSH)GetStockObject(WHITE_BRUSH));

        hThinPen = CreatePen(PS_SOLID, 1, RGB(217, 37, 187));
        hOldPen = (HPEN)SelectObject(memDC, hThinPen);

        MoveToEx(memDC, itemRect.left + MARGIN_LEFT, itemRect.top, NULL);
        LineTo(memDC, itemRect.left + MARGIN_LEFT, itemRect.bottom);

        hThinPen = CreatePen(PS_SOLID, 1, RGB(50, 50, 50));
        hOldPen = (HPEN)SelectObject(memDC, hThinPen);

        // Отрисовка текста элементов по колонкам
        int x = 0;
        for (int j = 0; j < this->header->GetColumnCount(); j++) {
            RECT columnRect = { x + HEADER_PADDING, itemRect.top, x + this->header->GetColumnWidths()[j] - HEADER_PADDING, itemRect.bottom };
            RECT highLightRect = { x + 1, itemRect.top, x + this->header->GetColumnWidths()[j], itemRect.bottom };

            if (i == selectedItem && j == selectedColumn) {
                HBRUSH selectionBrush = CreateSolidBrush(RGB(180, 220, 255));
                FillRect(memDC, &highLightRect, selectionBrush);
                DeleteObject(selectionBrush);
            }

            if (!items[i].columns[j].empty()) {
                HFONT hFont = j % 2 == 0 ? hFileNamesFont : hFileSizesFont;
                SelectObject(memDC, hFont);
                TextOutWithFont(memDC, &columnRect, items[i].columns[j].c_str(), hFont, DT_LEFT | DT_VCENTER | DT_SINGLELINE);
            }
            // Вертикальная линия между колонками
            MoveToEx(memDC, x + this->header->GetColumnWidths()[j], itemRect.top, NULL);
            LineTo(memDC, x + this->header->GetColumnWidths()[j], itemRect.bottom);
            x += this->header->GetColumnWidths()[j];

            //Сразу для вертикальных линий
            //MoveToEx(hdc, x, clientRect.top, NULL);
            //LineTo(hdc, x, clientRect.bottom);
        }

        // Горизонтальная линия под текущим элементом
        if (i == items.size() - 1) {
            hThinPen = CreatePen(PS_SOLID, 1, RGB(217, 37, 187));
            hOldPen = (HPEN)SelectObject(memDC, hThinPen);
        }

        MoveToEx(memDC, 0, itemRect.bottom - 1, NULL);
        LineTo(memDC, clientWidth, itemRect.bottom - 1);  // Отрисовка горизонтальной линии

        hThinPen = CreatePen(PS_SOLID, 1, RGB(217, 37, 187));
        hOldPen = (HPEN)SelectObject(memDC, hThinPen);

        MoveToEx(memDC, itemRect.right, itemRect.top, NULL);
        LineTo(memDC, itemRect.right, itemRect.bottom);

        hThinPen = CreatePen(PS_SOLID, 1, RGB(50, 50, 50));
        hOldPen = (HPEN)SelectObject(memDC, hThinPen);
    }

    SelectObject(memDC, hOldPen);
    DeleteObject(hThinPen);

    // 4. Копируем содержимое буфера на экран
    BitBlt(hdc, 0, 0, clientRect.right, clientRect.bottom, memDC, 0, 0, SRCCOPY);

    // 5. Освобождаем ресурсы
    SelectObject(memDC, hOldBitmap);
    DeleteObject(hbmMem);
    DeleteDC(memDC);


    EndPaint(hwnd, &ps);
}

#define IDT_SINGLE_CLICK_TIMER 0x1
#define IDT_SINGLE_RCLICK_TIMER 0x2

#define TIMER_DELAY 150

LRESULT CALLBACK CustomTable::CustomListWndProc(HWND hwnd, UINT message, WPARAM wParam, LPARAM lParam) {
    CustomTable* listView = (CustomTable*)GetWindowLongPtr(hwnd, GWLP_USERDATA);

    switch (message)
    {
    case WM_CREATE:
    {
        CREATESTRUCT* pCreate = reinterpret_cast<CREATESTRUCT*>(lParam);
        CustomTable* pThis = reinterpret_cast<CustomTable*>(pCreate->lpCreateParams);
        SetWindowLongPtr(hwnd, GWLP_USERDATA, (LONG_PTR)pThis);
    }
    break;

    case WM_DESTROY:
    {
        KillTimer(hwnd, IDT_SINGLE_CLICK_TIMER);
    }
    return 0;

    case WM_SIZE:
    {
        listView->OnSize();
    }
    return 0;

    case WM_VSCROLL:
    {
        listView->OnVScroll(wParam);
    }
    return 0;

    case WM_LBUTTONDOWN:
    {
        if (!listView->ignoreNextLButtonDown) {
            SetTimer(hwnd, IDT_SINGLE_CLICK_TIMER, TIMER_DELAY, NULL);
        } else {
            listView->ignoreNextLButtonDown = false;
        }
    }
    return 0;

    case WM_LBUTTONUP:
    {
        if (!listView->ignoreNextLButtonUp) {
            listView->OnLButtonUp(lParam);
        }
        else {
            listView->ignoreNextLButtonUp = false;
        }
    }
    return 0;

    case WM_LBUTTONDBLCLK:
    {
        listView->ignoreNextLButtonDown = true;
        listView->ignoreNextLButtonUp = true;
        KillTimer(hwnd, IDT_SINGLE_CLICK_TIMER);
        listView->OnLButtonDblClk(lParam);
    }
    return 0;

    case WM_TIMER: 
    {
        if (wParam == IDT_SINGLE_CLICK_TIMER) {
            KillTimer(hwnd, IDT_SINGLE_CLICK_TIMER);
            if (!listView->ignoreNextLButtonDown) {
                POINT point;
                GetCursorPos(&point);
                ScreenToClient(hwnd, &point);
                LPARAM newLParam = MAKELPARAM(point.x, point.y);
                listView->OnLButtonDown(newLParam);
            }
            listView->ignoreNextLButtonDown = false;
        }

        if (wParam == IDT_SINGLE_RCLICK_TIMER) {
            KillTimer(hwnd, IDT_SINGLE_RCLICK_TIMER);
            if (!listView->ignoreNextRButtonDown) {
                POINT point;
                GetCursorPos(&point);
                ScreenToClient(hwnd, &point);
                LPARAM newLParam = MAKELPARAM(point.x, point.y);
                listView->OnRButtonDown(newLParam);
            }
            listView->ignoreNextRButtonDown = false;
        }
    }
    return 0;

    case WM_RBUTTONDOWN:
    {
        if (!listView->ignoreNextRButtonDown) {
            SetTimer(hwnd, IDT_SINGLE_RCLICK_TIMER, TIMER_DELAY, NULL);
        }
        else {
            listView->ignoreNextRButtonDown = false;
        }
    }
    return 0;

    case WM_RBUTTONDBLCLK:
    {
        listView->ignoreNextRButtonDown = true;
        listView->ignoreNextRButtonUp = true;
        KillTimer(hwnd, IDT_SINGLE_RCLICK_TIMER);
        listView->OnRButtonDblClk(lParam);
    }
    return 0;

    case WM_RBUTTONUP:
    {
        if (!listView->ignoreNextRButtonUp) {
            //listView->OnRButtonUp(lParam);
        }
        else {
            listView->ignoreNextRButtonUp = false;
        }
    }
    return 0;

    case WM_MOUSEWHEEL:
    {
        listView->OnMouseWheel(wParam);
    }
    return 0;

    case WM_MOUSEMOVE:
    {
        listView->OnMouseMove(wParam, lParam);
    }
    return 0;

    case WM_ERASEBKGND:
    {

    }
    return 1;

    case WM_SETFONT:
    {
        listView->OnSetFont(wParam, lParam);
    }
    break;

    case WM_PAINT:
    {
        listView->OnPaint();
    }
    return 0;

    default:
        return DefWindowProc(hwnd, message, wParam, lParam);
    }

    return 0;
}