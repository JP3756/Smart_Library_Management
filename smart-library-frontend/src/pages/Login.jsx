import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { motion } from 'framer-motion';
import { BookOpen, Mail, Lock, Loader2 } from 'lucide-react';
import { authAPI } from '../services/api';
import { useAuth } from '../contexts/AuthContext';
import toast from 'react-hot-toast';
import Button from '../components/Button';

export default function Login() {
  const navigate = useNavigate();
  const { login } = useAuth();
  const [formData, setFormData] = useState({ email: '', password: '' });
  const [isLoading, setIsLoading] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();
    
    if (!formData.email || !formData.password) {
      toast.error('Please fill in all fields');
      return;
    }

    setIsLoading(true);

    try {
      const response = await authAPI.login(formData);
      const { token, user } = response.data;

      login(user, token);

      toast.success(`Welcome back, ${user.name}!`);
      navigate('/books');
    } catch (error) {
      toast.error(error.response?.data?.message || 'Login failed. Please try again.');
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="flex min-h-screen items-center justify-center bg-gradient-to-br from-primary-50 via-white to-success-50 dark:from-gray-900 dark:via-gray-950 dark:to-gray-900">
      <motion.div
        initial={{ opacity: 0, y: 20 }}
        animate={{ opacity: 1, y: 0 }}
        className="w-full max-w-md"
      >
        {/* Card */}
        <div className="rounded-2xl bg-white p-8 shadow-2xl dark:bg-gray-900">
          {/* Logo & Title */}
          <div className="mb-8 text-center">
            <div className="mx-auto mb-4 flex h-16 w-16 items-center justify-center rounded-full bg-primary-600">
              <BookOpen className="h-9 w-9 text-white" />
            </div>
            <h1 className="text-3xl font-bold text-gray-900 dark:text-white">
              Smart Library
            </h1>
            <p className="mt-2 text-sm text-gray-600 dark:text-gray-400">
              Sign in to manage your library
            </p>
          </div>

          {/* Form */}
          <form onSubmit={handleSubmit} className="space-y-6">
            {/* Email */}
            <div>
              <label
                htmlFor="email"
                className="mb-2 block text-sm font-medium text-gray-700 dark:text-gray-300"
              >
                Email Address
              </label>
              <div className="relative">
                <Mail className="absolute left-3 top-1/2 h-5 w-5 -translate-y-1/2 text-gray-400" />
                <input
                  id="email"
                  type="email"
                  placeholder="librarian@university.edu"
                  value={formData.email}
                  onChange={(e) => setFormData({ ...formData, email: e.target.value })}
                  className="w-full rounded-lg border border-gray-300 bg-white py-3 pl-10 pr-4 text-gray-900 placeholder-gray-400 transition-colors focus:border-primary-500 focus:outline-none focus:ring-2 focus:ring-primary-500/20 dark:border-gray-700 dark:bg-gray-800 dark:text-white dark:placeholder-gray-500"
                />
              </div>
            </div>

            {/* Password */}
            <div>
              <label
                htmlFor="password"
                className="mb-2 block text-sm font-medium text-gray-700 dark:text-gray-300"
              >
                Password
              </label>
              <div className="relative">
                <Lock className="absolute left-3 top-1/2 h-5 w-5 -translate-y-1/2 text-gray-400" />
                <input
                  id="password"
                  type="password"
                  placeholder="••••••••"
                  value={formData.password}
                  onChange={(e) => setFormData({ ...formData, password: e.target.value })}
                  className="w-full rounded-lg border border-gray-300 bg-white py-3 pl-10 pr-4 text-gray-900 placeholder-gray-400 transition-colors focus:border-primary-500 focus:outline-none focus:ring-2 focus:ring-primary-500/20 dark:border-gray-700 dark:bg-gray-800 dark:text-white dark:placeholder-gray-500"
                />
              </div>
            </div>

            {/* Remember Me */}
            <div className="flex items-center justify-between">
              <label className="flex items-center">
                <input
                  type="checkbox"
                  className="h-4 w-4 rounded border-gray-300 text-primary-600 focus:ring-primary-500"
                />
                <span className="ml-2 text-sm text-gray-600 dark:text-gray-400">
                  Remember me
                </span>
              </label>
              <a
                href="#"
                className="text-sm font-medium text-primary-600 hover:text-primary-700 dark:text-primary-400"
              >
                Forgot password?
              </a>
            </div>

            {/* Submit Button */}
            <Button
              type="submit"
              variant="primary"
              size="lg"
              className="w-full"
              isLoading={isLoading}
            >
              {isLoading ? 'Signing in...' : 'Sign In'}
            </Button>
          </form>

          {/* Demo Credentials */}
          <div className="mt-6 rounded-lg bg-gradient-to-br from-blue-50 to-cyan-50 p-4 dark:from-gray-800 dark:to-gray-800 border-2 border-blue-200 dark:border-blue-800">
            <p className="text-xs font-bold text-gray-800 dark:text-gray-200 mb-3">
              📚 Click to Login (no password required):
            </p>
            
            <div className="space-y-3">
              <div className="bg-white dark:bg-gray-900 rounded-lg p-2.5 border-2 border-green-400 dark:border-green-600 shadow-sm">
                <p className="text-xs font-bold text-green-700 dark:text-green-400 mb-1.5 flex items-center gap-1">
                  👨‍💼 Librarian (Full Admin):
                </p>
                <button
                  type="button"
                  onClick={() => setFormData({ email: 'admin@library.edu', password: 'dummy' })}
                  className="text-sm text-gray-900 dark:text-gray-100 font-mono bg-green-50 dark:bg-green-900/30 px-3 py-1.5 rounded hover:bg-green-100 dark:hover:bg-green-900/50 transition-colors w-full text-left font-semibold"
                >
                  admin@library.edu
                </button>
                <p className="text-xs text-gray-500 dark:text-gray-400 mt-1 italic">
                  ✨ Full access: Add/Edit books, Issue/Return, Manage all
                </p>
              </div>

              <div className="bg-white dark:bg-gray-900 rounded-lg p-2 border border-blue-200 dark:border-blue-700">
                <p className="text-xs font-semibold text-blue-600 dark:text-blue-400 mb-1.5">👨‍🏫 Faculty Account:</p>
                <button
                  type="button"
                  onClick={() => setFormData({ email: 'roberto.cruz@faculty.edu', password: 'dummy' })}
                  className="text-xs text-gray-700 dark:text-gray-300 font-mono hover:bg-blue-50 dark:hover:bg-blue-900/20 px-2 py-1 rounded transition-colors w-full text-left"
                >
                  roberto.cruz@faculty.edu
                </button>
                <p className="text-xs text-gray-500 dark:text-gray-400 mt-1 italic">
                  Can view books & reports, see own borrowed books
                </p>
              </div>
              
              <div className="bg-white dark:bg-gray-900 rounded-lg p-2 border border-purple-200 dark:border-purple-800">
                <p className="text-xs font-semibold text-purple-600 dark:text-purple-400 mb-1">🎓 Student Account:</p>
                <button
                  type="button"
                  onClick={() => setFormData({ email: 'juan.delacruz@student.edu', password: 'dummy' })}
                  className="text-xs text-gray-700 dark:text-gray-300 font-mono hover:bg-purple-50 dark:hover:bg-purple-900/20 px-2 py-1 rounded transition-colors w-full text-left"
                >
                  juan.delacruz@student.edu
                </button>
                <p className="text-xs text-gray-500 dark:text-gray-400 mt-1 italic">
                  Can view books, see own borrowed books only
                </p>
              </div>
            </div>
          </div>
        </div>

        {/* Footer */}
        <p className="mt-6 text-center text-sm text-gray-600 dark:text-gray-400">
          Smart Library Management System © 2025
        </p>
      </motion.div>
    </div>
  );
}
