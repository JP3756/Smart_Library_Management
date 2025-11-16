import { useState } from 'react';
import { useQuery } from '@tanstack/react-query';
import { motion } from 'framer-motion';
import { BookMarked, Calendar, AlertCircle } from 'lucide-react';
import { borrowAPI, booksAPI, usersAPI } from '../services/api';
import { useAuth } from '../contexts/AuthContext';
import Button from '../components/Button';
import LoadingSpinner from '../components/LoadingSpinner';
import Modal from '../components/Modal';
import toast from 'react-hot-toast';

export default function Borrow() {
  const { user, isLibrarian } = useAuth();
  const [isIssueModalOpen, setIsIssueModalOpen] = useState(false);
  const [issueData, setIssueData] = useState({
    bookId: '',
    userId: '',
  });

  const { data: borrowRecords, isLoading, refetch } = useQuery({
    queryKey: ['borrowRecords'],
    queryFn: borrowAPI.getAll,
    select: (response) => {
      const records = response.data;
      // Filter records based on user role
      if (!isLibrarian()) {
        // Students and Faculty see only their own records
        return records.filter(record => record.userId === user?.id);
      }
      // Only Librarians see all records
      return records;
    },
  });

  // Fetch books for dropdown
  const { data: books } = useQuery({
    queryKey: ['books-for-borrow'],
    queryFn: () => booksAPI.getAll({}),
    select: (response) => response.data,
  });

  // Fetch users for dropdown
  const { data: users } = useQuery({
    queryKey: ['users-for-borrow'],
    queryFn: () => usersAPI.getAll({}),
    select: (response) => response.data,
  });

  const handleIssueBook = async (e) => {
    e.preventDefault();
    
    if (!issueData.bookId || !issueData.userId) {
      toast.error('Please select both book and user');
      return;
    }
    
    try {
      await borrowAPI.borrowBook({
        bookId: parseInt(issueData.bookId),
        userId: parseInt(issueData.userId),
      });
      toast.success('Book issued successfully!');
      setIsIssueModalOpen(false);
      setIssueData({ bookId: '', userId: '' });
      refetch();
    } catch (error) {
      const errorMsg = error.response?.data?.message || error.response?.data || 'Failed to issue book';
      toast.error(errorMsg);
    }
  };

  const handleReturnBook = async (id) => {
    try {
      await borrowAPI.returnBook(id);
      toast.success('Book returned successfully!');
      refetch();
    } catch (error) {
      const errorMsg = error.response?.data?.message || 'Failed to return book';
      toast.error(errorMsg);
    }
  };

  const getStatusColor = (status) => {
    switch (status) {
      case 'Active':
        return 'bg-blue-100 text-blue-700 dark:bg-blue-900/20 dark:text-blue-400';
      case 'Overdue':
        return 'bg-red-100 text-red-700 dark:bg-red-900/20 dark:text-red-400';
      case 'Returned':
        return 'bg-success-100 text-success-700 dark:bg-success-900/20 dark:text-success-400';
      default:
        return 'bg-gray-100 text-gray-700 dark:bg-gray-800 dark:text-gray-400';
    }
  };

  return (
    <div className="space-y-6">
      {/* Header */}
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold text-gray-900 dark:text-white">
            {isLibrarian() ? 'Borrow & Return' : 'My Borrowed Books'}
          </h1>
          <p className="mt-1 text-sm text-gray-600 dark:text-gray-400">
            {isLibrarian() 
              ? 'Manage book loans and returns' 
              : 'View your active and past book loans'}
          </p>
        </div>
        {isLibrarian() && (
          <Button onClick={() => setIsIssueModalOpen(true)} className="flex items-center">
            <BookMarked className="mr-2 h-4 w-4" />
            Issue Book
          </Button>
        )}
      </div>

      {/* Stats Cards */}
      <div className="grid gap-6 sm:grid-cols-2 lg:grid-cols-4">
        <motion.div
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          className="rounded-lg border border-gray-200 bg-white p-6 dark:border-gray-800 dark:bg-gray-900"
        >
          <div className="flex items-center justify-between">
            <div>
              <p className="text-sm font-medium text-gray-600 dark:text-gray-400">Active Loans</p>
              <p className="mt-2 text-3xl font-bold text-gray-900 dark:text-white">
                {borrowRecords?.filter(r => r.status === 'Active').length || 0}
              </p>
            </div>
            <div className="rounded-full bg-blue-100 p-3 dark:bg-blue-900/20">
              <BookMarked className="h-6 w-6 text-blue-600 dark:text-blue-400" />
            </div>
          </div>
        </motion.div>

        <motion.div
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ delay: 0.1 }}
          className="rounded-lg border border-gray-200 bg-white p-6 dark:border-gray-800 dark:bg-gray-900"
        >
          <div className="flex items-center justify-between">
            <div>
              <p className="text-sm font-medium text-gray-600 dark:text-gray-400">Overdue</p>
              <p className="mt-2 text-3xl font-bold text-gray-900 dark:text-white">
                {borrowRecords?.filter(r => r.status === 'Overdue').length || 0}
              </p>
            </div>
            <div className="rounded-full bg-red-100 p-3 dark:bg-red-900/20">
              <AlertCircle className="h-6 w-6 text-red-600 dark:text-red-400" />
            </div>
          </div>
        </motion.div>

        <motion.div
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ delay: 0.2 }}
          className="rounded-lg border border-gray-200 bg-white p-6 dark:border-gray-800 dark:bg-gray-900"
        >
          <div className="flex items-center justify-between">
            <div>
              <p className="text-sm font-medium text-gray-600 dark:text-gray-400">Returned</p>
              <p className="mt-2 text-3xl font-bold text-gray-900 dark:text-white">
                {borrowRecords?.filter(r => r.status === 'Returned').length || 0}
              </p>
            </div>
            <div className="rounded-full bg-success-100 p-3 dark:bg-success-900/20">
              <Calendar className="h-6 w-6 text-success-600 dark:text-success-400" />
            </div>
          </div>
        </motion.div>

        <motion.div
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ delay: 0.3 }}
          className="rounded-lg border border-gray-200 bg-white p-6 dark:border-gray-800 dark:bg-gray-900"
        >
          <div className="flex items-center justify-between">
            <div>
              <p className="text-sm font-medium text-gray-600 dark:text-gray-400">Total Fines</p>
              <p className="mt-2 text-3xl font-bold text-gray-900 dark:text-white">
                ${borrowRecords?.reduce((sum, r) => sum + r.fine, 0).toFixed(2) || '0.00'}
              </p>
            </div>
            <div className="rounded-full bg-yellow-100 p-3 dark:bg-yellow-900/20">
              <span className="text-2xl">💰</span>
            </div>
          </div>
        </motion.div>
      </div>

      {/* Borrow Records Table */}
      {isLoading ? (
        <LoadingSpinner size="lg" className="py-12" />
      ) : (
        <motion.div
          initial={{ opacity: 0 }}
          animate={{ opacity: 1 }}
          className="overflow-hidden rounded-lg border border-gray-200 bg-white shadow-sm dark:border-gray-800 dark:bg-gray-900"
        >
          <div className="overflow-x-auto">
            <table className="w-full">
              <thead className="border-b border-gray-200 bg-gray-50 dark:border-gray-800 dark:bg-gray-800/50">
                <tr>
                  <th className="px-6 py-3 text-left text-xs font-medium uppercase tracking-wider text-gray-700 dark:text-gray-300">
                    Book Title
                  </th>
                  <th className="px-6 py-3 text-left text-xs font-medium uppercase tracking-wider text-gray-700 dark:text-gray-300">
                    User Name
                  </th>
                  <th className="px-6 py-3 text-left text-xs font-medium uppercase tracking-wider text-gray-700 dark:text-gray-300">
                    Borrow Date
                  </th>
                  <th className="px-6 py-3 text-left text-xs font-medium uppercase tracking-wider text-gray-700 dark:text-gray-300">
                    Due Date
                  </th>
                  <th className="px-6 py-3 text-left text-xs font-medium uppercase tracking-wider text-gray-700 dark:text-gray-300">
                    Status
                  </th>
                  <th className="px-6 py-3 text-left text-xs font-medium uppercase tracking-wider text-gray-700 dark:text-gray-300">
                    Fine
                  </th>
                  <th className="px-6 py-3 text-left text-xs font-medium uppercase tracking-wider text-gray-700 dark:text-gray-300">
                    Actions
                  </th>
                </tr>
              </thead>
              <tbody className="divide-y divide-gray-200 dark:divide-gray-800">
                {borrowRecords?.map((record, index) => (
                  <motion.tr
                    key={record.id}
                    initial={{ opacity: 0, x: -20 }}
                    animate={{ opacity: 1, x: 0 }}
                    transition={{ delay: index * 0.05 }}
                    className="hover:bg-gray-50 dark:hover:bg-gray-800/50"
                  >
                    <td className="whitespace-nowrap px-6 py-4 text-sm font-medium text-gray-900 dark:text-white">
                      {record.bookTitle}
                    </td>
                    <td className="whitespace-nowrap px-6 py-4 text-sm text-gray-900 dark:text-white">
                      {record.userName}
                    </td>
                    <td className="whitespace-nowrap px-6 py-4 text-sm text-gray-500 dark:text-gray-400">
                      {new Date(record.borrowDate).toLocaleDateString()}
                    </td>
                    <td className="whitespace-nowrap px-6 py-4 text-sm text-gray-500 dark:text-gray-400">
                      {new Date(record.dueDate).toLocaleDateString()}
                    </td>
                    <td className="whitespace-nowrap px-6 py-4">
                      <span
                        className={`inline-flex rounded-full px-2.5 py-1 text-xs font-medium ${getStatusColor(
                          record.status
                        )}`}
                      >
                        {record.status}
                      </span>
                    </td>
                    <td className="whitespace-nowrap px-6 py-4 text-sm text-gray-900 dark:text-white">
                      ${record.fine.toFixed(2)}
                    </td>
                    <td className="whitespace-nowrap px-6 py-4 text-sm">
                      {isLibrarian() && (record.status === 'Active' || record.status === 'Overdue') ? (
                        <Button
                          variant="success"
                          size="sm"
                          onClick={() => handleReturnBook(record.id)}
                        >
                          Return
                        </Button>
                      ) : (
                        <span className="text-gray-400">—</span>
                      )}
                    </td>
                  </motion.tr>
                ))}
              </tbody>
            </table>
          </div>
        </motion.div>
      )}

      {/* Issue Book Modal */}
      <Modal
        isOpen={isIssueModalOpen}
        onClose={() => setIsIssueModalOpen(false)}
        title="Issue Book"
      >
        <form onSubmit={handleIssueBook} className="space-y-4">
          <div>
            <label className="mb-1.5 block text-sm font-medium text-gray-700 dark:text-gray-300">
              Select Book *
            </label>
            <select
              required
              value={issueData.bookId}
              onChange={(e) => setIssueData({ ...issueData, bookId: e.target.value })}
              className="w-full rounded-lg border border-gray-300 bg-white px-4 py-2 text-gray-900 focus:border-primary-500 focus:outline-none focus:ring-2 focus:ring-primary-500/20 dark:border-gray-700 dark:bg-gray-800 dark:text-white"
            >
              <option value="">-- Select a book --</option>
              {books?.filter(book => book.copiesAvailable > 0).map(book => (
                <option key={book.id} value={book.id}>
                  {book.title} by {book.author} (Available: {book.copiesAvailable})
                </option>
              ))}
            </select>
          </div>

          <div>
            <label className="mb-1.5 block text-sm font-medium text-gray-700 dark:text-gray-300">
              Select User *
            </label>
            <select
              required
              value={issueData.userId}
              onChange={(e) => setIssueData({ ...issueData, userId: e.target.value })}
              className="w-full rounded-lg border border-gray-300 bg-white px-4 py-2 text-gray-900 focus:border-primary-500 focus:outline-none focus:ring-2 focus:ring-primary-500/20 dark:border-gray-700 dark:bg-gray-800 dark:text-white"
            >
              <option value="">-- Select a user --</option>
              {users?.filter(user => user.status === 'Active').map(user => (
                <option key={user.id} value={user.id}>
                  {user.name} ({user.role}) - {user.email}
                </option>
              ))}
            </select>
          </div>

          <div className="rounded-lg bg-blue-50 p-3 dark:bg-blue-900/20">
            <p className="text-sm text-blue-800 dark:text-blue-300">
              ℹ️ The book will be automatically assigned a 14-day due date from today.
            </p>
          </div>

          <div className="flex justify-end gap-3 pt-4">
            <Button
              type="button"
              variant="secondary"
              onClick={() => setIsIssueModalOpen(false)}
            >
              Cancel
            </Button>
            <Button type="submit" variant="primary">
              Issue Book
            </Button>
          </div>
        </form>
      </Modal>
    </div>
  );
}
