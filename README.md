# The Scouts - Recruitment Management System

## Project Overview

**The Scouts** is a robust web-based system designed to facilitate the recruitment and paper submission process within academic and institutional environments. This system addresses key challenges faced by both applicants and administrators, providing a streamlined, secure, and scalable digital platform for managing submissions, job postings, and application reviews.

The primary goal is to bridge the gap between candidates seeking academic or professional positions and institutions looking for qualified individuals, while also supporting academic paper submission workflows.

---

## Objectives

- Modernize the job and paper application process using a full-stack web application.
- Enable role-based access for different users (Admins and Applicants).
- Provide intuitive dashboards for managing job postings and submitted applications.
- Enhance accessibility, security, and usability across all user interactions.
- Improve transparency and reduce administrative overhead in recruitment.

---

## Technologies and Tools

### Frontend
- **React** (JavaScript library for building user interfaces)
- **Tailwind CSS** (Utility-first CSS framework for modern styling)
- **React Router** (Routing and navigation)
- **Axios** (HTTP client for API communication)

###  Backend
- **ASP.NET Core Web API** (RESTful API framework)
- **Entity Framework Core** (ORM for PostgreSQL)
- **JWT** (JSON Web Token for secure authentication)
- **Identity Framework** (Role and user management)

###  Database
- **PostgreSQL 15**
- Database schema designed with relational integrity and indexing for performance
  
---

##  System Architecture

The application follows a **three-tier architecture**:

```
 Client (React)
   ↕ Axios
 API Server (ASP.NET Core)
   ↕ EF Core
 Database (PostgreSQL)
```

- **Authentication**: Implemented using ASP.NET Identity and JWT tokens.
- **Authorization**: Role-based using claims (Admin, User).
- **State Management**: Browser local storage for token persistence and session control.

---

##  User Roles and Functionalities

###  Administrator (HR)
- Create, edit, and delete job postings.
- View and manage all applications.
- Update application statuses.
- Access dashboards for data overview.

###  User (Applicant)
- Register and login securely.
- View job postings.
- Submit applications for open positions.
- Track application status.

---

##  How to Run the Project Locally

### 1. Prerequisites
- Node.js (v18 or later)
- .NET SDK 8.0
- PostgreSQL Server

### 2. Backend Setup

```bash
cd The-Scouts.API
dotnet restore
dotnet ef database update
dotnet run
```

### 3. Frontend Setup

```bash
cd client
npm install
npm start
```

### 4. Configuration
Ensure your `.env` and `appsettings.json` are correctly configured for API and DB connection.

---

##  UI Mockups

Non-functional design prototypes are provided in Figma. These mockups include:
- Landing page
- Job listing table
- Application form
- Admin dashboard
- Login/Register interface


---

##  Testing

- Manual functional testing using Postman and browser sessions.
- Role-based scenario validation.
- API endpoints tested for authentication, validation, and edge cases.

---

