function doLoginJob() {
    $.ajax(
     {
         type: "POST",
         url: "login.aspx/doLoginJob",
         data: "{}",
         contentType: "application/json; charset=utf-8",
         dataType: "json",
         async: true,
         cache: false,
         success: function (msg) {
             var logstat = msg.d;
             $('#logoutstat').val(logstat);
             if (logstat === '0') {
                 alert('You have logged out!');
                 window.location = "Login.aspx";
             }
         },
         error: function (x, e) {
             //alert("The call to the server side failed. doCheckLoggedOut. " + x.responseText);
         }
     });
}

function doAlertJob() {
    //swal("Here's a message!");
    onco({
        title: "Auto close alert!",
        text: "I will close in 2 seconds.",
        timer: 2000,
        showConfirmButton: false
    });
}

$(function () {
    $("#card").flip({
        axis: "y",
        reverse: true,
        trigger: "manual"
    });

    $(".flip-on").click(function (e) {
        $("#card").flip(true);
    });

    $(".flip-off").click(function (e) {
        $("#card").flip(false);
    });

    $("[id*=cboUserName], [id*=txtPassword]").WaterMark();
});