class Author {
    id = 0;
    flag = '';
    name = "";
    username = "";
    pfpurl = `/papi/uwu/profile/${this.id}/pfp`;

    MIN_ID = 0;
    MAX_ID = 6969;

    DEFAULT_AUTHOR = [
        [ "Vladimir Vladimirovich Putin", "@putin" ],
        [ "Ivan Koldajne", "@sexo" ]
    ];

    DEFAULT_FLAG = [
        [ '🇰🇷', "Korea, Republic of" ],
        [ '🇦🇶', "Kosovo, Serbian Province of" ],
        [ '🇧🇬', "Bulgaria" ]
    ];

    generateId() {
        return randomInteger(this.MIN_ID, this.MAX_ID);
    }

    generateFlag() {
        return this.DEFAULT_FLAG[randomInteger(0, this.DEFAULT_FLAG.length)][0];
    }

    constructor(id, flag, author) {
        this.id = id != null ? id : this.generateId();
        if(author != null) {
            this.name = this.DEFAULT_AUTHOR[randomInteger(0, this.DEFAULT_AUTHOR.length - 1)][0];
            this.username = this.DEFAULT_AUTHOR[randomInteger(0, this.DEFAULT_AUTHOR.length - 1)][1];
        }
        this.flag = flag != null ? flag : this.generateFlag();
    }

    randomAuthor() {
        return new Author(this.generateId(), this.generateFlag(), null);
    }
}

class Post {
    id = 0;
    content = "";
    author = Object(Author);
    time = "";

    MIN_ID = 0;
    MAX_ID = 6969;

    DEFAULT_CONTENT = [
        "Me duele la pija",
        "Mi huevitoooo"
    ];

    DEFAULT_TIME = [
        "some time ago"
    ];

    generateId() {
        return randomInteger(this.MIN_ID, this.MAX_ID);
    }

    generateContent() {
        return this.DEFAULT_CONTENT[randomInteger(0, this.DEFAULT_CONTENT.length - 1)];
    }

    generateTime() {
        return this.DEFAULT_TIME[randomInteger(0, this.DEFAULT_TIME.length - 1)];
    }

    constructor(id, content, author, flag, time) {
        this.id = id != null ? id : this.generateId();
        this.content = content != null ? content : this.generateContent();
        this.author = author != null ? author : new Author().randomAuthor();
        this.time = time != null ? time : this.generateTime();
    }

    toString() {
        const str = "<div class=\"feed-post-post\">" + "<div>" +
        `<img class=\"feed-post-post-pfp\" src=${this.author.pfpurl} />` +
        `<p class=\"feed-post-post-name\">${this.author.name[0]}</p>` +
        "</div>" + `<p class=\"feed-post-post-uid\">${this.author.name[1]}</p>` +
        "<div>" + `<p class=\"feed-post-post-content\">${this.content}</p>` + "</div>" + "</div>";
        return str;
    }

    randomBulk(amount) {
        var str = "";
        for(let i = 0; i <= amount; i++)
            str += new Post(null, null, null, null, null).toString();

        return str;
    }
}