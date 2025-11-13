# 📚 Smart Library Management System

A full-stack library management application with React frontend and ASP.NET Core backend.

## 🏗️ Project Structure

```
Smart_Library_Management/
├── frontend/              # React + Vite frontend
│   ├── src/
│   ├── public/
│   ├── package.json
│   └── README.md
│
├── backend/               # ASP.NET Core Web API
│   ├── SmartLibraryAPI/
│   │   ├── Controllers/
│   │   ├── Models/
│   │   ├── Program.cs
│   │   └── appsettings.json
│   └── SmartLibrary.sln
│
└── README.md             # This file
```

## 🚀 Getting Started

### Prerequisites

- **Frontend**: Node.js 18+ and npm
- **Backend**: .NET 8 SDK
- **Database**: SQL Server or PostgreSQL (optional for development)

### Setup Instructions

#### 1. Clone the Repository

```powershell
git clone https://github.com/JP3756/Smart_Library_Management.git
cd Smart_Library_Management
```

#### 2. Frontend Setup

```powershell
# Navigate to frontend
cd smart-library-frontend

# Install dependencies
npm install

# Create environment file
cp .env.example .env

# Start development server
npm run dev
```

Frontend will run on: **http://localhost:5173**

#### 3. Backend Setup

```powershell
# Navigate to backend (from root)
cd backend/SmartLibraryAPI

# Restore dependencies
dotnet restore

# Run the API
dotnet run
```

Backend will run on: **http://localhost:5000** (or 5173, check terminal output)

## 📋 Features

- 🔐 **Authentication**: JWT-based login system
- 📖 **Books Management**: Browse, search, and manage book inventory
- 👥 **User Management**: Handle students, faculty, and librarians
- 📊 **Borrow/Return System**: Track book loans with due dates and fines
- 📈 **Reports & Analytics**: View library statistics
- ⚙️ **Settings**: User profile and preferences
- 🌓 **Dark Mode**: Beautiful light/dark theme

## 🛠️ Tech Stack

### Frontend
- React 19
- Vite
- Tailwind CSS
- React Router
- TanStack Query
- Axios

### Backend
- ASP.NET Core 8
- Entity Framework Core
- SQL Server / PostgreSQL
- JWT Authentication

## 📖 Documentation

- Frontend Documentation: `smart-library-frontend/README.md`
- Backend API Documentation: Coming soon
- Deployment Guide: `smart-library-frontend/DEPLOYMENT.md`

## 👥 Team

Development team working on Smart Library Management System

## 📝 License

This project is for educational purposes.
