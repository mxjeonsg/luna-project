#pragma once

#include "requests.h"

const icstr read_request_method(const char* request_line) {
    return strtok((icstr) request_line, " ");
}

const icstr read_request_path(const char* request_line) {
    strtok((icstr) request_line, " "); // Skip method lmao
    return strtok(null, " ");
}

const void read_headers(const int32 fd) {
    int8 buffer[1024] = {0};

    while(read(fd, buffer, 1024) && !strcmp(buffer, "\r\n")) {
        printf("Header: %s\n", buffer);
    }
}

const void read_body(const int32 fd, const uintsize content_length) {
    icstr body = malloc(sizeof(int8) * (content_length + 2));

    if(body == null) {
        // Handle memory allocation fail
    }
    read(fd, body, content_length);
    body[content_length] = '\0';

    printf("Body: %s\n", body);

    free(body);
}

const void handle_request(const int32 fd) {
    int8 buffer[1024] = {0};

    read(fd, buffer, 1024);

    const icstr method = read_request_method(buffer);
    const icstr path = read_request_path(buffer);

    printf("Method: %s\nPath: %s\n", method, path);

    read_headers(fd);

    int32 content_length = 0;
    if(strstr(buffer, "Content-Length:") != null) {
        sscanf(buffer, "Content-Length: %d", &content_length);
    }

    if(content_length > 0) {
        read_body(fd, content_length);
    }
}