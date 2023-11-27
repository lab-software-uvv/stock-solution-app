import React, { useEffect, useState } from "react";
import "./styles.css";
import getInitials from "../../../utils/functions/getInitials";
import { useNavigate, useLocation } from "react-router-dom";

//components
import TextInput from "../../ui/text.input";
import IconBtn from "../../ui/icon.btn";
import Popup from "../popup";
import RoundedBtn from "../../ui/rounded.btn";

//assets
import { Search, AlignToTop, Bell, TriangleDownFill, CircleMinus } from "akar-icons";

const Header = ({ user = { name: "", role: "" }, width, setShrinkState, shrinkState, setAuth }) => {
    const navigate = useNavigate();
    const location = useLocation();

    const [popupOn, setPopupOn] = useState(false);

    const [toggleProfile, setToggleProfile] = useState(false);

    const [pageTitle, setPageTitle] = useState("Painel inicial");

    const handleShrink = () => {
        setShrinkState(!shrinkState);
    };

    const handleLogout = () => {
        localStorage.clear();
        setAuth({ auth: false, token: null });
        navigate("/");
    };

    useEffect(() => {
        switch (location.pathname) {
            case "/dashboard":
                setPageTitle("Painel inicial");
                break;
            case "/sales":
                setPageTitle("Vendas");
                break;
            case "/products":
                setPageTitle("Produtos");
                break;
            case "/categories":
                setPageTitle("Categorias de produtos");
                break;
            case "/suppliers":
                setPageTitle("Fornecedores");
                break;
            case "/comercial-products":
                setPageTitle("Produto comercial");
                break;

            default:
                setPageTitle("Painel inicial");
                break;
        }
    }, [location]);

    return (
        <>
            {popupOn && (
                <Popup>
                    <div className="flex-center flex-column gap-10" style={{ padding: 20 }}>
                        <CircleMinus size={58} color="var(--color-red)" />
                        <p style={{ marginBottom: 10 }}>Desconectar do sistema?</p>
                        <RoundedBtn
                            onClick={() => {
                                handleLogout();
                            }}
                            title={"Desconectar"}
                        />
                        <RoundedBtn
                            onClick={() => {
                                setPopupOn(false);
                                setToggleProfile(false);
                            }}
                            title={"Voltar"}
                            hierarchy="secondary"
                        />
                    </div>
                </Popup>
            )}
            <div className="header-wrapper" style={{ width: width + "vw" }}>
                <div className="header-container header-container-l">
                    <IconBtn onClick={handleShrink} backgroundColor={"var(--color-black)"}>
                        <AlignToTop
                            size={16}
                            color="white"
                            style={{ transform: shrinkState ? "rotate(-90deg)" : "rotate(90deg)" }}
                        ></AlignToTop>
                    </IconBtn>
                    <p className="p-header ">{pageTitle}</p>
                </div>
                <div className="header-container header-container-r">
                    <div className="header-search-wrapper">
                        <TextInput
                            placeholder={"pesquisar..."}
                            paddingVertical={3.5}
                            icoRight={
                                <IconBtn backgroundColor="var(--color-black)">
                                    <Search size={16} color="white"></Search>
                                </IconBtn>
                            }
                        ></TextInput>
                    </div>
                    <div className="flex-center">
                        <Bell color="var(--color-grey)"></Bell>
                        <div className="header-status header-status-notification status-red" />
                    </div>
                    <div className="header-user-wrapper flex-row flex-center">
                        <div className="header-user-nameinitials-container flex-center">
                            <p className="p-subtitle p-white p-noselect">
                                {getInitials(user.name || "")}
                            </p>
                            <div className="header-status header-status-user status-green" />
                        </div>
                        <div>
                            <p className="header-user-name">{user.name || ""}</p>
                            <p className="p-subtitle p-grey">{user.role || ""}</p>
                        </div>
                        <div>
                            <IconBtn
                                backgroundColor={"white"}
                                onClick={() => {
                                    setToggleProfile(!toggleProfile);
                                }}
                            >
                                <TriangleDownFill size={16} color="var(--color-black)" />
                            </IconBtn>
                            {toggleProfile && (
                                <div className="header-profile-modal">
                                    <div
                                        className="flex-row flex-center gap-10 header-profile-modal-button button"
                                        onClick={() => {
                                            setPopupOn(true);
                                        }}
                                    >
                                        <CircleMinus size={16} color="var(--color-white)" />
                                        <p className="p-text p-white">Desconectar</p>
                                    </div>
                                </div>
                            )}
                        </div>
                    </div>
                </div>
            </div>
        </>
    );
};

export default Header;
