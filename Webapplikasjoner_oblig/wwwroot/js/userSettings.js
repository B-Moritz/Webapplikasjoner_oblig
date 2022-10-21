const userId = 1;
$(function () {
    getUser()

    $("#reset").click(function(data) {
        if (data) {
            displayUser(data);
        }
    }).fail(function (response) {
        alert(response.responseText);
    });

});
function getUser() {
    const url = `trading/getUser?userId=${userId}`;
    $.get(url, function (data) {
        if (data) {
            displayUser(data);
        }
    }).fail(function (response) {
        alert(response.responseText);
    });
}

function displayUser(user) {

    $("#FirstName").val(user.firstName);
    $("#LastName").val(user.lastName);
    $("#Email").val(user.email);
    $("#Password").val(user.password);
    $("#Currency").val(user.currency);
    $("#CostField").html(user.fundsSpent);
    $("#TotalFundsField").html(user.fundsAvailable);
}


/*function lagreSettings() {
    const user = {
        userId = 1,
        forname: $("FirstName").val(),
        lastname: $("LastName").val(), 
        email: $("Email").val(),
        password: $("Password").val(),
        currency: $("Currency").val()
    }
    const url = "Trading/UpdateUser"
    $.post(url, user, function (data) {
        if (data) {

        }
    }).fail(function (response) {
        selectedFavoriteStock = null;
        alert(response.responseText);
        $("#FavoriteLoading").addClass("hideLoading").removeClass("displayLoading");
        reenableFavoriteWidget();
    });
}
*/