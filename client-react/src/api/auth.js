// src/api/auth.js
export const getUserRole = () => {
    const user = JSON.parse(localStorage.getItem("user"));
    return user?.role;
  };
  
  export const isAdmin = () => getUserRole() === "Admin";
  