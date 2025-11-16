import { useQuery } from '@tanstack/react-query';
import { motion } from 'framer-motion';
import { BookOpen, Users, BookMarked, AlertCircle, TrendingUp, DollarSign, Activity } from 'lucide-react';
import { reportsAPI, borrowAPI } from '../services/api';
import LoadingSpinner from '../components/LoadingSpinner';

export default function Reports() {
  const { data: stats, isLoading } = useQuery({
    queryKey: ['reportStats'],
    queryFn: reportsAPI.getStats,
    select: (response) => response.data,
  });

  const { data: borrowingReport } = useQuery({
    queryKey: ['borrowingReport'],
    queryFn: async () => {
      const response = await fetch('http://localhost:5208/api/reports/borrowing');
      return response.json();
    },
  });

  const { data: recentLoans } = useQuery({
    queryKey: ['recentLoans'],
    queryFn: borrowAPI.getAll,
    select: (response) => response.data?.slice(0, 5) || [],
  });

  const { data: categoryStats } = useQuery({
    queryKey: ['categoryStats'],
    queryFn: async () => {
      const response = await fetch('http://localhost:5208/api/reports/books-by-category');
      return response.json();
    },
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
      <div className="grid gap-6 lg:grid-cols-2">
        {/* Recent Activity Summary */}
        <motion.div
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ delay: 0.4 }}
          className="rounded-lg border border-gray-200 bg-white p-6 dark:border-gray-800 dark:bg-gray-900"
        >
          <div className="mb-4 flex items-center gap-2">
            <Activity className="h-5 w-5 text-primary-600 dark:text-primary-400" />
            <h2 className="text-lg font-semibold text-gray-900 dark:text-white">
              Recent Activity Summary
            </h2>
          </div>
          <div className="space-y-3 text-sm text-gray-600 dark:text-gray-400">
            <p>• Total borrowings: <span className="font-medium text-gray-900 dark:text-white">{borrowingReport?.totalBorrowings || 0}</span></p>
            <p>• Currently borrowed: <span className="font-medium text-gray-900 dark:text-white">{borrowingReport?.currentlyBorrowed || 0} books</span></p>
            <p>• Returned: <span className="font-medium text-gray-900 dark:text-white">{borrowingReport?.returned || 0} books</span></p>
            <p>• Most popular category: <span className="font-medium text-gray-900 dark:text-white">{categoryStats?.[0]?.category || 'N/A'}</span></p>
            <p>• Books in that category: <span className="font-medium text-gray-900 dark:text-white">{categoryStats?.[0]?.totalBooks || 0}</span></p>
          </div>
        </motion.div>

        {/* Top Borrowers */}
        <motion.div
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ delay: 0.45 }}
          className="rounded-lg border border-gray-200 bg-white p-6 dark:border-gray-800 dark:bg-gray-900"
        >
          <h2 className="mb-4 text-lg font-semibold text-gray-900 dark:text-white">
            🏆 Top Borrowers
          </h2>
          <div className="space-y-3">
            {borrowingReport?.topBorrowers?.slice(0, 5).map((borrower, index) => (
              <div key={index} className="flex items-center justify-between text-sm">
                <span className="text-gray-700 dark:text-gray-300">
                  {index + 1}. {borrower.userName} <span className="text-xs text-gray-500">({borrower.userType})</span>
                </span>
                <span className="font-medium text-gray-900 dark:text-white">
                  {borrower.borrowCount} books
                </span>
              </div>
            )) || <p className="text-sm text-gray-500">No data available</p>}
          </div>
        </motion.div>
      </div>

      {/* Popular Books */}
      <motion.div
        initial={{ opacity: 0, y: 20 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ delay: 0.5 }}
        className="rounded-lg border border-gray-200 bg-white p-6 dark:border-gray-800 dark:bg-gray-900"
      >
        <h2 className="mb-4 text-lg font-semibold text-gray-900 dark:text-white">
          📚 Most Popular Books
        </h2>
        <div className="grid gap-3 sm:grid-cols-2 lg:grid-cols-3">
          {borrowingReport?.popularBooks?.slice(0, 6).map((book, index) => (
            <div key={index} className="rounded-lg bg-gray-50 p-3 dark:bg-gray-800">
              <div className="flex items-start justify-between">
                <div className="flex-1">
                  <p className="text-sm font-medium text-gray-900 dark:text-white line-clamp-1">
                    {book.title}
                  </p>
                  <p className="mt-1 text-xs text-gray-500 dark:text-gray-400">
                    by {book.author}
                  </p>
                </div>
                <span className="ml-2 rounded-full bg-primary-100 px-2 py-1 text-xs font-medium text-primary-700 dark:bg-primary-900/30 dark:text-primary-400">
                  {book.borrowCount}x
                </span>
              </div>
            </div>
          )) || <p className="text-sm text-gray-500">No data available</p>}
        </div>
      </motion.div>

      {/* Recent Loans */}
      <motion.div
        initial={{ opacity: 0, y: 20 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ delay: 0.55 }}
        className="rounded-lg border border-gray-200 bg-white p-6 dark:border-gray-800 dark:bg-gray-900"
      >
        <h2 className="mb-4 text-lg font-semibold text-gray-900 dark:text-white">
          📖 Recent Loans
        </h2>
        <div className="space-y-3">
          {recentLoans?.map((loan, index) => (
            <div key={index} className="flex items-center justify-between border-b border-gray-100 pb-3 last:border-0 dark:border-gray-800">
              <div className="flex-1">
                <p className="text-sm font-medium text-gray-900 dark:text-white">
                  {loan.bookTitle}
                </p>
                <p className="mt-1 text-xs text-gray-500 dark:text-gray-400">
                  {loan.userName} • {new Date(loan.borrowDate).toLocaleDateString()}
                </p>
              </div>
              <span className={`rounded-full px-2 py-1 text-xs font-medium ${
                loan.status === 'Active' 
                  ? 'bg-green-100 text-green-700 dark:bg-green-900/20 dark:text-green-400'
                  : loan.status === 'Overdue'
                  ? 'bg-red-100 text-red-700 dark:bg-red-900/20 dark:text-red-400'
                  : 'bg-gray-100 text-gray-700 dark:bg-gray-800 dark:text-gray-400'
              }`}>
                {loan.status}
              </span>
            </div>
          )) || <p className="text-sm text-gray-500">No recent loans</p>}
        </div>
      </motion.div>
    </div>
  );
}
