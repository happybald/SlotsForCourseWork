$(document).ready(function () {
    $('.modal').modal();
    $('ul.tabs').tabs();
    $(".button-collapse").sideNav();
    load();
    console.log("loaded");
});

var bet = $('input[name="bet"]');

bet.keyup(function () {
    const regex = /\d/g;
    if (regex.exec(bet.val()) != null) {
        load();
        if (parseInt($("#chip").html()) >= parseInt(bet.val())) {
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
    console.log(JSON.stringify(mise).length);
    if (JSON.stringify(mise).length > 7) {
        return "Error!";
    }
    return Math.pow((baseCredit), 2) * (mise - 1) + baseCredit;
}