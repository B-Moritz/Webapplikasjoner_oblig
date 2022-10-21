

$(function () {
    $("#SearchBar").change(function (event) {
        const keyword = event.target.value;
        const url = `/trading/saveSearchResult?keyword=${keyword}`;

        $.get(url, function (data) {

            let outHtml = "";
            for (let stock of data.stockList) {
                outHtml += `<li class="list-group-item">${stock.stockSymbol}</li>`;
            }
            $("#StockResultList").empty().html(outHtml);
        }).fail(function (resp) {
            alert(resp.responseText);
        });

        /*const searchString = event.target.value.toLowerCase();

        // if searchstr is H - h
        // if searchstr is h - h
        // convert name to lowercase and then compare
        // convert zone to lowercase and the copmare

        const filteredCharacters = hpCharacters.filter((character) => {
            return (
                character.name.toLowerCase().includes(searchString) ||
                character.zone.toLowerCase().includes(searchString)
            );
        });
        displayCharacters(filteredCharacters);*/
    });

});

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