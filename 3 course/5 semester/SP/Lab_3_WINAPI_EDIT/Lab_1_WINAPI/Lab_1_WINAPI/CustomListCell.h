#pragma once

#ifndef CUSTOM_LIST_CELL_H
#define CUSTOM_LIST_CELL_H

#include <string>

class CustomListCell {
public:
    CustomListCell();

    CustomListCell(const std::wstring& text);

    void SetText(const std::wstring& newText);

    const std::wstring& GetText();

private:
    std::wstring text; 
};

#endif 