import React, { useContext, useEffect } from "react";
import { Outlet, useNavigate } from "react-router-dom";
import { AuthContext } from "./AuthContext";

const ForwardingRoute = () => {
  const { account } = useContext(AuthContext);
  const navigate = useNavigate();

  useEffect(() => {
    if (account) {
      navigate("/dashboard");
    }
  }, [account, navigate]);

  return account ? null : <Outlet />;
};

export default ForwardingRoute;