#pragma once

#include "socket.h"

struct SocketDescriptor SocketDescriptor_new(const int16 port) {
    struct SocketDescriptor sd;

    sd.role = SD_ROLE_SRV;
    sd.port = port > 1234 ? port : 6969;
    sd.socket = socket(AF_INET, SOCK_STREAM, 0);
    sd.addrlen = sizeof(sd.address);

    if(sd.socket < 2) {
        perror("SocketDescriptor_new()#socket(2)");
	    sd.active = False;
    }

    sd.address.sin_family = AF_INET;
    sd.address.sin_addr.s_addr = INADDR_ANY;
    sd.address.sin_port = htons(sd.port);

    if(bind(sd.socket, (struct sockaddr*)&sd.address, sd.addrlen) < 0) {
        perror("SocketDescriptor_new()#bind(2)");
	    close(sd.socket);
	    sd.active = False;
    }

    if(listen(sd.socket, LISTEN_MAX) < 0) {
        perror("SocketDescriptor_new()#listen(2)");
	    close(sd.socket);
	    sd.active = False;
    }

    sd.active = sd.socket > 2 ? True: False;
    sd.shadow = dup(sd.socket);

    return sd;
}

struct SocketDescriptor SocketDescriptor_client(struct SocketDescriptor* srv) {
    struct SocketDescriptor clt;

    clt.socket = accept(srv->socket, (struct sockaddr*)&srv->address, (socklen_t*)&srv->addrlen);

    if(clt.socket < 2) {
        perror("SocketDescriptor_client()#accept(2)");
	    clt.active = False;
    }

    clt.port = ntohs(clt.address.sin_port);
    clt.active = clt.socket > 2 ? True: False;
    clt.role = SD_ROLE_CLT;
    clt.shadow = dup(clt.socket);

    return clt;
}

void SocketDescriptor_delete(struct SocketDescriptor* obj) {
    close(obj->socket);
    close(obj->shadow);
    obj->active = False;
    obj->role = SD_ROLE_NONE;
}