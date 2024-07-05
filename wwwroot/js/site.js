const connection = new signalR.HubConnectionBuilder()
  .withUrl("/notificationHub")
  .build();

connection.on("ReceiveNotification", function (user, message) {
  const notificationElement = document.createElement("div");
  notificationElement.innerText = message;
  notificationElement.style.backgroundColor = "green";
  notificationElement.style.Color = "white";
  notificationElement.style.padding = "10px";
  notificationElement.style.position = "fixed";
  notificationElement.style.top = "10px";
  notificationElement.style.right = "10px";
  notificationElement.style.zIndex = "1000";
  document.body.appendChild(notificationElement);
  setTimeout(() => {
    notificationElement.remove();
  }, 5000);
});

connection.start().catch(function (err) {
  return console.error(err.toString());
});
