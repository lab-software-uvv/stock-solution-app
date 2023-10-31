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
    step,
    required,
    disabled,
    style,
    name,
    textIco,
    maxLength,
    pattern,
    onChange = () => {},
}) => {
    return (
        <div
            className="inpt-wrapper"
            style={{
                paddingTop: paddingVertical,
                paddingBottom: paddingVertical,
                backgroundColor: disabled === true && "var(--color-grey)",
                ...style,
            }}
        >
            {icoLeft ? (
                <div className={textIco ? "inpt-text-ico" : "inpt-ico"}>{icoLeft}</div>
            ) : (
                <div style={{ width: "5%" }}></div>
            )}
            <input
                maxLength={maxLength}
                disabled={disabled}
                name={name}
                pattern={pattern}
                required={required}
                min={min}
                step={step}
                className="inpt-text"
                placeholder={placeholder}
                type={type}
                value={value}
                onChange={(e) => {
                    onChange();
                    setValue(e.target.value);
                }}
            />
            {icoRight && <div className="inpt-ico">{icoRight}</div>}
        </div>
    );
};

export default TextInput;
