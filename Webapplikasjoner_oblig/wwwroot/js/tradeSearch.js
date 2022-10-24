
const userId = 1;

$(function () {
    $("#SearchBar").change(function (event) {
        const keyword = event.target.value;
        const url = `/trading/saveSearchResult?keyword=${keyword}`;
        const addFavoriteHtml = `class="addFavorite btn btn-secondary">Add to watchlist</button>`;
        const removeFavoriteHtml = `class="removeFavorite btn btn-warning">Remove from watchlist</button>`;

        $.get(url, function (data) {

            let outHtml = "";
            $("#StockResultList").empty();

            let counter = 0;
            for (let stock of data.stockList) {
                const curId = "Stock_" + stock.symbol.replaceAll(".", "");
                outHtml = `<li class="list-group-item">
                                <span>${stock.symbol}</span>
                                <span>${stock.stockName}</span>
                                <button id="${curId}" ${stock.isFavorite ? removeFavoriteHtml : addFavoriteHtml}
                            </li>`;
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

            $(".addFavorite").click(function () { addFavoriteList($(this))});

            $(".removeFavorite").click(function () { removeFavoriteList($(this)) });

        }).fail(function (resp) {
            alert(resp.responseText);
        });
    });
});


function addFavoriteList (curElem) {
    const url = `/trading/addToFavoriteList?userId=${userId}&symbol=${curElem.data().searchStock.symbol}`;
    $.post(url, function () {
        curElem.text("Remove from watchlist").addClass(["removeFavorite", "btn", "btn-warning"]).removeClass(["addFavorite", "btn", "btn-secondary"]);
        curElem.click(function () { removeFavoriteList($(this))});
    }).fail(function (resp) {
        alert(resp.responseText);
    });
}

function removeFavoriteList(curElem) {
    const url = `/trading/deleteFromFavoriteList?userId=${userId}&keyword=${$(this).data().searchStock.symbol}`
    $.post(url, function () {
        curElem.text("Add to watchlist").addClass(["addFavorite", "btn", "btn-secondary"]).removeClass(["removeFavorite", "btn", "btn-warning"]);
        curElem.click(function () { addFavoriteList($(this)) });
    }).fail(function (resp) {
        alert(resp.responseText);
    });
}

/*let hpCharacters = [];

const loadCharacters = async () => {
    try {
        // https://hp-api.herokuapp.com/api/characters
        const res = await fetch('_apiKey');
        hpCharacters = await res.json();
        displayCharacters(hpCharacters);
    } catch (err) {
        console.error(err);
    }
};

const displayCharacters = (characters) => {
    const htmlString = characters
        .map((character) => {
            return `
            <li class="character">
                <h2>${character.name}</h2>
                <p>zone: ${character.zone}</p>
                <img src="${character.price}"></img>
            </li>
        `;
        })
        .join('');
    $("#charactersList").html(htmlString);
};

loadCharacters();
*/


//     <link rel="stylesheet" href="../css/tradeSearch.css" />
