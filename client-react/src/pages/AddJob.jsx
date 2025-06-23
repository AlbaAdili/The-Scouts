import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../api/axios';

export default function AddJob() {
  const [form, setForm] = useState({ jobTitle: '', city: '', country: '', jobDescription: '' });
  const navigate = useNavigate();

  const handleChange = e => setForm({ ...form, [e.target.name]: e.target.value });

  const handleSubmit = async e => {
    e.preventDefault();
    try {
      const token = localStorage.getItem('token');
      await api.post('/job', form, {
        headers: { Authorization: `Bearer ${token}` }
      });
      navigate('/jobs');
    } catch (error) {
      console.error('Error adding job:', error);
    }
  };

  return (
    <form onSubmit={handleSubmit} className="max-w-lg mx-auto mt-10 space-y-4">
      <input name="jobTitle" onChange={handleChange} placeholder="Job Title" className="border p-2 w-full" />
      <input name="city" onChange={handleChange} placeholder="City" className="border p-2 w-full" />
      <input name="country" onChange={handleChange} placeholder="Country" className="border p-2 w-full" />
      <textarea name="jobDescription" onChange={handleChange} placeholder="Description" className="border p-2 w-full" />
      <button type="submit" className="bg-green-600 text-white px-4 py-2">Add Job</button>
    </form>
  );
}
