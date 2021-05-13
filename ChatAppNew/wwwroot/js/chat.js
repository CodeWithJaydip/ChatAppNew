
"use strict"
var connection = new signalR.HubConnectionBuilder().withUrl('/chatHub').build();
var _connectionId = "";
connection.on("RecieveMessage", function (data) {
    console.log(data);
})



var joinRoom = function () {
    var url = '/Chat/JoinRoom/' + _connectionId + '/@Model.Id'
    axios.post(url, null)
        .then(res => console.log("Room joined!!", res))
        .catch(err => console.log("Failed to join Room!", err))
}
connection.start().then(function () {
    connection.invoke("getConnectionId").then(function (connectionId) {
        _connectionId = connectionId;
        joinRoom();


    })

}).catch(function (err) {
    console.error('Unable to join', err);
})

var sendMessage = function (event) {
    event.preventDefault();

    var data = new FormData(event.target);
    axios.post('/Chat/SendMessage', data)
        .then(res => console.log("Message sent..."))
        .catch(err => console.error("Failed to sent message!!", err));
    document.getElementById('msgInput').value = "";


}

