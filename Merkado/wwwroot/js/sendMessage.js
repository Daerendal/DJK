async function sendForm() {

    const toUserId = document.querySelector('#ToUser').value;
    const message = document.querySelector('#NewMessege').value;
    const reloader = document.querySelector(`[data-item-id='${toUserId}']`);

    $.ajax({
        type: 'POST',
        url: '/Chat/SendMessage',
        data: { ToUser: toUserId, NewMessege: message },
        success: function (data) {
            reloader.click();
            setTimeout(() => {
                window.scrollTo(0, document.body.offsetHeight);
            }, 300)
            
        },
        error: function (ex) {
        }
    })
    return false;
}