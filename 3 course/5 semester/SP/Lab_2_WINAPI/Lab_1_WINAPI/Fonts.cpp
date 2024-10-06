#define UNICODE
#define _UNICODE

#include "fonts.h"
#include "Constants.h"
#include "vector"

int GetTextHeightInPixels(HDC hdc, const std::wstring& text, HFONT hFont) {
    // Select the new font into the device context
    HFONT hOldFont = (HFONT)SelectObject(hdc, hFont);

    // Get the text dimensions using the specified font
    SIZE size;
    if (GetTextExtentPoint32(hdc, text.c_str(), text.length(), &size) == 0) {
        // Handle error: could not get text dimensions
        SelectObject(hdc, hOldFont); // Restore old font
        return -1;
    }

    // Get the font's height
    TEXTMETRIC tm;
    GetTextMetrics(hdc, &tm);

    // Restore the old font
    SelectObject(hdc, hOldFont);

    // Return the text height
    return tm.tmHeight; // Use tmHeight for more accurate height
}

int GetTextLengthInPixels(HDC hdc, const std::wstring& text, HFONT hFont) {
    // Select the new font into the device context
    HFONT hOldFont = (HFONT)SelectObject(hdc, hFont);

    // Get the text dimensions using the specified font
    SIZE size;
    if (GetTextExtentPoint32(hdc, text.c_str(), text.length(), &size) == 0) {
        // Handle error: could not get text dimensions
        SelectObject(hdc, hOldFont); // Restore old font
        return -1;
    }

    // Restore the old font
    SelectObject(hdc, hOldFont);

    // Return the width of the text
    return size.cx; // Return the width of the text
}

void TextOutWithFont(HDC hdc, RECT* rect, LPCWSTR text, HFONT hFont, UINT format) {

    // Выбираем шрифт в контекст устройства
    HFONT hOldFont = (HFONT)SelectObject(hdc, hFont);

    // Выводим текст с использованием DrawText
    DrawText(hdc, text, -1, rect, format);

    // Восстанавливаем старый шрифт
    SelectObject(hdc, hOldFont);
}

std::vector<std::wstring> gLoadedFonts;

// Функция для загрузки кастомного шрифта из файла
bool LoadCustomFont(const std::wstring& fontPath) {
    if (fontPath.empty()) return false;

    bool findResult = false;
    for (int i = 0; i < gLoadedFonts.size(); i++) {
        if (gLoadedFonts[i] == fontPath) {
            findResult = true;
        }
    }

    if (findResult) {
        return true;
    }

    if (AddFontResourceEx(fontPath.c_str(), FR_PRIVATE, 0) > 0) {
        gLoadedFonts.push_back(fontPath);
        return true;
    }
    return false;
}

HFONT CreateFontWithParams(std::wstring fontPath, std::wstring fontFamily, int fontSize) {
    HFONT hFont;

    // Check if the fontNameOrPath is a valid file path
    if (fontPath.length() > 0 && fontFamily != DEFAULT_FONT) {
        if (fontPath.find(L".ttf") != std::wstring::npos || fontPath.find(L".otf") != std::wstring::npos) {
            if (!LoadCustomFont(fontPath)) {
                MessageBox(NULL, L"Ошибка загрузки шрифта!", L"Ошибка", MB_OK | MB_ICONERROR);
                return nullptr;
            }
        }

        hFont = (HFONT)CreateFont(
            fontSize,                // Height of the font
            0,                       // Width of the font
            0,                       // Angle of escapement
            0,                       // Base-line orientation angle
            FW_NORMAL,               // Font weight
            FALSE,                  // Italic
            FALSE,                  // Underline
            FALSE,                  // Strikeout
            DEFAULT_CHARSET,        // Character set identifier
            OUT_DEFAULT_PRECIS,     // Output precision
            CLIP_DEFAULT_PRECIS,    // Clipping precision
            ANTIALIASED_QUALITY,         // Output quality
            DEFAULT_PITCH | FF_DONTCARE,            // Pitch and family
            fontFamily.c_str()      // Font name or path
        );
    }
    else {
        LOGFONT logFont;
        ZeroMemory(&logFont, sizeof(LOGFONT));
        HFONT defaultFont = (HFONT)GetStockObject(DEFAULT_GUI_FONT);

        // Retrieve the attributes of the default GUI font.
        GetObject(defaultFont, sizeof(LOGFONT), &logFont);

        // Set the new font size.
        logFont.lfHeight = fontSize;

        // Create a new font based on the modified LOGFONT structure.
        hFont = CreateFontIndirect(&logFont);
    }

    return hFont;

}

void UnloadCustomFonts() {
    for (const auto& fontPath : gLoadedFonts) {
        RemoveFontResourceEx(fontPath.c_str(), FR_PRIVATE, 0);
    }
    gLoadedFonts.clear();
}

// Delete the font object to free resources
 // DeleteObject(hFont);/