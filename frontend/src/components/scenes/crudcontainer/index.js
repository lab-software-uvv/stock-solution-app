import React, { useState } from "react";
import "./styles.css";
import IconBtn from "../../ui/icon.btn";

const CrudContainer = ({ title = "", form = <></>, list = <></>, icon }) => {
    const [currentPage, setCurrentPage] = useState("Cadastrar");

    return (
        <div>
            <div className="crudcontainer-content-wrapper">
                <div
                    className="flex-row"
                    style={{ gap: 10, alignItems: "center", paddingBottom: 5 }}
                >
                    <IconBtn backgroundColor={"transparent"}>{icon}</IconBtn>
                    <p>{currentPage + " " + (title.toLowerCase() || "")}</p>
                </div>
                <div className="crudcontainer-content-container">
                    {currentPage === "Listar" && list}
                    {currentPage === "Cadastrar" && form}
                </div>
            </div>
            <div className="crudcontainer-btn-wrapper flex-row">
                <div
                    className={
                        "button crudcontainer-btn-" + (currentPage === "Lista" ? "on" : "off")
                    }
                    onClick={() => setCurrentPage("Listar")}
                >
                    <p className="p-text">Listar {title.toLowerCase()}</p>
                </div>
                <div
                    className={
                        "button crudcontainer-btn-" + (currentPage === "Cadastrar" ? "on" : "off")
                    }
                    onClick={() => setCurrentPage("Cadastrar")}
                >
                    <p className="p-text">Cadastrar {title.toLowerCase()}</p>
                </div>
            </div>
        </div>
    );
};

export default CrudContainer;
