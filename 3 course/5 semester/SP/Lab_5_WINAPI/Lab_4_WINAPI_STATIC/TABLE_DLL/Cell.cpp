#include "Cell.h"

Cell::Cell() : text(L""), isEditable(false) {}

Cell::Cell(const std::wstring& text, bool isEditable) : text(text), isEditable(isEditable) {}

void Cell::SetText(const std::wstring& newText) {
	text = newText;
}

const std::wstring& Cell::GetText() const {
	return text;
}