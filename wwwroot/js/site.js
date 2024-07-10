"use strict";

var connection = new signalR.HubConnectionBuilder()
  .withUrl("/notificationHub")
  .build();

connection.on("ReceiveNotification", function (user, message) {
  // Fetch notifications when a new notification is received
  fetchNotifications();
});

connection.start().catch(function (err) {
  return console.error(err.toString());
});

document
  .getElementById("notificationDropdown")
  .addEventListener("click", function (event) {
    // Fetch notifications when the bell icon is clicked
    event.preventDefault();
    console.log("Bell icon clicked");
    fetchNotifications();
  });

function fetchNotifications() {
  console.log("Fetching notifications...");
  fetch("/Notification/GetUnreadNotifications")
    .then((response) => {
      if (!response.ok) {
        throw new Error("Network response was not ok " + response.statusText);
      }
      console.log("Response received");
      return response.json();
    })
    .then((data) => {
      console.log("Data:", data);
      var modalBody = document.getElementById("notificationModalBody");
      console.log(modalBody);

      // Clear existing notification items
      modalBody.innerHTML = "";

      // Create notification list items
      if (data.length === 0) {
        var emptyItem = document.createElement("p");
        emptyItem.className = "text-center";
        emptyItem.innerText = "No new notifications";
        modalBody.appendChild(emptyItem);
        console.log("no new notifications");
      } else {
        data.forEach((notification) => {
          var notificationItem = document.createElement("div");
          notificationItem.className = "notification-item mb-3";
          notificationItem.innerHTML = `
                <strong>${notification.message}</strong>
                <br>
                <small class="text-muted">${new Date(
                  notification.timestamp
                ).toLocaleString()}</small>
                <button class="btn btn-sm btn-primary mark-as-read" data-notification-id="${
                  notification.id
                }">Mark as Read</button>
                <hr>`;
          modalBody.appendChild(notificationItem);
        });

        // Add event listener for mark as read buttons
        modalBody.querySelectorAll(".mark-as-read").forEach((button) => {
          button.addEventListener("click", markNotificationAsRead);
        });
      }

      // Show the modal
      $("#notificationModal").modal("show");
    })
    .catch((error) => console.error("Error fetching notifications:", error));
}

function markNotificationAsRead(event) {
  var notificationId = event.target.getAttribute("data-notification-id");
  console.log("Marking notification as read:", notificationId);

  // Send a request to mark notification as read
  fetch(`/Notification/MarkAsRead/${notificationId}`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({ notificationId: notificationId }),
  })
    .then((response) => {
      if (!response.ok) {
        throw new Error("Network response was not ok");
      }
      console.log("Notification marked as read successfully.");
      // Optionally, update UI or fetch notifications again
      fetchNotifications(); // Refresh the notifications list
    })
    .catch((error) =>
      console.error("Error marking notification as read:", error)
    );
}
