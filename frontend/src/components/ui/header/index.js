import React, { useEffect } from "react";
import "./styles.css";
import getInitials from "../../../utils/functions/getInitials";

//components
import TextInput from "../text.input";
import IconBtn from "../icon.btn";

//assets
import { Search, AlignToTop, Bell, TriangleDownFill } from "akar-icons";

const Header = ({ user = { name: "", role: "" }, width, setShrinkState, shrinkState }) => {
    const handleShrink = () => {
        setShrinkState(!shrinkState);
    };

    useEffect(() => {
      console.log(user)
    }, [])
    

    return (
        <div className="header-wrapper" style={{ width: width + "vw" }}>
            <div className="header-container header-container-l">
                <IconBtn onClick={handleShrink} backgroundColor={"var(--color-black)"}>
                    <AlignToTop
                        size={16}
                        color="white"
                        style={{ transform: shrinkState ? "rotate(-90deg)"  : "rotate(90deg)"}}
                    ></AlignToTop>
                </IconBtn>
                <p className="p-header ">Painel inicial</p>
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
                        <IconBtn backgroundColor={"white"}>
                            <TriangleDownFill size={16} color="var(--color-black)" />
                        </IconBtn>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default Header;
