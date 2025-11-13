# Setup Instructions for Team Members

## 🚀 Quick Start Guide

### Prerequisites
- **Node.js 18+** (for frontend)
- **.NET 8 SDK** (for backend)
- **Git**

### Initial Setup

1. **Clone the repository**
   ```powershell
   git clone https://github.com/JP3756/Smart_Library_Management.git
   cd Smart_Library_Management
   ```

2. **Frontend Setup**
   ```powershell
   # Navigate to frontend folder
   cd smart-library-frontend
   
   # Install dependencies
   npm install
   
   # Create environment file
   cp .env.example .env
   
   # Start development server
   npm run dev
   ```
   
   Frontend runs on: **http://localhost:5173**

3. **Backend Setup** (in a new terminal)
   ```powershell
   # Navigate to backend folder (from project root)
   cd backend/SmartLibraryAPI
   
   # Run the API
   dotnet run
   ```
   
   Backend runs on: **http://localhost:5000** or **https://localhost:5001**

## 📁 Project Structure

```
Smart_Library_Management/
├── smart-library-frontend/   # React + Vite frontend
│   ├── src/
│   ├── public/
│   └── package.json
│
├── backend/                   # ASP.NET Core backend
│   ├── SmartLibraryAPI/
│   └── SmartLibrary.sln
│
└── README.md
```

## ⚠️ Common Issues

### Issue: "Cannot find path 'smart-library-frontend'"

**Solution**: You're already in the right place! After cloning, the repository IS the project. Just run:
```powershell
cd Smart_Library_Management
cd smart-library-frontend
npm install
npm run dev
```

### Issue: npm install fails

**Solution**:
```powershell
npm cache clean --force
npm install
```

### Issue: .env file missing

**Solution**:
```powershell
cd smart-library-frontend
cp .env.example .env
```

## 🔑 Demo Credentials

```
Email: admin@library.com
Password: password123
```
*(Currently using mock authentication)*

## 📞 Need Help?

Contact the team lead if you encounter any issues during setup.
