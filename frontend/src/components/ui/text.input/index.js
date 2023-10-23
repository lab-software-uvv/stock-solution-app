import React from "react";
import "./styles.css";

const TextInput = ({
    type = "text",
    value,
    setValue = () => {},
    icoLeft,
    icoRight,
    placeholder,
    paddingVertical,
    min,
    step
}) => {
    return (
        <div
            className="inpt-wrapper"
            style={{ paddingTop: paddingVertical, paddingBottom: paddingVertical }}
        >
            {icoLeft ? (
                <div className="inpt-ico">{icoLeft}</div>
            ) : (
                <div style={{ width: "5%" }}></div>
            )}
            <input
                min={min}
                step={step}
                className="inpt-text"
                placeholder={placeholder}
                type={type}
                value={value}
                onChange={(e) => setValue(e.target.value)}
            />
            {icoRight && <div className="inpt-ico">{icoRight}</div>}
        </div>
    );
};

export default TextInput;
