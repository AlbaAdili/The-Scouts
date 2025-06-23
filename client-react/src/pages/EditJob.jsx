// src/pages/EditJob.jsx
import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import api from '../api/axios';

export default function EditJob() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [form, setForm] = useState({ jobTitle: '', city: '', country: '', jobDescription: '' });

  useEffect(() => {
    api.get(`/job/${id}`).then(res => setForm(res.data));
  }, [id]);

  const handleChange = e => setForm({ ...form, [e.target.name]: e.target.value });

  const handleSubmit = async e => {
    e.preventDefault();
    try {
      await api.put(`/job/${id}`, form);
      navigate('/jobs');
    } catch (err) {
      console.error('Edit error:', err);
    }
  };

  return (
    <form onSubmit={handleSubmit} className="max-w-lg mx-auto mt-10 space-y-4">
      <input name="jobTitle" value={form.jobTitle} onChange={handleChange} className="border p-2 w-full" />
      <input name="city" value={form.city} onChange={handleChange} className="border p-2 w-full" />
      <input name="country" value={form.country} onChange={handleChange} className="border p-2 w-full" />
      <textarea name="jobDescription" value={form.jobDescription} onChange={handleChange} className="border p-2 w-full" />
      <button type="submit" className="bg-blue-600 text-white px-4 py-2">Update</button>
    </form>
  );
}
