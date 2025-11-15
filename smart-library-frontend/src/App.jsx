import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { Toaster } from 'react-hot-toast';
import { ThemeProvider } from './contexts/ThemeContext';
import AppLayout from './layouts/AppLayout';
import Login from './pages/Login';
import Books from './pages/Books';
import Users from './pages/Users';
import Borrow from './pages/Borrow';
import Reports from './pages/Reports';
import Settings from './pages/Settings';

// Protected Route Component
function ProtectedRoute({ children }) {
  const token = localStorage.getItem('token');
  
  // DEV MODE: Auto-login for development
  if (!token) {
    localStorage.setItem('token', 'dev-token-12345');
    localStorage.setItem('user', JSON.stringify({ 
      id: 1, 
      name: 'Dev Librarian', 
      email: 'dev@library.com', 
      role: 'Librarian' 
    }));
  }
  
  return children;
}

function App() {
  return (
    <ThemeProvider>
      <Router>
        <Routes>
          {/* Public Route */}
          <Route path="/login" element={<Login />} />

          {/* Protected Routes */}
          <Route
            path="/"
            element={
              <ProtectedRoute>
                <AppLayout />
              </ProtectedRoute>
            }
          >
            <Route index element={<Navigate to="/books" replace />} />
            <Route path="books" element={<Books />} />
            <Route path="users" element={<Users />} />
            <Route path="borrow" element={<Borrow />} />
            <Route path="reports" element={<Reports />} />
            <Route path="settings" element={<Settings />} />
          </Route>

          {/* Catch all */}
          <Route path="*" element={<Navigate to="/books" replace />} />
        </Routes>
      </Router>

      {/* Toast Notifications */}
      <Toaster
        position="top-right"
        toastOptions={{
          duration: 3000,
          style: {
            background: 'var(--toast-bg, #fff)',
            color: 'var(--toast-color, #000)',
          },
          success: {
            iconTheme: {
              primary: '#10b981',
              secondary: '#fff',
            },
          },
          error: {
            iconTheme: {
              primary: '#ef4444',
              secondary: '#fff',
            },
          },
        }}
      />
    </ThemeProvider>
  );
}

export default App;
