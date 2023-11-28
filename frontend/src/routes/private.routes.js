import React, { useEffect } from "react";
import { Route, Routes, useLocation, useNavigate } from "react-router-dom";

import Dashboard from "../pages/Dashboard/index";
import Products from "../pages/Products/index";
import Categories from "../pages/Categories/index";
import Suppliers from "../pages/Suppliers";
import ComercialProducts from "../pages/ComercialProducts";
import CPProducts from "../pages/CPProducts";
import Sales from "../pages/Sales";
import SalesProducts from "../pages/SalesProducts";

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
            <Route path="/sales" element={<Sales user={user} setAuth={setAuth}/>} />
            <Route path="/sales/:id/products" element={<SalesProducts user={user} setAuth={setAuth}/>} />
            <Route path="/products" element={<Products user={user} setAuth={setAuth}/>} />
            <Route path="/suppliers" element={<Suppliers user={user} setAuth={setAuth}/>} />
            <Route path="/categories" element={<Categories user={user} setAuth={setAuth}/>} />
            <Route path="/comercial-products" element={<ComercialProducts user={user} setAuth={setAuth}/>} />
            <Route path="/comercial-products/:id/products" element={<CPProducts user={user} setAuth={setAuth}/>} />
        </Routes>
    );
};

export default PrivateRoutes;
