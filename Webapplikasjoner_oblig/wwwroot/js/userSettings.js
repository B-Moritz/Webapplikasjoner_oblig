const userId = 1;
$(function () {
    getUser()

    $("#lagre").click(UpdateSettings);

    $("#reset").click(function () {

        const dialogHtml = `<h3 style="grid-area: Title">Reset the user account</h3>
                            <p style="grid-area: Text">Please note that the action of resetting your user account will remove
                            your entire wachlist, portfolio, earnings and trade history. The funds available will be set to 1 000 000 NOK</p>
                            <button id="ConfirmReset" style="grid-area: ResetBtn" class="btnbtn-lg btn-danger">Confirm reset</button>
                            <button id="CancelReset" style="grid-area: CancelBtn" class="btnbtn-lg btn-secondary">Cancel</button>`;

        $("#InnerDialog").html(dialogHtml).css("grid-template-areas", `'Title Title' 'Text Text' 'ResetBtn CancelBtn'`);
        $("#DialogContainer").removeClass("hideDialog");

        // Disable functionality
        $("input:enabled").prop("disabled", true);
        $("select:enabled").prop("disabled", true);
        $("#lagre").prop("disabled", true);
        $("#reset").prop("disabled", true);

        $("#ConfirmReset").click(function () {
            const url = `/trading/ResetProfile?userId=1`;
            $("#UserLoading").addClass("displayLoading").removeClass("hideLoading");
            $.post(url, function (data) {
                if (data) {
                    displayUser(data);
                }
                enableFunctionality();
                $("#UserLoading").addClass("hideLoading").removeClass("displayLoading");
            }).fail(function (response) {
                alert(response.responseText);
                enableFunctionality();
                $("#UserLoading").addClass("hideLoading").removeClass("displayLoading");
            });
            $("#DialogContainer").addClass("hideDialog");
        });

        $("#CancelReset").click(function () {
            $("#DialogContainer").addClass("hideDialog");
            // Enable functionality
            enableFunctionality();
        });
    })
});

function enableFunctionality() {
    $("input:disabled").prop("disabled", false);
    $("select:disabled").prop("disabled", false);
    $("#lagre").prop("disabled", false);
    $("#reset").prop("disabled", false);
}

function disableFunctionality() {
    $("input:enabled").prop("disabled", true);
    $("select:enabled").prop("disabled", true);
    $("#lagre").prop("disabled", true);
    $("#reset").prop("disabled", true);

    $("#UserLoading").addClass("displayLoading").removeClass("hideLoading");
}

function getUser() {
    const url = `trading/getUser?userId=${userId}`;

    // Disable functionality
    disableFunctionality();

    $("#UserLoading").addClass("displayLoading").removeClass("hideLoading");
    $.get(url, function (data) {
        if (data) {
            displayUser(data);
        }
        enableFunctionality();
        $("#UserLoading").addClass("hideLoading").removeClass("displayLoading");
    }).fail(function (response) {
        alert(response.responseText);
        enableFunctionality();
        $("#UserLoading").addClass("hideLoading").removeClass("displayLoading");
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


function UpdateSettings() {
    const user = {
        Id: 1,
        FirstName: $("#FirstName").val(),
        Lastname: $("#LastName").val(), 
        Email: $("#Email").val(),
        Currency: $("#Currency").val()
    }
    const url = "trading/updateUser"

    disableFunctionality();

    $("#UserLoading").addClass("displayLoading").removeClass("hideLoading");
    $.post(url, user, function (data) {
        if (data) {
            displayUser(data);
        }
        enableFunctionality();
        $("#UserLoading").addClass("hideLoading").removeClass("displayLoading");
    }).fail(function (response) {
        alert(response.responseText);
        enableFunctionality();
        $("#UserLoading").addClass("hideLoading").removeClass("displayLoading");
    });
}