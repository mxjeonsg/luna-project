class LoginInfo {
    // TODO: Upload the data to Chinese gvmt. servers.
    // TODO: Ask for cloud services to Tencent.

    displayname = "";
    username = "";
    birthyear = 1900;
    country = "kp";
    language = "en";
    extradata = "";

    constructor(dn, un, by, cn, lg, ed) {
        this.displayname = dn;
        this.username = un;
        this.birthyear = by;
        this.country = cn;
	    this.language = lg;
        this.extradata = ed;
    }

    upload() {
        // Make the string to send
        // TODO: check if a JS standard function
        // or class has something that does this
        // automatically. But for now, I'm doing
        // it this way. (Maybe this solution ends
        // up being better idk.)
        const tosend =
        	 "{\n"
            + `"placeholder": "urmom",\n`
        	+ `"displayname": "${this.displayname}",\n`
            + `"username": "${this.username}",\n`
            + `"birthyear": ${this.birthyear},\n`
            + `"locale": "${this.country}-${this.language}",\n`
            + `"extradata": "${this.extradata}"\n`
            + "}"
        ;

        const xmlhttp = new XMLHttpRequest();
        xmlhttp.open("POST", "http://127.0.0.1:6969", true);
        xmlhttp.setRequestHeader("Content-Type", "application/json");
        xmlhttp.setRequestHeader("Origin", "http://127.0.0.1");

        // TODO: maybe check a response?
        xmlhttp.onload = function() {
            console.log(xmlhttp.response);
        };

        xmlhttp.send(JSON.stringify(tosend));
    }
}

document.getElementById("login-form").addEventListener("submit", function(event) {
    event.preventDefault();

    const displayname = document.getElementById("display-name").value.trim();
    const username = document.getElementById("username").value.trim();
    const birthyear = document.getElementById("birth-year").value.trim();
    const country = document.getElementById("country").value;
    const language = document.getElementById("language").value;
    const extratext = document.getElementById("extra-text").value.trim();

    if(!birthyear) {
        alert("Birth year is mandatory!");

        return undefined;
    }

    if(birthyear < 1900 || birthyear > new Date().getFullYear()) {
        alert("Please enter a valid birth year.");

        return undefined;
    }

    document.getElementById("form-result").textContent = ret;
    console.log(ret);

    const data = new LoginInfo(displayname, username, birthyear, country, language, extratext);
    data.upload();

    setTimeout(() => {
        const ret = `Welcome ${displayname}!\n Your username is ${username}, your birth year is ${birthyear}, your chosen country is ${country} and your language is ${language}`;
        window.location.href = "http://localhost:6969/src/frontend/feed.htm";   
    }, 5);
});
