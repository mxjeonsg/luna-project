namespace Luna.Framework;

using System;

public class ComposeBuffer {
    private byte[] buffer;
    private int offset = 0;

    public ulong Length {
        get { return (ulong) this.buffer.Length; }
    }

    public ComposeBuffer(int size, byte[]? content) {
        int _size = size > 0 ? size : content!.Length;

        this.buffer = new byte[_size];
        if(content != null) Array.Copy(content, 0, this.buffer, this.offset, content!.Length);
    }

    public ComposeBuffer(int size, string? content) {
        int _size = size > 0 ? size : content!.Length;

        this.buffer = new byte[_size];
        if(content != null) this.write(content!);
    }

    public void write(byte[] data) {
        Array.Copy(data, 0, this.buffer, this.offset, data.Length);
        this.offset += data.Length;
    }

    public void write(string data)
    => this.write(System.Text.Encoding.ASCII.GetBytes(data));

    public byte[] asBytes(int size) {
        byte[] data = new byte[size];
        Array.Copy(this.buffer, 0, data, 0, size);
        return data;
    }

    public string asString()
    => string.Join("", System.Text.Encoding.ASCII.GetString(this.buffer));

    public bool isEmpty()
    => this.buffer.Length < 1;
}