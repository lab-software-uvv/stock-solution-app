import React from "react";
import "./styles.css";

const TextBtn = ({
    title,
    type = "button",
    color = "white",
    size = 12,
    align = "left",
    onClick,
}) => {
    return (
        <div style={{ display: "flex", width: "100%", justifyContent: align }}>
            <button
                onClick={onClick}
                className="btn-text"
                type={type}
                style={{
                    color: color,
                    fontSize: size,
                }}
            >
                {title}
            </button>
        </div>
    );
};

export default TextBtn;
