namespace Luna.Framework.Bindings;

using System.Reflection;
using System.Runtime.InteropServices;


public static class User32 {
    [DllImport("user32.dll")]
    public static extern int MessageBoxW(int hWnd, string text, string caption, uint type);

    [DllImport("user32.dll")]
    public static extern int MessageBoxA(int hWnd, string text, string caption, uint type);

    [DllImport("user32.dll")]
    public static extern int MessageBox(int hWnd, string text, string caption, uint type);

    [DllImport("user32.dll")]
    public static extern int MessageBoxEx(int hWnd, string text, string caption, uint type, ushort langId);

    // Constants
    public static int MB_ABORTRETRYIGNORE = 0x000000002;
    public static int MB_CANCELTRYCONTINUE = 0x00000006;
    public static int MB_HELP = 0x00004000;
    public static int MB_OK = 0x000000000;
    public static int MB_OKCANCEL = 0x000000001;
    public static int MB_RETRYCANCEL = 0x000000005;
    public static int MB_YESNO = 0x000000004;
    public static int MB_YESNOCANCEL = 0x000000003;
    public static int MB_ICONEXCLAMATION = 0x0000000030;
    public static int MB_ICONWARNING = 0x0000000030;
    public static int MB_ICONINFORMATION = 0x0000000040;
    public static int MB_ICONASTERISK = 0x0000000040;
    public static int MB_ICONQUESTION = 0x0000000020;
    public static int MB_ICONSTOP = 0x0000000010;
    public static int MB_ICONERROR = 0x0000000010;
    public static int MB_ICONHAND = 0x0000000010;
    public static int MB_DEFBUTTON1 = 0x0000000000;
    public static int MB_DEFBUTTON2 = 0x0000000100;
    public static int MB_DEFBUTTON3 = 0x0000000200;
    public static int MB_DEFBUTTON4 = 0x0000000300;
    public static int MB_APPLMODAL = 0x0000000000;
    public static int MB_SYSTEMMODAL = 0x0000001000;
    public static int MB_TASKMODAL = 0x0000002000;
    public static int MB_DEFAULT_DESKTOP_ONLY = 0x0002000000;
    public static int MB_RIGHT = 0x0008000000;
    public static int MB_RTLREADING = 0x0010000000;
    public static int MB_SETFOREGROUND = 0x0001000000;
    public static int MB_TOPMOST = 0x0004000000;
    public static int MB_SERVICE_NOTIFICATION = 0x0020000000;
    public static int IDABORT = 3;
    public static int IDCANCEL = 2;
    public static int IDCONTINUE = 11;
    public static int IDIGNORE = 5;
    public static int IDNO = 7;
    public static int IDOK = 1;
    public static int IDRETRY = 4;
    public static int IDTRYAGAIN = 10;
    public static int IDYES = 6;
}