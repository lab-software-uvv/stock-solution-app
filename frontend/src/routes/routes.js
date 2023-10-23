import React, { useState, useEffect } from "react";
import { BrowserRouter } from "react-router-dom";

import PublicRoutes from "./public.routes";
import PrivateRoutes from "./private.routes";

const Router = () => {
    const [auth, setAuth] = useState(localStorage.getItem("auth") || { auth: false, token: null });
    const [user, setUser] = useState(
        localStorage.getItem("user") || { name: "Fabiano Rabelo", role: "Gestor" }
    );

    return (
        <BrowserRouter basename={process.env.PUBLIC_URL}>
            {auth.auth ? (
                <PrivateRoutes user={user} setAuth={setAuth} setUser={setUser}></PrivateRoutes>
            ) : (
                <PublicRoutes setAuth={setAuth} setUser={setUser}></PublicRoutes>
            )}
        </BrowserRouter>
    );
};

export default Router;
