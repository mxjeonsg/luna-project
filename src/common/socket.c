#pragma once

// TODO: Check unexpected nullified pointers.

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <unistd.h>
#include <arpa/inet.h>
#include <sys/socket.h>
#include <netinet/in.h>

#include "types.h"

const usize SOCKET_READ_ALL_MIN = 4096;
const u16 SOCKET_MINIMUM_PORT = 1024, SOCKET_MAXIMUM_PORT = 65535;

typedef enum SocketIOErrors {
    SIO_READ_RECV_FAILED,
    SIO_WRITE_SEND_FAILED,
    SIO_READEX_NOT_ENOUGH_ARGS,
    SIO_READEX_INVALID_ARGS,
    SIO_WRITEEX_NOT_ENOUGH_ARGS,
    SIO_WRITEEX_INVALID_ARGS,
    SIO_READEX_READED_LESS_THAN_EXP,
    SIO_WRITEEX_WRITTEN_LESS_THAN_EXP,

    SIO_COUNT
} SocketIOStatus;


typedef struct SocketReadWriteInfo {
    usize to_read, to_write;
    usize minimum_to_write, minimum_to_read;
    usize expected_to_write, expected_to_read;
    usize bytes_readed, bytes_written;

    usize total_allocated;

    icstr destiny_buffer, source_buffer;

    u1 success;

    SocketIOStatus last_status, last_error;

    u1 should_malloc;
} SocketIOInfo;
const usize SocketIOInfo_size = sizeof(struct SocketReadWriteInfo);

typedef struct SocketDescriptor {
    u1 created, binded, listening, enabled, role, accepting;
    u16 port;
    i32 socket;
    struct sockaddr_in address;
    usize addrlen;

    usize total_received, total_transmit;

    SocketIOInfo ioinfo;
} *SocketDescr;
const usize SocketDescr_size = sizeof(struct SocketDescriptor);

const u16 getRandomPort() {
    return (rand() % (SOCKET_MAXIMUM_PORT - SOCKET_MINIMUM_PORT + 1)) + SOCKET_MINIMUM_PORT;
}

SocketDescr SocketDescriptor_new_srv(i16 port) {
    SocketDescr sk = malloc(SocketDescr_size);

    sk->role = True; // Server
    sk->addrlen = sizeof(sk->address);

    sk->socket = socket(AF_INET, SOCK_STREAM, 0);
    if(sk->socket == 0) {
        perror("SocketDescriptor_new_srv()#socket(2)");
        sk->created = False;
        sk->enabled = False;
    } else sk->created = True;

    if(port == -1) { // Random port then.
        port = getRandomPort();
    }

    sk->address.sin_family = AF_INET;
    sk->address.sin_addr.s_addr = INADDR_ANY;
    sk->address.sin_port = htons(port);

    if(bind(sk->socket, (struct sockaddr*)&sk->address, sk->addrlen) < 0) {
        perror("SocketDescriptor_new_srv()#bind(2)");
        sk->binded = False;
        sk->enabled = False;
        close(sk->socket);
    } else sk->binded = True;

    if(listen(sk->socket, 999999999) < 0) {
        perror("SocketDescriptor_new_srv()#listen(2)");
        close(sk->socket);
        sk->created = False;
        sk->enabled = False;
        sk->listening = False;
    } else sk->listening = True;

    if(sk->created && sk->binded && sk->listening) {
        sk->enabled = True;
    } else {
        sk->enabled = False;
    }

    sk->total_received = 0;
    sk->total_transmit = 0;

    fprintf(stderr, "SocketDescriptor_new_srv(NOTE): URL: 'http://localhost:%d.'\n", port);

    return sk;
}

SocketDescr SocketDescriptor_new_clt(SocketDescr srv) {
    SocketDescr sk = malloc(SocketDescr_size);

    sk->role = False; // Client
    sk->addrlen = sizeof(sk->address);

    sk->socket = accept(srv->socket, (struct sockaddr*)&srv->address, (socklen_t*)&srv->addrlen);
    if(sk->socket < 0) {
        perror("SocketDescriptor_new_clt()#accept(2)");
        sk->enabled = False;
        sk->accepting = False;
    } else sk->accepting = True;

    if(sk->accepting) {
        sk->enabled = True;
        sk->created = True;
    }

    sk->total_received = 0;
    sk->total_transmit = 0;

    return sk;
}

u1 SocketDescriptor_delete(SocketDescr sk) {
    if(sk != nil) {
        close(sk->socket);

        sk->accepting = False;
        sk->address = (struct sockaddr_in){0};
        sk->addrlen = 0;
        sk->binded = False;
        sk->created = False;
        sk->enabled = False;
        sk->listening = False;
        sk->port = 0;
        sk->role = False;
        sk->total_received = 0;
        sk->total_transmit = 0;

        free(sk);

        return True;
    } else {
        return False;
    }
}


// NOTE: This allocs memory.
u1 SocketDescriptor_read_ex(SocketDescr sk, SocketIOInfo* sioinfo, icstr destiny) {
    if(sioinfo == nil) { // Fail: not enough arguments
        fprintf(stderr, "SocketDescriptor_read_ex(): Not enough arguments provided.\n");
        sioinfo->last_error = SIO_READEX_NOT_ENOUGH_ARGS;
        return False;
    } else {
        // Add "destiny" reference to sioinfo.
        sioinfo->destiny_buffer = destiny;

        // Check for buffer
        if(sioinfo->destiny_buffer == nil) { // Default to not allocated
            // Check if has to be allocated
            if(!sioinfo->should_malloc) {
                // TODO: maybe pointer is a i8[?] array
                // NOTE: At the moment, fail
                fprintf(stderr,
                    "SocketDescriptor_read_ex(): Not enough arguments.\n"
                    "ERROR: destiny buffer wasn't provided.\n"
                );

                return False;
            } else {
                sioinfo->total_allocated = sizeof(i8) * SOCKET_READ_ALL_MIN;
                destiny = malloc(sioinfo->total_allocated);
                // TODO: check if correctly allocated.
                // NOTE: At the moment, continue anyway.
            }
        }

        // TODO: Check if socket is valid.
        // NOTE: Valid assumed.

        sioinfo->bytes_readed = recv(sk->socket, destiny, sioinfo->expected_to_read, 0);

        // Check for readed
        // TODO: Check if quantity readed was expected
        if(sioinfo->bytes_readed < sioinfo->expected_to_read) {
            fprintf(stderr,
                "SocketDescriptor_read_ex(): Expected to read %lu but %lu readed instead.\n",
                sioinfo->expected_to_read, sioinfo->bytes_readed
            );

            sioinfo->last_status = SIO_READEX_READED_LESS_THAN_EXP;

            fprintf(stderr, "SocketDescriptor_read(DEBUG): To return:\n%s\n", sioinfo->destiny_buffer);

            sk->total_received += sioinfo->bytes_readed > 0 ? sioinfo->bytes_readed : 0;

            return True;
        }

        // TODO: Check if more edge cases could ocur.
        if(sioinfo->bytes_readed == -1) { // Check if recv(2) failed.
            perror("SocketDescriptor_read_ex()#recv()");

            sioinfo->last_error = SIO_READ_RECV_FAILED;
            sioinfo->bytes_readed = 0;

            return False;
        } else {
            sioinfo->success = True;

            return True;
        }
    }
}

u1 SocketDescriptor_write_ex(SocketDescr sk, SocketIOInfo* sioinfo) {
    if(sioinfo == nil) { // Fail: not enough arguments
        fprintf(stderr, "SocketDescriptor_write_ex(): Not enough arguments provided.\n");
        sioinfo->last_error = SIO_WRITEEX_NOT_ENOUGH_ARGS;

        return False;
    } else {
        // Check for buffer
        if(sioinfo->source_buffer == nil) { // Passed null to write
            fprintf(stderr, "SocketDescriptor_write_ex(): Passed null to write.\n");

            return False;
        }

        // TODO: Check if socket is valid.
        // NOTE: Valid assumed.

        sioinfo->bytes_written = send(sk->socket, sioinfo->source_buffer, sioinfo->expected_to_write, 0);

        // Check for written
        // TODO: Check if quantity written was expected
        // TODO: Check if more edge cases could ocur.
        if(sioinfo->bytes_written == -1) { // Check if send(2) failed.
            perror("SocketDescriptor_write_ex()#send()");

            sioinfo->last_error = SIO_WRITE_SEND_FAILED;
            sioinfo->bytes_written = 0;

            return False;
        } else {
            sioinfo->success = True;

            return True;
        }
    }
}



// NOTE: This allocs memory.
u1 SocketDescriptor_read_all(SocketDescr sk, icstr destiny) {
    SocketIOInfo sioinfo;

    sioinfo.expected_to_read = SOCKET_READ_ALL_MIN;
    sioinfo.to_read = SOCKET_READ_ALL_MIN;
    sioinfo.should_malloc = True;
    sioinfo.minimum_to_read = SOCKET_READ_ALL_MIN;

    if(!SocketDescriptor_read_ex(sk, &sioinfo, destiny)) {
        fprintf(stderr,
            "SocketDescriptor_read_all(): Failed to read\n"
        );

        return False;
    }

    sk->total_received += sioinfo.bytes_readed > 0 ? sioinfo.bytes_readed : 0; // This might be useful.

    return True;
}

// This allocated memory.
u1 SocketDescriptor_read(SocketDescr sk, icstr destiny, const usize how_much) {
    if(how_much <= 0) {
        fprintf(stderr, "SocketDescriptor_read(): Are you really requesting to read %ld bytes?\n", how_much);
        fprintf(stderr, "ERROR: Could you just not call me then?\n");

        return False;
    }

    SocketIOInfo sioinfo;

    sioinfo.expected_to_read = how_much;
    sioinfo.to_read = how_much;
    sioinfo.should_malloc = True;
    sioinfo.minimum_to_read = how_much;

    if(!SocketDescriptor_read_ex(sk, &sioinfo, destiny)) {
        fprintf(stderr,
            "SocketDescriptor_read_all(): Failed to read\n"
        );

        return False;
    }

    sk->total_received += sioinfo.bytes_readed > 0 ? sioinfo.bytes_readed : 0; // This might be useful.

    return True;
}

u1 SocketDescriptor_write_all(SocketDescr sk, const icstr source) {
    SocketIOInfo sioinfo;

    const usize buflen = strlen(source);

    sioinfo.source_buffer = source;
    sioinfo.expected_to_write = buflen;
    sioinfo.to_write = buflen;
    sioinfo.should_malloc = False; // Redundant but whatever.
    sioinfo.minimum_to_write = buflen;

    if(!SocketDescriptor_write_ex(sk, &sioinfo)) {
        fprintf(stderr,
            "SocketDescriptor_write_all(): Failed to write\n"
        );

        return False;
    }

    sk->total_transmit += sioinfo.bytes_written > 0 ? sioinfo.bytes_written : 0; // This might be useful.

    return True;
}