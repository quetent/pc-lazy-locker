using System.Runtime.InteropServices;

namespace PCLazyLocker;

public partial class NativeMethods
{
    [LibraryImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool LockWorkStation();

    [LibraryImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.I4)]
    private static partial int GetAsyncKeyState(int virtualKey);

    public static bool LockPC()
    {
        return LockWorkStation();
    }

    public static int GetKeyState(int virtualKey)
    {
        return GetAsyncKeyState(virtualKey);
    }
}
