namespace Luna;

using System.Text;

using Luna.Framework;


static class UwU {
    public static void dispatchContent(Socket conn, HttpRequest req) {
        Console.WriteLine("Request: " + req.StringBuffer);

        if(req.Url == "/" || req.Url == "/home" || req.Url == "/index") {
            conn.write(new ComposeBuffer(-1, "HTTP/1.1 200 Ok\r\nContent-Type: text/html\r\n\r\n"));
            conn.write(new ComposeBuffer(-1, File.ReadAllText("../../../html/feed.htm")));
        } else if(req.Url == "/pwease_resources/styles/main.uwu") {
            conn.write(new ComposeBuffer(-1, "HTTP/1.1 200 Ok\r\nContent-Type: text/css\r\n\r\n"));
            conn.write(new ComposeBuffer(-1, File.ReadAllText("../../../css/main.css")));
        } else if(req.Url == "/pwease_resources/styles/feed.uwu") {
            conn.write(new ComposeBuffer(-1, "HTTP/1.1 200 Ok\r\nContent-Type: text/css\r\n\r\n"));
            conn.write(new ComposeBuffer(-1, File.ReadAllText("../../../css/feed.css")));
        } else if(req.Url == "/api/random_fact") {
            const string fact =
                "{\n" +
                "  \"factsito\": \"**Breaking News**: Elon Musk comes out as gay, announces that he is going to marry his lifelong love, Donald J. Trump. In a public statement, Elon states <<Trelon Munald for life ❤️ 🏳️‍🌈 >>\"" +
                "}"
            ;

            conn.write(new ComposeBuffer(-1, Encoding.ASCII.GetBytes("HTTP/1.1 200 Ok\r\nContent-Type: application/json")));
            conn.write(new ComposeBuffer(fact.Length, fact));
        } else if(req.Url == "/externapi/garydev/garypic") {
            Socket gateway = new Socket(conn, 6969, "garybot.dev");

            gateway.write(new ComposeBuffer(-1, @"
                GET /api/gary HTTP/1.1
                Connection: close
                X-Luna-Server-As: gateway
                X-Luna-Server-Codename: arya
                X-Luna-Server-Version: 0.4.69.4
                X-Luna-Server-OriginalPort: 6069

            "));

            // Delay so the server finishes writing to the socket.
            Thread.Sleep(1000 * 3);

            ComposeBuffer response = gateway.read(1024*1024*56);
            if(response.Length < 1) {
                conn.write(new ComposeBuffer(-1, "HTTP/1.1 404 Not Found\r\nContent-Type: text/plain\r\nContent-Length: 0\r\n\r\n"));
                conn.close();
            } else {
                conn.write(new ComposeBuffer(-1, "HTTP/1.1 200 Ok\r\nContent-Type: image/jpeg\r\nContent-Length" + response.Length + "\r\n\r\n"));
                conn.write(response);
                conn.close();
            }

        } else {
            conn.write(new ComposeBuffer(-1, "HTTP/1.1 404 Not Found\r\nContent-Type: text/html\r\n\r\n"));
            conn.write(new ComposeBuffer(-1, File.ReadAllText("../../../html/error.htm")));
        }
    }
    public static void Main(string[] args) {
        Socket server = new Socket(6069);

        while(true) {
            Socket client = new Socket(server);
            if(!client.Working) continue;

            ComposeBuffer recv = client.read(9096);
            if(recv.isEmpty()) {
                Console.WriteLine("Didn't receive shit");
            }

            HttpRequest request = new HttpRequest(recv.asBytes((int) recv.Length));

            UwU.dispatchContent(client, request);

            client.close();
        }

        // server.close();
    }
}

