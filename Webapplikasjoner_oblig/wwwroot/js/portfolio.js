﻿
$(function () {
    printAllMyPortfolio();
});

function printAllMyPortfolio() {
    $('#button#portfolios').click(function () {
        url: "trading/getPortfolio";
        $.get(url, function (data) {
            if (data != null) {
                $("#lastupdate").val(data.url);
                $("#totalvaluespent").val(data.url);
                $("#totalportfoliovalue").val(data.url);
                $("#stockks").val(data.url);
            }
            else {
                alert("something went wrong!");
            },
           
        });
        failure: function(response) {
            alert(response.responseText);
        },
        Error: function(response) {
            alert(response.responseText)
        }

    });
   
};


function formatPortfolio(portfolio) {
    return formatPortfolio(portfolio);

    /*let ut = "<table><tr></tr>";
    for (const minStock of portfolio) {
        ut += "<tr><td>" + minStock.LastUpdate + "</td><td>" + minStock.TotalValueSpent +
            "</td><td>" + minStock.TotalPortfolioValue + "</td><td>" + minStock.Stocks + "</td></tr>";
    }
    ut += "</table>";
    $("#portfolios").html(ut);*/
}


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







