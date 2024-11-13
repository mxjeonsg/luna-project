#include <stdio.h>

#include "../common/app_details.h"
#include "../common/extra_request_headers.h"

#include "../common/socket.c"

#include "../common/http_headers.c"

int main() {
    const icstr source = "<marquee>Sexo</marquee>";
    const usize res_len = 256000;

    i8 response[res_len];

    const HttpHeader rs_hea = {
        .code = 200,
        .code_str = "Ok",
        .headers = "Content-Type: text/html",
        .content = source
    };
    prependResponseHeaderToContent_NA(response, res_len, &rs_hea);


    SocketDescr server = SocketDescriptor_new_srv(8080);

    while(server->listening) {
        icstr destiny;

        SocketDescr client = SocketDescriptor_new_clt(server);

        SocketDescriptor_read(client, destiny, 4096);

        fprintf(stderr, "..............................\n");
        fprintf(stderr, "Readed (%p):\n%s\n", destiny, destiny);
        fprintf(stderr, "..............................\n");

        SocketDescriptor_write_all(client, source);

        SocketDescriptor_delete(client);

        if(destiny != nil) free(destiny);
    }

    SocketDescriptor_delete(server);
    // free(response);
}