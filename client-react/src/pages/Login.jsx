import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';

export default function Login() {
  const [form, setForm] = useState({ email: '', password: '' });
  const [error, setError] = useState('');
  const navigate = useNavigate();

  const handleChange = e => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = async e => {
    e.preventDefault();
    try {
      const res = await axios.post('/api/auth/login', form);
      localStorage.setItem('token', res.data.token);
      navigate('/');
    } catch (err) {
      setError('Invalid credentials');
    }
  };

  return (
    <form onSubmit={handleSubmit} className="p-4 space-y-4">
      <input name="email" type="email" value={form.email} onChange={handleChange} placeholder="Email" className="input" required />
      <input name="password" type="password" value={form.password} onChange={handleChange} placeholder="Password" className="input" required />
      <button type="submit" className="btn">Login</button>
      {error && <p className="text-red-500">{error}</p>}
    </form>
  );
}
