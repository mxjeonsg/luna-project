using System.Reflection;
using System.Runtime.InteropServices;

namespace Luna.Framework.Bindings;

[StructLayout(LayoutKind.Sequential)] public struct WSAData {
    public ushort wVersion;
    public ushort wHighVersion;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 257)] public string szDescription;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 129)] public string szSystemStatus;
    public ushort iMaxSockets;
    public ushort iMaxUdpDg;
    public IntPtr lpVendorInfo;
}

[StructLayout(LayoutKind.Sequential)] public struct sockaddr_in {
    public short sin_family;
    public ushort sin_port;
    public uint sin_addr;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)] public byte[] sin_zero;
}

public static class Winsock {
    public const string LIBRARY_NAME = "ws2_32.dll";
    
    [DllImport("ws2_32.dll")] public static extern int WSAGetLastError();
    [DllImport("ws2_32.dll")] public static extern int WSAStartup(ushort wVersionRequested, ref WSAData lpWSAData);
    [DllImport("ws2_32.dll")] public static extern int WSACleanup();
    [DllImport("ws2_32.dll")] public static extern int socket(int af, int type, int protocol);
    [DllImport("ws2_32.dll")] public static extern int bind(int s, ref sockaddr_in name, int namelen);
    [DllImport("ws2_32.dll")] public static extern int listen(int s, int backlog);
    [DllImport("ws2_32.dll")] public static extern int accept(int s, ref sockaddr_in addr, ref int addrlen);
    [DllImport("ws2_32.dll")] public static extern int closesocket(int s);
    [DllImport("ws2_32.dll")] public static extern int htons(ushort hostshort);
    [DllImport("ws2_32.dll")] public static extern int htonl(uint hostlong);
    [DllImport("ws2_32.dll")] public static extern int connect(int s, ref sockaddr_in name, int namelen);
    [DllImport("ws2_32.dll")] public static extern long send(int s, byte[] buf, int len, int flags);
    [DllImport("ws2_32.dll")] public static extern long recv(int s, byte[] buf, int len, int flags);
    [DllImport("ws2_32.dll")] public static extern int inet_addr(string cp);
    [DllImport("ws2_32.dll")] public static extern int inet_ntoa(uint inaddr);
    [DllImport("ws2_32.dll")] public static extern int gethostbyname(string name);
    [DllImport("ws2_32.dll")] public static extern int gethostbyaddr(string addr, int len, int type);
    [DllImport("ws2_32.dll")] public static extern void shutdown(int s, int how);
    [DllImport("ws2_32.dll")] public static extern int setsockopt(int s, int level, int optname, byte[] optval, int optlen);

    // Constants
    public const int AF_INET = 2;
    public const int SOCK_STREAM = 1;
    public const int SOCK_DGRAM = 2;
    public const int IPPROTO_TCP = 6;
    public const int IPPROTO_UDP = 17;
    public const int INADDR_ANY = 0;
    public const int INADDR_NONE = -1;
    public const int INADDR_LOOPBACK = 0x7f000001;
    public const int INADDR_BROADCAST = 0xfffffff;
    public const int SOMAXCONN = 0x7fffffff;
    public const int SOL_SOCKET = 0xffff;
    public const int SO_REUSEADDR = 0x0004;
    public const int SO_KEEPALIVE = 0x0008;
    public const int SO_LINGER = 0x0080;
    public const int SO_RCVBUF = 0x1002;
    public const int SO_SNDBUF = 0x1001;
    public const int SO_RCVTIMEO = 0x1006;
}