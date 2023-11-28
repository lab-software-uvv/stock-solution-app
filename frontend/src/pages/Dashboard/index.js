import React, { useEffect, useState } from "react";
import "./styles.css";
import getInitials from "../../utils/functions/getInitials";
import formatDate from "../../utils/functions/formatDate";

import { useNavigate } from "react-router-dom";

//components
import Navigator from "../../components/scenes/navigator";

//assets
import { TriangleAlert, Gear, File, ShippingBoxV1 } from "akar-icons";
import IconBtn from "../../components/ui/icon.btn";

import {
    startConnection,
    stopConnection,
    onSendProductsNearExpiration,
} from "../../services/HubRequests";
import { hubConnection } from "../../services/HubRequests";
import { HubConnectionBuilder, HubConnectionState } from "@microsoft/signalr";
import Requests from "../../services/requests";
import toast from "react-hot-toast";
import { DataGrid } from "@mui/x-data-grid";

const columns = [
    { field: "id", headerName: "id", width: 50 },
    { field: "name", headerName: "Nome", width: 100 },
    { field: "code", headerName: "Código", width: 125 },
    { field: "quantity", headerName: "Quantidade", width: 100 },
    { field: "supplierCode", headerName: "Fornecedor", width: 100 },
    { field: "price", headerName: "Preço unitário", width: 75 },
    { field: "categoryName", headerName: "Categoria", width: 100 },
    { field: "aquisitionDate", headerName: "Data de aquisição", width: 100 },
    { field: "expirationDate", headerName: "Data de vencimento", width: 100 },
    { field: "description", headerName: "Descrição", width: 100 },
];

const Dashboard = ({ user, setAuth }) => {
    const navigate = useNavigate();

    const [currentTool, setCurrentTool] = useState("Próximos vencimentos");

    const [productList, setProductList] = useState([]);

    const tools = [
        {
            name: "Próximos vencimentos",
            icon: <TriangleAlert strokeWidth={2} size={20} color="white" />,
        },
        {},
        {},
    ];

    useEffect(() => {
        loadContent();
        startConnection();
    }, []);

    const loadContent = async () => {
        const req = async () => {
            await Requests.get(
                `/products` /*, {
                headers: {
                    authentication: `bearer ${localStorage.getItem("token")}`,
                },
            }*/
            )
                .then((res) => {
                    setProductList(res.data);
                    console.log(res);
                })
                .catch((err) => {
                    console.log(err);
                    throw Error;
                });
        };

        toast.promise(req(), {
            loading: "Carregando...",
            success: "Produtos carregados!",
            error: "Erro, tente novamente mais tarde",
        });
    };

    useEffect(() => {
        onSendProductsNearExpiration((message) => {
            console.log("Received Message:", message);
            // Handle the received message as needed
        });
    }, []);

    useEffect(() => {
        handleSendMessage("7");
    }, []);

    const sendMessage = (message) => {
        if (hubConnection.state === HubConnectionState.Connected) {
            hubConnection
                .invoke("SendProductsNearExpiration", message)
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
            <div className="dashboard-wrapper flex-center gap-30">
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
                        <div className="flex-row" style={{ marginLeft: 150, gap: 20 }}>
                            {tools.map((element) => {
                                return (
                                    <div
                                        className="dasboard-user-card-tools flex-center button"
                                        key={"tool-" + element.name}
                                    >
                                        <div>{element.icon}</div>
                                        <p className="p-white p-subtitle p-bold">{element.name}</p>
                                    </div>
                                );
                            })}
                        </div>
                        <IconBtn
                            style={{ marginTop: "-5%" }}
                            backgroundColor="var(--color-primary)"
                        >
                            <Gear color="white" size={18} />
                        </IconBtn>
                    </div>
                </div>
                <div className="flex-row flex-center" style={{ width: "70vw" }}>
                    <div style={{ flex: 2, marginTop: -20 }} className="gap-20 flex-column">
                        <p className="p-center p-bold" style={{ marginLeft: -40 }}>
                            Atalhos
                        </p>
                        <div className="flex-row gap-20">
                            <div
                                className="button dashboard-shortcuts-button flex-center"
                                onClick={() => {
                                    navigate("/salesreport");
                                }}
                            >
                                <File color="var(--color-primary)" size={50} />
                                <p className="p-bold" style={{ marginTop: 10 }}>
                                    RELATÓRIO
                                </p>
                                <p className="p-subtitle p-center p-grey">
                                    Download de relatório de vendas de um período.
                                </p>
                            </div>
                            <div className="button dashboard-shortcuts-button flex-center"></div>
                        </div>
                        <div className="flex-row gap-20">
                            <div className="button dashboard-shortcuts-button flex-center"></div>
                            <div className="button dashboard-shortcuts-button flex-center"></div>
                        </div>
                    </div>
                    <div
                        style={{ flex: 3, backgroundColor: "white", borderRadius: 15 }}
                        className="flex-center flex-column"
                    >
                        <div
                            className="flex-row gap-10"
                            style={{
                                paddingTop: 20,
                                paddingBottom: 20,
                                marginLeft: 15,
                                marginRight: "auto",
                            }}
                        >
                            <ShippingBoxV1 color="var(--color-darkgrey)" />{" "}
                            <p style={{ color: "var(--color-darkgrey)" }}>
                                Produtos que vencem esse mês
                            </p>
                        </div>
                        <div style={{ height: "375px", width: "650px" }}>
                            <DataGrid
                                rows={productList.filter(
                                    (e) =>
                                        parseInt(e.expirationDate.split("-")[1]) ===
                                        new Date().getMonth() + 1
                                )}
                                columns={columns}
                                onRowDoubleClick={(e) => navigate("/products")}
                            />
                        </div>
                    </div>
                </div>
            </div>
        </Navigator>
    );
};

export default Dashboard;
