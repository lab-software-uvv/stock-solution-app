import React, { useState, useEffect } from "react";
import { useNavigate, useLocation } from "react-router-dom";
import "./styles.css";

//assets
import LogoFull from "../../../assets/logo/LOGO.png";
import LogoMini from "../../../assets/logo/LOGO@.png";

import { Dashboard, ShippingBoxV1, ShoppingBag, PeopleMultiple, Tag, Utensils, Money, } from "akar-icons";

//components
import ShrinkBtn from "../../ui/shrink.btn";

const NavBar = ({ width = 10, shrinkState }) => {
    const navigate = useNavigate();
    const location = useLocation();

    const [logo, setLogo] = useState(LogoMini);

    useEffect(() => {
        if (shrinkState) {
            setLogo(LogoFull);
        } else {
            setLogo(LogoMini);
        }
    }, [shrinkState]);

    const buttons = [
        {
            name: "Painel inicial",
            path: "/dashboard",
            icon: <Dashboard strokeWidth={2} size={20} color="white" />,
            action: () => navigate("/dashboard"),
        },
        {
            name: "Vendas",
            path: "/sales",
            icon: <Money strokeWidth={2} size={20} color="white" />,
            action: () => navigate("/sales"),
        },
        {
            name: "Produtos",
            path: "/products",
            icon: <ShippingBoxV1 strokeWidth={2} size={20} color="white" />,
            action: () => navigate("/products"),
        },
        {
            name: "Categorias",
            path: "/categories",
            icon: <Tag strokeWidth={2} size={20} color="white" />,
            action: () => navigate("/categories"),
        },
        {
            name: "Produtos Comerciais",
            path: "/comercial-products",
            icon: <Utensils strokeWidth={2} size={20} color="white" />,
            action: () => navigate("/comercial-products"),
        },
        {
            name: "Fornecedores",
            path: "/suppliers",
            icon: <ShoppingBag strokeWidth={2} size={20} color="white" />,
            action: () => navigate("/suppliers"),
        },
        {
            name: "Funcionários",
            path: "/employees",
            icon: <PeopleMultiple strokeWidth={2} size={20} color="white" />,
            action: () => navigate("/employees"),
        },
    ];

    return (
        <div className="navbar-wrapper" style={{ width: width + "vw" }}>
            <img
                src={logo}
                alt="stocksolution-logo"
                width={logo === LogoMini ? 45 : 225}
                style={{ marginBottom: 10 }}
            ></img>
            {buttons.map((element, i) => {
                return (
                    <ShrinkBtn
                        action={element.action}
                        key={element.name}
                        text={element.name}
                        backgroundColor={
                            (location.pathname) === element.path
                                ? "var(--color-primary)"
                                : "transparent"
                        }
                        shrink={shrinkState}
                    >
                        {element.icon}
                    </ShrinkBtn>
                );
            })}
        </div>
    );
};

export default NavBar;
