// src/api/axiosConfig.js
import axios from "axios";

const instance = axios.create({
  baseURL: "http://localhost:5185/api", // Your .NET backend URL
  headers: {
    "Content-Type": "application/json"
  }
});

// Automatically attach JWT token if it exists
instance.interceptors.request.use(config => {
  const token = localStorage.getItem("token");
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export default instance;



