import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { Toaster } from 'react-hot-toast';
import { ThemeProvider } from './contexts/ThemeContext';
import { AuthProvider } from './contexts/AuthContext';
import ProtectedRoute from './components/ProtectedRoute';
import AppLayout from './layouts/AppLayout';
import Login from './pages/Login';
import Books from './pages/Books';
import Users from './pages/Users';
import Borrow from './pages/Borrow';
import Reports from './pages/Reports';
import Settings from './pages/Settings';

function App() {
  return (
    <ThemeProvider>
      <AuthProvider>
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
              
              {/* Books - All roles */}
              <Route 
                path="books" 
                element={
                  <ProtectedRoute allowedRoles={['Librarian', 'Faculty', 'Student']}>
                    <Books />
                  </ProtectedRoute>
                } 
              />
              
              {/* Users - Librarian only */}
              <Route 
                path="users" 
                element={
                  <ProtectedRoute allowedRoles={['Librarian']}>
                    <Users />
                  </ProtectedRoute>
                } 
              />
              
              {/* Borrow - All roles */}
              <Route 
                path="borrow" 
                element={
                  <ProtectedRoute allowedRoles={['Librarian', 'Faculty', 'Student']}>
                    <Borrow />
                  </ProtectedRoute>
                } 
              />
              
              {/* Reports - Librarian and Faculty */}
              <Route 
                path="reports" 
                element={
                  <ProtectedRoute allowedRoles={['Librarian', 'Faculty']}>
                    <Reports />
                  </ProtectedRoute>
                } 
              />
              
              {/* Settings - All roles */}
              <Route 
                path="settings" 
                element={
                  <ProtectedRoute allowedRoles={['Librarian', 'Faculty', 'Student']}>
                    <Settings />
                  </ProtectedRoute>
                } 
              />
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
      </AuthProvider>
    </ThemeProvider>
  );
}

export default App;
