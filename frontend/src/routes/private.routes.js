import React, { useEffect } from "react";
import { Route, Routes, useLocation, useNavigate } from "react-router-dom";

import Dashboard from "../pages/Dashboard/index";
import Products from "../pages/Products/index";

const PrivateRoutes = ({ setAuth, user, setUser }) => {
    const navigate = useNavigate();
    const location = useLocation();

    useEffect(() => {
        if (location.pathname === "/") {
            navigate("/dashboard");
        }
    }, [location]);

    return (
        <Routes>
            <Route path="/" element={<Dashboard user={user} />} />
            <Route path="/dashboard" element={<Dashboard user={user} />} />
            <Route path="/products" element={<Products user={user} />} />
        </Routes>
    );
};

export default PrivateRoutes;
