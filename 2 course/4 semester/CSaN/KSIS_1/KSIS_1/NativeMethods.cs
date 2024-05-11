using System.Runtime.InteropServices;

namespace KSIS_1;

static class NativeMethods
{
    [DllImport("iphlpapi.dll", ExactSpelling = true)]
    public static extern int SendARP(int destIP, int srcIP, byte[] pMacAddr, ref uint phyAddrLen);
}