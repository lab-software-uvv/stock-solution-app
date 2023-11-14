import React, { useEffect } from "react";
import { Route, Routes, useLocation, useNavigate } from "react-router-dom";

import Dashboard from "../pages/Dashboard/index";
import Products from "../pages/Products/index";
import Categories from "../pages/Categories/index";

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
            <Route path="/" element={<Dashboard user={user} setAuth={setAuth}/>} />
            <Route path="/dashboard" element={<Dashboard user={user} setAuth={setAuth}/>} />
            <Route path="/products" element={<Products user={user} setAuth={setAuth}/>} />
            <Route path="/categories" element={<Categories user={user} setAuth={setAuth}/>} />
        </Routes>
    );
};

export default PrivateRoutes;
