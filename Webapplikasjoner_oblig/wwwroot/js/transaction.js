let selectedTransaksjonStock = null;
const userId = 1;

$(function () {
    getAllTransaction();

    $("#GetTransactionsBtn").click(function () {
        getAllTransaction();
    });

    $("#ClearTransactionBtn").click(function () {
        clearTransaction();
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

        const transactionTableElement = $("#TransactionList");
        transactionTableElement.empty();
        const transactionListHeader = `
                <tr>
                    <th>Symbol</th >
                    <th>Amount</th>
                    <th>UserBuying</th>
                    <th>Price(NOK)</th>               
                    <th>Date</th>

                </tr>`;
        transactionTableElement.append(transactionListHeader);

        for (let transaction of data) {
            let transactionListRow = `<tr class="PortfolioRow">
                    <td>${transaction.stockSymbol}</td>
                    <td>${transaction.stockCount}</td>
                    <td>${transaction.userBuying}</td>
                    <td>${transaction.price}</td>
                    <td>${dateTimeFormat(transaction.date)}</td>
                </tr>`
            transactionTableElement.append(transactionListRow)
           
        }
    }
    else {
        alert("something went wrong!");
    }
}

function getAllTransaction() {
    $("#TransactionLoading").removeClass("hideLoading").addClass("displayLoading");
    url = "trading/GetAllTrades?userId=1";
    $.get(url, function (data) {
        formatTransaction(data);
        $("#TransactionLoading").removeClass("displayLoading").addClass("hideLoading");
    }).fail(function (response) {
        $("#TransactionLoading").removeClass("displayLoading").addClass("hideLoading");
        alert(response.responseText);
    });
};

function disableTransactionFunctionality() {
    $("#GetTransactionsBtn").prop("disabled", true);
    $("#ClearTransactionBtn").prop("disabled", true);
}

function enableTransactionFunctionality() {
    $("#GetTransactionsBtn").prop("disabled", false);
    $("#ClearTransactionBtn").prop("disabled", false);
}


function clearTransaction() {
    disableTransactionFunctionality();
    $("#TransactionLoading").removeClass("hideLoading").addClass("displayLoading");
    url = `trading/clearAllTradeHistory?userId=${userId}`;
    $.post(url, function () {
        $("table").remove();
        $("#TransactionLoading").removeClass("displayLoading").addClass("hideLoading");
        enableTransactionFunctionality();
    }).fail(function (response) {
        $("#TransactionLoading").removeClass("displayLoading").addClass("hideLoading");
        enableTransactionFunctionality();
        alert(response.responseText);
    });
}
