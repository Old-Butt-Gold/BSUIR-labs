#pragma once

#include "windows.h"
#include <string>

int GetTextHeightInPixels(HDC hdc, const std::wstring& text, HFONT hFont);
int GetTextLengthInPixels(HDC hdc, const std::wstring& text, HFONT hFont);
void TextOutWithFont(HDC hdc, RECT* rect, LPCWSTR text, HFONT hFont, UINT format);
HFONT CreateFontWithParams(std::wstring fontPath, std::wstring fontFamily, int fontSize);
void UnloadCustomFonts();