namespace Luna;

using System.Text;

using Luna.Framework;

static class UwU {
    public static readonly string headers =
        "X-Luna-Server-As: gateway\r\n"
        + "X-Luna-Server-Codename: arya\r\n"
        + "X-Luna-Server-Version: 0.4.69.4\r\n"
        + "X-Luna-Server-OriginalPort: 6069"
    ;

    public static List<ushort> connections;
    public static bool logged_in = false;

    /// <summary>
    ///  This static function makes out the API routing
    ///  of the server at the frontend side.
    /// </summary>
    /// <param name="conn">The client socket instance</param>
    /// <param name="req">The request object</param>
    public static void dispatchContent(Socket conn, HttpRequest req) {
        if(req.Url == "/feed") {
            if(!UwU.logged_in) {
                string html = File.ReadAllText("../../../html/notauthorised.htm");

                conn.write(new ComposeBuffer(-1,
                    "HTTP/1.1 401 Unauthorised\r\n"
                    + "Content-Type: text/html\r\n"
                    + "Content-Length: " + html.Length + "\r\n"
                    + UwU.headers
                    + "\r\n\r\n"
                    + html
                ));
            }

            string html2 = File.ReadAllText("../../../html/feed.htm");

            conn.write(new ComposeBuffer(-1,
                "HTTP/1.1 200 Ok\r\n"
                + "Content-Type: text/html\r\n"
                + "Content-Length: " + html2.Length
                + UwU.headers
                + "\r\n\r\n"
                + html2
            ));
        } else if(req.Url == "/pwease_resources/styles/main.uwu") {
            string css = File.ReadAllText("../../../css/main.css");

            conn.write(new ComposeBuffer(-1,
                "HTTP/1.1 200 Ok\r\n"
                + "Content-Type: text/css\r\n"
                + "Content-Length: " + css.Length + "\r\n"
                + UwU.headers
                + "\r\n\r\n"
                + css
            ));
        } else if(req.Url == "/pwease_resources/styles/feed.uwu") {
            string css = File.ReadAllText("../../../css/feed.css");

            conn.write(new ComposeBuffer(-1, 
                "HTTP/1.1 200 Ok\r\n"
                + "Content-Type: text/css\r\n"
                + "Content-Length: " + css.Length
                + UwU.headers
                + "\r\n\r\n"
                + css
            ));
        } else if(req.Url == "/api/random_fact") {
            const string fact =
                "{\n" +
                "  \"factsito\": \"**Breaking News**: Elon Musk comes out as gay, announces that he is going to marry his lifelong love, Donald J. Trump. In a public statement, Elon states <<Trelon Munald for life ❤️ 🏳️‍🌈 >>\"" +
                "}"
            ;

            conn.write(new ComposeBuffer(-1,
                "HTTP/1.1 200 Ok\r\n"
                + "Content-Type: application/json\r\n"
                + "Content-Length: " + fact.Length + "\r\n"
                + UwU.headers
                + "\r\n\r\n"
                + fact
            ));
            conn.write(new ComposeBuffer(fact.Length, fact));
        } else if(req.Url == "/tos") {
            string tos = "Terms of service";

            conn.write(new ComposeBuffer(-1, 
                "HTTP/1.1 200 Ok\r\n"
                + "Content-Type: text/plain\r\n"
                + "Content-Length: " + tos.Length + "\r\n"
                + UwU.headers
                + "\r\n\r\n"
                + tos
            ));
        }  else if(req.Url == "/favicon.ico") {
            byte[] favicon = File.ReadAllBytes("../../../assets/favicon.ico");

            conn.write(new ComposeBuffer(-1, 
                "HTTP/1.1 200 Ok\r\n"
                + "Content-Type: image/x-icon\r\n"
                + "Content-Length: " + favicon.Length + "\r\n"
                + UwU.headers
                + "\r\n\r\n"
                + Encoding.ASCII.GetString(favicon)
            ));
        } else if(req.Url == "/pwease_resources/hacks/garypic.qwq") {
            string script = File.ReadAllText("../../../js/garypic.js");

            conn.write(new ComposeBuffer(-1,
                "HTTP/1.1 200 Ok\r\n"
                + "Content-Type: text/javascript\r\n"
                + "Content-Length" + script.Length + "\r\n"
                + UwU.headers
                + "\r\n\r\n"
                + script
            ));
        } else if(req.Url == "/pwease_resources/hacks/feed.qwq") {
            string script = File.ReadAllText("../../../js/feed.js");

            conn.write(new ComposeBuffer(-1,
                "HTTP/1.1 200 Ok\r\n"
                + "Content-Type: text/javascript\r\n"
                + "Content-Length: " + script.Length + "\r\n"
                + UwU.headers
                + "\r\n\r\n"
                + script
            ));
        } else if(req.Url == "/pwease_resources/static/default-pfp.owo") {
            byte[] pfp = File.ReadAllBytes("../../../assets/static/default-pfp.png");

            conn.write(new ComposeBuffer(-1,
                "HTTP/1.1 200 Ok\r\n"
                + "Content-Type: image/png\r\n"
                + "Content-Length: " + pfp.Length + "\r\n"
                + UwU.headers
                + "\r\n\r\n"
                + Encoding.ASCII.GetString(pfp)
            ));
        } else if(req.Url == "/") {
            if(UwU.logged_in) {
                string html = File.ReadAllText("../../../html/feed.htm");

                conn.write(new ComposeBuffer(-1,
                    "HTTP/1.1 200 Ok\r\n"
                    + "Content-Type: text/html\r\n"
                    + "Content-Length: " + html.Length + "\r\n"
                    + UwU.headers
                    + "\r\n\r\n"
                    + html
                ));
            } else {
                string html = @"
                    <html>
                      <head>
                        <title>Redirect to login... - Luna Project</title>
                      </head>
                      <body>
                        <script>
                            document.location.href = '/login';
                        </script>
                      </body>
                    </html>
                ";

                conn.write(new ComposeBuffer(-1,
                    "HTTP/1.1 302 Found\r\n"
                    + "Content-Type: text/html\r\n"
                    + "Content-Length: " + html.Length + "\r\n"
                    + UwU.headers
                    + "\r\n\r\n"
                    + html
                ));
            }
        } else if(req.Url == "/login") {
            string html = File.ReadAllText("../../../html/login.htm");

            conn.write(new ComposeBuffer(-1,
                "HTTP/1.1 200 Ok\r\n"
                + "Content-Type: text/html\r\n"
                + "Content-Length: " + html.Length + "\r\n"
                + UwU.headers
                + "\r\n\r\n"
                + html
            ));
        } else if(req.Url == "/pwease_resources/hacks/login.qwq") {
            string js = File.ReadAllText("../../../js/login.js");

            conn.write(new ComposeBuffer(-1,
                "HTTP/1.1 200 Ok\r\n"
                + "Content-Type: text/javascript\r\n"
                + "Content-Length: " + js.Length + "\r\n"
                + UwU.headers
                + "\r\n\r\n"
                + js
            ));
        } else if(req.Url == "/pwease_resources/styles/login.uwu") {
            string styles = File.ReadAllText("../../../css/login.css");

            conn.write(new ComposeBuffer(-1,
                "HTTP/1.1 200 Ok\r\n"
                + "Content-Type: text/css\r\n"
                + "Content-Length: " + styles.Length + "\r\n"
                + UwU.headers
                + "\r\n\r\n"
                + styles
            ));
        } else if(req.Url == "/api") {
            string html = File.ReadAllText("../../../html/notauthorised.htm");

            conn.write(new ComposeBuffer(-1,
                "HTTP/1.1 401 Unauthorised\r\n"
                + "Content-Type: text/html\r\n"
                + "Content-Length:" + html.Length + "\r\n"
                + UwU.headers
                + "\r\n\r\n"
                + html
            ));
        } else {
            conn.write(new ComposeBuffer(-1,
                "HTTP/1.1 404 Not Found\r\n"
                + "Content-Type: text/html\r\n"
                + UwU.headers
                + "\r\n\r\n"
            ));
            conn.write(new ComposeBuffer(-1, File.ReadAllText("../../../html/error.htm")));
        }
    }
    public static void Main(string[] args) {
        Socket server = new Socket(6069);

        UwU.connections = new List<ushort>(
            0
        );

        while(true) {
            Socket client = new Socket(server);
            if(!client.Working) continue;

            if(UwU.connections.Contains(client.Port)) {
                UwU.logged_in = false;
            } else {
                UwU.connections.Append(client.Port);
                UwU.logged_in = false;
            }

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

