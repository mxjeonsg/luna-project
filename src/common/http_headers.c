#pragma once

#include "types.h"

#include <stdio.h>
#include <stdlib.h>
#include <string.h>

typedef struct HttpHeaderStc {
    icstr method;
    icstr route;
    u16 code;
    icstr code_str;
    icstr headers;
    icstr content;
} HttpHeader;

// This allocates memory.
u1 prependRequestHeaderToContent(icstr result, HttpHeader* req) {
    if(!(req->method && req->route && req->headers)) return False;

    const usize len = 
          strlen(req->method)
        + strlen(req->route)
        + strlen(req->headers)
        + req->content != NULL
        ? strlen(req->content)
        : 0
        + 10
    ;
    result = malloc(sizeof(i8) * len);

    sprintf(result,
        "%s %s HTTP/1.1\r\n"
        "%s\r\n",
        req->method, req->route, req->headers
    );

    if(req->content) {
        sprintf(result, "%s\r\n%s", result, req->content);
    }

    return True;
}

u1 prependRequestHeaderToContent_NA(i8 result[], const usize res_len, HttpHeader* req) {
    if(!(req->method && req->route && req->headers)) return False;

    const usize len = 
          strlen(req->method)
        + strlen(req->route)
        + strlen(req->headers)
        + req->content != NULL
        ? strlen(req->content)
        : 0
        + 10
    ;

    snprintf(result, res_len,
        "%s %s HTTP/1.1\r\n"
        "%s\r\n",
        req->method, req->route, req->headers
    );

    if(req->content) {
        snprintf(result, res_len, "%s\r\n%s", result, req->content);
    }

    return True;
}

// This allocs memory.
u1 prependResponseHeaderToContent(icstr result, HttpHeader* req) {
    if(!(req->code > 0 && req->code_str && req->headers && req->content)) return False;

    const usize len = 
          strlen(req->code_str)
        + strlen(req->headers)
        + strlen(req->content)
        + 14
    ;
    result = malloc(sizeof(i8) * len);

    sprintf(result,
        "HTTP/1.1 %d %s\r\n%s\r\n\r\n%s",
        req->code, req->code_str, req->headers, req->content
    );

    return True;
}

u1 prependResponseHeaderToContent_NA(i8 result[], const usize res_len, HttpHeader* req) {
    if(!(req->code > 0 && req->code_str && req->headers && req->content)) return False;

    const usize len = 
          strlen(req->code_str)
        + strlen(req->headers)
        + strlen(req->content)
        + 14
    ;

    snprintf(result,res_len, 
        "HTTP/1.1 %d %s\r\n%s\r\n\r\n%s",
        req->code, req->code_str, req->headers, req->content
    );

    return True;
}