BROWSER ?= chrome

C ?= clang -x c

C_OPTIM ?= 0
C_FLAGS ?= -g -O$(C_OPTIM)

frontendpage: src/frontend/login.htm src/frontend/feed.htm
	$(BROWSER) src/frontend/login.htm >/dev/null 2>/dev/null &

backend-login: src/backend/login.c
	clear
	$(C) $(C_FLAGS) -o build/backend-login src/backend/login.c

backend-static:
	clear
	$(C) $(C_FLAGS) -o build/backend-static src/backend/static.c

backend: backend-login backend-static

frontend-login: src/frontend/login.c
	$(C) $(C_FLAGS) -o build/frontend-login src/frontend/login.c

frontend: frontend-login

all: backend frontend


clear-compiled: build/backend-login build/frontend-login
	rm build/backend-login
	rm build/frontend-login