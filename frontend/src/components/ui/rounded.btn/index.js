import React from "react";
import "./styles.css";

const RoundedBtn = ({ title, type = "button", hierarchy = "primary", onClick }) => {
    return (
        <button onClick={onClick} className={"btn-wrapper-" + hierarchy} type={type}>
            {title}
        </button>
    );
};

export default RoundedBtn;
