import React, { useContext } from 'react';
import { NavMenu } from './NavMenu';
import { AuthContext } from '../common/AuthContext';

export const Layout = ({ children }) => {
  const { account } = useContext(AuthContext);

  return (
    <div className="flex flex-col min-h-screen">
      {account && <NavMenu />}
      <main className="container mx-auto px-4 sm:px-6 lg:px-8 flex-grow">
        {children}
      </main>
    </div>
  );
};