#include "HeaderTable.h"

HeaderTable::HeaderTable(int colCount, int totalWidth)
    : colCount(colCount), totalWidth(totalWidth) {
    InitializeColumns();
    this->columnHeaderHeight = COLUMN_HEADER_HEIGHT;
}

void HeaderTable::InitializeColumns() {
    headers.resize(colCount, L"");
    columnWidths.resize(colCount, totalWidth / colCount);
}

void HeaderTable::Resize(int totalWidth)
{
    this->totalWidth = totalWidth;
    for (int i = 0; i < this->colCount; i++)
    {
        columnWidths[i] = this->totalWidth / colCount;
    }
}


int HeaderTable::GetColumnHeight() {
    return this->columnHeaderHeight;
}

void HeaderTable::SetColumnHeight(int width) {
    this->columnHeaderHeight = width;
}

void HeaderTable::SetColumnHeader(int columnIndex, const std::wstring& header) {
    if (columnIndex >= 0 && columnIndex < colCount) {
        headers[columnIndex] = header;
    }
}

std::wstring HeaderTable::GetColumnHeader(int columnIndex) {
    if (columnIndex >= 0 && columnIndex < colCount) {
        return headers[columnIndex];
    }
    return L"";
}

void HeaderTable::SetColumnWidth(int columnIndex, int width) {
    if (columnIndex >= 0 && columnIndex < colCount) {
        columnWidths[columnIndex] = width;
    }
}

int HeaderTable::GetColumnWidth(int columnIndex) {
    if (columnIndex >= 0 && columnIndex < colCount) {
        return columnWidths[columnIndex];
    }
    return 0;
}

int HeaderTable::GetColumnCount() {
    return colCount;
}

std::vector<int>& HeaderTable::GetColumnWidths() {
    return columnWidths;
}

std::vector<std::wstring>& HeaderTable::GetColumnHeaders() {
    return headers;
}
