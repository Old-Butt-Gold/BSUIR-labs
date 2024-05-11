#include "stdio.h"
#include "pci.h"
#include "sys/io.h"
#include "stdbool.h"

/*
2.	Если устройство не мост(0-бит поля Header Type =0) вывести  и расшифровать значение полей базовых регистров памяти.
6.	Если устройство не мост(0-бит поля Header Type =0) вывести  и расшифровать значение поля Interrupt Pin.
7.	Если устройство не мост(0-бит поля Header Type =0) вывести  и расшифровать значение поля Interrupt Line.
*/

bool isBridge(unsigned mainAddress) {
    outl(mainAddress + 0x0C, 0xCF8);
    return (inl(0xCFC) >> 16) != 0;
}

void deviceInfo(unsigned registerOutput)
{
    unsigned short vendorName = registerOutput;
    unsigned short deviceName = registerOutput >> 16;
    bool VendorIsFound = false;
    bool DeviceIsFound = false;
    for (int i = 0; i < PCI_VENTABLE_LEN; i++)
        if (VendorIsFound = vendorName == PciVenTable[i].VendorId)
        {
            printf("Vendor's 16-bit code: %04X\n", vendorName);
            printf("Vendor: %s\n", PciVenTable[i].VendorName);
            break;
        }
    if (VendorIsFound)
    {
        for (int i = 0; i < PCI_DEVTABLE_LEN; i++)
            if (DeviceIsFound = deviceName == PciDevTable[i].DeviceId)
            {
                printf("Device's 16-bit code: %04X\n", deviceName);
                printf("Device: %s\n", PciDevTable[i].DeviceName);
                break;
            }
        if (!DeviceIsFound)
            printf("Device not found \n");
    }
    else
        printf("Vendor not found \n");
}

void decodeBAR(unsigned mainAddress) {
    if (!isBridge(mainAddress))
        printf("Base Address Registers (BARs):\n");
        for (int i = 0; i < 6; i++)
        {
            outl(mainAddress + 0x10 + i * 4, 0xCF8);
            unsigned barValue = inl(0xCFC);
            bool isMemory = (barValue & 1) == 0;
            bool isPrefetchable = (barValue & 8);
            printf("BAR %d: %s,", i + 1, isMemory ? "Memory" : "I/O");
            switch (barValue & 6) {
                case 0:
                    printf(" Any 32-bit address space\n");
                    break;
                case 2:
                    printf(" Below 1MB\n");
                    break;
                case 4:
                    printf(" Any 64-bit address space\n");
                    break;
                default:
                    printf(" Reserved\n");
                    break;
            }
            printf("IsPrefetchable: %s\n", isPrefetchable ? "TRUE" : "FALSE");
            printf("Base address: %08X\n", barValue >> 4);
        }
}

void decodeInterrupt(unsigned mainAddress)
{
    if(!isBridge(mainAddress))
    {
        outl(mainAddress + 0x3C, 0xCF8);
        unsigned short inter = inl(0xCFC);
        printf("Interrupt Line: %02X \n", (unsigned char) inter);
        printf("Interrupt Pin: %02X \n", (inter >> 8));
    }
}

void printInfo(int bus, int device, int func) {
    unsigned mainAddress = (1 << 31) + (bus << 16) + (device << 11) + (func << 8);
    unsigned registerAddress = mainAddress;
    outl(registerAddress,0xCF8);
    unsigned registerOutput = inl(0xCFC);
    if ((registerOutput >> 16) != 0xFFFF) {
        printf("Address: %08X\nBus Number: %02X\nDevice Number: %02X\nFunction Number: %01X\n", mainAddress, bus, device, func);
        deviceInfo(registerOutput);
        printf("IsBridge: %s\n", (isBridge(mainAddress) ? "True" : "False"));
        decodeInterrupt(mainAddress);
        decodeBAR(mainAddress);
        printf("--------------------------------------------------------------\n");
    }
}

int main()
{
    if(iopl(3))
    {
        printf("No access");
        return -1;
    }
    for (int busNumber = 0; busNumber < 256; busNumber++)
        for (int deviceNum = 0; deviceNum < 32; deviceNum++)
            for (int funcNum = 0; funcNum < 8; funcNum++)
                printInfo(busNumber, deviceNum, funcNum);
    return 0;
}