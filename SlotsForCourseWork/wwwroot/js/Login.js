$(document).ready(function () {
    $("#loginbtn").click(
        function () {
            $('#result_Loginform').html('');
            $('#loginbtn').addClass("disabled");
            if ($('#UserNameL').val() == "" || $('#PasswordL').val() == "") {
                console.log(true);
                setTimeout(function () {
                    $('#loginbtn').removeClass('disabled');
                }, 8000);
                $('#result_Loginform').html('Fill all fields!');
                return;
            }
            sendAjaxLogin('/Account/Login')
            return false;
        });
});
function sendAjaxLogin(url) {
    console.log("send block");
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
    console.log($("[name='__RequestVerificationToken']").val());
    console.log(Result);
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
                location.reload();
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