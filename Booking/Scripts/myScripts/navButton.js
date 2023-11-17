$(window).resize(function () {
    if ($(window).width() <= 1100) {
        $('.full-name').hide();
        $('.initials').show();
    } else {
        $('.full-name').show();
        $('.initials').hide();
    }
}).resize();