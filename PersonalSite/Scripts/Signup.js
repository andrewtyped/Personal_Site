var Signup;
var SignupPost;

var validUsername;
var validEmail;
var validPasswordLength;
var matchingPasswords;

var usernameValidation;
var emailValidation;
var passwordValidation;
var confirmPasswordValidation;

$(document).ready(function () {

    validUsername = function () {

        var username = $("#signupUsernameText").val();

        if ($.trim(username).length < 6) {
            return false;
        }
        else {
            return true;
        }
    };

    usernameValidation = function () {

        var signupUsernameValidation = $("#signupUsernameValidation");

        if (!validUsername()) {
            signupUsernameValidation.text("User name must be at least 6 characters");
            return false;
        }
        else {
            signupUsernameValidation.text("");
            return true;
        }
    };

    $("#signupUsernameText").bind("change", usernameValidation);
    $("#signupUsernameText").bind("keyup", usernameValidation);

    validEmail = true;

    validPasswordLength = function () {

        var pass = $.trim($("#signupPasswordText").val());

        if (pass.length >= 8) {
            return true;
        }
        else {
            return false;
        }

    };

    passwordValidation = function () {
        var signupPasswordValidation = $("#signupPasswordValidation");

        if (!validPasswordLength()) {
            signupPasswordValidation.text("Password must be at least 8 characters");
            return false;
        }
        else {
            signupPasswordValidation.text("");
            return true;
        }
    };

    $("#signupPasswordText").bind("change", passwordValidation);
    $("#signupPasswordText").bind("keyup", passwordValidation);

    matchingPasswords = function () {
        var pass = $("#signupPasswordText").val();
        var confirmPass = $("#signupConfirmPasswordText").val();

        if (pass === confirmPass) {
            return true;
        }
        else {
            return false;
        }
    };

    confirmPasswordValidation = function () {
        var signupConfirmPasswordValidation = $("#signupConfirmPasswordValidation");

        if (!matchingPasswords()) {
            signupConfirmPasswordValidation.text("These password don't match!");
            return false;
        }
        else {
            signupConfirmPasswordValidation.text("");
            return true;
        }
    };

    $("#signupConfirmPasswordText").bind("change", confirmPasswordValidation);
    $("#signupConfirmPasswordText").bind("keyup", confirmPasswordValidation);

    SignupPost = function (username, email, password, confirmPassword) {
        $.ajax({
            url: '/Signup/CreateUser',
            type: 'POST',
            data: JSON.stringify({
                username: username,
                email: email,
                password: password,
                confirmPassword: confirmPassword
            }),
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                $("#signupResult").text(data.Result)
            },
            error: function () {
                $("#signupResult").text("There was a problem sending this data to the server.");
            }
        });
    };

    $("#signupSubmitButton").bind("click", function () {
        if (usernameValidation()
            && passwordValidation()
            && confirmPasswordValidation()) {

            SignupPost($("#signupUsernameText").val(),
                $("#signupEmailText").val(),
                $("#signupPasswordText").val(),
                $("#signupConfirmPasswordText").val());
        }
    });
});