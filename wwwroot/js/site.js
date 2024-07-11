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

document.addEventListener("DOMContentLoaded", function () {
  // Fetch unread notifications on page load to update the badge
  fetchUnreadNotificationCount();

  // Fetch unread notifications periodically (e.g., every 60 seconds)
  setInterval(fetchUnreadNotificationCount, 3000); // 10,000 milliseconds = 10 seconds
});

function fetchUnreadNotificationCount() {
  console.log("Fetching unread notification count...");
  fetch("/Notification/GetUnreadNotifications")
    .then((response) => {
      if (!response.ok) {
        throw new Error("Network response was not ok " + response.statusText);
      }
      console.log("Response received");
      return response.json();
    })
    .then((data) => {
      console.log("Unread Notifications:", data);
      var notificationBadge = document.getElementById("notificationBadge");

      // Update notification badge count
      var unreadCount = data.length;
      if (unreadCount > 0) {
        notificationBadge.innerText = unreadCount;
        notificationBadge.style.display = "inline-block";
      } else {
        notificationBadge.style.display = "none";
      }
    })
    .catch((error) =>
      console.error("Error fetching unread notification count:", error)
    );
}

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
      // Refresh the notifications list
      fetchUnreadNotificationCount();
      fetchNotifications();
    })
    .catch((error) =>
      console.error("Error marking notification as read:", error)
    );
}
function initMap() {
  // Initialize the map with a default location
  //   var map = new google.maps.Map(document.getElementById("map"), {
  //     center: { lat: -34.397, lng: 150.644 },
  //     zoom: 8,
  //   });
}

function loadMap(city) {
  // Use the Google Maps Geocoding API to get the coordinates of the city
  var geocoder = new google.maps.Geocoder();
  geocoder.geocode({ address: city }, function (results, status) {
    if (status === "OK") {
      var map = new google.maps.Map(document.getElementById("map"), {
        zoom: 10,
        center: results[0].geometry.location,
      });

      var marker = new google.maps.marker.AdvancedMarkerElement({
        position: results[0].geometry.location,
        map: map,
      });
    } else {
      alert("Geocode was not successful for the following reason: " + status);
    }
  });
}

$(document).ready(function () {
  $(".buy-btn").click(function (e) {
    e.preventDefault();
    var button = $(this);
    var propertyId = button.data("property-id");

    // Perform AJAX request to mark property as sold
    $.post("/PropertyDetails/MarkAsSold", { id: propertyId })
      .done(function (data) {
        if (data.success) {
          // Update UI to show the Sold tag
          button
            .removeClass("btn-success")
            .addClass("btn-secondary")
            .text("Sold")
            .removeAttr("href")
            .removeAttr("data-property-id")
            .addClass("disabled");
        } else {
          alert("Failed to mark property as sold.");
        }
      })
      .fail(function () {
        alert("Failed to mark property as sold.");
      });
  });

  $(".map-btn").click(function () {
    var city = $(this).data("city");
    // Open the map modal
    $("#mapModal").modal("show");
    // Load the map centered on the city
    loadMap(city);
  });
});
