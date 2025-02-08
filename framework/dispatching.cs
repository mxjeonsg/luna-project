using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Luna.Framework;

public enum MimeTypes {
    TextPlain,
    TextHtml,

    // Audio
    AudioAAC,

    // Application
    ApplicationOctetStream,
    ApplicationXFreeArc,

    // Image
    ImageApng,
    ImageAvif,
    ImageXIcon,

    // Video
    VideoXMsVideo
}

public class HttpRequest {
    private ComposeBuffer original_buffer;
    public byte[] Buffer {
        get { return this.original_buffer.asBytes((int) this.original_buffer.Length); }
    }

    public string StringBuffer {
        get { return this.original_buffer.asString(); }
    }
    private string method = "GET";
    public string Method { get { return this.method; } }
    private string url = "";
    public string Url { get { return this.url; } }
    private string version = "";
    public string Version { get { return this.version; } }
    private Dictionary<string, string> headers;
    public Dictionary<string, string> Headers { get { return this.headers; } }
    private string body = "";
    public string Body { get { return this.body; } }

    public HttpRequest(byte[] data) {
        this.headers = new Dictionary<string, string>();
        this.original_buffer = new ComposeBuffer(-1, data);

        this.parseRequest(data);
    }

    private void parseRequest(byte[] data) {
        string request = Encoding.ASCII.GetString(data);
        string[] lines = request.Split(new[] { "\r\n" }, StringSplitOptions.None);

        string[] requestLine = lines[0].Split(' ');
        this.method = requestLine[0];
        this.url = requestLine[1];
        this.version = requestLine[2];

        int i = 1;
        while (!string.IsNullOrEmpty(lines[i])) {
            string[] header = lines[i].Split(new[] { ": " }, 2, StringSplitOptions.None);
            this.headers[header[0]] = header[1];
            i++;
        }

        this.body = string.Join("\r\n", lines, i + 1, lines.Length - i - 1);
    }
}

public class HttpResponse {
    private ComposeBuffer original_buffer;
    private string version = "";
    public string Version { get { return this.version; } }
    private int statuscode = 0;
    public int StatusCode { get { return this.statuscode; } }
    private string reasonPhrase = "";
    public string ReasonPhrase { get { return this.reasonPhrase; } }
    private Dictionary<string, string> headers;
    public Dictionary<string, string> Headers { get { return this.headers; } }
    private string body = "";
    public string Body { get { return this.body; } }

    public HttpResponse(byte[] data) {
        this.headers = new Dictionary<string, string>();
        this.original_buffer = new ComposeBuffer(-1, data);

        this.parseResponse(data);
    }

    private void parseResponse(byte[] data) {
        string response = Encoding.ASCII.GetString(data);
        string[] lines = response.Split(new[] { "\r\n" }, StringSplitOptions.None);

        string[] statusLine = lines[0].Split(' ');
        this.version = statusLine[0];
        this.statuscode = int.Parse(statusLine[1]);
        this.reasonPhrase = string.Join(" ", statusLine, 2, statusLine.Length - 2);

        int i = 1;
        while (!string.IsNullOrEmpty(lines[i])) {
            string[] header = lines[i].Split(new[] { ": " }, 2, StringSplitOptions.None);
            this.headers[header[0]] = header[1];
            i++;
        }

        this.body = string.Join("\r\n", lines, i + 1, lines.Length - i - 1);
    }
}