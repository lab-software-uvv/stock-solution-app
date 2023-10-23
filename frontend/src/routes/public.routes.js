import React from "react";
import { Route, Routes } from "react-router-dom";

import Login from "../pages/Login/index";

const PublicRoutes = ({setAuth, setUser}) => {
    return (
        <Routes>
            <Route path="/" element={<Login setAuth={setAuth} setUser={setUser} />} />
            <Route path="/login" element={<Login setAuth={setAuth} setUser={setUser} />} />
        </Routes>
    );
};

export default PublicRoutes;
