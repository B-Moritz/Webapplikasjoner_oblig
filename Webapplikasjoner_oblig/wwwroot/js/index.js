// Må sende via URL "Trade/Sell"    Trade er controller?? og sell er function i controller?

function buyStock() {
    const innkjop = {
        aksje: $("#stockBuy").val(),
    }
    const url = "Trading/BuyStock";
    $.post(url, innkjop, function (OK) {
        if (OK) {
            window.location.href = 'index.html';
        }
        else {
            $("#feil").html("Feil i db - prøv igjen senere");
        }


function sellStock() {
    const utsalg = {
        aksje: $("#stockSell").val(),
    }
    const url = "Trading/SellStock";
    $.post(url, utsalg, function (OK) {
        if (OK) {
            window.location.href = 'index.html';
        }
        else {
            $("#feil").html("Feil i db - prøv igjen senere");
        }
    });
};

function hentAlleAksjer() {
    $.get("portfolio/hentAlleAksjer", function (stocks) {
        formaterPortfolio(stocks);
    });
}

function formaterPortfolio(stocks) {
    let ut = "<table class='table table-striped'>" +
        "<tr>" +
        "<th>Name</th><th>Antall</th><th></th><th></th>" +
        "</tr>";
    for (let stock of stocks) {
        ut += "<tr>" +
            "<td>" + stock.name + "</td>" +
            "<td>" + stock.antall + "</td>" +
            "</tr>";
    }
    ut += "</table>";
    $("#srivportfolio").html(ut);
}
