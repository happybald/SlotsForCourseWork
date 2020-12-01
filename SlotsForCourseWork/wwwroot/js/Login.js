$(document).ready(function () {
    $("#loginFormActive").on('submit',
        function () {
            $('#result_Loginform').html('');
            $('#loginbtn').addClass("disabled");
            if ($('#UserNameL').val() === "" || $('#PasswordL').val() === "") {
                setTimeout(function () {
                    $('#loginbtn').removeClass('disabled');
                }, 8000);
                $('#result_Loginform').html('Fill all fields!');
                return false;
            }
            sendAjaxLogin('/Account/Login')
            return false;
        });
});
function sendAjaxLogin(url) {
    var rememberbool;
    if (($('#rememberMeL').val().localeCompare("On"))) {
        rememberbool = true;
    }
    else {
        rememberbool = false;
    }
    var Result = {
        UserName: "" + $('#UserNameL').val() + "",
        Password: "" + $('#PasswordL').val() + "",
        RememberMe: "" + rememberbool + "",
        ReturnUrl: "null",
    }
    $.ajax({
        url: url, //url страницы 
        type: "POST", //метод отправки
        beforeSend: function (request) {
            request.setRequestHeader("RequestVerificationToken", $("[name='__RequestVerificationToken']").val());
        },
        data: Result,
        success: function (response) { //Данные отправлены 
            if (!response.status) {
                Materialize.toast(response.statusMessage, 4000) // 4000 is the duration of the toast
                $('#result_Loginform').html(response.statusMessage);
                setTimeout(function () {
                    $('#loginbtn').removeClass('disabled');
                }, 8000)
            } else {
                Materialize.toast(response.statusMessage, 4000) // 4000 is the duration of the toast
                $('#result_Loginform').html(response.statusMessage);
                setTimeout(function () { location.reload() }, 1000);
            }
        },
        error: function (response) { // Данные не отправлены
            Materialize.toast('Error!', 4000) // 4000 is the duration of the toast
            $('#result_Loginform').html('Error. Send error!');
            setTimeout(function () {
                $('#loginbtn').removeClass('disabled');
            }, 8000);
        }
    });
};