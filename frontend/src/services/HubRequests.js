// signalRService.js
import * as signalR from "@microsoft/signalr";

const hubConnection = new signalR.HubConnectionBuilder()
  .withUrl("https://localhost:7156/notification-hub", {
    withCredentials: true
  })
  .configureLogging(signalR.LogLevel.Information)
  .build();

const startConnection = () => {
  hubConnection
    .start()
    .then(() => console.log("SignalR Connected"))
    .catch((err) => console.error("Error connecting to SignalR:", err));
};

const stopConnection = () => {
  hubConnection.stop();
};

const onSendProductsNearExpiration = (callback) => {
  hubConnection.on("SendProductsNearExpiration", (message) => {
    callback(message);
  });
};

export { hubConnection, startConnection, stopConnection, onSendProductsNearExpiration };
