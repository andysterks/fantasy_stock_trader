import React, { Component } from 'react';
import { Route, Routes, useNavigate } from 'react-router-dom';
import AppRoutes from './AppRoutes';
import { Layout } from './components/Layout';
import './custom.css';
import globalRouter from './common/GlobalRouter';

const App = () => {
  const navigate = useNavigate();
  globalRouter.navigate = navigate;

  return (
    <Layout>
      <Routes>
        {AppRoutes.map((route, index) => {
          const { element, ...rest } = route;
          return <Route key={index} {...rest} element={element} />;
        })}
      </Routes>
    </Layout>
  );
}

export default App;