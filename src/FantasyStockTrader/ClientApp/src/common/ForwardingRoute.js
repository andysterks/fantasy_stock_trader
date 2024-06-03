import React, { useContext } from "react";
import { Outlet, Navigate } from "react-router-dom";
import { AuthContext } from "./AuthContext";

const ForwardingRoute = () => {
  const { account, setAccount } = useContext(AuthContext);

  return account ? <Navigate to="/dashboard" /> : <Outlet />;
};

export default ForwardingRoute;
