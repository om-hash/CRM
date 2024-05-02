

function initPhoneNumber(e) {
    var input = document.querySelector(`#${e}`),
        errorMsg = document.querySelector("#error-msg"),
        validMsg = document.querySelector("#valid-msg");

    var errorMap = ["Invalid number", "Invalid country code", "Too short", "Too long", "Invalid number", "Invalid number"];

    var e = window.intlTelInput(input, {
        allowDropdown: true,
        autoHideDialCode: false,
        autoPlaceholder: "on",

        formatOnDisplay: false,

        initialCountry: "tr",

        nationalMode: false,

        placeholderNumberType: "MOBILE",

        separateDialCode: true,

        utilsScript: "/lib/intl-tel-input/js/utils.js",
    });
    var reset = function () {
        input.classList.remove("error");
        errorMsg.innerHTML = "";
        errorMsg.classList.add("hide");
        validMsg.classList.add("hide");
    };

    // on blur: validate
    input.addEventListener('blur', function () {
        reset();
        if (input.value.trim()) {
            if (e.isValidNumber()) {
                validMsg.classList.remove("hide");
            } else {
                input.classList.add("error");
                var errorCode = e.getValidationError();
                errorMsg.innerHTML = errorMap[errorCode];
                errorMsg.classList.remove("hide");
            }
        }
    });

    //// on keyup / change flag: reset
    input.addEventListener('change', reset);
    input.addEventListener('keyup', reset);
                    //-----------------------------------------------------------------
}

