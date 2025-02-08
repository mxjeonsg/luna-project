namespace Luna.Constants;



public static class Htmls {
    public const string index =
        "<html>\n" +
        "  <head>\n" +
        "    <title>Home - Luna Project</title>\n" +
        "  </head>\n" +
        "  <body>\n" +
        "    <marquee>I feel like nuking France.</marquee>\n" +
        "  </body>\n" +
        "</html>"
    ;

    public static string error_404() {
        const string part1 =
            "<html>\n" +
            "  <head>\n" +
            "    <title>Oops... D:</title>\n" +
            "    <link rel=\"stylesheet\" href=\"/pwease_resources/index_styles.uwu\">" +
            "  </head>\n" +
            "  <body>\n" +
            "    <h1>404 Not found Error</h1>\n" +
            "    <p>OOpsie:(</p>\n" +
            "    </br>\n" +
            "    <p>Oops! Something went wrong on our end (or perhaps in yours). But don't be sad. In compensation, enjoy some nice pics of Gary. :D</p>\n" +
            "    <div class=\"container\">\n"
        ;

        const string part2 =
            "    </div>" +
            "  </body>\n" +
            "</html>"
        ;

        return
            part1 +
            "      <img src=\"https://cdn.garybot.dev/Gary" +
            new Random().Next(1, 350) +
            ".jpg\" alt=\"Gary :3\" width=300 height=240/>\n" +
            part2
        ;
    }

    public static string error_500() {
        const string part1 =
            "<html>\n" +
            "  <head>\n" +
            "    <title>Oops... D:</title>\n" +
            "    <link rel=\"stylesheet\" href=\"/pwease_resources/index_styles.uwu\">" +
            "  </head>\n" +
            "  <body>\n" +
            "    <h1>500 Internal Server Error</h1>\n" +
            "    <p>OOpsie:(</p>\n" +
            "    </br>\n" +
            "    <p>Oops! Something went wrong on our end. But don't be sad. In compensation, enjoy some nice pics of Gary. :D</p>\n" +
            "    <div class=\"container\">\n"
        ;

        const string part2 =
            "    </div>" +
            "  </body>\n" +
            "</html>"
        ;

        return
            part1 +
            "      <img src=\"https://cdn.garybot.dev/Gary" +
            new Random().Next(1, 350) +
            ".jpg\" alt=\"Gary :3\" width=300 height=240/>\n" +
            part2
        ;
    }


}

public static class Csss {
    public const string main_css =
        "body {\n" +
        "  font-family: \"Comic Sans MS\", cursive, sans-serif;\n" +
        "  text-align: center;\n" +
        "  background-color: #f0f0f0;\n" +
        "  padding: 50px;\n" +
        "}\n" +
        "\n" +
        "h1 {\n" +
        "  color: #ff0000;\n" +
        "  font-size: 50px;\n" +
        "}\n" +
        "\n" +
        "p {\n" +
        "  color: #333333;\n" +
        "  font-size: 20px;\n" +
        "}\n" +
        "\n" +
        ".container {\n" +
        "  margin-top: 20px;\n" +
        "}"
    ;
}