import React, { useContext } from "react";
import { Outlet, Navigate } from "react-router-dom";
import { AuthContext } from "./AuthContext";

const ProtectedRoute = () => {
  const { account, setAccount } = useContext(AuthContext);

  return account ? <Outlet /> : <Navigate to="/login" />;
};

export default ProtectedRoute;
