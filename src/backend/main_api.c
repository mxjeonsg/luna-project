#include <stdio.h>

#include "../common/socket.c"

int main() {
    const icstr source = "HTTP/1.1 200 Ok\r\nContent-Type: text/html\r\n\r\n<marquee>Sexo</marquee>";
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
}