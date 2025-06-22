import React, { useState } from 'react';
import api from '../api/axios';

export default function Contact() {
  const [form, setForm] = useState({ name: '', email: '', description: '' });
  const [message, setMessage] = useState('');

  const handleChange = (e) => {
    const { name, value } = e.target;
    setForm((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await api.post('/api/contact', form);
      setMessage('✅ Message sent successfully!');
      setForm({ name: '', email: '', description: '' }); // reset form
    } catch (err) {
      console.error(err);
      setMessage('❌ Failed to send message.');
    }
  };

  return (
    <div className="p-6 max-w-md mx-auto space-y-4">
      <h1 className="text-2xl font-bold">Contact Us</h1>
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
          name="email"
          value={form.email}
          onChange={handleChange}
          placeholder="Email"
          type="email"
          className="input w-full"
          required
        />
        <textarea
          name="description"
          value={form.description}
          onChange={handleChange}
          placeholder="Your message..."
          className="input w-full h-28"
          required
        />
        <button type="submit" className="btn w-full">Send</button>
        {message && <p className="text-center text-sm">{message}</p>}
      </form>
    </div>
  );
}
