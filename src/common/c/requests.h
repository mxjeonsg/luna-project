#pragma once

#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <string.h>

#include "../../types.h"

struct RequestDescriptor {
    icstr
    originalMethodPtr, originalPathPtr,
    originalHeadersPtr, originalBodyPtr;

    icstr
    methodBuff, pathBuff, headersBuff,
    bodyBuff;
};

const icstr read_request_method(const char* request_line);
const icstr read_request_path(const char* request_line);
const void read_headers(const int32 fd);
const void read_body(const int32 fd, const uintsize content_length);
const void handle_request(const int32 fd);