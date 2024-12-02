#pragma once

#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <string.h>

#include <sys/socket.h>
#include <sys/types.h>

#include <netinet/in.h>
#include <arpa/inet.h>

#include "../../types.h"

struct SocketDescriptor {
    enum SocketDescriptor_role {
        SD_ROLE_SRV, SD_ROLE_CLT,
        SD_ROLE_NONE
    } role;

    boolean active;
    int16 socket, shadow, port;
    struct sockaddr_in address;
    uintsize addrlen;
    #define Socket(x) x.shadow
};
const uintsize SocketDescriptor_typesize = sizeof(struct SocketDescriptor);
const int32 LISTEN_MAX = 999999;

struct SocketDescriptor SocketDescriptor_new(const int16 port);
struct SocketDescriptor SocketDescriptor_client(struct SocketDescriptor* srv);

void SocketDescriptor_delete(struct SocketDescriptor* obj);