const userId = 1;
$(function () {
    getUser()
});
function getUser() {
    const url = "trading/getUser?userId=1"
    $.get(url, function (data) {
        if (data) {
            $("#FirstName").val(data.firstName)
            $("#LastName").val(data.lasttName)
            $("#Email").val(data.email)
            $("#Currency").val(data.currency)
        }
    }).fail(function (response) {
        selectedFavoriteStock = null;
        alert(response.responseText);
        $("#FavoriteLoading").addClass("hideLoading").removeClass("displayLoading");
        reenableFavoriteWidget();
    });
}

/*
function UpdateSettings() {
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