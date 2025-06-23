import React, { useEffect, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import api from '../api/axios';
import { getUserRole } from '../api/auth';

export default function Jobs() {
  const [jobs, setJobs] = useState([]);
  const [loading, setLoading] = useState(true);
  const role = getUserRole(); // 'Admin' or 'User'
  const navigate = useNavigate();

  useEffect(() => {
    fetchJobs();
  }, []);

  const fetchJobs = async () => {
    try {
      const res = await api.get('/job');
      setJobs(res.data);
    } catch (err) {
      console.error('Error fetching jobs:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (id) => {
    if (!window.confirm("Are you sure you want to delete this job?")) return;

    try {
      await api.delete(`/job/${id}`, {
        headers: {
          Authorization: `Bearer ${localStorage.getItem("token")}`
        }
      });
      setJobs(jobs.filter(job => job.id !== id)); // Remove from list
    } catch (error) {
      console.error("Delete failed:", error);
      alert("Failed to delete job. Are you logged in as Admin?");
    }
  };

  if (loading) return <p className="text-center mt-10">Loading jobs...</p>;

  return (
    <div className="p-6 max-w-6xl mx-auto">
      <h1 className="text-3xl font-bold text-center mb-6">Open Positions</h1>

      {role === 'Admin' && (
        <div className="flex justify-end mb-4">
          <button
            onClick={() => navigate('/admin/jobs/add')}
            className="px-4 py-2 bg-black text-white rounded"
          >
            Add Position
          </button>
        </div>
      )}

      <div className="overflow-x-auto">
        <table className="w-full border text-left">
          <thead>
            <tr className="bg-gray-100">
              <th className="p-3">Location</th>
              <th className="p-3">Job Title</th>
              <th className="p-3">Description</th>
              {role === 'Admin' && <th className="p-3">Actions</th>}
            </tr>
          </thead>
          <tbody>
            {jobs.map(job => (
              <tr key={job.id} className="border-t">
                <td className="p-3">{job.city}, {job.country}</td>
                <td className="p-3">{job.jobTitle}</td>
                <td className="p-3">
                  <Link to={`/jobs/${job.id}`} className="text-blue-600 underline">Read More</Link>
                </td>
                {role === 'Admin' && (
                  <td className="p-3 space-x-2">
                    <button
                      onClick={() => navigate(`/admin/jobs/${job.id}/edit`)}
                      className="px-3 py-1 bg-blue-500 text-white rounded"
                    >
                      Edit
                    </button>
                    <button
                      onClick={() => handleDelete(job.id)}
                      className="px-3 py-1 bg-red-500 text-white rounded"
                    >
                      Delete
                    </button>
                    <button
                      onClick={() => navigate(`/admin/jobs/${job.id}/applications`)}
                      className="px-3 py-1 bg-gray-600 text-white rounded"
                    >
                      Applications
                    </button>
                  </td>
                )}
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}
