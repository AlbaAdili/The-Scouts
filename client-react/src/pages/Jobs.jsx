import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import api from '../api/axios'; // make sure this is your configured axios instance

export default function Jobs() {
  const [jobs, setJobs] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    api.get('/job')
      .then(res => setJobs(res.data))
      .catch(err => console.error('Error fetching jobs:', err))
      .finally(() => setLoading(false));
  }, []);

  if (loading) return <p className="text-center mt-10">Loading jobs...</p>;

  return (
    <div className="p-4 max-w-4xl mx-auto">
      <h1 className="text-2xl font-bold mb-6">Available Jobs</h1>
      {jobs.length === 0 ? (
        <p>No jobs available at the moment.</p>
      ) : (
        <ul className="space-y-4">
          {jobs.map(job => (
            <li key={job.id} className="border p-4 rounded shadow hover:shadow-md transition">
              <h2 className="text-xl font-semibold text-blue-800">{job.jobTitle}</h2>
              <p className="text-gray-600">{job.city}, {job.country}</p>
              <Link
                to={`/jobs/${job.id}`}
                className="inline-block mt-2 text-sm text-blue-600 underline hover:text-blue-800"
              >
                View Details
              </Link>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}
