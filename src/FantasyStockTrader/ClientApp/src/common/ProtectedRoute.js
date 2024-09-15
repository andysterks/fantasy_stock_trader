import React, { useContext, useEffect } from "react";
import { Outlet, useNavigate } from "react-router-dom";
import { AuthContext } from "./AuthContext";

const ProtectedRoute = () => {
  const { account } = useContext(AuthContext);
  const navigate = useNavigate();

  useEffect(() => {
    console.log("ProtectedRoute - account:", account);
    if (!account) {
      navigate("/login");
    }
  }, [account, navigate]);

  return account ? <Outlet /> : null;
};

export default ProtectedRoute;
