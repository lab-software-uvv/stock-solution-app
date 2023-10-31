import React from "react";
import { animated } from "@react-spring/web";
import "./styles.css";

const LoginBox = (props) => {
    return (
        <animated.div className={"login-box-wrapper " + props.className} style={props.style}>
            <div className="login-box-border">
                <div className="login-box-content">{props.children}</div>
            </div>
        </animated.div>
    );
};

export default LoginBox;
