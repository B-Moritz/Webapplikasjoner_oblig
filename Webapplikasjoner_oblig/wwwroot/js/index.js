$(function () {
    printUsers();
});

function printUsers() {
    $.get("kunde/hentAlle", function (users) {
        formaterUsers(users);
    });
}

function formaterUsers(users) {
    let ut = "<table class='table table-striped'>" +
        "<tr>" +
        "<th>Name</th><th>Lastname</th><th>Email</th><th>Password</th><th>Poststed</th><th></th><th></th>" +
        "</tr>";
    for (let user of users) {
        ut += "<tr>" +
            "<td>" + user.name + "</td>" +
            "<td>" + user.lastname + "</td>" +
            "<td>" + user.email + "</td>" +
            "<td>" + user.password + "</td>" +
            "<td> <a class='btn btn-primary' href='endre.html?id=" + user.id + "'>Endre</a></td>" +
            "<td> <button class='btn btn-danger' onclick='slettKunde(" + user.id + ")'>Slett</button></td>" +
            "</tr>";
    }
    ut += "</table>";
    $("#kundene").html(ut);
}

function slettKunde(id) {
    const url = "Kunde/Slett?id=" + id;
    $.get(url, function (OK) {
        if (OK) {
            window.location.href = 'index.html';
        }
        else {
            $("#feil").html("Feil i db - prøv igjen senere");
        }

    });
};

