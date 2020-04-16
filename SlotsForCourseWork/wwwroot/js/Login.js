$(document).ready(function () {
    $("#loginbtn").click(
        function () {
            //$('#loginbtn').addClass("disabled");
            sendAjaxLogin('/Account/Login');
            return false;
        }
    )
});

function sendAjaxLogin(url) {
    console.log("Login func");
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
        RememberMe: ""+rememberbool+"",
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
            var resStr = JSON.stringify(response);
            console.log(resStr);
        },
        error: function (response) { // Данные не отправлены
            Materialize.toast('Error!', 4000) // 4000 is the duration of the toast
            $('#result_Loginform').html('Ошибка. Данные не отправлены.');
            $('#loginbtn').removeClass("disabled");
        }
    });
}