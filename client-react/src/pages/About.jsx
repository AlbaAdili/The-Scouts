import React from 'react';
import { Link } from 'react-router-dom';
import MissionImage from '../assets/recruitment-illustration.jpeg'; // Replace with actual image

const About = () => {
  return (
    <div className="bg-white text-gray-800">
      {/* Top Illustration */}
      <img
        src={MissionImage}
        alt="Mission Illustration"
        className="w-full object-cover max-h-[400px] mx-auto"
      />

      {/* Mission Section */}
      <section className="bg-gray-900 text-white py-12 px-4 text-center space-y-8">
        <h2 className="text-3xl font-bold">Our Mission</h2>
        <div className="max-w-6xl mx-auto grid md:grid-cols-3 gap-6 text-lg">
          <div>
            <p className="italic">“To hire is to take people higher…”</p>
            <p>– Haresh Sippy</p>
          </div>
          <div>
            <p>
              Our group makes a positive impact on people every day. The mission of our group is to help people get a dream job and also make the recruitment process easier for companies that constantly hire people.
            </p>
          </div>
        </div>
      </section>

      {/* Strategy Section */}
      <section className="py-12 px-6 text-center space-y-8 bg-gray-50">
        <h2 className="text-3xl font-bold">Our Strategy</h2>
        <p className="italic text-lg">“You cannot push anyone up the ladder unless he is willing to climb.” – Andrew Carnegie</p>
        <p className="max-w-3xl mx-auto text-gray-700">
          We guide companies and individuals through their journey. By offering continuous support and tools for personal growth, our platform empowers job seekers to thrive and companies to hire meaningfully.
        </p>
      </section>

      {/* Values Section */}
      <section className="py-12 px-6 text-center space-y-8 bg-white">
        <h2 className="text-3xl font-bold">Our Values</h2>
        <p className="italic text-lg">“Recruiting should be viewed as a business partner, someone who is critical to the success of the business.” – Mathew Caldwell</p>
        <p className="max-w-3xl mx-auto text-gray-700">
          We believe in teamwork, trust, and transparency. Our values shape how we collaborate with clients, approach innovation, and ensure inclusivity in everything we build.
        </p>
      </section>

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
    </div>
  );
};

export default About;
