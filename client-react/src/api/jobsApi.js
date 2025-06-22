import axios from './axios';

export const fetchJobs = async () => {
  const response = await axios.get('/job'); // hits http://localhost:5185/api/job
  return response.data;
};
