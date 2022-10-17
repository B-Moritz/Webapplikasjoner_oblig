﻿$(function () {
    printAllMyPortfolio();

    $("#get").click(function () {
        printAllMyPortfolio();
    });

});


function printAllMyPortfolio() {

        url = "trading/getPortfolio?userId=1";


        $.get(url, function (data) {
            if (data != null) {
                const regex = /([0-9]{4})-([0-9]{2})-([0-9]{2})T([0-9]{2}):([0-9]{2}):([0-9])/;
                let machResult = regex.exec(data.lastUpdate);
                const updateString = `${machResult[1]}.${machResult[2]}.${machResult[3]}   ${machResult[4]}:${machResult[5]}:${machResult[6]}`
                $("#lastupdate").html(updateString);
                $("#totalvaluespent").html(data.totalValueSpent);
                $("#totalportfoliovalue").html(data.totalPortfolioValue);
                $("#portfoliocurrency").html(data.portfolioCurrency);

                portfolioListHtml = ` <table class='table table-striped'>
                <tr>
                    <th>Stock symbol</th >
                    <th>Number of shares</th>
                    <th>Stock Name</th>
                    <th>Description</th>
                    <th>Currency</th>
                    <th>Funds spent on stock</th>
                    <th>Total value</th>
                </tr>`;

                for (let stock of data.stocks) {
                    portfolioListHtml += `<tr class="">
                    <td>${stock.symbol}</td >
                    <td>${stock.stockCounter}</td>
                    <td>${stock.stockName}</td>
                    <td>${stock.description}</td>
                    <td>${stock.stockCurrency}</td>
                    <td>${stock.totalFundsSpent}</td>
                    <td>${stock.totalValue}</td>
                </tr >`
                }
                portfolioListHtml += "</table>";
                $("#PortfolioStockList").html(portfolioListHtml);
            }
            else {
                alert("something went wrong!");
            }
        }).fail(function (response) {
            alert(response.responseText);e

        });
};

function formatPortfolio(portfolio) {
    return formatPortfolio(portfolio);
}


    /*let ut = "<table><tr></tr>";
    for (const minStock of portfolio) {
        ut += "<tr><td>" + minStock.LastUpdate + "</td><td>" + minStock.TotalValueSpent +
            "</td><td>" + minStock.TotalPortfolioValue + "</td><td>" + minStock.Stocks + "</td></tr>";
    }
    ut += "</table>";
    $("#portfolios").html(ut);*/



/*$(document).ready(function () {
    $("button").click(function () {
        $.get("Trading/getPortfolio", function (data) {
            formatPortfolio(data);
        })
    });
});*/

/*function formatPortfolio(minStocks) {
    let ut = "<table class='table table-striped'>" +
        "<tr>" +
        "<th>Stock_Name</th><th>Discriptions</th><th>value</th><th>Values</th><th></th>" +
        "</tr>";

    for (let stock of minStocks) {
        ut += "<tr>" +
            "<td>" + stock.LastUpdate + "</td>" +
            "<td>" + stock.TotalValueSpent + "</td>" +
            "<td>" + stock.TotalPortfolioValue + "</td>" +
            "<td>" + stock.Stocks + "</td>" +
            "<td> <button class='btn btn-danger' onclick='sellStock(" + stock.id + ")'>sell</button></td>" +

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

