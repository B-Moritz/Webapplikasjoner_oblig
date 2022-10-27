
const userId = 1;

$(function () {
    $("#SearchBar").change(function (event) {
        const keyword = event.target.value;
        const url = `/trading/GetUserSearchResult?keyword=${keyword}&userId=${userId}`;
        const addFavoriteHtml = `class="addFavorite btn btn-lg btn-secondary">Add to watchlist</button>`;
        const removeFavoriteHtml = `class="removeFavorite btn btn-lg btn-warning">Remove from watchlist</button>`;

        $("#MarketLoading").removeClass("hideLoading").addClass("displayLoading");
        $("#SearchBar").prop("disabled", true);
        $.get(url, function (data) {

            let outHtml = "";
            $("#StockResultList").empty()

            if (data.stockList.length === 0) {
                $("#StockResultList").append("<tr><td><p>Could not find any stock matching your search keyword.</p></td><tr>");
            }

            for (let stock of data.stockList) {
                const curId = "Stock_" + stock.symbol.replaceAll(".", "");
                outHtml = `<tr>
                                <td>${stock.symbol}</td>
                                <td>${stock.stockName}</td>
                                <td><button id="${curId}" ${stock.isFavorite ? removeFavoriteHtml : addFavoriteHtml}</td>
                            </tr>`;
                const curStockObj = {
                    searchStock: {
                        symbol: stock.symbol,
                        stockName: stock.stockName,
                        description: stock.description
                    }
                }
                $("#StockResultList").append(outHtml);
                $(`#${curId}`).data(curStockObj);
            }

            $("#MarketLoading").removeClass("displayLoading").addClass("hideLoading");
            $("#SearchBar").prop("disabled", false);

            $(".addFavorite").click(function () { addFavoriteList($(this))});

            $(".removeFavorite").click(function () { removeFavoriteList($(this)) });

        }).fail(function (resp) {
            alert(resp.responseText);
            $("#MarketLoading").removeClass("displayLoading").addClass("hideLoading");
            $("#SearchBar").prop("disabled", false);
        });
    });
});


function addFavoriteList (curElem) {
    const url = `/trading/addToFavoriteList?userId=${userId}&symbol=${curElem.data().searchStock.symbol}`;
    $.post(url, function () {
        curElem.text("Remove from watchlist").removeClass("addFavorite btn-secondary").addClass("removeFavorite btn-warning");
        curElem.off().click(function () { removeFavoriteList($(this))});
    }).fail(function (resp) {
        alert(resp.responseText);
    });
}

function removeFavoriteList(curElem) {
    const url = `/trading/deleteFromFavoriteList?userId=${userId}&symbol=${curElem.data().searchStock.symbol}`
    $.post(url, function () {
        curElem.text("Add to watchlist").removeClass("removeFavorite btn-warning").addClass("addFavorite btn-secondary");
        curElem.off().click(function () { addFavoriteList($(this))});
    }).fail(function (resp) {
        alert(resp.responseText);
    });
}
