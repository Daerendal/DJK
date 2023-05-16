jQuery(function ($) {
    $(".hearte").on('click touchstart', function () {
        $(this).toggleClass('is_animating');
        $(this).toggleClass('liked');
    });

    /*when the animation is over, remove the class*/
    $(".hearte").on('animationend', function () {
        $(this).toggleClass('is_animating');
    });


});

function addtofavouriteSeller(Id) {
        $.ajax({
            type: 'POST',
            url: '/UserInfo/addtoFavouritee',
            data: { UserId: Id },
            success: function (data) {

            },
            error: function (ex) {

            }
        })
    }

function removefavouriteSeller(Id) {
        $.ajax({
            type: 'POST',
            url: '/UserInfo/removefavourite',
            data: { UserId: Id },
            success: function (data) {

            },
            error: function (ex) {

            }
        })
    }
function SendMail(id) {
    $.ajax({
        type: 'POST',
        url: '/PaymentPage/ValueBox',
        data: { checkboxId: id },
        success: function (data) {

        },
        error: function (ex) {

        }
    })
}

function sendMessage(Id) {
    window.location.pathname = `Chat`;
}