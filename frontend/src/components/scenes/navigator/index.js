import React, { useEffect, useState } from "react";
import "./styles.css";

//components
import Header from "../../ui/header";
import NavBar from "../../ui/navbar";

const Navigator = (props) => {
    const [asideWidth, setAsideWidth] = useState(6);
    const [shrinkState, setShrinkState] = useState(false);

    useEffect(() => {
        if (shrinkState) {
            setAsideWidth(17.5);
        } else {
            setAsideWidth(6);
        }
    }, [shrinkState]);

    return (
        <div className="global-wrapper container flex-row">
            <div className="global-aside" style={{ width: asideWidth + "vw" }}>
                <NavBar shrinkState={shrinkState} width={asideWidth} />
            </div>
            <div className="global-content-wrapper" style={{ width: 100 - asideWidth + "vw" }}>
                <Header
                    width={100 - asideWidth}
                    user={props.user}
                    setShrinkState={setShrinkState}
                    shrinkState={shrinkState}
                />
                <div style={{ paddingTop: 65, height: "100vh", boxSizing: "border-box" }}>
                    {props.children}
                </div>
            </div>
        </div>
    );
};

export default Navigator;
