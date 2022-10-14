$(function () {
    printAllMyPortfolio();
});

function printAllMyPortfolio() {
    $.get("trading/getPortfolio", function (minStocks){
        formatPortfolio(minStocks);
    });
}

function formatPortfolio(minStocks) {

    let ut = "<table class='table table-striped'>" +
        "<tr>" +
        "<th>Stock_Name</th><th>Discriptions</th><th>value</th><th>Values</th><th></th>" +
        "</tr>";

    for (let trading of minStocks) {
        ut += "<tr>" +
            "<td>" + trading.LastUpdate + "</td>" +
            "<td>" + trading.TotalValueSpent + "</td>" +
            "<td>" + trading.TotalPortfolioValue + "</td>" +
            "<td>" + trading.Stocks + "</td>" +
            "<td> <button class='btn btn-danger' onclick='sellStock(" + trading.id + ")'>sell</button></td>" +

            "</tr>";
    }

    ut += "</table>";
    $("#portfolios").html(ut);
}

/*function slettKunde(id) {
    const url = "trading/SellStock?id=" + id;
    $.get(url, function (OK) {
        if (OK) {
            window.location.href = 'portfolio.html';
        }
        else {
            $("#feil").html("Feil i db - prøv igjen senere");
        }

    });
};*/

