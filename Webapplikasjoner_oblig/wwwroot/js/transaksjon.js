let selectedTransaksjonStock = null;
const userId = 1;

$(function () {
    getAllTransaction();

    $("#GetPortfolioBtn").click(function () {
        getAllTransaction();
    });

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
        //let curStockObj = {};

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
    $.get(url, function (data) {
        formatTransaction(data);

    }).fail(function (response) {
        alert(response.responseText);
    });
};

/*function clearTransaksjon(data) {
    //const url = "trading/GetAllTrades?userId=1";

    $("#ClearTransaksjonBtn").click(function () {
        
    });

}*/

function clearTransaksjon() {

    url = `trading/clearAllTradeHistory?userId=${userId}`;
    $.post(url, function () {
        $("table").remove();
    }).fail(function (response) {
        alert(response.responseText);
    });
}
