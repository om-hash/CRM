

"use strict";

var connection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .configureLogging(signalR.LogLevel.Error)
    .build();

//----------------------------------------
connection.start().then(function () {
    //console.warn("notificationHub Connection Started");
    //connection.invoke("joinGroup", sender).catch(function (err) {
    //    return console.error(err.toString());
    //});
    connection.invoke('GetAllNotification', 25,).then((res) => {
        AppendNotification(res);
    }).catch((e) => {
        console.error("Error ", e);
    });
}).catch(function (err) {
    return console.error(err.toString());
});



//----------------------------------------
connection.on("notify", function (data) {
    let NotificationCount = Number.parseInt(document.getElementById('cotificationCount').textContent);
    document.getElementById('cotificationCount').textContent = (isNaN(NotificationCount) ? 0 : NotificationCount) + 1;

    var langid = document.getElementById('languageId').getAttribute('value');
    var maindiv = document.getElementById('notificationHolder');
    data.forEach((val, i) => {

        if (val.languageId == langid) {

            var date1 = new Date(val.date);
            var date2 = new Date();
            var difdate = dateDiff(date1, date2);

            NotificationToast(difdate, val.title, val.url);

            maindiv.innerHTML = (` <a href="${val.url}" onclick="setAsSeen(${val.id})" class="dropdown-item">
                <i class="fas fa-envelope mr-2"></i>
                <span class="float-right" style="height: 10px; width: 10px; border-radius: 50%; background-color: red;"></span>
                <p>${val.title}</p>
                <span class="float-right text-muted text-sm">${difdate}</span>
            </a>`) + maindiv.innerHTML;
        }

    });

});



//----------------------------------------
//$('document').ready(function () {
//    console.log('sdfsdfsd');
//    $.ajax({
//        type: "post",
//        url: "/admin/Dashboard/GetNotifications",
//        success: function (message) {
//            if (message.responseType != null) {
//                AppendNotification(message.result);

//            }
//            else {
//                ErrorDialog("Error", "something happend while Showing the Notification!!");
//            }
//        },
//        error: function () {
//            alert("there was error Showing notification!");
//        }
//    });
//})

function AppendNotification(res) {
    let notiCount = 0;
    var maindiv = document.getElementById('notificationHolder');
    res.forEach((val, i) => {
        if (!notiCount) notiCount = 0;
        if (!val.isSeen) {
            notiCount += 1;
        };

        var date1 = new Date(val.date);
        var date2 = new Date();
        var difdate = dateDiff(date1, date2);
        maindiv.innerHTML += (` <a href="${val.url}" onclick="setAsSeen(${val.id})" class="dropdown-item">
                <i class="fas fa-envelope mr-2"></i>
                ${val.isSeen == true ? '' : ' <span class="float-right" style="height: 10px; width: 10px; border-radius: 50%; background-color: red;"></span>'}
                <p>${val.title}</p>
                <span class="float-right text-muted text-sm">${difdate}</span>
            </a>`) ;


    });

    if (notiCount > 25) {
        document.getElementById('cotificationCount').textContent = 25 + "+";
    } else if (notiCount == 0) {
        document.getElementById('cotificationCount').textContent = "";
    }
    else {
        document.getElementById('cotificationCount').textContent = notiCount;
    }
};

//---------------------------------


//$('#NotificationBTN').on('click', function (e) {

//    $.ajax({
//        url: '/admin/Dashboard/SetNotificatoinSeen/',
//        type: 'post',
//        success: function (res) {
//            if (res.responseType === 1) {
//                console.log('Notificationseen');
//            }
//        },
//        error: function (e) {
//            console.error(e);
//        }
//    });
//});



function setAsSeen(e) {

    $.ajax({
        url: '/admin/Dashboard/SetNotificatoinSeen/'+e,
        type: 'post',
        success: function (res) {
            if (res.responseType === 1) {
                console.log('Notificationseen');

            }
        },
        error: function (e) {
            console.error(e);
        }
    });
}

