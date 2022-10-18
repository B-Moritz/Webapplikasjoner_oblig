
let selectedStock = null;
const userId = 1;

$(function () {
    printAllMyPortfolio();

    $("#GetPortfolioBtn").click(function () {
        printAllMyPortfolio();
    });

    hentFavorite();

    $("#update").click(function () {
        hentFavorite();
    });

    $("#BuyPortfolioBtn").click(function () {
        if (selectedStock == null) {
            alert("No stock was selected. Please select a stock");
            return;
        }

        const innerHtml = `
            <h3>Buy stock</h3>
            <label style="grid-area: label1;" name="Stock Symbol">Stock Symbol:</label>
            <p style="grid-area: static1">${selectedStock.symbol}</p>
            <label styles="grid-area: label2;" name="Stock Name">Stock Name:</label>
            <p style="grid-area: static2">${selectedStock.stockName}</p>
            <label style="grid-area: label3;" name="Stock Symbol">Number of shares in portfolio:</label>
            <p style="grid-area: static3">${selectedStock.stockCounter}</p>
            <label style="grid-area: label4;" name="Stock Name">Currency:</label>
            <p style="grid-area: static4">${selectedStock.stockCurrency}</p>
            <div style="grid-area: inputGroup1;" class="input-group">
                <label class="input-group-addon">Number of stocks</label>
                <input id="StockCounterInput" class="form-control" type="number" name="StockCounter" value="1">
            </div>
            <button id="ConfirmBuy" class="btn-success">Confirm Transaction</button>
            <button id="CancelDialog" class="btn-secondary">Cancel Transaction</button>`;

        createDialog(innerHtml);

        $("#ConfirmBuy").click(() => {
            const count = parseInt($("#StockCounterInput").val());

            $("#PortfolioLoading").removeClass("hideLoading").addClass("displayLoading");
            $.post(`trading/buyStock?userId=${userId}&symbol=${selectedStock.symbol}&count=${count}`, (data) => {
                updatePortfolioList(data);
                $("#PortfolioLoading").removeClass("displayLoading").addClass("hideLoading");
                reenablePortfolioWidget(false);
            }).fail((response) => {
                alert(response.responseText);
                $("#PortfolioLoading").removeClass("displayLoading").addClass("hideLoading");
                reenablePortfolioWidget(false);
            });
            $("#DialogContainer").removeClass("showDialog");
            $("#DialogContainer").addClass("hideDialog");
        });
    });

    $("#SellPortfolioBtn").click(function () {
        if (selectedStock == null) {
            alert("No stock was selected. Please select a stock");
            return;
        }

        const innerHtml = `
            <h3>Sell stock</h3>
            <label style="grid-area: label1;" name="Stock Symbol">Stock Symbol:</label>
            <p style="grid-area: static1">${selectedStock.symbol}</p>
            <label styles="grid-area: label2;" name="Stock Name">Stock Name:</label>
            <p style="grid-area: static2">${selectedStock.stockName}</p>
            <label style="grid-area: label3;" name="Stock Symbol">Number of shares in portfolio:</label>
            <p style="grid-area: static3">${selectedStock.stockCounter}</p>
            <label style="grid-area: label4;" name="Stock Name">Currency:</label>
            <p style="grid-area: static4">${selectedStock.stockCurrency}</p>
            <div style="grid-area: inputGroup1;" class="input-group">
                <label class="input-group-addon">Number of stocks</label>
                <input id="StockCounterInput" class="form-control" type="number" name="StockCounter" value="1">
            </div>
            <button id="ConfirmSell" class="btn-success">Confirm Transaction</button>
            <button id="CancelDialog" class="btn-secondary">Cancel Transaction</button>`;

        createDialog(innerHtml);

        $("#ConfirmSell").click(function () {
            const count = parseInt($("#StockCounterInput").val());

            if (selectedStock.count - count < 0) {
                alert("You have not enough stocks of this type to perform this operation.");
            }

            $("#PortfolioLoading").removeClass("hideLoading").addClass("displayLoading");
            $.post(`trading/sellStock?userId=${userId}&symbol=${selectedStock.symbol}&count=${count}`, (data) => {
                updatePortfolioList(data);
                $("#PortfolioLoading").removeClass("displayLoading").addClass("hideLoading");
                reenablePortfolioWidget(false);
            }).fail((response) => {
                alert(response.responseText);
                $("#PortfolioLoading").removeClass("displayLoading").addClass("hideLoading");
                reenablePortfolioWidget(false);
            });
            $("#DialogContainer").removeClass("showDialog");
            $("#DialogContainer").addClass("hideDialog");
            
        });

    });


});

function updatePortfolioList(data) {
    if (data != null) {
        const regex = /([0-9]{4})-([0-9]{2})-([0-9]{2})T([0-9]{2}):([0-9]{2}):([0-9])/;
        let matchResult = regex.exec(data.lastUpdate);
        const updateString = `${matchResult[1]}.${matchResult[2]}.${matchResult[3]}   ${matchResult[4]}:${matchResult[5]}:${matchResult[6]}`
        $("#lastupdate").html(updateString);
        $("#totalvaluespent").html(data.totalValueSpent);
        $("#totalportfoliovalue").html(data.totalPortfolioValue);
        $("#portfoliocurrency").html(data.portfolioCurrency);

        const portfolioTableElement = $("#PortfolioStockList");

        portfolioTableElement.empty();

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
                    stockName: stock.stockName,
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
                $(this).toggleClass("highlightRow");
                selectedStock = $(this).data("StockData");
                //console.log(`Stock ${selectedStock} is selected! From null`);
            } else if (selectedStock.symbol == $(this).data("StockData").symbol) {
                $(this).toggleClass("highlightRow");
                selectedStock = null;
                //console.log("No stock is selected.");
            } else {
                $(".PortfolioRow").removeClass("highlightRow");
                $(this).addClass("highlightRow");
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
    disablePortfolioWidget();

    $(".PortfolioRow").off("click");

    $.get(url, function (data) {
        updatePortfolioList(data);
        $("#PortfolioLoading").removeClass("displayLoading").addClass("hideLoading");
        reenablePortfolioWidget(false);
    }).fail(function (response) {
        alert(response.responseText);
        $("#PortfolioLoading").removeClass("displayLoading").addClass("hideLoading");
        reenablePortfolioWidget(false);
    });
};

function reenablePortfolioWidget(isFromCancelBtn) {
    $("#GetPortfolioBtn").removeClass("disabled").removeAttr("aria-disabled");
    $("#SellPortfolioBtn").removeClass("disabled").removeAttr("aria-disabled");
    $("#BuyPortfolioBtn").removeClass("disabled").removeAttr("aria-disabled");
    if (selectedStock != null && isFromCancelBtn == false) {
        $(`#${selectedStock.symbol}`).toggleClass("highlightRow");
        selectedStock = $(`#${selectedStock.symbol}`).data("StockData");
    }
    
}

function disablePortfolioWidget() {
    $("#GetPortfolioBtn").addClass("disabled").attr("aria-disabled", "disabled");
    $("#SellPortfolioBtn").addClass("disabled").attr("aria-disabled", "disabled");
    $("#BuyPortfolioBtn").addClass("disabled").attr("aria-disabled", "disabled")
}

function createDialog(innerHtml) {
    disablePortfolioWidget();
    $("#DialogContainer").removeClass("hideDialog");
    $("#DialogContainer").addClass("showDialog");

    $("#InnerDialog").empty();
    $("#InnerDialog").append(innerHtml);


    $("#CancelDialog").click(function () {
        $("#DialogContainer").removeClass("showDialog");
        $("#DialogContainer").addClass("hideDialog");
        reenablePortfolioWidget(true);
    });
}

function formatPortfolio(portfolio) {
    return formatPortfolio(portfolio);
}

function formatFavorite(favorites) {
    if (favorites != null) {
        const regex = /([0-9]{4})-([0-9]{2})-([0-9]{2})T([0-9]{2}):([0-9]{2}):([0-9]{2})/;
        let matchResult = regex.exec(favorites.lastUpdated);

        const updateFavorite = `${matchResult[1]}.${matchResult[2]}.${matchResult[3]}   ${matchResult[4]}:${matchResult[5]}:${matchResult[6]}`
        $("#lastupdateFav").html(updateFavorite);


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

