import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import api from '../api/axios';
import { getUserRole } from '../api/auth';

export default function Jobs() {
  const [jobs, setJobs] = useState([]);
  const [loading, setLoading] = useState(true);
  const [searchTerm, setSearchTerm] = useState('');
  const role = getUserRole(); // "Admin" or "User"

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
    if (window.confirm("Are you sure you want to delete this job?")) {
      await api.delete(`/job/${id}`);
      setJobs(jobs.filter(job => job.id !== id));
    }
  };

  const searchJobs = async () => {
    try {
      const res = await api.get(`/job/search?query=${searchTerm}`);
      setJobs(res.data);
    } catch (error) {
      console.error('Error searching jobs', error);
    }
  };

  if (loading) return <p className="text-center mt-10">Loading jobs...</p>;

  return (
    <div className="p-6 max-w-6xl mx-auto">
      <h1 className="text-3xl font-bold text-center mb-6">Open Positions</h1>

      {role === 'Admin' && (
        <div className="flex justify-between mb-4 flex-col sm:flex-row gap-4">
          <Link to="/jobs/new" className="px-4 py-2 bg-black text-white rounded text-sm">Add Position</Link>

          <div className="flex gap-2">
            <input
              type="text"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              placeholder="Search Position"
              className="border px-3 py-2 rounded w-full sm:w-auto"
            />
            <button
              onClick={searchJobs}
              className="px-4 py-2 bg-blue-600 text-white rounded text-sm"
            >
              Search
            </button>
          </div>
        </div>
      )}

      <div className="overflow-x-auto">
        <table className="w-full border text-left">
          <thead>
            <tr className="bg-gray-100">
              <th className="p-3">Location</th>
              <th className="p-3">Job Title</th>
              <th className="p-3">Job Description</th>
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
                    <Link to={`/jobs/edit/${job.id}`} className="px-3 py-1 bg-blue-500 text-white rounded">Edit</Link>
                    <button onClick={() => handleDelete(job.id)} className="px-3 py-1 bg-red-500 text-white rounded">Delete</button>
                    <Link to={`/applications/${job.id}`} className="px-3 py-1 bg-gray-500 text-white rounded">Applications</Link>
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
