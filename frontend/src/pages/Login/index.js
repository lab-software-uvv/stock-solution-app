import React, { useEffect, useState } from "react";
import { useSpring } from "@react-spring/web";
import "./styles.css";

//components
import { Toaster } from "react-hot-toast";

import LoginBox from "../../components/ui/login.box";
import SignIn from "../../components/scenes/signin";
import SignUp from "../../components/scenes/signup";

//assets
import BgLogin from "../../assets/background/bg-login.png";
import SSLogo from "../../assets/logo/LOGO.png";

const Login = ({ setAuth, setUser }) => {
    const [currentPage, setCurrentPage] = useState(0);

    const url = new URL(window.location.href);
    const params = new URLSearchParams(url.search);

    useEffect(() => {
        if (params.get("invite") || params.get("email")) {
            handleMove();
            setCurrentPage(1);
        }
    }, []);

    const pages = [
        {
            name: "signin",
            component: (
                <SignIn
                    setAuth={setAuth}
                    setUser={setUser}
                    setCurrentPage={setCurrentPage}
                    handleMove={() => handleMove()}
                ></SignIn>
            ),
        },
        {
            name: "signup",
            component: (
                <SignUp setAuth={setAuth} setUser={setUser} setCurrentPage={setCurrentPage} handleMove={() => handleMove()}></SignUp>
            ),
        },
    ];

    const [moveBox, boxAnim] = useSpring(() => ({
        from: { x: "111%" },
        config: {
            mass: 1,
            friction: 100,
            tension: 0,
        },
    }));

    const handleMove = () => {
        if (currentPage === 0) {
            boxAnim.start({
                from: {
                    x: "111%",
                },
                to: {
                    x: "0%",
                },
            });
        } else {
            boxAnim.start({
                from: {
                    x: "0%",
                },
                to: {
                    x: "111%",
                },
            });
        }
    };

    return (
        <div className="container login-wrapper">
            <div>
                <Toaster />
            </div>
            <img className="login-img-bg" src={BgLogin} alt={"background-restaurant"}></img>
            <div
                className="login-logo-wrapper"
                style={currentPage === 0 ? { left: 0 } : { right: 0 }}
            >
                <img className="login-logo" src={SSLogo} alt={"stocksolution-logo"}></img>
            </div>
            <LoginBox style={moveBox}>{pages[currentPage].component}</LoginBox>
        </div>
    );
};

export default Login;
