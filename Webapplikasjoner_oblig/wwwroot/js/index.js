
let selectedStock = null;

$(function () {
    printAllMyPortfolio();

    $("#get").click(function () {
        printAllMyPortfolio();
    });

    hentFavorite();

    $("#update").click(function () {
        hentFavorite();
    });

    $("#sell").click(function () {
        if (selectedStock == null) {
            alert("No stock was selected. Please select a stock");
            return;
        }
        $("#SellDialog").removeClass("hideDialog");
        $("#SellDialog").addClass("showDialog");
        const receiptHtml = `
            <label class="control-label">Stock Symbol</label>
            <div>
                <p class="form-control-static"></p>
            </div>`
    });

    $("#confirmSell").click(function () {
        const count = $("#StockCouterInput").val();
        $.post(`trading/sellStock?userId=1&symbol=MSFT&count=${count}`, (data) => {
            updatePortfolioList(data);
        });
        $("#SellDialog").removeClass("showDialog");
        $("#SellDialog").addClass("hideDialog");
    });

    $("#CancelSellDialog").click(function () {
        $("#SellDialog").removeClass("showDialog");
        $("#SellDialog").addClass("hideDialog");
    });
});

function updatePortfolioList(data) {
    if (data != null) {
        const regex = /([0-9]{4})-([0-9]{2})-([0-9]{2})T([0-9]{2}):([0-9]{2}):([0-9])/;
        let machResult = regex.exec(data.lastUpdate);
        const updateString = `${machResult[1]}.${machResult[2]}.${machResult[3]}   ${machResult[4]}:${machResult[5]}:${machResult[6]}`
        $("#lastupdate").html(updateString);
        $("#totalvaluespent").html(data.totalValueSpent);
        $("#totalportfoliovalue").html(data.totalPortfolioValue);
        $("#portfoliocurrency").html(data.portfolioCurrency);

        const portfolioTableElement = $("#PortfolioStockList");

        const portfolioListHeader = `
                <tr>
                    <th>Stock symbol</th >
                    <th>Number of shares</th>
                    <th>Stock Name</th>
                    <th>Description</th>
                    <th>Currency</th>
                    <th>Funds spent on stock</th>
                    <th>Total value</th>
                </tr>`;
        portfolioTableElement.append(portfolioListHeader);
        let curStockObj = {};

        for (let stock of data.stocks) {
            let portfolioListRow = `<tr id="${stock.symbol}" class="PortfolioRow">
                    <td>${stock.symbol}</td>
                    <td>${stock.stockCounter}</td>
                    <td>${stock.stockName}</td>
                    <td>${stock.description}</td>
                    <td>${stock.stockCurrency}</td>
                    <td>${stock.totalFundsSpent}</td>
                    <td>${stock.totalValue}</td>
                </tr>`
            portfolioTableElement.append(portfolioListRow)
            curStockObj = {
                StockData: {
                    symbol: stock.symbol,
                    stockCounter: stock.stockCounter,
                    description: stock.description,
                    stockCurrency: stock.stockCurrency,
                    totalFundsSpent: stock.totalFundsSpent,
                    totalValue: stock.totalValue
                }
            };
            $(`#${stock.symbol}`).data(curStockObj);
        }

        $(".PortfolioRow").click(function () {
            if (selectedStock == null) {
                $(this).toggleClass("bg-info");
                selectedStock = $(this).data("StockData");
                //console.log(`Stock ${selectedStock} is selected! From null`);
            } else if (selectedStock.symbol == $(this).data("StockData").symbol) {
                $(this).toggleClass("bg-info");
                selectedStock = null;
                //console.log("No stock is selected.");
            } else {
                $(".PortfolioRow").removeClass("bg-info");
                $(this).addClass("bg-info");
                selectedStock = $(this).data("StockData");
                //console.log(`Stock ${selectedStock} is selected! different stock is selected`);
            }

        });
    }
    else {
        alert("something went wrong!");
    }
}

function printAllMyPortfolio() {

    url = "trading/getPortfolio?userId=1";
    $("#PortfolioLoading").removeClass("hideLoading").addClass("displayLoading");
    $("#get").addClass("disabled").attr("aria-disabled", "disabled");   
    $("#sell").addClass("disabled").attr("aria-disabled", "disabled");

    $(".PortfolioRow").off("click");

    $.get(url, function (data) {
        updatePortfolioList(data);
        $("#PortfolioLoading").removeClass("displayLoading").addClass("hideLoading");
        $("#get").removeClass("disabled");
        $("#sell").removeClass("disabled");
    }).fail(function (response) {
        alert(response.responseText);
        $("#PortfolioLoading").removeClass("displayLoading").addClass("hideLoading");
        $("#get").removeClass("disabled");
        $("#sell").removeClass("disabled");
    });
};

function formatPortfolio(portfolio) {
    return formatPortfolio(portfolio);
}

function formatFavorite(favorites) {
    if (favorites != null) {
        const regex = /([0-9]{4})-([0-9]{2})-([0-9]{2})T([0-9]{2}):([0-9]{2}):([0-9])/;
        let machResult = regex.exec(favorites.lastUpdated);

        const updateFavorite = `${machResult[1]}.${machResult[2]}.${machResult[3]}   ${machResult[4]}:${machResult[5]}:${machResult[6]}`
        $("#lastupdate").html(updateFavorite);


        favoritListHtml = ` <table class='table table-striped'>
                <tr>
                    <th>Stock Name</th>
                    <th>Stock Symbol</th>
                    <th>Description</th>
                    <th>Last updated</th>
                </tr>`;

        for (let enfavorite of favorites.stockList) {
            favoritListHtml += `<tr>
                    <td>${enfavorite.stockName}</td >
                    <td>${enfavorite.stockSymbol}</td>
                    <td>${enfavorite.description}</td>
                    <td>${enfavorite.lastUpdated}</td>
                    </tr >`
        }
        favoritListHtml += "</table>";
        $("#skrivfavorite").html(favoritListHtml);
    }
    else {
        alert("something went wrong!");
    }

}

function hentFavorite() {
    url = "trading/getFavoriteList?userId=1";
    $.get(url, function (favorites) {
        formatFavorite(favorites);
    }).fail(function (response) {
        alert(response.responseText);
    });
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

