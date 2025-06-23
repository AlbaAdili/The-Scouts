import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Navbar from "./components/Navbar";
import Home from "./pages/Home";
import Jobs from "./pages/Jobs";
import JobDetails from "./pages/JobDetails";
import Apply from "./pages/Apply";
import Contact from "./pages/Contact";
import Login from "./pages/Login";
import Register from "./pages/Register";
import AdminDashboard from "./pages/AdminDashboard";
import About from "./pages/About";
import Faq from './pages/Faq';
import AddJob from './pages/AddJob';
import EditJob from './pages/EditJob';
import AdminApplications from './pages/AdminApplications';



export default function App() {
  return (
    <Router>
      <div className="min-h-screen bg-white text-gray-800">
        <Navbar />
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/jobs" element={<Jobs />} />
          <Route path="/jobs/:id" element={<JobDetails />} />
          <Route path="/apply/:id" element={<Apply />} />
          <Route path="/about" element={<About />} />
          <Route path="/contact" element={<Contact />} />
          <Route path="/faq" element={<Faq />} />
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
          <Route path="/admin" element={<AdminDashboard />} />
          <Route path="/admin/jobs/add" element={<AddJob />} />
          <Route path="/admin/jobs/:id/edit" element={<EditJob />} />
          <Route path="/admin/applications" element={<AdminApplications />} />
        </Routes>
      </div>
    </Router>
  );
}