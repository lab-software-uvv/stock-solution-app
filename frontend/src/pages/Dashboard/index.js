import React, { useState } from "react";
import "./styles.css";
import getInitials from "../../utils/functions/getInitials";
import formatDate from "../../utils/functions/formatDate";

//components
import Navigator from "../../components/scenes/navigator";

//assets
import { TriangleAlert, Gear } from "akar-icons";
import IconBtn from "../../components/ui/icon.btn";

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

    return (
        <Navigator user={user} setAuth={setAuth}>
            <div className="dashboard-wrapper flex-center">
                <div className="">
                    <div className="dashboard-user-card flex-row">
                        <div className="dashboard-user-card-initials-wrapper flex-center">
                            <p className="p-white p-main">{getInitials(user.name)}</p>
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
