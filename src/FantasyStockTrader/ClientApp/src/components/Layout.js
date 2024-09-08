import React, { useContext } from 'react';
import { Container } from 'reactstrap';
import { NavMenu } from './NavMenu';
import { AuthContext } from '../common/AuthContext';

export const Layout = ({ children }) => {
  const { account } = useContext(AuthContext);

  return (
    <div>
      {account && <NavMenu />}
      <Container tag="main">
        {children}
      </Container>
    </div>
  );
};