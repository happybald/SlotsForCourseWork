$(document).ready(function () {
    $("#registerbtn").click(
        function () {
            $('#result_RegisterForm').html('');
            $('#registerbtn').addClass("disabled");
            if ($('#UserNameR').val() == "" || $('#PasswordR').val() == "" || $('#EmailR').val() == "" || $('#ConfirmPasswordR').val() == "") {
                setTimeout(function () {
                    $('#registerbtn').removeClass('disabled');
                }, 8000);
                $('#result_RegisterForm').html('Fill all fields!');
                return;
            }
            console.log($('#EmailR').val().indexOf("@"));
            if ($('#EmailR').val().indexOf("@") != -1) {
                setTimeout(function () {
                    $('#registerbtn').removeClass('disabled');
                }, 8000);
                $('#result_RegisterForm').html('Enter correct email!');
                return;
            }
            sendAjaxRegister('/Account/Register')
            return false;
        });
});
function sendAjaxRegister(url) {
    var Result = {
        UserName: "" + $('#UserNameR').val() + "",
        Email: "" + $('#EmailR').val() + "",
        Password: "" + $('#PasswordR').val() + "",
        PasswordConfirm: "" + $('#ConfirmPasswordR').val() + "",
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
                var errorsmeassge = response.statusMessage  .replace(/,/g, "<br/>");
                console.log(errorsmeassge);
                Materialize.toast(errorsmeassge, 4000) // 4000 is the duration of the toast
                $('#result_RegisterForm').html(errorsmeassge);
                setTimeout(function () {
                    $('#registerbtn').removeClass('disabled');
                }, 8000)
            } else {
                Materialize.toast(response.statusMessage, 4000) // 4000 is the duration of the toast
                $('#result_RegisterForm').html(response.statusMessage);
                location.reload();
            }
        },
        error: function (response) { // Данные не отправлены
            Materialize.toast('Error!', 4000) // 4000 is the duration of the toast
            $('#result_RegisterForm').html('Error. Send error!');
            setTimeout(function () {
                $('#registerbtn').removeClass('disabled');
            }, 8000);
        }
    });
};