
function showHide(id) {
    var password = document.getElementById(id);
    var cond = password.type === "password";
    password.type = cond ? "text" : "password";
}

function generatePassword() {
    var characters = "";
    var input = document.getElementById("length");
    var label = document.getElementById("error-label");
    var includesLowercase = document.getElementById("lowercase");
    var includesUppercase = document.getElementById("uppercase");
    var includesNumbers = document.getElementById("numbers");
    var includesAscii = document.getElementById("ascii");
    var length = document.getElementById("length").value;
    if (validateLength())
    {
        if (includesLowercase.checked) { characters += "abcdefghijklmnopqrstuvwxyz"; }
        if (includesUppercase.checked) { characters += "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; }
        if (includesAscii.checked) { characters += "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~"; }
        if (includesNumbers.checked) { characters += "0123456789"; }
        if (characters != "") {
            var password = "";
            for (var i = 0; i < length; i++) {
                var randomChar = characters.charAt(Math.floor(Math.random() * characters.length));
                password += randomChar;
            }

            var outcome = document.getElementById("outcome");
            outcome.value = password;
        } else {
            var label = document.getElementById("error-label");
            label.innerHTML = "Select at least one of the options.";
            label.style.display = "inline-block";
            input.classList.add("input-invalid");
            input.classList.remove("input-valid");
        }
    } 
}

function validateLength() {
    var input = document.getElementById("length");
    var label = document.getElementById("error-label");
    console.log(input.value)
    console.log(input.value.length)
    if (input.value === "" || isNaN(input.value)) {
        label.style.display = "inline-block";
        label.innerHTML = "Invalid length."
        input.classList.add("input-invalid");
        input.classList.remove("input-valid");
        return false;
    }
    input.value.trim();
    if (input.value.length > 6)
    {
        label.style.display = "inline-block";
        label.innerHTML = "Maximum length for generation is 999999."
        input.classList.add("input-invalid");
        input.classList.remove("input-valid");
        return false;
    }
    label.style.display = "none";
    input.classList.add("input-valid");
    input.classList.remove("input-invalid");
    return true;
}

function copyToClipboard() {
    var outcomeInput = document.getElementById("outcome");
    outcomeInput.select();
    outcomeInput.setSelectionRange(0, 99999);
    document.execCommand("copy");
    window.getSelection().removeAllRanges();
}