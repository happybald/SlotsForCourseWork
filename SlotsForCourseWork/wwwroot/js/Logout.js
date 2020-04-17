$(document).ready(function () {
    $("#logoutbtn").click(
        function () {
            sendAjaxLogout('/Account/Logout');
            return false;
        }
    );
    $("#logoutbtn1").click(
        function () {
            sendAjaxLogout('/Account/Logout');
            return false;
        }
    );
});

function sendAjaxLogout(url) {
    $.ajax({
        url: url, //url страницы 
        type: "POST", //метод отправки
        beforeSend: function (request) {
            request.setRequestHeader("RequestVerificationToken", $("[name='__RequestVerificationToken']").val());
        },
        success: function (response) { //Данные отправлены 
            var resStr = JSON.stringify(response);
            console.log(resStr);
            if (resStr.localeCompare("Good")) {
                    location.reload();
            }
        },
        error: function (response) { // Данные не отправлены
            Materialize.toast('Error!', 4000) // 4000 is the duration of the toast
        }
    });
}