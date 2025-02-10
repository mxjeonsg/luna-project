const pullGaryQuote = function() {
    fetch("https://garybot.dev/api/quote").then((response) => {
        if(!response.ok) {
            throw new Error("Failed to pull a gary quote.:(" + response.status);
        }
        
        return response.json();
    }).then((data) => {
        console.log(data.quote);
    }).catch((error) => {
        console.error("Failed to pull a gary quote:(", response.status);
    });
}

const entrypoint = function() {
    pullGaryQuote();
}

entrypoint();