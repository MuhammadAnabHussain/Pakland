const connection = new signalR.HubConnectionBuilder()
  .withUrl("/notificationHub")
  .build();

let notifications = JSON.parse(localStorage.getItem("notifications")) || [];
let notificationsViewed =
  JSON.parse(localStorage.getItem("notificationsViewed")) || false;

// Function to update the notification list in the dropdown
function updateNotificationList() {
  const notificationList = document.getElementById("notificationList");
  notificationList.innerHTML = "";

  notifications.forEach((notification) => {
    const listItem = document.createElement("li");
    listItem.classList.add("dropdown-item");
    listItem.innerText = `${notification.message} - ${notification.timestamp}`;
    notificationList.appendChild(listItem);
  });

  // Update the notification count
  const notificationCount = document.getElementById("notificationCount");
  const count = notificationsViewed ? 0 : notifications.length;
  notificationCount.innerText = count;

  // Show or hide the notification badge based on the count
  if (count === 0) {
    notificationCount.style.display = "none";
  } else {
    notificationCount.style.display = "inline";
  }
}

connection.on("ReceiveNotification", function (user, message) {
  // Store the notification in the array
  const newNotification = {
    user,
    message,
    timestamp: new Date().toLocaleString(),
  };
  notifications.push(newNotification);
  localStorage.setItem("notifications", JSON.stringify(notifications));
  notificationsViewed = false;
  localStorage.setItem(
    "notificationsViewed",
    JSON.stringify(notificationsViewed)
  );

  // Update the notification list and count
  updateNotificationList();
});

connection.start().catch(function (err) {
  return console.error(err.toString());
});

// Add an event listener to the notification icon to populate the notification list when clicked
document
  .getElementById("notificationDropdown")
  .addEventListener("click", function () {
    if (!notificationsViewed) {
      notificationsViewed = true;
      localStorage.setItem(
        "notificationsViewed",
        JSON.stringify(notificationsViewed)
      );
      localStorage.clear();
    } else {
      // Clear the notifications array and update localStorage
      notifications = [];
      localStorage.setItem("notifications", JSON.stringify(notifications));
    }
    // Update the notification list and count
    updateNotificationList();
  });

// Initial call to update the notification list and count on page load
updateNotificationList();
