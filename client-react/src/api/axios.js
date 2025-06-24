import axios from 'axios';

const api = axios.create({
  baseURL: 'http://localhost:5185/api', // or https if your backend uses https
});

// Attach token to every request
api.interceptors.request.use(config => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export default api;
