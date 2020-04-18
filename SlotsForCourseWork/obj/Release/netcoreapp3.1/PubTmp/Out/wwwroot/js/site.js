$(document).ready(function () {
    $('.modal').modal();
    $('ul.tabs').tabs();
    $(".button-collapse").sideNav();
    $("#loading").hide();
    load();
    console.log("loaded");
});

var bet = $('input[name="bet"]');

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
    $("#all").html(calcCredit(bet.val(), 5));
    $("#doublePair").html(calcCredit(bet.val(), 3));
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

