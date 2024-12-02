const populateFeed = function() {
    const postAmount = 50;

    const feedobject = document.getElementsByClassName("feed-post-container")[0];

    feedobject.innerHTML += new Post(null, null, null, null, null).randomBulk(postAmount);
}