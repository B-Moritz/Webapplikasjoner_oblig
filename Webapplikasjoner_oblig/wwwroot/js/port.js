/*$(function () {
    $('#button#portfolios').click(function () {
        $.ajax({
            type: "GET",
            url: "/Trading/getPortfolio",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                if (response != null) {
                    $("#portfolios").val(response.url);
                }
                else {
                    alert("something went wrong");
                }
            },
            failure: function (response) {
                alert(response.responseText);
            },
            error: function (response) {
                alert(response.responseText);
            }
        });
    });
});



var container = $('div.container');

$('input#get').click(function () {
    $.ajax({
        type: 'GET',
        url: 'trading/getPortfolio',
        dataType: [],
        success: function (data) {
            $.each(data, function (index, item) {
                $.each(item, function (key, value) {
                    container.append(key + ': ' + value + '</br>');
                });
                container.append('<br/></br>');
            });
        }
    });
});

