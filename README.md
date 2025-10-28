# 📚 Smart Library Management System - Frontend

A modern, full-stack library management web application built with React and designed to integrate with an ASP.NET Core Web API backend.

## ✨ Features

- 🔐 **Authentication**: Mock JWT-based login system (ready for backend integration)
- 📖 **Books Management**: Browse, search, filter, and manage book inventory
- 👥 **User Management**: Handle students, faculty, and librarians
- 📊 **Borrow/Return System**: Track book loans with due dates and fines
- 📈 **Reports & Analytics**: View library statistics and performance metrics
- ⚙️ **Settings**: User profile and notification preferences
- 🌓 **Dark Mode**: Beautiful light/dark theme with persistent preference
- 🎨 **Modern UI**: Vercel Dashboard × Linear × Notion aesthetic
- 📱 **Responsive Design**: Fully optimized for desktop, tablet, and mobile

## 🛠️ Tech Stack

### Core
- **React 19** - UI library
- **Vite** - Build tool and dev server
- **React Router DOM** - Client-side routing

### UI & Styling
- **Tailwind CSS** - Utility-first CSS framework
- **Framer Motion** - Animation library
- **Lucide React** - Beautiful icon set

### State & Data
- **TanStack React Query** - Server state management
- **Axios** - HTTP client with interceptors

### Utilities
- **React Hot Toast** - Toast notifications
- **Google Fonts (Inter)** - Typography

## 📁 Project Structure

```
src/
├── components/         # Reusable UI components
│   ├── Sidebar.jsx
│   ├── Header.jsx
│   ├── Button.jsx
│   ├── Modal.jsx
│   └── LoadingSpinner.jsx
├── contexts/          # React Context providers
│   └── ThemeContext.jsx
├── layouts/           # Page layouts
│   └── AppLayout.jsx
├── pages/             # Route pages
│   ├── Login.jsx
│   ├── Books.jsx
│   ├── Users.jsx
│   ├── Borrow.jsx
│   ├── Reports.jsx
│   └── Settings.jsx
├── services/          # API integration
│   └── api.js         # Axios instance + mock data
├── App.jsx            # Main app component
├── main.jsx           # App entry point
└── index.css          # Global styles
```

## 🚀 Getting Started

### Prerequisites
- Node.js 18+ and npm/yarn/pnpm

### Installation

1. **Clone the repository**
   ```bash
   cd smart-library-frontend
   ```

2. **Install dependencies**
   ```bash
   npm install
   ```

3. **Configure environment variables**
   ```bash
   cp .env.example .env
   ```
   
   Update `.env` with your API URL:
   ```env
   VITE_API_URL=http://localhost:5000/api
   ```

4. **Start development server**
   ```bash
   npm run dev
   ```

5. **Open in browser**
   ```
   http://localhost:5173
   ```

### 🔑 Demo Credentials
```
Email: admin@library.com
Password: password123
```
*(Any email/password will work with mock authentication)*

## 📦 Available Scripts

- `npm run dev` - Start development server
- `npm run build` - Build for production
- `npm run preview` - Preview production build
- `npm run lint` - Run ESLint

## 🔌 Backend Integration

The frontend is designed to work with an **ASP.NET Core Web API** backend.

### API Endpoints Expected:
```
POST   /api/auth/login
GET    /api/books
POST   /api/books
GET    /api/users
POST   /api/users
GET    /api/borrow
POST   /api/borrow
POST   /api/borrow/{id}/return
GET    /api/reports/stats
```

### To Connect Backend:

1. Update `VITE_API_URL` in `.env` to your backend URL
2. Replace mock API calls in `src/services/api.js` with real endpoints
3. Ensure backend has CORS configured for your frontend domain

Example (in `api.js`):
```javascript
// Replace mock with real API
export const booksAPI = {
  getAll: async (params) => {
    return api.get('/books', { params }); // Real API call
  },
  // ... other methods
};
```

## 🌐 Deployment

### Deploy to Vercel

1. **Push to GitHub**
   ```bash
   git init
   git add .
   git commit -m "Initial commit"
   git remote add origin <your-repo-url>
   git push -u origin main
   ```

2. **Connect to Vercel**
   - Go to [vercel.com](https://vercel.com)
   - Import your GitHub repository
   - Add environment variable: `VITE_API_URL=https://your-backend-api.com/api`
   - Deploy!

### Deploy to Netlify

1. Build the project
   ```bash
   npm run build
   ```

2. Deploy `dist/` folder to Netlify
   - Add environment variable: `VITE_API_URL`

## 🎨 Theme Customization

Edit `tailwind.config.js` to customize colors, fonts, and design tokens:

```javascript
theme: {
  extend: {
    colors: {
      primary: { /* Your brand colors */ },
      success: { /* Success colors */ },
    },
  },
}
```

## 📸 Screenshots

*(Add screenshots of your app here)*

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit changes (`git commit -m 'Add amazing feature'`)
4. Push to branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License.

## 👨‍💻 Author

Built with ❤️ for educational purposes.

## 🔗 Related Projects

- **Backend API**: ASP.NET Core Web API (link to repo)
- **Database**: MySQL with Entity Framework Core

---

**Note**: This is a frontend-only implementation with mock data. Backend integration is required for full functionality.

## React Compiler

The React Compiler is not enabled on this template because of its impact on dev & build performances. To add it, see [this documentation](https://react.dev/learn/react-compiler/installation).

## Expanding the ESLint configuration

If you are developing a production application, we recommend using TypeScript with type-aware lint rules enabled. Check out the [TS template](https://github.com/vitejs/vite/tree/main/packages/create-vite/template-react-ts) for information on how to integrate TypeScript and [`typescript-eslint`](https://typescript-eslint.io) in your project.
