const getGaryPic = function() {    
    fetch("https://garybot.dev/api/gary").then((response) => {
        if(!response.ok) {
            throw new Error("Failed to fetch a gary pic. :(" + response.status);
        }

        return response.json();
    }).then((data) => {
        document.getElementById("garypic_container").src = data.url;
    }).catch((error) => {
        console.error("Failed to fetch a gary pict. :(" + error.status);
    });
}

const entrypoint = function() {
    getGaryPic();
}

entrypoint();