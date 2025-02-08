using System.Reflection;
using System.Runtime.InteropServices;
using Luna.Framework.Bindings;

namespace Luna.Framework;

// This enum denotes the kind of labour the
// `Socket` instance serves.
public enum SocketRole {
    None,
    Server = 6, Client = 9,
    Gateway = 8
}

// This class serves as representant of a connection.
public class Socket {
    // Kind of connection the socket identifies.
    private SocketRole role = SocketRole.None;

    // Windows' winsock preinit bullshit.
    private WSAData wsaData = new WSAData();

    // The socket descriptor.
    private ushort descriptor = 0;

    // Information about the connection.
    private sockaddr_in sockaddr = new sockaddr_in();

    // The port the connection works under.
    private ushort port = 6069;

    // How much bytes were sent/received over the connection.
    private ulong recvBytes = 0, sentBytes = 0;

    // FIXME: this doesn't feel right.
    private int addrlen = 16;

    // Tracking for connection status.
    private bool working = false;

    // Reading access to the connection port.
    public ushort Port {
        get { return this.port; }
    }

    // Reading access to the connection role.
    public SocketRole Role {
        get { return this.role; }
    }

    // Reading access to the connection information.
    public sockaddr_in SockAddr {
        get { return this.sockaddr; }
    }

    // Reading access to how much data was readed through.
    public ulong RecvBytes {
        get { return this.recvBytes; }
    }

    // Reading access to how much data was written through.
    public ulong SentBytes {
        get { return this.sentBytes; }
    }

    // Reading access to the socket descriptor.
    public ushort Descriptor {
        get { return this.descriptor; }
    }

    // Reading access to the connection state.
    public bool Working {
        get { return this.working; }
    }

    // Main constructor for the `Socket` object.
    // It creates a object representing a Server listener socket.
    // Its role is `SocketRole.Server`.
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

    // Secondary constructor for the `Socket` object.
    // It represents an accepting client socket.
    // Its role is `SocketRole.Client`.
    public Socket(Socket srv) {
        this.role = SocketRole.Client;

        this.descriptor = (ushort) Winsock.accept(srv.Descriptor, ref this.sockaddr, ref this.addrlen);
        if(this.descriptor <= 0) {
            // throw new Exception("Failed to create a server connection from the server socket.");
            this.working = false;
        }

        this.working = true;
    }

    // Secondary and third constructor for the `Socket` object.
    // It represents a gateway socket.
    // Its role is `SocketRole.Gateway`
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

    // Socket destructor.
    // It shutdowns the socket work, trying to write
    // everything in queue to be written, dropping further read/write
    // capabilities and closing the socket.
    public void close() {
        if(this.descriptor != 0) {
            Winsock.shutdown(this.descriptor, 2);
            Winsock.closesocket(this.descriptor);
        }

        if(this.role == SocketRole.Server) {
            Winsock.WSACleanup();
        }
    }

    // Official desctructor method.
    // Its bound to the public `Socket.close` method.
    ~Socket()
    => this.close();

    // This method performs write operations to the underlying socket.
    //
    // Parametres:
    // - `byte[]` data: the data to be written.
    //
    // Returns:
    // (ulong): The ammount of bytes written.
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

    // This method performs reading operations to the underlying socket.
    // If **-1** is passed to the method, it attempts to read eveything
    // that's available to read.
    //
    // Parametres:
    // - `int` len: The ammount of bytes to read from the socket.
    //
    // Returns:
    // (byte[]): The data readed from the socket.
    public byte[] readRaw(int len) {
        byte[] data = new byte[len];
        long recv = 0;

        if(this.working) {
            recv = Winsock.recv(this.descriptor, data, len, 0);
            if(recv == -1) {
                throw new Exception("Failed to receive data");
            }
        }

        this.recvBytes += (ulong) recv;
        return data;
    }

    public ComposeBuffer read(int len) {
        byte[] data = this.readRaw(len);
        return new ComposeBuffer(data.Length, data);
    }

    public void write(ComposeBuffer data) {
        this.writeRaw(data.asBytes((int) data.Length));
    }
}