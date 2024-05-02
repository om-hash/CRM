"use strict";
var chatConnection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .configureLogging(signalR.LogLevel.Error)
    .build();
var globalChatInboxId = "";
var chatNotificationsList = [];

//----------------------------------------
chatConnection.start().then(function () {
    

}).catch(function (err) {
    return console.error(err.toString());
});



//----------------------------------------

chatConnection.on("testtest", function (data) {
    addChatNotification(data);
});

//----------------------------------------
function addChatNotification(data) {
    console.log(data);
    //increaseMsgsCount(data);

}




//---------------------------------
function increaseMsgsCount(data) {
    if (globalChatInboxId === generateGlobalChatInboxId(data.chatType, data.referenceId1, data.referenceId2)) return;

    if (chatNotificationsList.indexOf(data.senderId) === -1) {
        chatNotificationsList.push(data.senderId);
    }

    $('#msgsBadge').html(chatNotificationsList.length < 1 ? '' : chatNotificationsList.length);
}
//---------------------------------
function generateGlobalChatInboxId(chatType, ref1, ref2) {
    return `${chatType}-${ref1}-${ref2}`;
}

//----------------------------------------
//chatConnection.on("receiveChatMsg", function (data) {
//    console.log("receiveChatMsg")
//    ShowMsgOnScreen(data);
//});