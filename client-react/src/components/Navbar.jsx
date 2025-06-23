import { Link } from "react-router-dom";

export default function Navbar() {
  return (
    <nav className="bg-white border-b p-4">
      <div className="container mx-auto flex justify-between items-center">
        {/* Left: Logo */}
        <div className="text-xl font-semibold text-gray-800">
          <Link to="/">The Scouts</Link>
        </div>

        {/* Center: Navigation Links */}
        <div className="hidden md:flex space-x-6 text-gray-700">
          <Link to="/" className="hover:text-black">Home</Link>
          <Link to="/about" className="hover:text-black">About</Link>
          <Link to="/faq" className="hover:text-black">FAQ</Link>
          <Link to="/jobs" className="hover:text-black">Jobs</Link>
          <Link to="/contact" className="hover:text-black">Contact</Link>

        </div>

        {/* Right: Auth Buttons */}
        <div className="flex space-x-2">
          <Link
            to="/login"
            className="border border-gray-400 text-gray-700 px-3 py-1 rounded hover:bg-gray-100"
          >
            Login
          </Link>
          <Link
            to="/register"
            className="border border-gray-400 text-gray-700 px-3 py-1 rounded hover:bg-gray-100"
          >
            Register
          </Link>
        </div>
      </div>
    </nav>
  );
}
