#pragma once

#ifndef HEADER_TABLE_H
#define HEADER_TABLE_H

#include <windows.h>
#include <vector>
#include <string>
#include <CommCtrl.h>

#define COLUMN_HEADER_HEIGHT 40

class HeaderTable {
public:
    HeaderTable(int colCount, int totalWidth);

    void SetColumnHeader(int columnIndex, const std::wstring& header);
    void SetColumnWidth(int columnIndex, int width);

    std::wstring GetColumnHeader(int columnIndex);
    int GetColumnWidth(int columnIndex);

    int GetColumnCount();
    int GetColumnHeight();
    void SetColumnHeight(int width);

    void Resize(int totalWidth);

    std::vector<int>& GetColumnWidths();
    std::vector<std::wstring>& GetColumnHeaders();

private:
    int colCount;
    int totalWidth;
    int columnHeaderHeight;

    std::vector<int> columnWidths;
    std::vector<std::wstring> headers;

    void InitializeColumns();
};

#endif

