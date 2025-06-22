import React, { useState } from 'react';
import api from '../api/axios';
import { useAuth } from '../context/AuthContext';

export default function Apply() {
  const { token } = useAuth();
  const [form, setForm] = useState({
    name: '',
    surname: '',
    email: '',
    jobId: '',
    resume: null
  });
  const [message, setMessage] = useState('');

  const handleChange = (e) => {
    const { name, value } = e.target;
    setForm(prev => ({ ...prev, [name]: value }));
  };

  const handleFileChange = (e) => {
    setForm(prev => ({ ...prev, resume: e.target.files[0] }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    const data = new FormData();
    Object.entries(form).forEach(([key, value]) => data.append(key, value));

    try {
      await api.post('/api/application', data, {
        headers: {
          Authorization: `Bearer ${token}`,
          'Content-Type': 'multipart/form-data'
        }
      });
      setMessage('✅ Application submitted successfully!');
    } catch (err) {
      console.error(err);
      setMessage('❌ Submission failed. Please try again.');
    }
  };

  return (
    <div className="p-6 max-w-md mx-auto space-y-4">
      <h1 className="text-2xl font-bold">Apply for a Job</h1>
      <form onSubmit={handleSubmit} className="space-y-3">
        <input
          name="name"
          value={form.name}
          onChange={handleChange}
          placeholder="Name"
          className="input w-full"
          required
        />
        <input
          name="surname"
          value={form.surname}
          onChange={handleChange}
          placeholder="Surname"
          className="input w-full"
          required
        />
        <input
          name="email"
          value={form.email}
          onChange={handleChange}
          placeholder="Email"
          type="email"
          className="input w-full"
          required
        />
        <input
          name="jobId"
          value={form.jobId}
          onChange={handleChange}
          placeholder="Job ID"
          type="number"
          className="input w-full"
          required
        />
        <input
          type="file"
          onChange={handleFileChange}
          accept=".pdf,.docx"
          className="input w-full"
          required
        />
        <button type="submit" className="btn w-full">Submit Application</button>
        {message && <p className="text-center text-sm">{message}</p>}
      </form>
    </div>
  );
}
