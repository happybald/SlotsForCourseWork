$(document).ready(function () {
    $('.modal').modal();
    $('ul.tabs').tabs();
    $(".button-collapse").sideNav();
    $("#loading").hide();
    load();
    console.log("loaded");
});

var bet = $('input[name="bet"]');

function setVolume() {
    var media0 = document.getElementById("spin");
    var media1 = document.getElementById("win");
    var media2 = document.getElementById("lose");
    media0.volume = document.getElementById("vol").value;
    media1.volume = document.getElementById("vol").value;
    media2.volume = document.getElementById("vol").value;
    if (media0.volume < 1) {
        $("#change").html("volume_down");
    }
    if (media0.volume == 0) {
        $("#change").html("volume_off");
    }
    if (media0.volume == 1) {
        $("#change").html("volume_up");
    }
}

bet.keyup(function () {
    const regex = /\d/g;
    if (regex.exec(bet.val()) != null) {
        load();
        $('#control').addClass("disabled");
        if (parseInt($("#chip").text()) >= parseInt(bet.val()) && parseInt(bet.val())>0) {
            $('#control').removeClass("disabled");
        }
    }
});

function load() {
    $("#all").html(calcCredit(bet.val(), 4));
    $("#doublePair").html(calcCredit(bet.val(), 2));
    $("#onePair").html(calcCredit(bet.val(), 1));
}

function calcCredit(mise, baseCredit) {
    if (mise == 0) {
        return 0;
    }
    if (JSON.stringify(mise).length > 7) {
        return "Error!";
    }
    return Math.pow((baseCredit), 2) * (mise - 1) + baseCredit;
}

