#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>

#include "../common/c/socket.c"
#include "../common/c/requests.c"

int32 main(int32 argc, icstr argv[], icstr envp[]) {
    struct SocketDescriptor sd = SocketDescriptor_new(-1);

    int8 response[9999] = {0}, request[9999] = {0};
    const icstr html = "<html lang=\"by\">\r\n    <head>\r\n        <title>Sexito</title>\r\n    </head>\r\n    <body>\r\n        <marquee>It works!</marquee>\r\n    </body>\r\n</html>";
    const icstr json = "{\r\n \"answer\": \"ur good to go m8\"\r\n}";

    snprintf(response, 9999,
        "HTTP/1.1 200 Ok\r\n"
	    "Content-Type: text/html\r\n"
	    "Content-Length: %ld\r\n"
        "Access-Control-Allow-Origin: http://127.0.0.1:6969\r\n"
        "Access-Control-Allow-Methods: GET, POST, DELETE\r\n"
	    "\r\n"
	    "%s",
	    strlen(html), html
    );

    if(sd.active) while(True) {
        struct SocketDescriptor cd = SocketDescriptor_client(&sd);

	    if(cd.active) {
	        read(Socket(cd), request, 9999);

	        printf("Request (from port %d): %s\n", cd.port, request);

	        // handle_request(Socket(cd));

            write(Socket(cd), response, strlen(response));

            SocketDescriptor_delete(&cd);
	    }
    }

    SocketDescriptor_delete(&sd);

    return(0);
}
