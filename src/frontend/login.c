#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <string.h>

#include <sys/types.h>
#include <sys/socket.h>

#include <arpa/inet.h>
#include <netinet/in.h>

#include "../types.h"

int32 main(int32 argc, icstr argv[], icstr envp[]) {
    int8
    response[9999] = {0}, request[9999] = {0},
    name[250] = {0}, username[250] = {0},
    locale[250] = {0}, extra[2500] = {0};
    int32 year = 0;


    while(True) {
        printf("Name: ");
        scanf("%s", name);
        printf("Username: ");
        scanf("%s", username);
        printf("Year: ");
        scanf("%d", &year);
        printf("Locale: ");
        scanf("%s", locale);
        printf("Extra: ");
        scanf("%s", extra);

        printf("Sending...\n");

        snprintf(request, 9999,
            "{\r\n"
            "  \"displayname\": \"%s\",\r\n"
            "  \"username\": \"%s\",\r\n"
            "  \"year\": %d,\r\n"
            "  \"locale\": \"%s\",\r\n"
            "  \"extra\": \"%s\",\r\n"
            "}",
            name, username, year, locale, extra
        );

        int32 sk = socket(AF_INET, SOCK_STREAM, 0);
        if(sk < 0) {
            perror("socket(2)");
        }

        struct sockaddr_in addr = {
            .sin_family = AF_INET,
            .sin_port = htons(6969),
            .sin_addr.s_addr = inet_addr("127.0.0.1")
        };

        if(connect(sk, (struct sockaddr*)&addr, sizeof(addr)) < 0) {
            perror("connect(2)");
            close(sk);
            continue;
        }

        send(sk, request, strlen(request), 0);
        printf("Sent.\n");

        
        if(read(sk, response, 9999) > 0) printf("Response: %s\n", response);
        else printf("No response was received.\n");
        continue;
    }

    return(0);
}