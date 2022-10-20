
let selectedPortfolioStock = null;
let selectedFavoriteStock = null;
const userId = 1;
//selectedFavoriteStock.symbol

$(function () {
    printAllMyPortfolio();

    $("#GetPortfolioBtn").click(function () {
        printAllMyPortfolio();
    });

    getFavorite();

    $("#GetFavoriteBtn").click(function () {
        getFavorite();
    });

    $("#DeleteFavoriteBtn").click(function () {
        deleteFromFavorite();
    });

    $("#BuyPortfolioBtn").click(function () {
        if (selectedPortfolioStock == null) {
            alert("No stock was selected. Please select a stock");
            return;
        }

        const innerHtml = `
            <h3>Buy stock</h3>
            <label style="grid-area: label1;" name="Stock Symbol">Stock Symbol:</label>
            <p style="grid-area: static1">${selectedPortfolioStock.symbol}</p>
            <label styles="grid-area: label2;" name="Stock Name">Stock Name:</label>
            <p style="grid-area: static2">${selectedPortfolioStock.stockName}</p>
            <label style="grid-area: label3;" name="Quantity">Number of shares in portfolio:</label>
            <p style="grid-area: static3">${selectedPortfolioStock.quantity}</p>
            <label style="grid-area: label4;" name="EstPrice">Estimated price per share:</label>
            <p style="grid-area: static4">${selectedPortfolioStock.estPrice}</p>
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
            $.post(`trading/buyStock?userId=${userId}&symbol=${selectedPortfolioStock.symbol}&count=${count}`, (data) => {
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
        if (selectedPortfolioStock == null) {
            alert("No stock was selected. Please select a stock");
            return;
        }

        const innerHtml = `
            <h3>Sell stock</h3>
            <label style="grid-area: label1;" name="Stock Symbol">Stock Symbol:</label>
            <p style="grid-area: static1">${selectedPortfolioStock.symbol}</p>
            <label styles="grid-area: label2;" name="Stock Name">Stock Name:</label>
            <p style="grid-area: static2">${selectedPortfolioStock.stockName}</p>
            <label style="grid-area: label3;" name="Quantity">Number of shares in portfolio:</label>
            <p style="grid-area: static3">${selectedPortfolioStock.quantity}</p>
            <label style="grid-area: label4;" name="EstPrice">Estimated price per share:</label>
            <p style="grid-area: static4">${selectedPortfolioStock.estPrice}</p>
            <div style="grid-area: inputGroup1;" class="input-group">
                <label class="input-group-addon">Number of stocks</label>
                <input id="StockCounterInput" class="form-control" type="number" name="StockCounter" value="1">
            </div>
            <button id="ConfirmSell" class="btn-success">Confirm Transaction</button>
            <button id="CancelDialog" class="btn-secondary">Cancel Transaction</button>`;

        createDialog(innerHtml);

        $("#ConfirmSell").click(function () {
            const count = parseInt($("#StockCounterInput").val());

            if (selectedPortfolioStock.count - count < 0) {
                alert("You have not enough stocks of this type to perform this operation.");
            }

            $("#PortfolioLoading").removeClass("hideLoading").addClass("displayLoading");
            $.post(`trading/sellStock?userId=${userId}&symbol=${selectedPortfolioStock.symbol}&count=${count}`, (data) => {
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

function dateTimeFormat(rawFormat) {
    // This method convert the date time string to a more concise format
    const regex = /([0-9]{4})-([0-9]{2})-([0-9]{2})T([0-9]{2}):([0-9]{2}):([0-9])/;
    let matchResult = regex.exec(rawFormat);
    const tradingTimeFormat = `${matchResult[1]}.${matchResult[2]}.${matchResult[3]}   ${matchResult[4]}:${matchResult[5]}:${matchResult[6]}`
    return tradingTimeFormat;
}

function updatePortfolioList(data) {
    // This function updates the portfolio list
    // Argument: data - the response object from the getPortfolio endpoint on server 
    if (data != null) {
        // If the response data contains contnt, proceed with updating
        
        $("#LastUpdate").html(dateTimeFormat(data.lastUpdate));
        $("#TotalValueSpent").html(data.totalValueSpent);
        $("#TotalPortfolioValue").html(data.estPortfolioValue);
        $("#PortfolioCurrency").html(data.portfolioCurrency);
        $("#BuyingPower").html(data.buyingPower);
        $("#UnrealizedPL").html(data.unrealizedPL);

        const portfolioTableElement = $("#PortfolioStockList");

        portfolioTableElement.empty();

        const portfolioListHeader = `
                <tr>
                    <th>Stock symbol</th >
                    <th>Stock Name</th>
                    <th>Quantity</th>
                    <th>Estimated price</th>
                    <th>Portfolio partition %</th>
                    <th>Estimated total value</th>
                    <th>Total cost</th>
                    <th>Unrealized profit/loss</th>
                </tr>`;
        portfolioTableElement.append(portfolioListHeader);
        let curStockObj = {};

        for (let stock of data.stocks) {
            let portfolioListRow = `<tr id="${stock.symbol}_portfolio" class="PortfolioRow">
                    <td>${stock.symbol}</td>
                    <td>${stock.stockName}</td>
                    <td>${stock.quantity}</td>
                    <td>${stock.estPrice}</td>
                    <td>${stock.portfolioPortion}</td>
                    <td>${stock.estTotalMarketValue}</td>
                    <td>${stock.totalCost}</td>
                    <td>${stock.unrealizedPL}</td>
                </tr>`
            portfolioTableElement.append(portfolioListRow)
            curStockObj = {
                StockData: {
                    symbol: stock.symbol,
                    stockName: stock.stockName,
                    quantity: stock.quantity,
                    description: stock.description,
                    totalCost: stock.totalCost,
                    estTotalMarketValue: stock.estTotalMarketValue,
                    estPrice: stock.estPrice,
                    portfolioPortion: stock.portfolioPortion,
                    unrealizedPL: stock.unrealizedPL,
                }
            };
            $(`#${stock.symbol}_portfolio`).data(curStockObj);
        }

        $(".PortfolioRow").click(function () {
            if (selectedPortfolioStock == null) {
                $(this).toggleClass("highlightRow");
                selectedPortfolioStock = $(this).data("StockData");
                //console.log(`Stock ${selectedStock} is selected! From null`);
            } else if (selectedPortfolioStock.symbol == $(this).data("StockData").symbol) {
                $(this).toggleClass("highlightRow");
                selectedPortfolioStock = null;
                //console.log("No stock is selected.");
            } else {
                $(".PortfolioRow").removeClass("highlightRow");
                $(this).addClass("highlightRow");
                selectedPortfolioStock = $(this).data("StockData");
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
    if (selectedPortfolioStock != null && isFromCancelBtn == false) {
        $(`#${selectedPortfolioStock.symbol}_portfolio`).toggleClass("highlightRow");
        selectedPortfolioStock = $(`#${selectedPortfolioStock.symbol}_portfolio`).data("StockData");
    }
    
}

function disablePortfolioWidget() {
    $("#GetPortfolioBtn").addClass("disabled").attr("aria-disabled", "disabled");
    $("#SellPortfolioBtn").addClass("disabled").attr("aria-disabled", "disabled");
    $("#BuyPortfolioBtn").addClass("disabled").attr("aria-disabled", "disabled")
}

function disableFavoriteWidget() {
    $("#GetFavoriteBtn").addClass("disabled").attr("aria-disabled", "disabled");
    $("#BuyFavoriteBtn").addClass("disabled").attr("aria-disabled", "disabled");

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
        
        $("#lastupdateFav").html(dateTimeFormat(favorites.lastUpdated));

        const favoriteTableContainer = $("#skrivfavorite");

        favoriteTableContainer.empty();
        favoriteTableContainer.append(`
                <tr>
                    <th>Stock Name</th>
                    <th>Stock Symbol</th>
                    <th>Description</th>
                    <th>Last updated</th>
                </tr>`);

        let curFavObj = {};

        for (let enfavorite of favorites.stockList) {
            favoriteTableContainer.append(`<tr id="${enfavorite.stockSymbol}_favorites" class="favoritesRow">
                    <td>${enfavorite.stockName}</td >
                    <td>${enfavorite.stockSymbol}</td>
                    <td>${enfavorite.description}</td>
                    <td>${dateTimeFormat(enfavorite.lastUpdated)}</td>
                    </tr >`);

            curFavObj = {
                StockData: {
                    symbol: enfavorite.stockSymbol,
                    stockName: enfavorite.stockName,
                    description: enfavorite.description,
                    lastUpdated: enfavorite.lastUpdated
                }
            };

            $(`#${enfavorite.stockSymbol}_favorites`).data(curFavObj);

            if (selectedFavoriteStock == null) {
                $(`#${enfavorite.stockSymbol}_favorites`).toggleClass("highlightRow");
                selectedFavoriteStock = curFavObj.StockData;
                displayStockQuote(curFavObj.StockData.symbol)
            }
        }

        $(".favoritesRow").click(() => {
            if (selectedFavoriteStock == null) {
                $(this).toggleClass("highlightRow");
                selectedFavoriteStock = $(this).data().StockData;
                //console.log(`Stock ${selectedStock} is selected! From null`);
            } else if (selectedFavoriteStock.symbol == $(this).data().StockData.symbol) {
                $(this).toggleClass("highlightRow");
                selectedFavoriteStock = null;
                //console.log("No stock is selected.");
            } else {
                $(".favoritesRow").removeClass("highlightRow");
                $(this).addClass("highlightRow");
                selectedFavoriteStock = $(this).data().StockData;
                //console.log(`Stock ${selectedStock} is selected! different stock is selected`);
            }
        });
    }
    else {
        alert("something went wrong!");
    }
}

function displayStockQuote(symbol) {
    $.get(`/trading/GetStockQuote?symbol=${symbol}`, (respData) => {
        // Create the html for the quote and add it to quote container
        const quoteHtml = `
            <h3>Current Stock Quote</h3>
            <label>Symbol: </label>
            <span>${respData.symbol}</span><br />
            <label>Name: </label>
            <span>${respData.stockName}</span><br />
            <label>Last updated: </label>
            <span>${dateTimeFormat(respData.lastUpdated)}</span><br />
            <label>Open: </label>
            <span>${respData.open}</span><br />
            <label>High: </label>
            <span>${respData.high}</span><br />
            <label>Low: </label>
            <span>${respData.low}</span><br />
            <label>Price: </label>
            <span>${respData.price}</span><br />
            <label>Volume: </label>
            <span>${respData.volume}</span><br />
            <label>Latest trading day: </label>
            <span>${respData.latestTradingDay}</span><br />
            <label>Previous close: </label>
            <span>${respData.previousClose}</span><br />
            <label>Change: </label>
            <span>${respData.change}</span><br />
            <label>Change percent: </label>
            <span>${respData.changePercent}</span><br />
         `;

        $("#QuoteContainer").append(quoteHtml);

    }).fail(function (response) {
        alert(response.responseText);
    });
}

function getFavorite() {
    url = "trading/getFavoriteList?userId=1";

    $("#FavoriteLoading").removeClass("hideLoading").addClass("displayLoading");


    $.get(url, function (favorites) {
        formatFavorite(favorites);
        $("#FavoriteLoading").addClass("hideLoading").removeClass("displayLoading");
    }).fail(function (response) {
        alert(response.responseText);
        $("#FavoriteLoading").addClass("hideLoading").removeClass("displayLoading");
    });
}

function deleteFavorite() {
    if (selectedFavoriteStock == null) {
        alert("No stock was selected. Please select a stock");
        return;
    }
    url = `trading/deleteFromFavoriteList?userId=${userId}&symbol=${selectedFavoriteStock.symbol}`;
    $.post(url, function (deletefavorite) {
        formatFavorite(deletefavorite);
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

