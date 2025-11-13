import { useQuery } from '@tanstack/react-query';
import { motion } from 'framer-motion';
import { BookOpen, Users, BookMarked, AlertCircle, TrendingUp, DollarSign } from 'lucide-react';
import { reportsAPI } from '../services/api';
import LoadingSpinner from '../components/LoadingSpinner';

export default function Reports() {
  const { data: stats, isLoading } = useQuery({
    queryKey: ['reportStats'],
    queryFn: reportsAPI.getStats,
    select: (response) => response.data,
  });

  const statsCards = [
    {
      title: 'Total Books',
      value: stats?.totalBooks || 0,
      icon: BookOpen,
      color: 'blue',
      bgColor: 'bg-blue-100 dark:bg-blue-900/20',
      iconColor: 'text-blue-600 dark:text-blue-400',
    },
    {
      title: 'Books Issued',
      value: stats?.booksIssued || 0,
      icon: BookMarked,
      color: 'purple',
      bgColor: 'bg-purple-100 dark:bg-purple-900/20',
      iconColor: 'text-purple-600 dark:text-purple-400',
    },
    {
      title: 'Books Available',
      value: stats?.booksAvailable || 0,
      icon: TrendingUp,
      color: 'success',
      bgColor: 'bg-success-100 dark:bg-success-900/20',
      iconColor: 'text-success-600 dark:text-success-400',
    },
    {
      title: 'Total Users',
      value: stats?.totalUsers || 0,
      icon: Users,
      color: 'indigo',
      bgColor: 'bg-indigo-100 dark:bg-indigo-900/20',
      iconColor: 'text-indigo-600 dark:text-indigo-400',
    },
    {
      title: 'Active Loans',
      value: stats?.activeLoans || 0,
      icon: BookMarked,
      color: 'cyan',
      bgColor: 'bg-cyan-100 dark:bg-cyan-900/20',
      iconColor: 'text-cyan-600 dark:text-cyan-400',
    },
    {
      title: 'Overdue Books',
      value: stats?.overdueBooks || 0,
      icon: AlertCircle,
      color: 'red',
      bgColor: 'bg-red-100 dark:bg-red-900/20',
      iconColor: 'text-red-600 dark:text-red-400',
    },
    {
      title: 'Total Fines',
      value: `$${stats?.totalFines?.toFixed(2) || '0.00'}`,
      icon: DollarSign,
      color: 'yellow',
      bgColor: 'bg-yellow-100 dark:bg-yellow-900/20',
      iconColor: 'text-yellow-600 dark:text-yellow-400',
    },
    {
      title: 'New Members',
      value: stats?.newMembersThisMonth || 0,
      icon: Users,
      color: 'primary',
      bgColor: 'bg-primary-100 dark:bg-primary-900/20',
      iconColor: 'text-primary-600 dark:text-primary-400',
      subtitle: 'This Month',
    },
  ];

  return (
    <div className="space-y-6">
      {/* Header */}
      <div>
        <h1 className="text-3xl font-bold text-gray-900 dark:text-white">Reports & Analytics</h1>
        <p className="mt-1 text-sm text-gray-600 dark:text-gray-400">
          Overview of library statistics and performance
        </p>
      </div>

      {/* Stats Grid */}
      {isLoading ? (
        <LoadingSpinner size="lg" className="py-12" />
      ) : (
        <div className="grid gap-6 sm:grid-cols-2 lg:grid-cols-4">
          {statsCards.map((stat, index) => (
            <motion.div
              key={stat.title}
              initial={{ opacity: 0, y: 20 }}
              animate={{ opacity: 1, y: 0 }}
              transition={{ delay: index * 0.05 }}
              className="rounded-lg border border-gray-200 bg-white p-6 shadow-sm transition-shadow hover:shadow-md dark:border-gray-800 dark:bg-gray-900"
            >
              <div className="flex items-center justify-between">
                <div className="flex-1">
                  <p className="text-sm font-medium text-gray-600 dark:text-gray-400">
                    {stat.title}
                  </p>
                  <p className="mt-2 text-3xl font-bold text-gray-900 dark:text-white">
                    {stat.value}
                  </p>
                  {stat.subtitle && (
                    <p className="mt-1 text-xs text-gray-500 dark:text-gray-500">
                      {stat.subtitle}
                    </p>
                  )}
                </div>
                <div className={`rounded-full p-3 ${stat.bgColor}`}>
                  <stat.icon className={`h-6 w-6 ${stat.iconColor}`} />
                </div>
              </div>
            </motion.div>
          ))}
        </div>
      )}

      {/* Additional Info */}
      <motion.div
        initial={{ opacity: 0, y: 20 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ delay: 0.4 }}
        className="rounded-lg border border-gray-200 bg-white p-6 dark:border-gray-800 dark:bg-gray-900"
      >
        <h2 className="mb-4 text-lg font-semibold text-gray-900 dark:text-white">
          Recent Activity Summary
        </h2>
        <div className="space-y-3 text-sm text-gray-600 dark:text-gray-400">
          <p>• Library utilization rate: <span className="font-medium text-gray-900 dark:text-white">64%</span></p>
          <p>• Average loan duration: <span className="font-medium text-gray-900 dark:text-white">18 days</span></p>
          <p>• Most popular category: <span className="font-medium text-gray-900 dark:text-white">Programming</span></p>
          <p>• Peak borrowing hours: <span className="font-medium text-gray-900 dark:text-white">10 AM - 2 PM</span></p>
        </div>
      </motion.div>

      {/* Placeholder for Charts */}
      <motion.div
        initial={{ opacity: 0, y: 20 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ delay: 0.5 }}
        className="rounded-lg border border-dashed border-gray-300 bg-gray-50 p-12 text-center dark:border-gray-700 dark:bg-gray-800/50"
      >
        <p className="text-sm text-gray-600 dark:text-gray-400">
          📊 Charts and graphs will be displayed here using Recharts or Chart.js
        </p>
        <p className="mt-2 text-xs text-gray-500 dark:text-gray-500">
          Integration with visualization libraries coming soon
        </p>
      </motion.div>
    </div>
  );
}
