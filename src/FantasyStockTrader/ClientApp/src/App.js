import React from "react";
import { Route, Routes, useNavigate } from "react-router-dom";
import AppRoutes from "./AppRoutes";
import { Layout } from "./components/Layout";
import "./custom.css";
import globalRouter from "./common/GlobalRouter";
import ProtectedRoutes from "./ProtectedRoutes";
import ProtectedRoute from "./common/ProtectedRoute";
import { AuthProvider } from "./common/AuthContext";
import ForwardingRoute from "./common/ForwardingRoute";

const App = () => {
  const navigate = useNavigate();
  globalRouter.navigate = navigate;

  return (
    <AuthProvider>
      <Layout>
        <Routes>
          {AppRoutes.map((route, index) => {
            const { element, ...rest } = route;
            return route.index ? (
              <Route exact {...rest} element={element} />
            ) : (
              <Route key={index} {...rest} element={<ForwardingRoute />}>
                <Route exact {...rest} element={element} />
              </Route>
            );
          })}
          {ProtectedRoutes.map((route, index) => {
            const { element, ...rest } = route;
            return (
              <Route key={index} exact {...rest} element={<ProtectedRoute />}>
                <Route exact {...rest} element={element} />
              </Route>
            );
          })}
        </Routes>
      </Layout>
    </AuthProvider>
  );
};

export default App;
