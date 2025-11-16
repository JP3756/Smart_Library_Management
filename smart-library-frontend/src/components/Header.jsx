import { Moon, Sun, Bell } from 'lucide-react';
import { useTheme } from '../contexts/ThemeContext';
import { useAuth } from '../contexts/AuthContext';

export default function Header() {
  const { theme, toggleTheme } = useTheme();
  const { user } = useAuth();

  return (
    <header className="sticky top-0 z-30 border-b border-gray-200 bg-white/80 backdrop-blur-sm dark:border-gray-800 dark:bg-gray-900/80">
      <div className="flex h-16 items-center justify-between px-6">
        {/* App Title */}
        <div className="flex items-center">
          <h2 className="text-lg font-semibold text-gray-900 dark:text-white">
            Smart Library Management
          </h2>
        </div>

        {/* Actions */}
        <div className="flex items-center space-x-4">
          {/* Theme Toggle */}
          <button
            onClick={toggleTheme}
            className="rounded-lg p-2 text-gray-600 transition-colors hover:bg-gray-100 dark:text-gray-300 dark:hover:bg-gray-800"
            aria-label="Toggle theme"
          >
            {theme === 'dark' ? (
              <Sun className="h-5 w-5" />
            ) : (
              <Moon className="h-5 w-5" />
            )}
          </button>

          {/* Notifications */}
          <button
            className="relative rounded-lg p-2 text-gray-600 transition-colors hover:bg-gray-100 dark:text-gray-300 dark:hover:bg-gray-800"
            aria-label="Notifications"
          >
            <Bell className="h-5 w-5" />
            <span className="absolute right-1.5 top-1.5 h-2 w-2 rounded-full bg-red-500 ring-2 ring-white dark:ring-gray-900"></span>
          </button>

          {/* User Profile */}
          <div className="flex items-center space-x-3 rounded-lg border border-gray-200 bg-gray-50 px-3 py-2 dark:border-gray-700 dark:bg-gray-800">
            <div className="flex h-8 w-8 items-center justify-center rounded-full bg-primary-600 text-sm font-medium text-white">
              {user.name?.charAt(0) || 'U'}
            </div>
            <div className="hidden md:block">
              <p className="text-sm font-medium text-gray-900 dark:text-white">
                {user.name || 'User'}
              </p>
              <p className="text-xs text-gray-500 dark:text-gray-400">
                {user.role || 'Librarian'}
              </p>
            </div>
          </div>
        </div>
      </div>
    </header>
  );
}
