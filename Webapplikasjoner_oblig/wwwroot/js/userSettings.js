const userId = 1;
$(function () {
    getUser()
});
function getUser() {
    const url = "Trading/GetUser?userId=1"
    $.get(url, user, function (data) {
        if (data) {
            $("#FirstName").val(data.firstName)
            $("#FirstName").innerhtml(data.firstName)
            $("LastName").val(data.lasttName)
            $("Email").val(data.email)
            $("Password").val(data.password)
            $("Currency").val(data.currency)
            $("#Currency").html(data.currency)
        }
    }).fail(function (response) {
        selectedFavoriteStock = null;
        alert(response.responseText);
        $("#FavoriteLoading").addClass("hideLoading").removeClass("displayLoading");
        reenableFavoriteWidget();
    });
}


function lagreSettings() {
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
