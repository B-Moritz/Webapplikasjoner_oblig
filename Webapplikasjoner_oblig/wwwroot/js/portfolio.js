$(function () {
    printAllMyPortfolio();
});
function printAllMyPortfolio() {
    $.get("Trading/getPortfolio", function (my_stocks) {
        formatPortfolio(my_stocks);
    });
}

function formatPortfolio(my_stócks) {
    let ut = "<table class='table table-striped'>" +
        "<tr>" +
        "<th>Stock_Name</th><th>Discriptions</th><th>value</th><th>Values</th><th></th>" +
        "</tr>";

    for (let trading of my_stócks) {
        ut += "<tr>" +
            "<td>" + trading.LastUpdate + "</td>" +
            "<td>" + trading.TotalValueSpent + "</td>" +
            "<td>" + trading.TotalPortfolioValue + "</td>" +
            "<td>" + trading.Stocks + "</td>" +

            /*
                        "<td> <a class='btn btn-primary' href='endre.html?id="+stock.id+"'>sell</a></td>"+
            */
            "<td> <button class='btn btn-danger' onclick='sellStock(" + trading.id + ")'>sell</button></td>" +

            "</tr>";
    }
    ut += "</table>";
    $("#portfolios").html(ut);
}

function slettKunde(id) {
    const url = "Trading/Slett?id=" + id;
    $.get(url, function (OK) {
        if (OK) {
            window.location.href = 'portfolio.html';
        }
        else {
            $("#feil").html("Feil i db - prøv igjen senere");
        }

    });
};

