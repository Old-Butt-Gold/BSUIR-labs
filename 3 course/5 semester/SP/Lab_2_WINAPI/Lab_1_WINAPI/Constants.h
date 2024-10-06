#pragma once

#include "string"

#define WC_CUSTOM_LIST_VIEW L"CustomListView"
#define SCREEN_WIDTH  GetSystemMetrics(SM_CXSCREEN)
#define SCREEN_HEIGHT GetSystemMetrics(SM_CYSCREEN)

#define FontWindowClass L"FontWindowClass"

struct FileData {
    std::wstring fileName;
    std::wstring fileSize;
};

//ID FOR HMENU COMPONENTS

#define EnterButtonClicked 0x1c
#define ID_SELECT_FONT_BTN 0x1010

#define ID_FONTS_LOAD_BTN 0x10

#define ID_COMBOBOX_HEADER 0x101
#define ID_COMBOBOX_HEADER_SIZES 0x102
#define ID_COMBOBOX_FILENAMES 0x103
#define ID_COMBOBOX_FILENAMES_SIZES 0x104
#define ID_COMBOBOX_FILESIZES 0x105
#define ID_COMBOBOX_FILESIZES_SIZES 0x106

#define ID_LIST_VIEW 0x107

#define hEditX 1
#define hEditY 3

#define FONTS_DIRECTORY L"D://Fonts//"

#define DEFAULT_FONT L"Segoe UI"
#define DEFAULT_FONT_SIZE 21

struct FontSettings {
    std::wstring fileNameFontFamily;  // Font family for file names
    std::wstring fileNameFontPath;
    int fileNameFontSize;             // Font size for file names
    HFONT fileNameFontHandle;         // Handle for the file name font

    std::wstring headerFontFamily;    // Font family for headers
    std::wstring headerFontPath;
    int headerFontSize;               // Font size for headers
    HFONT headerFontHandle;           // Handle for the header font

    std::wstring fileSizeFontFamily;  // Font family for file sizes
    std::wstring fileSizeFontPath;
    int fileSizeFontSize;             // Font size for file sizes
    HFONT fileSizeFontHandle;         // Handle for the file size font

};