
//let selectedPortfolioStock = null;
//let selectedFavoriteStock = null;
//const userId = 1;
//selectedFavoriteStock.symbol

$(function () {
    printAllMyPortfolio();

    /*$("#GetPortfolioBtn").click(function () {
        printAllMyPortfolio();
    });

    $("#SellPortfolioBtn").click(function () {
        if (selectedPortfolioStock == null) {
            alert("No stock was selected. Please select a stock");
            return;
        }     
    });*/
});

function dateTimeFormat(rawFormat) {
    //This method convert the date time string to a more concise format
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

        const portfolioTableElement = $("#PortfolioStockList");

        portfolioTableElement.empty();

        const portfolioListHeader = `
                <tr>
                    <th>Symbol</th >
                    <th>Amount</th>
                    <th>UserBuying</th>
                    <th>Price(NOK)</th>               
                    <th>Date</th>

                </tr>`;
        portfolioTableElement.append(portfolioListHeader);
        let curStockObj = {};

        for (let stock of data) {
            let portfolioListRow = `<tr id="${stock.symbol}_portfolio" class="PortfolioRow">
                    <td>${stock.stockSymbol}</td>
                    <td>${stock.stockCount}</td>
                    <td>${stock.userBuying}</td>
                    <td>${stock.price}</td>
                    <td>${dateTimeFormat(stock.date)}</td>
                </tr>`
    
  
           portfolioTableElement.append(portfolioListRow)
            /*curStockObj = {
                StockData: {
                    symbol: stock.stockSymbol,
                    stockCount: stock.stockCount,
                    userBuying: stock.userBuying,
                    date: stock.date.tradingTimeFormat,    
                }
            };*/
            $(`#${stock.symbol}_portfolio`).data(curStockObj);
        }

        /*$(".PortfolioRow").click(function () {
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
        }); */

    }
    else {
        alert("something went wrong!");
    }
}

function printAllMyPortfolio() {

    url = "trading/GetAllTrades?userId=1";
    //$("#PortfolioLoading").removeClass("hideLoading").addClass("displayLoading");
    //disablePortfolioWidget();

   // $(".PortfolioRow").off("click");

    $.get(url, function (data) {
        updatePortfolioList(data);
        //$("#PortfolioLoading").removeClass("displayLoading").addClass("hideLoading");
        //reenablePortfolioWidget(false);

    }).fail(function (response) {
        alert(response.responseText);
       // $("#PortfolioLoading").removeClass("displayLoading").addClass("hideLoading");
        //reenablePortfolioWidget(false);
    });
};

// portofolio











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






/*$(function () {
    hentlTransaksjon();
    updatePortfolioList();
    $("#GetPortfolioBtn").click(function () {
        updatePortfolioList();
    });
});

function hentlTransaksjon() {
    url = "trading/GetAllTrades?userId=1";
    $.get(url, function (trasaksjon) {
        formatPortfolio(trasaksjon);
        $("#PortfolioLoading").removeClass("displayLoading").addClass("hideLoading");
    }).fail(function (response) {
        alert(response.responseText);
        $("#PortfolioLoading").removeClass("displayLoading").addClass("hideLoading");
    });
};
function formatPortfolio(transaksjon) {
    transaksjontHtml = ` <table class='table table-striped'>
               <tr>
                    <th>Stock symbol</th >
                    <th>StockCount</th>
                    <th>UserBuying</th>
                    <th>Date</th>
               </tr>`;

    for (let stock of transaksjon) {
        transaksjontHtml += `<tr>
                    <td>${stock.stockSymbol}</td >
                    <td>${stock.stockCount}</td>
                    <td>${stock.userBuying}</td>
                    <td>${stock.date}</td>
                    </tr >`
    }

    transaksjontHtml += "</table>";
    $("#skrivTransaksjon").html(transaksjontHtml);
};

function formatPortfolio(portfolio) {
    return formatPortfolio(portfolio);
}*/





