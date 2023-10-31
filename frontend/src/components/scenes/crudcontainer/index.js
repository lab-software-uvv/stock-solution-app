import React, { useEffect, useState } from "react";
import "./styles.css";
import IconBtn from "../../ui/icon.btn";

const CrudContainer = ({ title = "", form = <></>, list = <></>, icon, changePage }) => {
    const [currentPage, setCurrentPage] = useState(1);

    const handleGetName = () => {
        switch (currentPage) {
            case 0:
                return "Listar";
            case 1:
                return "Cadastrar";

            default:
                return "Cadastrar";
        }
    };

    useEffect(() => {
        if (currentPage === 0) {
            setCurrentPage(1);
        } else {
            setCurrentPage(0);
        }
    }, [changePage]);

    return (
        <div>
            <div className="crudcontainer-content-wrapper">
                <div
                    className="flex-row"
                    style={{ gap: 10, alignItems: "center", paddingBottom: 5 }}
                >
                    <IconBtn backgroundColor={"transparent"}>{icon}</IconBtn>
                    <p>{handleGetName(currentPage) + " " + (title.toLowerCase() || "")}</p>
                </div>
                <div className="crudcontainer-content-container">
                    {currentPage === 0 && list}
                    {currentPage === 1 && form}
                </div>
            </div>
            <div className="crudcontainer-btn-wrapper flex-row">
                <div
                    className={"button crudcontainer-btn-" + (currentPage === 0 ? "on" : "off")}
                    onClick={() => setCurrentPage(0)}
                >
                    <p className="p-text">Listar {title.toLowerCase()}</p>
                </div>
                <div
                    className={"button crudcontainer-btn-" + (currentPage === 1 ? "on" : "off")}
                    onClick={() => setCurrentPage(1)}
                >
                    <p className="p-text">Cadastrar {title.toLowerCase()}</p>
                </div>
            </div>
        </div>
    );
};

export default CrudContainer;
