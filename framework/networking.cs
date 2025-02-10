using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Luna.Framework.Bindings;

namespace Luna.Framework;

/// <summary>
/// This enum holds the kinds of
/// labours the Socket could serve
/// for.
/// </summary>
public enum SocketRole {
    None,
    Server = 6, Client = 9,
    Gateway = 8
}

/// <summary>
/// This class servers as representant of a connection.
/// </summary>
public class Socket {
    /// <summary>
    /// Kind of connection the socket identifies.
    /// </summary>
    private SocketRole role = SocketRole.None;

    /// <summary>
    /// Windows' Winsock preinit bullshit.
    /// </summary>
    private WSAData wsaData = new WSAData();

    /// <summary>
    /// The socket descriptor.
    /// </summary>
    private ushort descriptor = 0;

    /// <summary>
    /// Information structure about the connection.
    /// </summary>
    private sockaddr_in sockaddr = new sockaddr_in();

    /// <summary>
    /// The port the connection works under.
    /// </summary>
    private ushort port = 6069;

    /// <summary>
    /// How much bytes were sent/received over the connection.
    /// </summary>
    private ulong recvBytes = 0, sentBytes = 0;

    // FIXME: this doesn't feel right.
    /// <summary>
    /// The size of the address information structure.
    /// </summary>
    private int addrlen = Marshal.SizeOf<sockaddr_in>();

    /// <summary>
    /// Tracking for connection status.
    /// </summary>
    private bool working = false;

    /// <summary>
    /// Reading access to the connection port.
    /// </summary>
    public ushort Port {
        get { return this.port; }
    }

    /// <summary>
    /// Reading access to the connection role.
    /// </summary>
    public SocketRole Role {
        get { return this.role; }
    }

    /// <summary>
    /// Reading access to the connection information.
    /// </summary>
    public sockaddr_in SockAddr {
        get { return this.sockaddr; }
    }

    /// <summary>
    /// Reading access to how much data was readed through.
    /// </summary>
    public ulong RecvBytes {
        get { return this.recvBytes; }
    }

    /// <summary>
    /// Reading access to how much data was written thourgh.
    /// </summary>
    public ulong SentBytes {
        get { return this.sentBytes; }
    }

    /// <summary>
    /// Reading access to the socket descriptor.
    /// </summary>
    public ushort Descriptor {
        get { return this.descriptor; }
    }

    /// <summary>
    /// Reading access to the connection state.
    /// </summary>
    public bool Working {
        get { return this.working; }
    }

    /// <summary>
    /// Main constructor for the `Socket` object.
    /// It creates an object representing a Server listener socket.
    /// Its role is `SocketRole.Server`.
    /// </summary>
    /// <param name="port">The port the server will work over.</param>
    /// <exception cref="Exception">The constructor could throw because a WSAStartup call failed.</exception>
    public Socket(ushort port) {
        if(Winsock.WSAStartup(0x202, ref wsaData) != 0) {
            throw new Exception("WSAStartup failed");
        }

        this.role = SocketRole.Server;

        this.descriptor = (ushort) Winsock.socket(Winsock.AF_INET, Winsock.SOCK_STREAM, 0);
        if(this.descriptor == 0) {
            // throw new Exception("Socket creation failed");
            this.working = false;
        }

        this.port = (ushort)(port > 1024 ? port : 6069);

        this.sockaddr.sin_family = (short) Winsock.AF_INET;
        this.sockaddr.sin_port = (ushort) Winsock.htons(this.port);
        this.sockaddr.sin_addr = 0;

        if(Winsock.bind(this.descriptor, ref this.sockaddr, Marshal.SizeOf(this.sockaddr)) != 0) {
            // throw new Exception("Socket bind failed");
            this.working = false;
        }

        if(Winsock.listen(this.descriptor, 0) != 0) {
            // throw new Exception("Socket listen failed");
            this.working = false;
        }

        Console.WriteLine("Server started on port " + this.port);
        this.working = true;
    }

    /// <summary>
    /// Secondary constructor for the `Socket` class.
    /// It represents an accepting client socket.
    /// Its role is `SocketRole.Client`.
    /// </summary>
    /// <param name="srv">The server socket instance the client instance will work for.</param>
    public Socket(Socket srv) {
        this.role = SocketRole.Client;

        this.descriptor = (ushort) Winsock.accept(srv.Descriptor, ref this.sockaddr, ref this.addrlen);
        if(this.descriptor <= 0) {
            // throw new Exception("Failed to create a server connection from the server socket.");
            this.working = false;
        }

        this.working = true;
    }

    /// <summary>
    /// Third constructor for the `Socket` object.
    /// It represents a gateway socket.
    /// Its role is `SocketRole.Gateway`.
    /// </summary>
    /// <param name="srv"></param>
    /// <param name="port"></param>
    /// <param name="address"></param>
    public Socket(Socket srv, ushort port, string address) {
        this.role = SocketRole.Gateway;

        if(srv == null) {
            this.working = false;
        }

        this.descriptor = (ushort) Winsock.socket(Winsock.AF_INET, Winsock.SOCK_STREAM, Winsock.IPPROTO_TCP);
        if(this.descriptor <= 0) {
            this.working = false;
        }

        this.sockaddr = new sockaddr_in {
            sin_family = Winsock.AF_INET,
            sin_port = (ushort) Winsock.htons(srv!.Port),
            sin_addr = (uint) Winsock.inet_addr(address!),
            sin_zero = new byte[8]
        };

        if(Winsock.connect(this.descriptor, ref this.sockaddr, Marshal.SizeOf(this.sockaddr)) != 0) {
            this.working = false;
        }
    }

    /// <summary>
    /// Socket destructor.
    /// It shutdowns the socket work, trying to write
    /// everything in queue to be written, dropping further read/write
    /// capabilities and closing the socket.
    /// </summary>
    public void close() {
        if(this.descriptor != 0) {
            Winsock.shutdown(this.descriptor, 2);
            Winsock.closesocket(this.descriptor);
        }

        if(this.role == SocketRole.Server) {
            Winsock.WSACleanup();
        }
    }

    /// <summary>
    /// Official destructor method.
    /// It's bound to the public `Socket.close` method.
    /// </summary>
    ~Socket()
    => this.close();

    /// <summary>
    /// This method performs write operations to the underlying socket.
    /// </summary>
    /// <param name="data">the data buffer to be written</param>
    /// <returns>The ammount of bytes that ended up written.</returns>
    /// <exception cref="Exception">Throws if the data couldn't be sent. In both "sent = 0" or a failed call to send().</exception>
    public ulong writeRaw(byte[] data) {
        long sent = 0;

        if(this.working) {
            sent = data.Length;

            sent = (long) Winsock.send(this.descriptor, data, (int) sent, 0);
            if(sent == -1) {
                throw new Exception("Failed to send data");
            }
        }

        this.sentBytes += (ulong) sent;
        return (ulong) sent;
    }

    /// <summary>
    /// This method performs reading operations to the underlying socket.
    /// 
    /// A buffer of total size `len` is allocated first, if the data
    /// size is less than the provided parametre, the buffer is further
    /// shortened to avoid keeping allocated more memory than used.
    /// (Even in managed garbage-collected langs. Let it sink in.)
    /// 
    /// If **-1** is passed to the method, it attempts to read everything
    /// that's available to read.
    /// </summary>
    /// <param name="len">The ammount of bytes to read from the socket.</param>
    /// <returns>The data readed from the socket.</returns>
    /// <exception cref="Exception">Throws if the data couldn't be read. In both "recv = 0" or a failed call to recv().</exception>
    public byte[] readRaw(int len) {
        byte[] data = new byte[len];
        long recv = 0;

        if(this.working) {
            recv = Winsock.recv(this.descriptor, data, len, 0);
            if(recv == -1) {
                throw new Exception("Failed to receive data");
            }
        }

        // In cases where the received data bytes count
        // are less than the limit (~56MB), the array is reduced
        // so the program doesn't end up consuming RAM like
        // Firefox does.
        //
        // Kinda funny, because Chrome/Chromium had the stereotype
        // of thirsty, but not so much anymore, Firefox overtook it.
        if(recv < len)
            Array.Resize<byte>(ref data, (int) recv);

        this.recvBytes += (ulong) recv;
        Console.WriteLine("--------------------");
        Console.WriteLine("Received: " + Encoding.ASCII.GetString(data));
        Console.WriteLine("--------------------\n");
        return data;
    }

    /// <summary>
    /// This method is the high level version of `Socket.readRaw()`.
    /// Under it performs the lower lever version, returning
    /// a `ComposeBuffer` object that can allow easier operations
    /// on the received data.
    /// 
    /// Essentially, this is the method lazy ahh devs should use.
    /// If you like it <i>low</i>, use `Socket.readRaw()` instead.
    /// </summary>
    /// <param name="len">The ammount of data that's expected to receive.</param>
    /// <returns>The buffer with the expected data inside.</returns>
    public ComposeBuffer read(int len) {
        byte[] data = this.readRaw(len);
        return new ComposeBuffer(data.Length, data);
    }

    /// <summary>
    /// The lazier high level counterpart of `Socket.writeRaw()`.
    /// It performs the normal write opperations to the inner socket, but
    /// writing whatever is inside the `data` parametre.
    /// </summary>
    /// <param name="data">The data to be written inside the socket.</param>
    public void write(ComposeBuffer data) {
        this.writeRaw(data.asBytes((int) data.Length));
    }
}