#pragma once

#include <stdint.h>
#include <stdbool.h>

const uint8_t* COMPILER = "clang";
const uint8_t USE_OPENSSL = true;

// You're right. The reason I chose to make this
// macros instead of const u8* is because of
// string concatenation. It's easier and safer this
// way. :3
#define BUILD_OUTPUT_PATH "build"
#define BUILD_OUTPUT "luna"

#define CERTS_PATH "certs"
#define CERTS_KEY CERTS_PATH"/server.key"
#define CERTS_CRT CERTS_PATH"/server.cer"
const uint8_t* CERTS_EXPIRACY_DAYS = "365";

const uint8_t
    MUTE_ALL_WARNINGS = false,
    ENABLE_SYMBOLS = true,
    DISABLE_OPENSSL = false
;

// Application details. Nothing particularly
// special or important to modify.
const uint8_t
    *APP_NAME = "Luna Project - Backend",
    *APP_DESC = "<none>",
    *APP_VERSION = "0.0",
    *APP_CODENAME = "lilith",
    *NOB_VERSION = "release v1.9.0"
;