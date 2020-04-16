    function getlist() {
        $.ajax({
            type: "GET",
            url: "/api/CredtiAPI",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                $('#userslist').empty();
                $.each(data, function (i, item) {
                    var rows = "<tr>" +
                        "<td id='n'>" + ++i + "</td>" +
                        "<td id='UserName'>" + item.userName + "</td>" +
                        "<td id='Credits'>" + item.credits + "</td>" +
                        "</tr>";
                    $('#TableBS').append(rows);
                }); //End of foreach Loop   
                console.log(data);
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
$('#bScoresButton').click(function () {
    getlist();
});
$(document).ready(getlist());
