main_api: src/backend/main_api.c
	clear
	mkdir -p ./build
	clang -o ./build/main_api ./src/backend/main_api.c