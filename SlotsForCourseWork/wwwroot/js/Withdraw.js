$(document).ready(function () {
    $("#WithdrawButton").click(
        function () {
            sendAjaxForm('/Account/Withdraw');
            return false;
        }
    );
});

function sendAjaxForm(url) {
    ResValue = $('#Withdraw').val();
    console.log(ResValue);
    var myData = { Value: ResValue }; // #1
    $.ajax({
        url: url, //url страницы
        type: "POST", //метод отправки
        data: myData,
        complete: function (response) { //Данные отправлены успешно
            console.log(response);
            $('#result_form_Withdraw').html('Result string:' + response.responseJSON.message);
            $('#creditsc').html(response.responseJSON.newCredits);
        }
    });
}