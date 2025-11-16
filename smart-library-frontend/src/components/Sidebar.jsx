import { NavLink } from 'react-router-dom';
import { BookOpen, Users, BookMarked, BarChart3, Settings, LogOut } from 'lucide-react';
import { motion } from 'framer-motion';
import { useAuth } from '../contexts/AuthContext';

const navigation = [
  { name: 'Books', href: '/books', icon: BookOpen, roles: ['Librarian', 'Faculty', 'Student'] },
  { name: 'Users', href: '/users', icon: Users, roles: ['Librarian'] },
  { name: 'Borrow', href: '/borrow', icon: BookMarked, roles: ['Librarian', 'Faculty', 'Student'] },
  { name: 'Reports', href: '/reports', icon: BarChart3, roles: ['Librarian', 'Faculty'] },
  { name: 'Settings', href: '/settings', icon: Settings, roles: ['Librarian', 'Faculty', 'Student'] },
];

export default function Sidebar() {
  const { user, logout } = useAuth();

  const handleLogout = () => {
    logout();
    window.location.href = '/login';
  };

  // Filter navigation based on user role
  const allowedNavigation = navigation.filter((item) => 
    item.roles.includes(user?.role)
  );

  return (
    <motion.aside
      initial={{ x: -280 }}
      animate={{ x: 0 }}
      className="fixed left-0 top-0 z-40 h-screen w-64 border-r border-gray-200 bg-white dark:border-gray-800 dark:bg-gray-900"
    >
      <div className="flex h-full flex-col">
        {/* Logo */}
        <div className="flex h-16 items-center border-b border-gray-200 px-6 dark:border-gray-800">
          <BookOpen className="h-8 w-8 text-primary-600" />
          <span className="ml-3 text-xl font-semibold text-gray-900 dark:text-white">
            Smart Library
          </span>
        </div>

        {/* Navigation */}
        <nav className="flex-1 space-y-1 px-3 py-4">
          {allowedNavigation.map((item) => (
            <NavLink
              key={item.name}
              to={item.href}
              className={({ isActive }) =>
                `group flex items-center rounded-lg px-3 py-2.5 text-sm font-medium transition-colors ${
                  isActive
                    ? 'bg-primary-50 text-primary-600 dark:bg-primary-900/20 dark:text-primary-400'
                    : 'text-gray-700 hover:bg-gray-100 dark:text-gray-300 dark:hover:bg-gray-800'
                }`
              }
            >
              {({ isActive }) => (
                <>
                  <item.icon
                    className={`mr-3 h-5 w-5 flex-shrink-0 ${
                      isActive ? 'text-primary-600 dark:text-primary-400' : 'text-gray-400'
                    }`}
                  />
                  {item.name}
                </>
              )}
            </NavLink>
          ))}
        </nav>

        {/* Logout */}
        <div className="border-t border-gray-200 p-3 dark:border-gray-800">
          <button
            onClick={handleLogout}
            className="group flex w-full items-center rounded-lg px-3 py-2.5 text-sm font-medium text-gray-700 transition-colors hover:bg-red-50 hover:text-red-600 dark:text-gray-300 dark:hover:bg-red-900/20 dark:hover:text-red-400"
          >
            <LogOut className="mr-3 h-5 w-5 flex-shrink-0 text-gray-400 group-hover:text-red-600 dark:group-hover:text-red-400" />
            Logout
          </button>
        </div>
      </div>
    </motion.aside>
  );
}
