// Må sende via URL "Trade/Sell"    Trade er controller?? og sell er function i controller?

$(function () {
    hentAlleAksjer();
});

function hentAlleAksjer() {
    $.get("Trading/GetPortfolio", function (stocks) {
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
            "<td> <a class='btn btn-primary' onclick='sellStock' (" + stock.id + ")'>sell</a></td>" +
            "<td> <button class='btn btn-danger' onclick='sellStock(" + stock.id + ")'>buy</button></td>" +
            "</tr>";
    }
    ut += "</table>";
    $("#skrivportfolio").html(ut);
}

function buyStock() {
    const innkjop = {
        aksje: $("#stockBuy").val(),
    }

    const url = "Trade/Buy";
    $.get(url, innkjop, function (OK) {
        if (OK) {
            window.location.href = 'index.html';
        } else {
            $("#feil").html("Feil i db - prøv igjen senere");
        }
    });
}

function sellStock() {
    const utsalg = {
        aksje: $("#stockSell").val(),
    }
    const url = "Trade/Sell";
    $.get(url, utsalg, function (OK) {
        if (OK) {
            window.location.href = 'index.html';
        } else {
            $("#feilSell").html("Feil i db - prøv igjen senere");
        }
    });
}


