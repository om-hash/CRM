
const isPasswordSecure = (password) => {

    const SmallLetter = new RegExp("^(?=.*[a-z])");
    const UpdateLetter = new RegExp("^(?=.*[A-Z])");
    const Number = new RegExp("^(?=.*[0-9])");
    const Charectars = new RegExp("^(?=.*[!#@\$%\^&\*])");
    const Lenth = new RegExp("^(?=.{8,})");

    if (password === '')
        return 1;
    else if (!SmallLetter.test(password))
        return 2;
    else if (!UpdateLetter.test(password))
        return 3
    else if (!Number.test(password))
        return 4
    else if (!Charectars.test(password))
        return 5
    else if (!Lenth.test(password))
        return 6
    else
        return 0

};

function IsUserNameExisting(e) {
    let value;
    $.ajax({
        type: "post",
        url: `/Account/IsUserNameExist?userName=${e}`,
        contentType: false,
        processData: false,
        success: function (res) {
            if (res.responseType === 0) {
                value = 0;
            } else {
                value = 1;
            }
        },
        error: function (e) {
            console.error(e);
        }
    });
    if (value === 0) {
        return 0;
    } else {
        return 1;
    }
}

function IsEmailExisting(e) {
    let value;
    $.ajax({
        type: "post",
        url: `/Account/IsEmailExist?email=${e}`,
        contentType: false,
        processData: false,
        success: function (res) {
            if (res.responseType === 0) {
                value = 0;
            } else {
                value = 1;
                console.log(value)
            }
        },
        error: function (e) {
            console.error(e);
        }
    });
    if (value === 0) {
        return 0;
    } else {
        return 1;
    }
}