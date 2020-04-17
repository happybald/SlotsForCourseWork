$(document).ready(function () {
    $("#control").click(
        function () {
            $('#control').addClass("disabled");
            $('#bet').attr("disabled", "disabled");
            $('li').addClass("disabled");
            $("#loading").show();
            timeForRun = 200;
            timeForFinal = timeForRun;
            counter = false;
            run();
            sendAjaxSpin('/Spin/Start')
        });
});

slotitem = new Array('0', '1', '2', '3');

var bet = $('input[name="bet"]');

function randomSlot(slot) {
    turn = 4 + Math.floor((Math.random() * 4));
    for (a = 0; a < turn; a++) {
        $(slot).attr("src", "../images/" + slotitem[a % 4] + ".png");
    }
};

function finalSlot(slot, n) {
    $(slot).attr("src", "../images/" + slotitem[n] + ".png");
    console.log("../images/" + slotitem[n] + ".png");
};


function run() {
    randomSlot(".slot1");
    randomSlot(".slot2");
    randomSlot(".slot3");
    randomSlot(".slot4");
    if (counter!=true) {
        setTimeout("run();", 200);
    }
}

function finalrun(a, b, c, d) {
    finalSlot(".slot1",a);
    finalSlot(".slot2",b);
    finalSlot(".slot3",c);
    finalSlot(".slot4",d);
}


function sendAjaxSpin(url) {
    var Result = {
        Credits:""+$('#chip').text()+"",
        Bet: "" + $('#bet').val() + "",
        BestScore: "" + $('#highScore').text() + ""
    }
    $.ajax({
        url: url, //url страницы 
        type: "POST", //метод отправки
        beforeSend: function (request) {
            request.setRequestHeader("RequestVerificationToken", $("[name='__RequestVerificationToken']").val());
        },
        data: Result,
        success: function (response) {
            setTimeout(function () {
                counter = true;
                console.log(JSON.stringify(response));
                setTimeout(finalrun,210,response.a, response.b, response.c, response.d);
                setTimeout(function () { $("#loading").hide(); }, 230);
                $('#chip').html(response.newCredits);
                if (response.win) {
                    $('#highScore').html(response.newBestScore);
                    Materialize.toast("Nice, you win " + response.winValue + " credits", 4000, 'green');
                } else {
                    Materialize.toast("Unfortunately you lost " + $('#bet').val() + " credits", 4000,'red');
                }
                $('#control').removeClass("disabled");
                $('#bet').removeAttr("disabled");
                $('li').removeClass("disabled");
            }, 6000);
        },
        error: function (response) { // Данные не отправлены
            console.log(JSON.stringify(response));
            Materialize.toast('Error!', 4000) // 4000 is the duration of the toast
        }
    });
};