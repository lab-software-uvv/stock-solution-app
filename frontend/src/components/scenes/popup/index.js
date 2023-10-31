import React from "react";
import "./styles.css";

const Popup = (props) => {
    return (
        <div className="popup-modal-wrapper flex-center">
            <div className="popup-modal-container flex-center">
                <div className="popup-modal-border">{props.children}</div>
            </div>
        </div>
    );
};

export default Popup;
