#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>

#include "../types.h"

#include "../common/c/socket.c"
#include "../common/c/string.c"

int32 main(int32 argc, icstr argv[]) {
    struct SocketDescriptor server = SocketDescriptor_new(6069);

    while(True) {
        struct SocketDescriptor client = SocketDescriptor_client(&server);



        SocketDescriptor_delete(&client);
    }

    SocketDescriptor_delete(&server);
}