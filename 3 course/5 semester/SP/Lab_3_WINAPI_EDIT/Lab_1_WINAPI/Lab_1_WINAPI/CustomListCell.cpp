#include "CustomListCell.h"

CustomListCell::CustomListCell() {
	this->text = L"";
}

CustomListCell::CustomListCell(const std::wstring& text) {
	this->text = text;
}

void CustomListCell::SetText(const std::wstring& text) {
	this->text = text;
}

const std::wstring& CustomListCell::GetText() {
	return this->text;
}