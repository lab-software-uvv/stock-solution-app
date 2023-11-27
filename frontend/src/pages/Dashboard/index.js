import React, { useEffect, useState } from "react";
import "./styles.css";
import getInitials from "../../utils/functions/getInitials";
import formatDate from "../../utils/functions/formatDate";

//components
import Navigator from "../../components/scenes/navigator";

//assets
import { TriangleAlert, Gear } from "akar-icons";
import IconBtn from "../../components/ui/icon.btn";


import { startConnection, stopConnection, onSendProductsNearExpiration } from "../../services/HubRequests";
import { hubConnection } from "../../services/HubRequests";
import { HubConnectionBuilder, HubConnectionState } from "@microsoft/signalr";

const Dashboard = ({ user, setAuth }) => {
    const [currentTool, setCurrentTool] = useState("Próximos vencimentos");
    const tools = [
        {
            name: "Próximos vencimentos",
            icon: <TriangleAlert strokeWidth={2} size={20} color="white" />,
        },
        {},
        {},
    ];

    useEffect(() => {
        startConnection();
    }, []);
    
    
    useEffect(() => {
        onSendProductsNearExpiration((message) => {
          console.log("Received Message:", message);
          // Handle the received message as needed
        });
    }, []);

    useEffect(() => {
        handleSendMessage("7")
    }, []);

    const sendMessage = (message) => {
        if (hubConnection.state === HubConnectionState.Connected) {
          hubConnection.invoke("SendProductsNearExpiration", message)
            .catch((err) => console.error("Error sending message:", err))
            .then((response) => console.log(response));
        } else {
          console.error("Connection is not in the 'Connected' state.");
        }
    };
    
    const handleSendMessage = (daysAfterToday) => {
        if (hubConnection.state === HubConnectionState.Connected) {
            sendMessage(daysAfterToday);
        } else {
            console.error("Connection is not in the 'Connected' state.");
        }
    };    

    return (
        <Navigator user={user} setAuth={setAuth}>
            <div className="dashboard-wrapper flex-center">
                <div className="">
                    <div className="dashboard-user-card flex-row">
                        <div className="dashboard-user-card-initials-wrapper flex-center">
                            <p className="p-white p-main">{getInitials(user?.name)}</p>
                        </div>
                        <div>
                            <p className="p-white p-main">
                                {formatDate(new Date()).greeting + "!"}
                            </p>
                            <p className="p-white p-subtitle">
                                Hoje é {formatDate(new Date()).weekDay},{" "}
                                {formatDate(new Date()).data}
                            </p>
                        </div>
                        <div className="flex-row" style={{marginLeft: 150, gap: 20}}>
                            {tools.map((element) => {
                                return (
                                    <div className="dasboard-user-card-tools flex-center button" key={"tool-" + element.name}>
                                        <div>{element.icon}</div>
                                        <p className="p-white p-subtitle p-bold">{element.name}</p>
                                    </div>
                                );
                            })}
                        </div>
                        <IconBtn style={{marginTop: "-5%"}} backgroundColor="var(--color-primary)"><Gear color="white" size={18}/></IconBtn>
                    </div>
                </div>
                <div>
                    <div></div>
                    <div></div>
                </div>
            </div>
        </Navigator>
    );
};

export default Dashboard;
