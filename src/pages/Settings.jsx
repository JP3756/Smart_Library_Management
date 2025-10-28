import { useState } from 'react';
import { motion } from 'framer-motion';
import { User, Mail, Save, Moon, Sun, Bell } from 'lucide-react';
import { useTheme } from '../contexts/ThemeContext';
import Button from '../components/Button';
import toast from 'react-hot-toast';

export default function Settings() {
  const { theme, toggleTheme } = useTheme();
  const [userData, setUserData] = useState(() => {
    const user = JSON.parse(localStorage.getItem('user') || '{}');
    return {
      name: user.name || '',
      email: user.email || '',
      role: user.role || '',
    };
  });

  const [preferences, setPreferences] = useState({
    emailNotifications: true,
    overdueAlerts: true,
    newBookAlerts: false,
    weeklyReports: true,
  });

  const handleSaveProfile = (e) => {
    e.preventDefault();
    
    // Update user in localStorage
    const updatedUser = { ...JSON.parse(localStorage.getItem('user') || '{}'), ...userData };
    localStorage.setItem('user', JSON.stringify(updatedUser));
    
    toast.success('Profile updated successfully!');
  };

  const handleSavePreferences = () => {
    localStorage.setItem('preferences', JSON.stringify(preferences));
    toast.success('Preferences saved successfully!');
  };

  return (
    <div className="space-y-6">
      {/* Header */}
      <div>
        <h1 className="text-3xl font-bold text-gray-900 dark:text-white">Settings</h1>
        <p className="mt-1 text-sm text-gray-600 dark:text-gray-400">
          Manage your account and preferences
        </p>
      </div>

      <div className="grid gap-6 lg:grid-cols-2">
        {/* Profile Settings */}
        <motion.div
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          className="rounded-lg border border-gray-200 bg-white p-6 dark:border-gray-800 dark:bg-gray-900"
        >
          <h2 className="mb-6 flex items-center text-lg font-semibold text-gray-900 dark:text-white">
            <User className="mr-2 h-5 w-5" />
            Profile Information
          </h2>

          <form onSubmit={handleSaveProfile} className="space-y-4">
            <div>
              <label className="mb-1.5 block text-sm font-medium text-gray-700 dark:text-gray-300">
                Full Name
              </label>
              <input
                type="text"
                value={userData.name}
                onChange={(e) => setUserData({ ...userData, name: e.target.value })}
                className="w-full rounded-lg border border-gray-300 bg-white px-4 py-2 text-gray-900 focus:border-primary-500 focus:outline-none focus:ring-2 focus:ring-primary-500/20 dark:border-gray-700 dark:bg-gray-800 dark:text-white"
              />
            </div>

            <div>
              <label className="mb-1.5 block text-sm font-medium text-gray-700 dark:text-gray-300">
                Email Address
              </label>
              <input
                type="email"
                value={userData.email}
                onChange={(e) => setUserData({ ...userData, email: e.target.value })}
                className="w-full rounded-lg border border-gray-300 bg-white px-4 py-2 text-gray-900 focus:border-primary-500 focus:outline-none focus:ring-2 focus:ring-primary-500/20 dark:border-gray-700 dark:bg-gray-800 dark:text-white"
              />
            </div>

            <div>
              <label className="mb-1.5 block text-sm font-medium text-gray-700 dark:text-gray-300">
                Role
              </label>
              <input
                type="text"
                value={userData.role}
                disabled
                className="w-full rounded-lg border border-gray-300 bg-gray-100 px-4 py-2 text-gray-500 dark:border-gray-700 dark:bg-gray-800 dark:text-gray-400"
              />
              <p className="mt-1 text-xs text-gray-500 dark:text-gray-500">
                Role cannot be changed
              </p>
            </div>

            <Button type="submit" variant="primary" className="w-full">
              <Save className="mr-2 h-4 w-4" />
              Save Profile
            </Button>
          </form>
        </motion.div>

        {/* Appearance */}
        <motion.div
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ delay: 0.1 }}
          className="rounded-lg border border-gray-200 bg-white p-6 dark:border-gray-800 dark:bg-gray-900"
        >
          <h2 className="mb-6 text-lg font-semibold text-gray-900 dark:text-white">
            Appearance
          </h2>

          <div className="space-y-4">
            <div>
              <label className="mb-3 block text-sm font-medium text-gray-700 dark:text-gray-300">
                Theme
              </label>
              <div className="grid grid-cols-2 gap-3">
                <button
                  type="button"
                  onClick={() => theme === 'dark' && toggleTheme()}
                  className={`flex items-center justify-center rounded-lg border-2 p-4 transition-all ${
                    theme === 'light'
                      ? 'border-primary-600 bg-primary-50 dark:bg-primary-900/20'
                      : 'border-gray-300 dark:border-gray-700'
                  }`}
                >
                  <Sun className="mr-2 h-5 w-5" />
                  <span className="font-medium">Light</span>
                </button>

                <button
                  type="button"
                  onClick={() => theme === 'light' && toggleTheme()}
                  className={`flex items-center justify-center rounded-lg border-2 p-4 transition-all ${
                    theme === 'dark'
                      ? 'border-primary-600 bg-primary-50 dark:bg-primary-900/20'
                      : 'border-gray-300 dark:border-gray-700'
                  }`}
                >
                  <Moon className="mr-2 h-5 w-5" />
                  <span className="font-medium">Dark</span>
                </button>
              </div>
            </div>

            <div className="rounded-lg bg-gray-50 p-4 dark:bg-gray-800">
              <p className="text-sm text-gray-600 dark:text-gray-400">
                Current theme: <span className="font-medium text-gray-900 dark:text-white">{theme === 'dark' ? 'Dark Mode' : 'Light Mode'}</span>
              </p>
              <p className="mt-1 text-xs text-gray-500 dark:text-gray-500">
                Theme preference is saved automatically
              </p>
            </div>
          </div>
        </motion.div>

        {/* Notifications */}
        <motion.div
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ delay: 0.2 }}
          className="rounded-lg border border-gray-200 bg-white p-6 dark:border-gray-800 dark:bg-gray-900 lg:col-span-2"
        >
          <h2 className="mb-6 flex items-center text-lg font-semibold text-gray-900 dark:text-white">
            <Bell className="mr-2 h-5 w-5" />
            Notification Preferences
          </h2>

          <div className="space-y-4">
            {[
              { key: 'emailNotifications', label: 'Email Notifications', description: 'Receive important updates via email' },
              { key: 'overdueAlerts', label: 'Overdue Book Alerts', description: 'Get notified when books are overdue' },
              { key: 'newBookAlerts', label: 'New Book Alerts', description: 'Notifications for newly added books' },
              { key: 'weeklyReports', label: 'Weekly Reports', description: 'Receive weekly activity summaries' },
            ].map((pref) => (
              <div key={pref.key} className="flex items-center justify-between rounded-lg border border-gray-200 p-4 dark:border-gray-800">
                <div className="flex-1">
                  <p className="font-medium text-gray-900 dark:text-white">{pref.label}</p>
                  <p className="text-sm text-gray-500 dark:text-gray-400">{pref.description}</p>
                </div>
                <label className="relative inline-flex cursor-pointer items-center">
                  <input
                    type="checkbox"
                    checked={preferences[pref.key]}
                    onChange={(e) => setPreferences({ ...preferences, [pref.key]: e.target.checked })}
                    className="peer sr-only"
                  />
                  <div className="peer h-6 w-11 rounded-full bg-gray-200 after:absolute after:left-[2px] after:top-[2px] after:h-5 after:w-5 after:rounded-full after:border after:border-gray-300 after:bg-white after:transition-all after:content-[''] peer-checked:bg-primary-600 peer-checked:after:translate-x-full peer-checked:after:border-white peer-focus:ring-2 peer-focus:ring-primary-500/20 dark:bg-gray-700 dark:after:border-gray-600"></div>
                </label>
              </div>
            ))}
          </div>

          <div className="mt-6 flex justify-end">
            <Button onClick={handleSavePreferences} variant="primary">
              <Save className="mr-2 h-4 w-4" />
              Save Preferences
            </Button>
          </div>
        </motion.div>

        {/* About */}
        <motion.div
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ delay: 0.3 }}
          className="rounded-lg border border-gray-200 bg-white p-6 dark:border-gray-800 dark:bg-gray-900 lg:col-span-2"
        >
          <h2 className="mb-4 text-lg font-semibold text-gray-900 dark:text-white">
            About Smart Library
          </h2>
          <div className="space-y-2 text-sm text-gray-600 dark:text-gray-400">
            <p><strong>Version:</strong> 1.0.0</p>
            <p><strong>Built with:</strong> React + Vite, Tailwind CSS, Framer Motion</p>
            <p><strong>Backend API:</strong> ASP.NET Core Web API (integration pending)</p>
            <p className="pt-2 text-xs text-gray-500 dark:text-gray-500">
              © 2025 Smart Library Management System. All rights reserved.
            </p>
          </div>
        </motion.div>
      </div>
    </div>
  );
}
