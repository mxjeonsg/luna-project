#pragma once

#include "../../types.h"

struct String {
    boolean pre;
    icstr data;
    uintsize capacity, size, index;
};
typedef struct String String, StringView;

#define String_arg(x) x.data

String String_new(const icstr data, const intsize len);
boolean String_renew(String* orig, const icstr cstr, const intsize hm);
void String_delete(String* obj);
boolean String_isValid(const String* obj);


String String_concat(const String* orig, const String* b);
String String_duplicate(const String* orig);
boolean String_compare(const String* a, const String* b);
boolean String_Ccompare(const String* a, const icstr b, const intsize hm);