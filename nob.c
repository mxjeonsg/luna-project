#include <stdio.h>
#include <stdint.h>

#include "src/types.h"

#ifdef unix
    // This mostly carries the meaning "UNIX system detected."
    // So `uname(2)` can be used to know which environment the
    // program is compiled onto.
    #include <sys/utsname.h>
#endif

#define NOB_IMPLEMENTATION
#include "nob.h"
#include "nob_config.h"

int32 help_screen(const int32 argc, const int8* argv[]) {
    const uint8* local_main_help =
        "The available options are:\n"
        "--? <topic>        Show this screen.\n"
        "--help <topic>     Show this screen.\n"
        "? <topic>          Show this screen.\n"
        "*) The topics are:\n"
        "   -) config       Shows all the configurations that are possible to\n"
        "                   be changed in the \"./config.h\" file used by Nob\n"
        "                   to change the compiling options and behaviours.\n"
        "\n"
        "   -) certs        Show all the information related to the certificates\n"
        "                   created by the `--gen-certs` flag.\n"
        "\n"
        "   -) nob          This script looks interesting for you? Well, there's a\n"
        "                   dedicated topic for it, as I found it interesting\n"
        "                   philosophywise and technicalwise.\n"
        "*) The commands are:\n"
        "-) --gen-certs        This generates the needed certificate files\n"
        "                      for the backend to work. Since secure connections\n"
        "                      are to be tried to be made.\n"
        "                      You can disable it setting `DISABLE_OPENSSL` in the\n"
        "                      \"./config.h\" file."
        "\n"
        "-) --compile-in <in>  This flag enforces using default compiling behaviours\n"
        "                      and/or configurations for a determined target (<in>).\n"
        "                      The targets are:\n"
        "                      >) FreeBSD\n"
        "                      >) WSL2\n"
        "                      >) Linux\n"
        "                      >) MSYS (Windows)\n"
        "                      >) MinGW (Windows)\n"
        "\n\n"
        "Build script(?) powered by NoBuild(r) and GO REBUILD URSELF(r) technologies.\n"
        "Nob version release v1.9.0 - <https://github.com/tsoding/nob.h>\n"
        "Thanks to Zozzin<3"
    ;

    const uint8* local_topic_nob =
        "Luna Project > Help > Topics > nob\n"
        "\n"
        "\n"
        "In fact, you watched the root directory of the repo holds two interesting files:\n"
        "`nob.h` and `nob.c`. Well, `nob.c` is the build script that's used to compile Luna.\n"
        "The usage is fairly simple:\n"
        "*) `clang -o nob nob.c`\n"
        "And that's supposed to output this builder, that holds the help screens, and everything\n"
        "that's needed to compile Luna. And it's all possible thanks to the `nob.h` file, which is\n"
        "a header library including NoBuild. And yeah, I could've used Makefiles and call it a day. But,\n"
        "as a Zozzin follower, I watched the \"nob\" videos, that covers making NoBuild(r), why Zozzin came up\n"
        "with the idea, and the philosophy behind the project, which I'll try to explain by my own words:\n"
        "Why using another languages into your project when you can just use C. If your project it's made\n"
        "in C, why won't you build that project with C as well?\n"
        "Sometimes, even Makefiles are an overhead for simpler projects, sometimes building C with C is cooler, \n"
        "and god forbids you using Cmake, your code won't build the next 10 years. (Well, you can, but you have to\n"
        "find the correct Cmake version you used when you made your Cmake setup and files, which makes things more\n"
        "difficultier than have to be really.)\n"
        "And since Zozzin proved NoBuild(r) is resilient through time (he'd use older versions of Nob(r) in his\n"
        "projects because the Nob(r) headers would be there in the proper project's source code and the header would\n"
        "still compile, I think I want to spend more time taking care of maintaining my project source code, than\n"
        "checking the shit could compile in the first place. I can feel safe with the fact that making this file once\n"
        "and maintaning ONLY the help section will be a safe bet, this will still compile, and if it doesn't, it\n"
        "has to be mostly my fault writing this than the header's one.\n"
        "\n"
        "I really encourage you to check through NoBuild code, and potentially using it for your own projects.\n"
        "(If they're recreational programming, sure you'll enjoy using this, if you pretend to use it for\n"
        "production, I won't stop you, but keep in mind this technologies are focused more in recreational\n"
        "programming projects.)\n"
        "\n"
        "\n"
        "NoBuild(r) repo: (https://github.com/tsoding/nob.h)\n"
        "Zozzin playlist of Nob(r): (https://www.youtube.com/playlist?list=PLpM-Dvs8t0Va1sCJpPFjs2lKzv8bw3Lpw)"
    ;

    printf("%s - version %s (codenamed %s.)\n",
        APP_NAME, APP_VERSION, APP_CODENAME
    );
    printf("%s\n", APP_DESC);

    for(int16 arg = 0; arg <= argc; arg++) {
        if(argv[arg] == NULL) {
            printf("%s\n", local_main_help);
            return true;
        }

        if(!strcmp(argv[arg], "config")) {
            printf("ERROR: Unimplemented help topic.\n");
            return false;
        } else if(!strcmp(argv[arg], "certs")) {
            printf("ERROR: Unimplemented help topic.\n");
            return false;
        } else if(!strcmp(argv[arg], "nob")) {
            printf("%s\n", local_topic_nob);
            return true;
        } else if(!strcmp(argv[arg], "--gen-certs")) {
            printf("ERROR: Unimplemented help topic.\n");
            return false;
        } else if(!strcmp(argv[arg], "--compile-in")) {
            printf("ERROR: Unimplemented help topic.\n");
            return false;
        } else {
            printf("ERROR: Unknown help topic: \"%s\".\n", argv[arg]);
            return false;
        }
    }

    return true;
}

int32 main(int32 argc, int8* argv[]) {
    NOB_GO_REBUILD_URSELF(argc, argv);

    // Sometimes I build it in MSYS,
    // sometimes with MinGW, sometimes
    // in wsl, or FreeBSD. I need to not
    //hardcode this thing.
    const int8* PROGRAM = argv[0];

    // By the way, skip first argument.
    argc--; argv++;


    uint8
        compileIn = 0,
        compileEnv = 0
    ;

    if(argc > 0) for(int arg = 0; arg <= argc; arg++) {
        if(!strcmp(argv[arg], "--compile-in")) {
            if(argv[arg + 1] == NULL) {
                fprintf(stderr, "ERROR: Not enough arguments were provided.\n");
                fprintf(stderr, "ERROR: In \"--compile-in\": argument \"<target>\" was expected.\n");
                fprintf(stderr, "ERROR: Argument n.-%d.\n", arg + 1);
                fprintf(stderr, "ERROR: Do `%s --compile-in help` to see the correct usage of this command.\n", PROGRAM);
                return false;
            } else {
                if(!strcmp(argv[arg + 1], "freebsd")) {
                    compileIn = 1;
                } else if(!strcmp(argv[arg + 1], "wsl")) {
                    compileIn = 2;
                } else if(!strcmp(argv[arg + 1], "msys")) {
                    compileIn = 3;
                } else if(!strcmp(argv[arg + 1], "mingw")) {
                    compileIn = 4;
                } else if(!strcmp(argv[arg + 1], "help")) {
                    // Implement the help page.
                    fprintf(stderr, "NOTE: Not implemented.\n");
                    fprintf(stderr, "NOTE: `%s --compileIn help`\n", PROGRAM);
                } else {
                    fprintf(stderr, "ERROR: Argument isn't valid.\n");
                    fprintf(stderr, "ERROR: In \"--compileIn\": argument \"%s\" isn't a valid target.\n", argv[arg + 1]);
                    fprintf(stderr, "ERROR: Argument n.-%d.\n", arg + 1);
                    fprintf(stderr, "ERROR: Do `%s --compileIn help` to see the correct usage of this command.\n", PROGRAM);

                    return false;
                }
            }
        } else if(!strcmp(argv[arg], "--run")) {
            Nob_Cmd command = {};
            nob_cmd_append(&command, BUILD_OUTPUT_PATH"/"BUILD_OUTPUT);

            if(!nob_cmd_run_sync(command)) {
                fprintf(stderr, "OWWW: Execution failed.\n");
                return false;
            }
        } else if(!strcmp(argv[arg], "--gen-certs")) {
            Nob_Cmd command = {};
            if(!nob_mkdir_if_not_exists(CERTS_PATH)) {
                fprintf(stderr, "ERROR: Failed to create certificate path.\n");
                fprintf(stderr, "ERROR: Check the path you've chosen in the \"./config.h\" file and try again.\n");
                fprintf(stderr, "ERROR: By the way, the path chosen is: \"./%s\".\n", CERTS_PATH);

                return false;
            }

            nob_cmd_append(&command,
                "openssl", "req", "-x509", "-nodes", "-days", CERTS_EXPIRACY_DAYS,
                "-newkey", "rsa:2048", "-keyout", CERTS_KEY, "-out", CERTS_CRT
            );

            if(!nob_cmd_run_sync(command)) {
                fprintf(stderr, "OUCH: Execution failed. :(\n");

                return false;
            }

            return true;
        } else if(!strcmp(argv[arg], "--?") || !strcmp(argv[arg], "--help") || !strcmp(argv[arg], "?")) {
            argc--; argv++;
            return help_screen(argc, argv);
        } else {
            fprintf(stderr, "EHMM: The command \"%s\" is supposed to do what exactly?\n", argv[arg]);
            fprintf(stderr, "ERROR: You might want to see the help screen?\n");
            fprintf(stderr, "ERROR: Fyi, run `%s --help` or `%s --?` or at least `%s ?` to see the help screen.\n",
                PROGRAM, PROGRAM, PROGRAM
            );

            return false;
        }
    }

    // Try to fetch system type automatically.
    struct utsname uname_info = {};
    if(uname(&uname_info) < 0) {
        fprintf(stderr, "WARN: Failed to get uname(2) info.\n");
        fprintf(stderr, "WARN: So, we're defaulting to Linux environment.\n");
    }

    if(!strcmp(uname_info.sysname, "Linux")) {
        compileEnv = 1;
    } else {
        fprintf(stderr, "ERROR: Platform \"%s\" mightn't be supported or not implemented yet.\n", uname_info.sysname);
        return false;
    }

    // Print current environmental settings.
    printf("------------------------------\n");
    printf(
        "OS to compile to: %s\n",
        compileEnv == 1 ? "Linux" : "Unknown"
    );
    printf("------------------------------\n");
    
    Nob_Cmd command = {};
    nob_cmd_append(&command, COMPILER);
    if(!nob_mkdir_if_not_exists(BUILD_OUTPUT_PATH)) {
        fprintf(stderr, "ERROR: Failed to create the folder \"%s\" in current working directory.\n", BUILD_OUTPUT_PATH);
        fprintf(stderr, "ERROR: Check your path choice in the file \"./config.h\" and try it again.\n");
        return false;
    }
    nob_cmd_append(&command, "-o", BUILD_OUTPUT_PATH"/"BUILD_OUTPUT);
    nob_cmd_append(&command, "src/main.c");

    if(USE_OPENSSL){ 
        nob_cmd_append(&command, "-lssl", "-lcrypto");
    }

    if(MUTE_ALL_WARNINGS) {
        nob_cmd_append(&command, "-w");
    }

    if(ENABLE_SYMBOLS) {
        nob_cmd_append(&command, "-g");
    }

    if(!nob_cmd_run_sync(command)) {
        fprintf(stderr, "OUCH: Compiling failed. :(\n");
        fprintf(stderr, "OUWIE: Check what could've go wrong. :c\n");

        return false;
    }

    return true;
}