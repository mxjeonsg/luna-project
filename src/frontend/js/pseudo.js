const feed = document.getElementById("feed");

const typeCheck = function(against, type, msg) {
    if(typeof(against) === type) {
        if(msg !== null || msg !== false) {
            throw new Error(msg);
        } else {
            throw new Error(`typeCheck: ERROR: Passed ${against.type} instead of ${type}.`);
        }
    }
}

const asPost = function(user, data) {
    typeCheck(data, String, null);
    typeCheck(user, String, null);

    // const to_ret = `<div class="feed-post"><p><strong>${user}:</strong>${data}</p></div>`
    const to_ret = `<div class="feed-post"><p><strong>@${user}:</strong>${data}</p></div>`;
    
    console.log(`asPost: return: ${to_ret}`);

    return to_ret;
}

const pushToFeed = function(string) {
    typeCheck(feed, HTMLElement, null);
    typeCheck(string, String, null);

    document.getElementById("feed").innerHTML += asPost("user3", "sexito");
}