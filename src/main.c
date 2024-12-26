#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#include <unistd.h>

#include <arpa/inet.h>

#include <openssl/ssl.h>
#include <openssl/err.h>

#include "../nob_config.h"

#include "types.h"

const short PORT = 4433;

const char* CERT_FILE = CERTS_CRT;
const char* KEY_FILE = CERTS_KEY;


void init_openssl() {
    SSL_load_error_strings();

    OpenSSL_add_ssl_algorithms();
}

void cleanup_openssl() {
    EVP_cleanup();
}

SSL_CTX* createContext() {
    SSL_CTX* ctx = SSL_CTX_new(TLS_server_method());

    if(!ctx) {
        perror("Unable to create SSL context.");
        ERR_print_errors_fp(stderr);
        exit(2);
    }

    return ctx;
}

void configureCtx(SSL_CTX* ctx) {
    if(SSL_CTX_use_certificate_file(ctx, CERT_FILE, SSL_FILETYPE_PEM) <= 0) {
        ERR_print_errors_fp(stderr);
        exit(2);
    }

    if(SSL_CTX_use_PrivateKey_file(ctx, KEY_FILE, SSL_FILETYPE_PEM) <= 0) {
        ERR_print_errors_fp(stderr);
        exit(2);
    }
}

_Bool main(int32 argc, int8* argv[]) {
    static int32 server_fd = 0;
    struct sockaddr_in addr = {};
    socklen_t addrlen = sizeof(addr);

    init_openssl();
    SSL_CTX* ctx = createContext();
    configureCtx(ctx);

    addr.sin_family = AF_INET;
    addr.sin_port = htons(PORT);
    addr.sin_addr.s_addr = INADDR_ANY;

    if(bind(server_fd, (struct sockaddr*) &addr, sizeof(addr)) < 0) {
        perror("Unable to bind.");
        return 1;
    }

    if(listen(server_fd, 1) < 0) {
        perror("Unable to listen.");
        return 1;
    }

    printf("Listening on port %d.\n", PORT);

    while(1) {
        int client_fd = accept(server_fd, (struct sockaddr*)&addr, &addrlen);
        if(client_fd < 0) {
            perror("Unable to accept.");
            return 1;
        }

        SSL* ssl = SSL_new(ctx);
        SSL_set_fd(ssl, client_fd);

        if(SSL_accept(ssl) < 0) {
            ERR_print_errors_fp(stderr);
        } else {
            const int8* response =
                "HTTP/1.1 200 Ok\r\n"
                "Content-Type: text/html\r\n"
                "\r\n"
                "<html>\r\n"
                "    <head>\r\n"
                "    </head>\r\n"
                "    <body>\r\n"
                "        <marquee>Welcum</marquee>\r\n"
                "    </body>\r\n"
                "</html>"
            ;

            SSL_write(ssl, response, strlen(response));
        }

        SSL_shutdown(ssl);
        SSL_free(ssl);
        close(client_fd);
    }

    close(server_fd);
    SSL_CTX_free(ctx);
    cleanup_openssl();

    return 0;
}