jQuery(function ($) {
    $(".heart").on('click touchstart', function () {
        $(this).toggleClass('is_animating');
        $(this).toggleClass('liked');
    });

    /*when the animation is over, remove the class*/
    $(".heart").on('animationend', function () {
        $(this).toggleClass('is_animating');
    });


});

function addtofavourite(productId) {
    $.ajax({
        type: 'POST',
        url: '/ProductPage/addtoFavourite',
        dataType: 'json',
        data: { productID: productId },
        success: function (data) {

        },
        error: function (ex) {

        }
    })
}

function removefavourite(productId) {
    $.ajax({
        type: 'POST',
        url: '/ProductPage/removefavourite',
        dataType: 'json',
        data: { productID: productId },
        success: function (data) {

        },
        error: function (ex) {

        }
    })
}
