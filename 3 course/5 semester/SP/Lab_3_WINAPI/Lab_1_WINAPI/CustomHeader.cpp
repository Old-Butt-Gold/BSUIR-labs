#include "CustomHeader.h"

CustomHeader::CustomHeader(int colCount, int totalWidth)
    : colCount(colCount), totalWidth(totalWidth) {
    InitializeColumns();
    this->columnHeaderHeight = COLUMN_HEADER_HEIGHT;
}

void CustomHeader::InitializeColumns() {
    headers.resize(colCount, L"");
    columnWidths.resize(colCount, totalWidth / colCount);
}

int CustomHeader::GetColumnHeight() {
    return this->columnHeaderHeight;
}

void CustomHeader::SetColumnHeight(int width) {
    this->columnHeaderHeight = width;
}

void CustomHeader::SetColumnHeader(int columnIndex, const std::wstring& header) {
    if (columnIndex >= 0 && columnIndex < colCount) {
        headers[columnIndex] = header;
    }
}

std::wstring CustomHeader::GetColumnHeader(int columnIndex) {
    if (columnIndex >= 0 && columnIndex < colCount) {
        return headers[columnIndex];
    }
    return L"";
}

void CustomHeader::SetColumnWidth(int columnIndex, int width) {
    if (columnIndex >= 0 && columnIndex < colCount) {
        columnWidths[columnIndex] = width;
    }
}

int CustomHeader::GetColumnWidth(int columnIndex) {
    if (columnIndex >= 0 && columnIndex < colCount) {
        return columnWidths[columnIndex];
    }
    return 0;
}

int CustomHeader::GetColumnCount() {
    return colCount;
}

std::vector<int>& CustomHeader::GetColumnWidths() {
    return columnWidths;
}

std::vector<std::wstring>& CustomHeader::GetColumnHeaders() {
    return headers;
}
