let selectedTransaksjonStock = null;
const userId = 1;

$(function () {
    getAllTransaction();

    /*$("#GetPortfolioBtn").click(function () {
        printAllMyPortfolio();
    });*/
    $("#ClearTransaksjonBtn").click(function () {
        clearTransaksjon();
    });

});

function dateTimeFormat(rawFormat) {
    //This method convert the date time string to a more concise format
    const regex = /([0-9]{4})-([0-9]{2})-([0-9]{2})T([0-9]{2}):([0-9]{2}):([0-9])/;
    let matchResult = regex.exec(rawFormat);
    const tradingTimeFormat = `${matchResult[1]}.${matchResult[2]}.${matchResult[3]}   ${matchResult[4]}:${matchResult[5]}:${matchResult[6]}`
    return tradingTimeFormat;
}



function formatTransaction(data) {
    // This function updates the portfolio list
    // Argument: data - the response object from the getPortfolio endpoint on server 
    if (data != null) {
        // If the response data contains contnt, proceed with updating

        const transaksjonTableElement = $("#PortfolioStockList");
        transaksjonTableElement.empty();
        const transactionListHeader = `
                <tr>
                    <th>Symbol</th >
                    <th>Amount</th>
                    <th>UserBuying</th>
                    <th>Price(NOK)</th>               
                    <th>Date</th>

                </tr>`;
        transaksjonTableElement.append(transactionListHeader);
        let curStockObj = {};

        for (let transaksjon of data) {
            let portfolioListRow = `<tr id="${transaksjon.symbol}_transaction" class="PortfolioRow">
                    <td>${transaksjon.stockSymbol}</td>
                    <td>${transaksjon.stockCount}</td>
                    <td>${transaksjon.userBuying}</td>
                    <td>${transaksjon.price}</td>
                    <td>${dateTimeFormat(transaksjon.date)}</td>
                </tr>`
            transaksjonTableElement.append(portfolioListRow)
            $(`#${transaksjon.symbol}_transaction`).data(curStockObj);
        }

    }
    else {
        alert("something went wrong!");
    }
}

function getAllTransaction() {

    url = "trading/GetAllTrades?userId=1";
    //$("#PortfolioLoading").removeClass("hideLoading").addClass("displayLoading");
    //disablePortfolioWidget();

   // $(".PortfolioRow").off("click");

    $.get(url, function (data) {
        formatTransaction(data);
        //$("#PortfolioLoading").removeClass("displayLoading").addClass("hideLoading");
        //reenablePortfolioWidget(false);

    }).fail(function (response) {
        alert(response.responseText);
       // $("#PortfolioLoading").removeClass("displayLoading").addClass("hideLoading");
        //reenablePortfolioWidget(false);
    });
};

function clearTransaksjon(data) {
    //const url = "trading/GetAllTrades?userId=1";

    $("#ClearTransaksjonBtn").click(function () {
        $("table").remove();
    });

}

// portofolio
function clearTransaksjon() {
    if (selectedTransaksjonStock == null) {
        
        alert("No stock was selected. Please select a stock");
        return;
    }
    url = `trading/ClearTradeHistory?userId=${userId}&symbol=${selectedTransaksjonStock.symbol}`;
    $.post(url, function (cleattransaksjon) {
        formatTransaction(cleattransaksjon);
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





