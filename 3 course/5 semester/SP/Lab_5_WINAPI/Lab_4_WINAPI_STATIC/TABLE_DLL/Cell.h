#pragma once

#ifndef CELL_H
#define CELL_H

#include <string>

class Cell {
public:
    Cell();
    Cell(const std::wstring& text, bool isEditable);

    void SetText(const std::wstring& newText);
    const std::wstring& GetText() const;

    bool IsEditable() const
    {
        return isEditable;
    }

    void SetEditable(bool isEditable)
    {
        this->isEditable = isEditable;
    }

private:
    std::wstring text;
    bool isEditable;
};

#endif // CELL_H
