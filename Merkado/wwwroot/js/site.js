// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
//Get the button:
mybutton = document.getElementById("GoTop");

// When the user scrolls down 20px from the top of the document, show the button
window.onscroll = function () { scrollFunction() };

function scrollFunction() {
    if (document.body.scrollTop > 20 || document.documentElement.scrollTop > 20) {
        mybutton.style.display = "block";
    } else {
        mybutton.style.display = "none";
    }
}
let btnchange = document.querySelector('#Button2');
let Price1 = document.querySelector('#btnProviderPrice');
let Price2 = document.querySelector('#AllPrice');
let Price3 = document.querySelector('#AllPrice2');
let Price4 = document.querySelector('#inactive');
let inputcode = document.querySelector("#andrzej");
    btnchange.addEventListener('click', () => {
        var promocode = $("#andrzej").val();
        $.ajax({
            type: 'GET',
            url: '/PaymentPage/promocode',
            data: { promocode: promocode },
            success: function (data) {
                if (data == "activecode") {
                    Price1.innerHTML = '0 zł'
                    Price2.style.display = 'none'
                    Price3.style.display = 'block'
                    Price4.style.display = 'none'
                    btnchange.style.display = 'none'
                    inputcode.style.display = 'none'
                }
                else {
                    Price4.style.display = 'block'
                }
            },
            error: function (ex) {
                var x = 1;
            }
        })

    });



//function promocode() {
//    var promocode = $("#andrzej").val();
//    
//}

// When the user clicks on the button, scroll to the top of the document
function topFunction() {
    document.body.scrollTop = 0; // For Safari
    document.documentElement.scrollTop = 0; // For Chrome, Firefox, IE and Opera
}


function SendMail(Id, pId) {
    $.ajax({
        type: 'POST',
        url: '/PaymentPage/SendMails',
        data: { SellerId: Id, idprod:pId},
        success: function (data) {

        },
        error: function (ex) {

        }
    })
}