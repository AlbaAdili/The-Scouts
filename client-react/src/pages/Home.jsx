import React, { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { fetchJobs } from "../api/jobsApi";
import RecruitmentImg from "../assets/recruitment-illustration.jpeg";
import JobHuntingImg from "../assets/job-hunting.jpeg";

const Home = () => {
  const [jobs, setJobs] = useState([]);

  useEffect(() => {
    const loadJobs = async () => {
      try {
        const data = await fetchJobs();
        console.log("Fetched jobs:", data); 
        setJobs(data);
      } catch (error) {
        console.error("Error fetching jobs:", error);
      }
    };
    loadJobs();
  }, []);

  return (
    <>
      {/* Main Content */}
      <div className="flex flex-col items-center px-4 py-10 space-y-10">
        {/* Hero Section */}
        <div className="max-w-4xl text-center">
          <h1 className="text-4xl font-bold mb-4">
            Explore our innovative approach, emphasizing scouting and connecting with the best talents.
          </h1>
          <Link
            to="/about"
            className="text-blue-600 font-semibold underline hover:text-blue-800"
          >
            Learn more &rsaquo;
          </Link>
          <img
            src={RecruitmentImg}
            alt="Recruitment Illustration"
            className="mt-6 mx-auto w-full max-w-md"
          />
        </div>

        {/* Job Table */}
        <div className="w-full max-w-5xl">
          <table className="w-full text-left border border-gray-300 shadow-md">
            <thead className="bg-gray-100">
              <tr>
                <th className="p-3">Location</th>
                <th className="p-3">Job Title</th>
                <th className="p-3 text-right">Job Description</th>
              </tr>
            </thead>
            <tbody>
              {jobs.length > 0 ? (
                <>
                  {jobs.slice(0, 5).map((job) => (
                    <tr key={job.id} className="border-t">
                      <td className="p-3">{job.city}, {job.country}</td>
                      <td className="p-3">{job.jobTitle}</td>
                      <td className="p-3 text-right">
                        <Link to={`/jobs/${job.id}`} className="text-blue-600 hover:underline">
                          Read More &#x2198;
                        </Link>
                      </td>
                    </tr>
                  ))}
                  <tr>
                    <td colSpan="3" className="p-3 text-right">
                      <Link to="/jobs" className="text-blue-600 hover:underline">
                        Show All Open Positions &#x2198;
                      </Link>
                    </td>
                  </tr>
                </>
              ) : (
                <tr>
                  <td colSpan="3" className="p-3 text-center text-gray-500">No jobs found.</td>
                </tr>
              )}
            </tbody>
          </table>
        </div>

        {/* Bottom Section */}
        <div className="flex flex-col md:flex-row justify-between items-center max-w-5xl mt-10 space-y-6 md:space-y-0 md:space-x-10">
          <img
            src={JobHuntingImg}
            alt="Job Hunting Illustration"
            className="w-full md:w-1/2 rounded shadow-md"
          />
          <p className="text-gray-700 text-justify">
            <strong>Scout</strong> is a soldier or other person sent out ahead of a main force to gather information about the enemy's position, strength, or movements. <strong>"The Scouts"</strong> represents a group of individuals working together to scout and identify qualified, and good-fit candidates for the best-match job position.
          </p>
        </div>
      </div>

      {/* Footer */}
      <footer className="mt-16 w-full border-t pt-6 pb-8 text-center text-sm text-gray-600">
        <div className="flex justify-center space-x-6 mb-4">
          <Link to="/">Home</Link>
          <Link to="/about">About</Link>
          <Link to="/faq">FAQ</Link>
          <Link to="/jobs">Jobs</Link>
          <Link to="/contact">Contact</Link>
        </div>
        <form className="flex flex-col sm:flex-row justify-center items-center gap-2">
          <input type="email" placeholder="Email address" className="border px-4 py-2" />
          <button type="submit" className="bg-black text-white px-4 py-2">Subscribe</button>
        </form>
      </footer>
    </>
  );
};

export default Home;
