
let selectedPortfolioStock = null;
let selectedFavoriteStock = null;
const userId = 1;
const commonDialogPart = `<div style="grid-area: inputGroup1;" class="input-group">
                            <label class="input-group-addon">Number of shares</label>
                            <input id="StockCounterInput" class="form-control" type="number" name="StockCounter" value="1">
                          </div>
                          <div id="DialogErrorMsg" style="grid-area: errorMsg;"></div>
                          <button type="button" id="Confirm" class="btn btn-success btn-lg">Confirm Transaction</button>
                          <button type="button" id="CancelDialog" class="btn btn-secondary btn-lg">Cancel Transaction</button>`;


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

        let innerHtml = `
            <h3>Buy stock</h3>
            <label style="grid-area: label1;" name="Stock Symbol">Stock Symbol:</label>
            <p style="grid-area: static1">${selectedPortfolioStock.symbol}</p>
            <label styles="grid-area: label2;" name="Stock Name">Stock Name:</label>
            <p style="grid-area: static2">${selectedPortfolioStock.stockName}</p>
            <label style="grid-area: label3;" name="Quantity">Number of shares in portfolio:</label>
            <p style="grid-area: static3">${selectedPortfolioStock.quantity}</p>
            <label style="grid-area: label4;" name="EstPrice">Estimated price per share:</label>
            <p style="grid-area: static4">${selectedPortfolioStock.estPrice}</p>`;

        innerHtml += commonDialogPart;

        const template = `'title title'
                          'label1 static1'
                          'label2 static2'
                          'label3 static3'
                          'label4 static4'
                          'inputGroup1 inputGroup1'
                          'errorMsg errorMsg'
                          'ConfirmButton CancelButton'`;

        const buyPortfolio = function () {
            const count = parseInt($("#StockCounterInput").val());

            $("#PortfolioLoading").removeClass("hideLoading").addClass("displayLoading");
            $.post(`trading/buyStock?userId=${userId}&symbol=${selectedPortfolioStock.symbol}&count=${count}`, (data) => {
                updatePortfolioList(data);
                $("#PortfolioLoading").removeClass("displayLoading").addClass("hideLoading");
                reenablePortfolioWidget(false);
                reenableFavoriteWidget(false);
            }).fail((response) => {
                alert(response.responseText);
                $("#PortfolioLoading").removeClass("displayLoading").addClass("hideLoading");
                reenablePortfolioWidget(false);
                reenableFavoriteWidget(false);
            });
            $("#DialogContainer").removeClass("showDialog");
            $("#DialogContainer").addClass("hideDialog");
            $("body").css("overflow", "auto");
        };

        createDialog(innerHtml, template, buyPortfolio);

        //$("#Confirm").click(buyPortfolio);
    });

    $("#BuyFavoriteBtn").click(function () {
        if (selectedFavoriteStock == null) {
            alert("No stock was selected. Please select a stock");
            return;
        }

        let innerHtml = `
            <h3>Buy stock</h3>
            <label style="grid-area: label1;" name="Stock Symbol">Stock Symbol:</label>
            <p style="grid-area: static1">${selectedFavoriteStock.symbol}</p>
            <label styles="grid-area: label2;" name="Stock Name">Stock Name:</label>
            <p style="grid-area: static2">${selectedFavoriteStock.stockName}</p>
            <label style="grid-area: label3;" name="EstPrice">Estimated price per share:</label>
            <p style="grid-area: static3">${selectedFavoriteStock.estPrice}</p>`;

        innerHtml += commonDialogPart;

        const template = `'title title'
                          'label1 static1'
                          'label2 static2'
                          'label3 static3'
                          'inputGroup1 inputGroup1'
                          'errorMsg errorMsg'
                          'ConfirmButton CancelButton'`;

        const buyFavorites = () => {
            const count = parseInt($("#StockCounterInput").val());

            $("#PortfolioLoading").removeClass("hideLoading").addClass("displayLoading");
            $.post(`trading/buyStock?userId=${userId}&symbol=${selectedFavoriteStock.symbol}&count=${count}`, (data) => {
                updatePortfolioList(data);
                $("#PortfolioLoading").removeClass("displayLoading").addClass("hideLoading");
                reenablePortfolioWidget(false);
                reenableFavoriteWidget(false);

            }).fail((response) => {
                alert(response.responseText);
                $("#PortfolioLoading").removeClass("displayLoading").addClass("hideLoading");
                reenablePortfolioWidget(false);
                reenableFavoriteWidget(false);

            });
            $("#DialogContainer").removeClass("showDialog");
            $("#DialogContainer").addClass("hideDialog");
            $("body").css("overflow", "auto");
        };

        createDialog(innerHtml, template, buyFavorites);

        //$("#Confirm").click(buyFavorites);
    });

    $("#SellPortfolioBtn").click(function () {
        if (selectedPortfolioStock == null) {
            alert("No stock was selected. Please select a stock");
            return;
        }

        let innerHtml = `
            <h3>Sell stock</h3>
            <label style="grid-area: label1;" name="Stock Symbol">Stock Symbol:</label>
            <p style="grid-area: static1">${selectedPortfolioStock.symbol}</p>
            <label styles="grid-area: label2;" name="Stock Name">Stock Name:</label>
            <p style="grid-area: static2">${selectedPortfolioStock.stockName}</p>
            <label style="grid-area: label3;" name="Quantity">Number of shares in portfolio:</label>
            <p style="grid-area: static3">${selectedPortfolioStock.quantity}</p>
            <label style="grid-area: label4;" name="EstPrice">Estimated price per share:</label>
            <p style="grid-area: static4">${selectedPortfolioStock.estPrice}</p>`;

        innerHtml += commonDialogPart;

        const template = `'title title'
                          'label1 static1'
                          'label2 static2'
                          'label3 static3'
                          'label4 static4'
                          'inputGroup1 inputGroup1'
                          'errorMsg errorMsg'
                          'ConfirmButton CancelButton'`;

        const sellPortfolio = function () {
            const count = parseInt($("#StockCounterInput").val());

            if (selectedPortfolioStock.count - count < 0) {
                alert("You have not enough stocks of this type to perform this operation.");
            }

            $("#PortfolioLoading").removeClass("hideLoading").addClass("displayLoading");
            $.post(`trading/sellStock?userId=${userId}&symbol=${selectedPortfolioStock.symbol}&count=${count}`, (data) => {
                updatePortfolioList(data);
                $("#PortfolioLoading").removeClass("displayLoading").addClass("hideLoading");
                reenablePortfolioWidget(false);
                reenableFavoriteWidget(false);
            }).fail((response) => {
                alert(response.responseText);
                $("#PortfolioLoading").removeClass("displayLoading").addClass("hideLoading");
                reenablePortfolioWidget(false);
                reenableFavoriteWidget(false);
            });
            $("#DialogContainer").removeClass("showDialog");
            $("#DialogContainer").addClass("hideDialog");
            $("body").css("overflow", "auto");
        };

        createDialog(innerHtml, template, sellPortfolio);

        //$("#Confirm").click(sellPortfolio);
    });
});

function reenablePortfolioWidget(isFromCancelBtn) {
    $("#GetPortfolioBtn").removeClass("disabled").removeAttr("aria-disabled");
    $("#SellPortfolioBtn").removeClass("disabled").removeAttr("aria-disabled");
    $("#BuyPortfolioBtn").removeClass("disabled").removeAttr("aria-disabled");
    if (selectedPortfolioStock != null && isFromCancelBtn == false) {
        // Make sure that the selected stock stil is selected after the dialog closes
        $(`#Portfolio_${selectedPortfolioStock.symbol}`).addClass("highlightRow");
        selectedPortfolioStock = $(`#Portfolio_${selectedPortfolioStock.symbol}`).data("StockData");
    }
}

function reenableFavoriteWidget(isFromCancelBtn) {
    $("#GetFavoriteBtn").removeClass("disabled").removeAttr("aria-disabled", "disabled");
    $("#BuyFavoriteBtn").removeClass("disabled").removeAttr("aria-disabled", "disabled");
    $("#DeleteFavoriteBtn").removeClass("disabled").removeAttr("aria-disabled", "disabled");
    if (selectedFavoriteStock != null && isFromCancelBtn == false) {
        // Make sure that the selected stock stil is selected after the dialog closes
        $(`#Favorites_${selectedFavoriteStock.symbol}`).addClass("highlightRow");
        selectedFavoriteStock = $(`#Favorites_${selectedFavoriteStock.symbol}`).data().StockData;
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
    $("#DeleteFavoriteBtn").addClass("disabled").attr("aria-disabled", "disabled");
}

function createDialog(innerHtml, template, confirmCallback) {
    disablePortfolioWidget();
    disableFavoriteWidget();
    $("#DialogContainer").removeClass("hideDialog").addClass("showDialog");
    $("body").css("overflow", "hidden");

    $("#InnerDialog").empty();
    $("#InnerDialog").append(innerHtml)
    $("#InnerDialog").css("grid-template-areas", template);

    $("#CancelDialog").click(function () {
        $("#DialogContainer").removeClass("showDialog").addClass("hideDialog");
        reenablePortfolioWidget(true);
        reenableFavoriteWidget(true);
        $("body").css("overflow", "auto");
    });
    // Ensure that the confirm button is active as default
    $("#Confirm").off().click(confirmCallback);

    // Add change event handler to the input element for amount of shares
    $("#StockCounterInput").change(function () {
        // Get the amount of shares
        const curVal = $(this).val();
        if (curVal < 1 || curVal % 1 != 0) {
            // If the amount of shares is invalid (not an integer greater than 1) -> disable confirm button and display error message
            $("#DialogErrorMsg").html(`<p class="errorMsg">The provided number of shares is not valid. 
                                            Please make sure that the value is an integer greather than 1.</p>`);
            $(this).parent().addClass("has-error").removeClass("has-success");
            $("#Confirm").addClass("disabled").attr("aria-disabled", "disabled").off();
        } else {
            // If the value is valid, ensure that confirm is not disabled and error message is removed
            $(this).parent().addClass("has-success").removeClass("has-error");
            $("#DialogErrorMsg").empty();
            $("#Confirm").removeClass("disabled").removeAttr("aria-disabled", "disabled").off().click(confirmCallback);
        }
    });
}

function setColoredValue(value) {
    
    if (value[0] !== "-") {
        return "greenValue";
    } else {
        return "redValue";
    }
}

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
        $("#UnrealizedPL").html(`<p class="${setColoredValue(data.unrealizedPL)} colorValue">${data.unrealizedPL}</p>`);

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
            const curId = "Portfolio_" + stock.symbol.replace(".", "");
            let portfolioListRow = `<tr id="${curId}" class="PortfolioRow">
                    <td>${stock.symbol}</td>
                    <td>${stock.stockName}</td>
                    <td>${stock.quantity}</td>
                    <td>${stock.estPrice}</td>
                    <td>${stock.portfolioPortion}</td>
                    <td>${stock.estTotalMarketValue}</td>
                    <td>${stock.totalCost}</td>
                    <td><span class="${setColoredValue(stock.unrealizedPL)} colorValue">${stock.unrealizedPL}</span></td>
                </tr>`
            portfolioTableElement.append(portfolioListRow)
            curStockObj = {
                StockData: {
                    symbol: stock.symbol,
                    stockName: stock.stockName,
                    quantity: stock.quantity,
                    type: stock.type,
                    totalCost: stock.totalCost,
                    estTotalMarketValue: stock.estTotalMarketValue,
                    estPrice: stock.estPrice,
                    portfolioPortion: stock.portfolioPortion,
                    unrealizedPL: stock.unrealizedPL,
                }
            };
            $(`#${curId}`).data(curStockObj);
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
                    <th>Type</th>
                    <th>Last updated</th>
                </tr>`);

        let curFavObj = {};

        for (let enfavorite of favorites.stockList) {
            const curId = "Favorites_" + enfavorite.symbol.replace(".", "");
            favoriteTableContainer.append(`<tr id="${curId}" class="favoritesRow">
                    <td>${enfavorite.stockName}</td >
                    <td>${enfavorite.symbol}</td>
                    <td>${enfavorite.type}</td>
                    <td>${dateTimeFormat(enfavorite.lastUpdated)}</td>
                    </tr >`);

            curFavObj = {
                StockData: {
                    symbol: enfavorite.symbol,
                    stockName: enfavorite.stockName,
                    type: enfavorite.type,
                    lastUpdated: enfavorite.lastUpdated
                }
            };

            $(`#${curId}`).data(curFavObj);

            if (selectedFavoriteStock == null) {
                $(`#${curId}`).toggleClass("highlightRow");
                selectedFavoriteStock = curFavObj.StockData;
                displayStockQuote(selectedFavoriteStock)
            }
        }

        $(".favoritesRow").click(function (event) {
            const curElem = $(this);
            if (selectedFavoriteStock == null) {
                curElem.toggleClass("highlightRow");
                selectedFavoriteStock = curElem.data().StockData;
                displayStockQuote(selectedFavoriteStock);
                //console.log(`Stock ${selectedStock} is selected! From null`);
            } else if (selectedFavoriteStock.symbol == curElem.data().StockData.symbol) {
                curElem.toggleClass("highlightRow");
                selectedFavoriteStock = null;
                //console.log("No stock is selected.");
            } else {
                $(".favoritesRow").removeClass("highlightRow");
                curElem.addClass("highlightRow");
                selectedFavoriteStock = curElem.data().StockData;
                displayStockQuote(selectedFavoriteStock);
                //console.log(`Stock ${selectedStock} is selected! different stock is selected`);
            }
        });
    }
    else {
        alert("something went wrong!");
    }
}

function displayStockQuote(curFavObj) {
    $.get(`/trading/GetStockQuote?symbol=${curFavObj.symbol}`, (respData) => {
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
            <span>${dateTimeFormat(respData.latestTradingDay)}</span><br />
            <label>Previous close: </label>
            <span>${respData.previousClose}</span><br />
            <label>Change: </label>
            <span class="${setColoredValue(respData.change)} colorValue">${respData.change}</span><br />
            <label>Change percent: </label>
            <span class="${setColoredValue(respData.changePercent)} colorValue">${respData.changePercent}</span><br />
         `;
        curFavObj["estPrice"] = respData.price;

        $("#QuoteContainer").empty();
        $("#QuoteContainer").append(quoteHtml);

    }).fail(function (response) {
        alert(response.responseText);
    });
}

function getFavorite() {
    url = "trading/getFavoriteList?userId=1";

    $("#FavoriteLoading").removeClass("hideLoading").addClass("displayLoading");
    disableFavoriteWidget();

    $(".favoritesRow").off("click");

    $.get(url, function (favorites) {
        formatFavorite(favorites);
        $("#FavoriteLoading").addClass("hideLoading").removeClass("displayLoading");
        reenableFavoriteWidget();
    }).fail(function (response) {
        alert(response.responseText);
        $("#FavoriteLoading").addClass("hideLoading").removeClass("displayLoading");
        reenableFavoriteWidget();
    });
}

function deleteFromFavorite() {
    if (selectedFavoriteStock == null) {
        alert("No stock was selected. Please select a stock");
        return;
    }
    $("#FavoriteLoading").removeClass("hideLoading").addClass("displayLoading");
    disableFavoriteWidget();

    $(".favoritesRow").off("click");
    url = `trading/deleteFromFavoriteList?userId=${userId}&symbol=${selectedFavoriteStock.symbol}`;
    $.post(url, function (deletefavorite) {
        selectedFavoriteStock = null;
        formatFavorite(deletefavorite);
        $("#FavoriteLoading").addClass("hideLoading").removeClass("displayLoading");
        reenableFavoriteWidget();
    }).fail(function (response) {
        selectedFavoriteStock = null;
        alert(response.responseText);
        $("#FavoriteLoading").addClass("hideLoading").removeClass("displayLoading");
        reenableFavoriteWidget();
    });
}