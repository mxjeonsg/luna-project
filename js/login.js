const submitForm = function() {
    const formdata = {
        country: document.getElementById("form_country").value,
        age: document.getElementById("form_age").value,
        email: document.getElementById("form_mail").value,
        username: document.getElementById("form_nick").value,
        password: document.getElementById("form_password").value,
        displayName: document.getElementById("form_displayname").value
    };

    console.log("User data:", formdata);

    alert("Login successful! Now into the IQ test.");
}