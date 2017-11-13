

$(document).ready(function () {
    $('[data-toggle="tab"]').click(function (e) {
        e.preventDefault();
        $(this).tab('show');
    });
});

$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip();
});
$(function () {
    $("#default-focus-a").focus();
});


// /Home/UpdateSettings Ajax

function onBegin() {
    $('#btnSaveUserSettings').addClass('disabled');
}

function onSuccess(data) {
    if (data.HasDisplayName) {
        $('#spanUserAnonymous').addClass('hide');
        $('#spanUserNamed').removeClass('hide');
    }
    else {
        $('#spanUserAnonymous').removeClass('hide');
        $('#spanUserNamed').addClass('hide');
    }

    $('#spanUserDisplayName').text(data.DisplayName);

    $('#modalUserSettings').modal('hide');
    $('#btnSaveUserSettings').removeClass('disabled');
}



