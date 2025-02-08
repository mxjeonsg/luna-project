using System.Runtime.InteropServices;
using System;
using System.ComponentModel.DataAnnotations;

namespace Luna.Framework.Bindings;

public static class Secur32 {
    public const string LIBRARY_NAME = "secur32.dll";

    [DllImport("secur32.dll", SetLastError = true)]
    public static extern int AcquireCredentialsHandle(
        string pszPrincipal,
        string pszPackage,
        int fCredentialUse,
        nint pvLogonId,
        nint pAuthData,
        nint pGetKeyFn,
        nint pvGetKeyArgument,
        out CredHandle phCredential,
        out TimeStamp ptsExpiry
    );
}

[StructLayout(LayoutKind.Sequential)]
public struct CredHandle {
    public nint dwLower, dwUpper;
}

[StructLayout(LayoutKind.Sequential)]
public struct TimeStamp {
    public uint lowPart;
    public int highPart;
}