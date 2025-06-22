import React, { useState } from "react";
import { Link } from "react-router-dom";

const faqs = [
  "How does the system notify me about new job opportunities?",
  "How can I receive feedback on my job applications?",
  "Is there a mobile app available for on-the-go job searching and application tracking?",
  "Can I apply to multiple positions simultaneously?",
  "Can I connect with recruiters or hiring managers directly?"
];

export default function Faq() {
  const [openIndex, setOpenIndex] = useState(null);

  const toggle = (index) => {
    setOpenIndex(index === openIndex ? null : index);
  };

  return (
    <div className="min-h-screen flex flex-col justify-between bg-white text-gray-800">
      {/* FAQ content */}
      <div className="max-w-4xl mx-auto py-16 px-4 sm:px-6 lg:px-8">
        <h1 className="text-4xl font-bold text-center mb-2">Frequently Asked Questions</h1>
        <p className="text-center text-gray-600 mb-10">
          Find answers to the most frequently asked questions.
        </p>

        <div className="space-y-4">
          {faqs.map((question, index) => (
            <div
              key={index}
              className="border border-gray-300 rounded-lg shadow-sm overflow-hidden"
            >
              <button
                onClick={() => toggle(index)}
                className="w-full flex justify-between items-center text-left px-5 py-4 text-base sm:text-lg font-medium hover:bg-gray-50 transition-all"
              >
                <span className="flex-1">{question}</span>
                <span className="ml-4 text-xl">
                  {openIndex === index ? "▲" : "▼"}
                </span>
              </button>
              {openIndex === index && (
                <div className="px-5 py-3 bg-gray-50 text-sm sm:text-base text-gray-700">
                  This is the answer to: <strong>{question}</strong>
                </div>
              )}
            </div>
          ))}
        </div>
      </div>

      {/* Footer */}
      <footer className="mt-16 w-full border-t pt-6 pb-8 text-center text-sm text-gray-600">
        <div className="flex flex-wrap justify-center space-x-4 mb-4">
          <Link to="/">Home</Link>
          <Link to="/about">About</Link>
          <Link to="/faq">FAQ</Link>
          <Link to="/jobs">Jobs</Link>
          <Link to="/contact">Contact</Link>
        </div>
        <form className="flex flex-col sm:flex-row justify-center items-center gap-2">
          <input
            type="email"
            placeholder="Email address"
            className="border px-4 py-2 rounded"
          />
          <button
            type="submit"
            className="bg-black text-white px-4 py-2 rounded"
          >
            Subscribe
          </button>
        </form>
      </footer>
    </div>
  );
}
