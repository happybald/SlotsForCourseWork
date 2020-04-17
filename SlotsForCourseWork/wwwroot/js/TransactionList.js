function getTrList() {
    $.ajax({
        type: "GET",
        url: "/api/TransactionAPI",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            console.log(data);
            console.log(JSON.stringify(data));
            $('#transactList').empty();
            $.each(data, function (i, item) {
                var rows = "<tr>" +
                    "<td id='Date'>" + item.dateTime + "</td>" +
                    "<td id='UserName'>" + item.userName + "</td>" +
                    "<td id='Bet'>" + item.bet + "</td>" +
                    "<td id='Result'>" + item.win + "</td>" +
                    "</tr>";
                $('#TableTL').append(rows);
            }); //End of foreach Loop   
        }, //End of AJAX Success function  

        failure: function (data) {
            alert(data.responseText);
        }, //End of AJAX failure function  
        error: function (data) {
            alert(data.responseText);
        } //End of AJAX error function  

    });
};
var interval_id;
$('#lTransactButton ').click(function () {
    getTrList();
});
$(document).ready(getTrList());
