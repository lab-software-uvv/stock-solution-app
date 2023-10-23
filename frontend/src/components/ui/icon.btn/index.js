import React from "react";
import "./styles.css";

const IconBtn = (props) => {
    return (
        <button
            onClick={props.onClick}
            type={props.type || "button"}
            className={"icobtn-wrapper " + props.className}
            style={{
                backgroundColor: props.backgroundColor,
                borderColor: props.backgroundColor,
                borderWidth: props.backgroundColor && 0,
                ...props.style
            }}
        >
            {props.children}
        </button>
    );
};

export default IconBtn;
