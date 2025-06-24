import React, { createContext, useContext, useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

const AuthContext = createContext();

export function AuthProvider({ children }) {
  const [token, setToken] = useState(localStorage.getItem('token') || '');
  const navigate = useNavigate(); // ✅ move inside component

  useEffect(() => {
    localStorage.setItem('token', token);
  }, [token]);

  const logout = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("roles");
    navigate("/login"); 
  };

  return (
    <AuthContext.Provider value={{ token, setToken, logout }}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  return useContext(AuthContext);
}
