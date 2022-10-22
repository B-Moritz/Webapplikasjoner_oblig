
import React from 'react';

//const charactersList = document.getElementById('charactersList');
//const searchBar = document.getElementById('searchBar');
//let hpCharacters = [];

const stocksList = document.getElementById('charactersList');
const searchStockBar = document.getElementById('searchBar');
let hpStocks = [];

searchStockBar.addEventListener('keyup', (e) => {
    const searchstring = e.target.value.toLowerCase();
    const filteredStocks = hpStocks.filter((character) => {
        return (
            character.name.toLowerCase().includes(searchstring) ||
            character.zone.toLowerCase().includes(searchstring) ||

            );
    });
    displayCharacters(filteredStocks);

});


/*searchBar.addEventListener('keyup', (e) => {
    const searchString = e.target.value.toLowerCase();

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
    displayCharacters(filteredCharacters);
});*/



const loadCharacters = async () => {
    try {
        // https://hp-api.herokuapp.com/api/characters
        const res = await fetch('localhost:1635/trading/getPortfolio?userId=1');
        hpStocks = await res.json();
        displayCharacters(hpStocks);
    } catch (err) {
        console.error(err);
    }
};

const displayCharacters = (characters) => {
    const htmlString = characters
        .map((character) => {
            return `
            <li class="character">
                <h2>${character.stockSymbol}</h2>
                <h2>${character.date}</h2>
                <h2>${character.stockCount}</h2>

                <p>zone: ${character.zone}</p>
                <img src="${character.price}"></img>
            </li>
        `;
        })
        .join('');
    charactersList.innerHTML = htmlString;
};

loadCharacters();
