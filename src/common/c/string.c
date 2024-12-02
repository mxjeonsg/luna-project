#pragma once

#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <string.h>

#include "string.h"


String String_new(const icstr data, const intsize hm) {
    String ret;
    ret.capacity = 0;
    ret.index = 0;
    ret.size = 0;

    if(data != null) {
        const uintsize len = hm > 0 ? hm : strlen(data);

        ret.pre = True;
        ret.capacity = (sizeof(int8) * len) + 2;
        
        ret.data = (icstr) malloc(ret.capacity);
        //TODO: check if malloc failed.
        // NOTE: continuing anyway.

        snprintf(ret.data, len, "%s", data);

        return ret;
    } else return ret;
}

boolean String_renew(String* orig, const icstr cstr, const intsize hm) {
    if(orig == null || cstr == null || orig->data == null) return False;

    const intsize len = hm > 0 ? hm : strlen(cstr);

    orig->capacity = (sizeof(int8) * len) + 2;
    orig->data = realloc(orig->data, orig->capacity);
    snprintf(orig->data, len, "%s", cstr);
    return True;
}
    
void String_delete(String* obj) {
    if(obj != NULL) {
        obj->capacity = 0;
        obj->index = 0;
        obj->size = 0;
        free(obj->data);
    }
}


String String_concat(const String* orig, const String* b) {
    String ret;
    ret.capacity = 0;
    ret.index = 0;
    ret.size = 0;

    if(orig != null && b != null && orig->data != null && b->data != null && (orig->size + b->size) > 0) {
        ret.pre = True;

        const uintsize len = orig->size + b->size;
        ret.capacity = (sizeof(int8) * len) + 2;

        ret.data = (icstr) malloc(ret.capacity);

        snprintf(ret.data, len, "%s%s", orig->data, b->data);
    }

    return ret;
}

String String_duplicate(const String* orig) {
    String ret;
    ret.capacity = 0;
    ret.index = 0;
    ret.size = 0;

    if(orig != null && orig->data != null && orig->size > 0) {
        ret.pre = True;

        const uintsize len = orig->size;
        ret.capacity = (sizeof(int8) * len) + 2;

        ret.data = (icstr) malloc(ret.capacity);

        snprintf(ret.data, len, "%s", orig->data);
    }

    return ret;
}

boolean String_isValid(const String* obj) {
    if(obj == null) return False;
    if(obj->data == null) {
        return False;
    } else {
        if(obj->capacity < obj->size) return False;
    }

    return True;
}

boolean String_compare(const String* a, const String* b) {
    if(a == null || a->data == null || b == null || b->data == null) return False;

    if(a->size != b->size) return False;

    for(uintsize i = 0; i <= a->size; i++) {
        if(a->data[i] != b->data[i]) return False;
    }

    return True;
}

boolean String_Ccompare(const String* a, const icstr b, const intsize hm) {
    if(a == null || a->data == null || b == null) return False;
    if(a->size != strlen(b)) return False;

    for(uintsize i = 0; i <= a->size; i++) {
        if(a->data[i] != b[i]) return False;
    }

    return True;
}