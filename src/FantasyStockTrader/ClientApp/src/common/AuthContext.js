import React, { createContext, useState } from "react";

// Create the context
export const AuthContext = createContext("");

// Create a provider component
export const AuthProvider = ({ children }) => {
  const [account, setAccount] = useState(
    JSON.parse(window.localStorage.getItem("account"))
  );

  const updateAccount = (value) => {
    setAccount(value);
    window.localStorage.setItem("account", JSON.stringify(value));
  };

  return (
    <AuthContext.Provider value={{ account, setAccount: updateAccount }}>
      {children}
    </AuthContext.Provider>
  );
};
