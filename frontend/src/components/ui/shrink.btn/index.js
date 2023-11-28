import React, { useState } from "react";
import "./styles.css";

const ShrinkBtn = (props) => {
    const [mouseOn, setMouseOn] = useState(false);

    return (
        <>
            {!props.shrink ? (
                <button
                    onClick={props.action}
                    type={props.type || "button"}
                    className={"shrinkbtn-wrapper flex-center flex-row " + props.className}
                    onMouseEnter={() => setMouseOn(true)}
                    onMouseLeave={() => setMouseOn(false)}
                    style={{
                        backgroundColor: mouseOn
                            ? props.mouseOnBg? props.mouseOnBg : "rgba(255, 255, 255, 0.35)"
                            : props.backgroundColor,
                        borderColor: mouseOn ? "rgba(255, 255, 255, 0.35)" : props.backgroundColor,
                        borderWidth: props.backgroundColor && 0,
                        width: props.width && props.width
                    }}
                >
                    {props.children}
                </button>
            ) : (
                <button
                    className="shrinkedbtn-wrapper flex-row"
                    onClick={props.action}
                    type={props.type || "button"}
                    onMouseEnter={() => setMouseOn(true)}
                    onMouseLeave={() => setMouseOn(false)}
                    style={{
                        backgroundColor: mouseOn
                            ? props.mouseOnBg? props.mouseOnBg : "rgba(255, 255, 255, 0.35)"
                            : props.backgroundColor,
                        borderColor: mouseOn ? "rgba(255, 255, 255, 0.35)" : props.backgroundColor,
                        borderWidth: props.backgroundColor && 0,
                        width: props.width && props.width
                    }}
                >
                    <div className={"shrinkbtn-wrapper flex-center flex-row " + props.className} style={{backgroundColor: "transparent"}}>
                        {props.children}
                    </div>
                    <p className="p-white">{props.text}</p>
                </button>
            )}
        </>
    );
};

export default ShrinkBtn;
