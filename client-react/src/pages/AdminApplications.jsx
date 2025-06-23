import React, { useEffect, useState } from 'react';
import api from '../api/axios';

export default function AdminApplications() {
  const [applications, setApplications] = useState([]);

  useEffect(() => {
    const fetchApplications = async () => {
      try {
        const res = await api.get('/application', {
          headers: {
            Authorization: `Bearer ${localStorage.getItem("token")}`
          }
        });
        setApplications(res.data);
      } catch (err) {
        console.error('Error fetching applications:', err);
      }
    };

    fetchApplications();
  }, []);

  return (
    <div className="p-6 max-w-5xl mx-auto">
      <h1 className="text-2xl font-bold mb-4">All Applications</h1>
      {applications.length === 0 ? (
        <p>No applications found.</p>
      ) : (
        <table className="w-full border text-left">
          <thead>
            <tr className="bg-gray-100">
              <th className="p-3">Applicant Name</th>
              <th className="p-3">Email</th>
              <th className="p-3">Status</th>
              <th className="p-3">Actions</th>
            </tr>
          </thead>
          <tbody>
            {applications.map(app => (
              <tr key={app.id} className="border-t">
                <td className="p-3">{app.name}</td>
                <td className="p-3">{app.email}</td>
                <td className="p-3">{app.status}</td>
                <td className="p-3 space-x-2">
                  <button className="bg-green-500 text-white px-3 py-1 rounded" onClick={() => updateStatus(app.id, 'Accepted')}>Accept</button>
                  <button className="bg-yellow-500 text-white px-3 py-1 rounded" onClick={() => updateStatus(app.id, 'Pending')}>Pending</button>
                  <button className="bg-red-500 text-white px-3 py-1 rounded" onClick={() => updateStatus(app.id, 'Rejected')}>Reject</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );

  async function updateStatus(id, status) {
    try {
      await api.put(`/application/${id}/status?status=${status}`, null, {
        headers: {
          Authorization: `Bearer ${localStorage.getItem("token")}`
        }
      });
      setApplications(applications.map(app => app.id === id ? { ...app, status } : app));
    } catch (error) {
      console.error("Status update failed", error);
    }
  }
}
