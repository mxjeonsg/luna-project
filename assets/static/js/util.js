const randomInteger = function(min, max) {
    const init = Math.floor(Math.random() * max);

    return init % (((max - min) + 1) - min);
}