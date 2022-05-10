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