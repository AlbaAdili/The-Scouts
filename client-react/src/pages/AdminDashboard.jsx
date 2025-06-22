import React, { useEffect, useState } from "react";
import api from "../api/axios";
import { useAuth } from "../context/AuthContext";

const AdminDashboard = () => {
  const { token } = useAuth();
  const [applications, setApplications] = useState([]);
  const [contacts, setContacts] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchAdminData = async () => {
      try {
        const [appsRes, contactsRes] = await Promise.all([
          api.get("/api/application", {
            headers: { Authorization: `Bearer ${token}` },
          }),
          api.get("/api/contact", {
            headers: { Authorization: `Bearer ${token}` },
          }),
        ]);
        setApplications(appsRes.data);
        setContacts(contactsRes.data);
      } catch (err) {
        console.error("Failed to load admin data:", err);
      } finally {
        setLoading(false);
      }
    };

    if (token) fetchAdminData();
  }, [token]);

  const updateStatus = async (id, newStatus) => {
    try {
      await api.put(
        `/api/application/status/${id}`,
        `"${newStatus}"`,
        {
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`,
          },
        }
      );
      setApplications((prev) =>
        prev.map((app) =>
          app.id === id ? { ...app, status: newStatus } : app
        )
      );
    } catch (err) {
      console.error("Failed to update status:", err);
    }
  };

  if (loading) return <p className="text-center mt-10">Loading...</p>;

  return (
    <div className="p-6 space-y-10">
      <h2 className="text-2xl font-bold">📥 Applications</h2>
      <table className="w-full table-auto border text-sm shadow">
        <thead className="bg-gray-100">
          <tr>
            <th className="border px-2">Name</th>
            <th className="border px-2">Email</th>
            <th className="border px-2">Status</th>
            <th className="border px-2">Actions</th>
          </tr>
        </thead>
        <tbody>
          {applications.map((app) => (
            <tr key={app.id}>
              <td className="border px-2">{app.name} {app.surname}</td>
              <td className="border px-2">{app.email}</td>
              <td className="border px-2">{app.status}</td>
              <td className="border px-2 space-x-2">
                <button
                  onClick={() => updateStatus(app.id, "Accepted")}
                  className="px-2 py-1 bg-green-500 text-white rounded"
                >
                  Accept
                </button>
                <button
                  onClick={() => updateStatus(app.id, "Rejected")}
                  className="px-2 py-1 bg-red-500 text-white rounded"
                >
                  Reject
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>

      <h2 className="text-2xl font-bold">📨 Contact Messages</h2>
      <ul className="divide-y border text-sm">
        {contacts.map((msg) => (
          <li key={msg.id} className="p-3">
            <p className="font-semibold">{msg.name} ({msg.email})</p>
            <p>{msg.description}</p>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default AdminDashboard;
