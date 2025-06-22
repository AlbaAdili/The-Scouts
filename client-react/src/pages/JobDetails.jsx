import React, { useEffect, useState } from 'react';
import { useParams, Link } from 'react-router-dom';
import axios from 'axios';

export default function JobDetails() {
  const { id } = useParams();
  const [job, setJob] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    axios.get(`/api/job/${id}`)
      .then(res => setJob(res.data))
      .catch(err => console.error(err))
      .finally(() => setLoading(false));
  }, [id]);

  if (loading) return <p className="text-center mt-10">Loading job details...</p>;
  if (!job) return <p className="text-center mt-10 text-red-500">Job not found.</p>;

  return (
    <div className="max-w-3xl mx-auto p-6 space-y-4">
      <h1 className="text-3xl font-bold text-blue-700">{job.jobTitle}</h1>
      <p className="text-gray-600">{job.city}, {job.country}</p>
      <div className="bg-gray-50 p-4 rounded shadow">
        <h2 className="text-lg font-semibold mb-2">Job Description</h2>
        <p className="text-gray-700 whitespace-pre-line">{job.jobDescription}</p>
      </div>
      <Link
        to={`/apply/${job.id}`}
        className="inline-block mt-4 px-6 py-2 bg-blue-600 text-white rounded hover:bg-blue-700 transition"
      >
        Apply Now
      </Link>
    </div>
  );
}
