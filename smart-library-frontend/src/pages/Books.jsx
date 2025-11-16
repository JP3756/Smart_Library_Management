import { useState } from 'react';
import { useQuery } from '@tanstack/react-query';
import { motion } from 'framer-motion';
import { Search, Plus, BookOpen, Filter, Edit2, Trash2 } from 'lucide-react';
import { booksAPI } from '../services/api';
import { useAuth } from '../contexts/AuthContext';
import Button from '../components/Button';
import LoadingSpinner from '../components/LoadingSpinner';
import Modal from '../components/Modal';
import toast from 'react-hot-toast';

export default function Books() {
  const { isLibrarian } = useAuth();
  const [searchQuery, setSearchQuery] = useState('');
  const [categoryFilter, setCategoryFilter] = useState('All');
  const [statusFilter, setStatusFilter] = useState('All');
  const [isAddModalOpen, setIsAddModalOpen] = useState(false);
  const [isEditModalOpen, setIsEditModalOpen] = useState(false);
  const [selectedBook, setSelectedBook] = useState(null);
  const [newBook, setNewBook] = useState({
    title: '',
    author: '',
    isbn: '',
    category: '',
    publishedYear: '',
    totalCopies: 1,
    publisher: '',
  });

  const { data: books, isLoading, refetch } = useQuery({
    queryKey: ['books', searchQuery, categoryFilter, statusFilter],
    queryFn: () => booksAPI.getAll({ search: searchQuery, category: categoryFilter, status: statusFilter }),
    select: (response) => response.data,
  });

  const categories = [
    'All',
    'Fiction',
    'Non-Fiction',
    'Science Fiction',
    'Fantasy',
    'Mystery',
    'Thriller',
    'Romance',
    'Biography',
    'History',
    'Science',
    'Mathematics',
    'Physics',
    'Chemistry',
    'Biology',
    'Computer Science',
    'Programming',
    'Engineering',
    'Business',
    'Economics',
    'Finance',
    'Marketing',
    'Management',
    'Psychology',
    'Philosophy',
    'Religion',
    'Self-Help',
    'Health',
    'Medicine',
    'Art',
    'Music',
    'Literature',
    'Poetry',
    'Drama',
    'Children',
    'Young Adult',
    'Education',
    'Travel',
    'Cooking',
    'Sports',
    'Politics',
    'Law',
    'Environment',
    'Technology',
    'AI',
    'Database',
    'Networking',
    'Cybersecurity',
  ];
  const statuses = ['All', 'Available', 'Borrowed', 'Reserved'];

  const handleAddBook = async (e) => {
    e.preventDefault();
    
    try {
      // Map frontend fields to backend DTO
      const bookData = {
        isbn: newBook.isbn,
        title: newBook.title,
        author: newBook.author,
        category: newBook.category,
        publicationYear: parseInt(newBook.publishedYear) || new Date().getFullYear(),
        totalCopies: parseInt(newBook.totalCopies) || 1,
        publisher: newBook.publisher || 'Unknown Publisher',
      };
      
      await booksAPI.create(bookData);
      toast.success('Book added successfully!');
      setIsAddModalOpen(false);
      setNewBook({ title: '', author: '', isbn: '', category: '', publishedYear: '', totalCopies: 1, publisher: '' });
      refetch();
    } catch (error) {
      toast.error(error.response?.data?.message || 'Failed to add book');
    }
  };

  const handleEditBook = async (e) => {
    e.preventDefault();
    
    try {
      await booksAPI.update(selectedBook.id, selectedBook);
      toast.success('Book updated successfully!');
      setIsEditModalOpen(false);
      setSelectedBook(null);
      refetch();
    } catch (error) {
      toast.error(error.response?.data?.message || 'Failed to update book');
    }
  };

  const handleDeleteBook = async (bookId) => {
    if (!confirm('Are you sure you want to delete this book?')) return;
    
    try {
      await booksAPI.delete(bookId);
      toast.success('Book deleted successfully!');
      refetch();
    } catch (error) {
      toast.error(error.response?.data?.message || 'Failed to delete book');
    }
  };

  const openEditModal = (book) => {
    setSelectedBook({ ...book });
    setIsEditModalOpen(true);
  };

  const getStatusColor = (status) => {
    switch (status) {
      case 'Available':
        return 'bg-success-100 text-success-700 dark:bg-success-900/20 dark:text-success-400';
      case 'Borrowed':
        return 'bg-red-100 text-red-700 dark:bg-red-900/20 dark:text-red-400';
      case 'Reserved':
        return 'bg-yellow-100 text-yellow-700 dark:bg-yellow-900/20 dark:text-yellow-400';
      default:
        return 'bg-gray-100 text-gray-700 dark:bg-gray-800 dark:text-gray-400';
    }
  };

  return (
    <div className="space-y-6">
      {/* Header */}
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold text-gray-900 dark:text-white">Books</h1>
          <p className="mt-1 text-sm text-gray-600 dark:text-gray-400">
            Manage your library collection
          </p>
        </div>
        {isLibrarian() && (
          <Button onClick={() => setIsAddModalOpen(true)} className="flex items-center">
            <Plus className="mr-2 h-4 w-4" />
            Add Book
          </Button>
        )}
      </div>

      {/* Filters */}
      <div className="flex flex-wrap gap-4">
        {/* Search */}
        <div className="relative flex-1 min-w-[300px]">
          <Search className="absolute left-3 top-1/2 h-5 w-5 -translate-y-1/2 text-gray-400" />
          <input
            type="text"
            placeholder="Search by title, author, or ISBN..."
            value={searchQuery}
            onChange={(e) => setSearchQuery(e.target.value)}
            className="w-full rounded-lg border border-gray-300 bg-white py-2.5 pl-10 pr-4 text-sm text-gray-900 placeholder-gray-500 transition-colors focus:border-primary-500 focus:outline-none focus:ring-2 focus:ring-primary-500/20 dark:border-gray-700 dark:bg-gray-800 dark:text-white dark:placeholder-gray-400"
          />
        </div>

        {/* Category Filter */}
        <select
          value={categoryFilter}
          onChange={(e) => setCategoryFilter(e.target.value)}
          className="rounded-lg border border-gray-300 bg-white px-4 py-2.5 text-sm text-gray-900 transition-colors focus:border-primary-500 focus:outline-none focus:ring-2 focus:ring-primary-500/20 dark:border-gray-700 dark:bg-gray-800 dark:text-white"
        >
          {categories.map((cat) => (
            <option key={cat} value={cat}>
              {cat === 'All' ? 'All Categories' : cat}
            </option>
          ))}
        </select>

        {/* Status Filter */}
        <select
          value={statusFilter}
          onChange={(e) => setStatusFilter(e.target.value)}
          className="rounded-lg border border-gray-300 bg-white px-4 py-2.5 text-sm text-gray-900 transition-colors focus:border-primary-500 focus:outline-none focus:ring-2 focus:ring-primary-500/20 dark:border-gray-700 dark:bg-gray-800 dark:text-white"
        >
          {statuses.map((status) => (
            <option key={status} value={status}>
              {status === 'All' ? 'All Status' : status}
            </option>
          ))}
        </select>
      </div>

      {/* Books Grid */}
      {isLoading ? (
        <LoadingSpinner size="lg" className="py-12" />
      ) : (
        <motion.div
          initial={{ opacity: 0 }}
          animate={{ opacity: 1 }}
          className="grid gap-6 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4"
        >
          {books?.map((book, index) => (
            <motion.div
              key={book.id}
              initial={{ opacity: 0, y: 20 }}
              animate={{ opacity: 1, y: 0 }}
              transition={{ delay: index * 0.05 }}
              className="group rounded-lg border border-gray-200 bg-white p-5 shadow-sm transition-all hover:shadow-lg dark:border-gray-800 dark:bg-gray-900"
            >
              <div className="mb-4 flex items-start justify-between">
                <div className="flex h-12 w-12 items-center justify-center rounded-lg bg-primary-100 dark:bg-primary-900/20">
                  <BookOpen className="h-6 w-6 text-primary-600 dark:text-primary-400" />
                </div>
                <span
                  className={`rounded-full px-2.5 py-1 text-xs font-medium ${getStatusColor(
                    book.status
                  )}`}
                >
                  {book.status}
                </span>
              </div>

              <h3 className="mb-2 font-semibold text-gray-900 dark:text-white line-clamp-2">
                {book.title}
              </h3>
              <p className="mb-1 text-sm text-gray-600 dark:text-gray-400">
                by {book.author}
              </p>
              <p className="mb-3 text-xs text-gray-500 dark:text-gray-500">
                ISBN: {book.isbn}
              </p>

              <div className="flex items-center justify-between border-t border-gray-100 pt-3 dark:border-gray-800">
                <div className="text-xs text-gray-600 dark:text-gray-400">
                  <span className="font-medium text-gray-900 dark:text-white">
                    {book.copiesAvailable}
                  </span>{' '}
                  / {book.totalCopies} available
                </div>
                <span className="rounded bg-gray-100 px-2 py-1 text-xs font-medium text-gray-700 dark:bg-gray-800 dark:text-gray-300">
                  {book.category}
                </span>
              </div>

              {/* Action Buttons */}
              {isLibrarian() && (
                <div className="mt-3 flex gap-2 border-t border-gray-100 pt-3 dark:border-gray-800">
                  <Button
                    size="sm"
                    variant="secondary"
                    onClick={() => openEditModal(book)}
                    className="flex-1"
                  >
                    <Edit2 className="mr-1.5 h-3.5 w-3.5" />
                    Edit
                  </Button>
                  <Button
                    size="sm"
                    variant="danger"
                    onClick={() => handleDeleteBook(book.id)}
                    className="flex-1"
                  >
                    <Trash2 className="mr-1.5 h-3.5 w-3.5" />
                    Delete
                  </Button>
                </div>
              )}
            </motion.div>
          ))}
        </motion.div>
      )}

      {/* Add Book Modal */}
      <Modal
        isOpen={isAddModalOpen}
        onClose={() => setIsAddModalOpen(false)}
        title="Add New Book"
      >
        <form onSubmit={handleAddBook} className="space-y-4">
          <div>
            <label className="mb-1.5 block text-sm font-medium text-gray-700 dark:text-gray-300">
              Title *
            </label>
            <input
              type="text"
              required
              value={newBook.title}
              onChange={(e) => setNewBook({ ...newBook, title: e.target.value })}
              className="w-full rounded-lg border border-gray-300 bg-white px-4 py-2 text-gray-900 focus:border-primary-500 focus:outline-none focus:ring-2 focus:ring-primary-500/20 dark:border-gray-700 dark:bg-gray-800 dark:text-white"
            />
          </div>

          <div>
            <label className="mb-1.5 block text-sm font-medium text-gray-700 dark:text-gray-300">
              Author *
            </label>
            <input
              type="text"
              required
              value={newBook.author}
              onChange={(e) => setNewBook({ ...newBook, author: e.target.value })}
              className="w-full rounded-lg border border-gray-300 bg-white px-4 py-2 text-gray-900 focus:border-primary-500 focus:outline-none focus:ring-2 focus:ring-primary-500/20 dark:border-gray-700 dark:bg-gray-800 dark:text-white"
            />
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="mb-1.5 block text-sm font-medium text-gray-700 dark:text-gray-300">
                ISBN *
              </label>
              <input
                type="text"
                required
                value={newBook.isbn}
                onChange={(e) => setNewBook({ ...newBook, isbn: e.target.value })}
                className="w-full rounded-lg border border-gray-300 bg-white px-4 py-2 text-gray-900 focus:border-primary-500 focus:outline-none focus:ring-2 focus:ring-primary-500/20 dark:border-gray-700 dark:bg-gray-800 dark:text-white"
              />
            </div>

            <div>
              <label className="mb-1.5 block text-sm font-medium text-gray-700 dark:text-gray-300">
                Year
              </label>
              <input
                type="number"
                value={newBook.publishedYear}
                onChange={(e) => setNewBook({ ...newBook, publishedYear: e.target.value })}
                className="w-full rounded-lg border border-gray-300 bg-white px-4 py-2 text-gray-900 focus:border-primary-500 focus:outline-none focus:ring-2 focus:ring-primary-500/20 dark:border-gray-700 dark:bg-gray-800 dark:text-white"
              />
            </div>
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="mb-1.5 block text-sm font-medium text-gray-700 dark:text-gray-300">
                Category *
              </label>
              <select
                required
                value={newBook.category}
                onChange={(e) => setNewBook({ ...newBook, category: e.target.value })}
                className="w-full rounded-lg border border-gray-300 bg-white px-4 py-2 text-gray-900 focus:border-primary-500 focus:outline-none focus:ring-2 focus:ring-primary-500/20 dark:border-gray-700 dark:bg-gray-800 dark:text-white"
              >
                <option value="">Select...</option>
                {categories.filter((c) => c !== 'All').map((cat) => (
                  <option key={cat} value={cat}>
                    {cat}
                  </option>
                ))}
              </select>
            </div>

            <div>
              <label className="mb-1.5 block text-sm font-medium text-gray-700 dark:text-gray-300">
                Copies *
              </label>
              <input
                type="number"
                required
                min="1"
                value={newBook.totalCopies}
                onChange={(e) => setNewBook({ ...newBook, totalCopies: parseInt(e.target.value) })}
                className="w-full rounded-lg border border-gray-300 bg-white px-4 py-2 text-gray-900 focus:border-primary-500 focus:outline-none focus:ring-2 focus:ring-primary-500/20 dark:border-gray-700 dark:bg-gray-800 dark:text-white"
              />
            </div>
          </div>

          <div>
            <label className="mb-1.5 block text-sm font-medium text-gray-700 dark:text-gray-300">
              Publisher
            </label>
            <input
              type="text"
              value={newBook.publisher}
              onChange={(e) => setNewBook({ ...newBook, publisher: e.target.value })}
              placeholder="e.g., Pearson, O'Reilly, McGraw-Hill"
              className="w-full rounded-lg border border-gray-300 bg-white px-4 py-2 text-gray-900 focus:border-primary-500 focus:outline-none focus:ring-2 focus:ring-primary-500/20 dark:border-gray-700 dark:bg-gray-800 dark:text-white"
            />
          </div>

          <div className="flex justify-end gap-3 pt-4">
            <Button
              type="button"
              variant="secondary"
              onClick={() => setIsAddModalOpen(false)}
            >
              Cancel
            </Button>
            <Button type="submit" variant="primary">
              Add Book
            </Button>
          </div>
        </form>
      </Modal>

      {/* Edit Book Modal */}
      <Modal
        isOpen={isEditModalOpen}
        onClose={() => setIsEditModalOpen(false)}
        title="Edit Book"
      >
        <form onSubmit={handleEditBook} className="space-y-4">
          <div>
            <label className="mb-1.5 block text-sm font-medium text-gray-700 dark:text-gray-300">
              Title *
            </label>
            <input
              type="text"
              required
              value={selectedBook?.title || ''}
              onChange={(e) => setSelectedBook({ ...selectedBook, title: e.target.value })}
              className="w-full rounded-lg border border-gray-300 bg-white px-4 py-2 text-gray-900 focus:border-primary-500 focus:outline-none focus:ring-2 focus:ring-primary-500/20 dark:border-gray-700 dark:bg-gray-800 dark:text-white"
            />
          </div>

          <div>
            <label className="mb-1.5 block text-sm font-medium text-gray-700 dark:text-gray-300">
              Author *
            </label>
            <input
              type="text"
              required
              value={selectedBook?.author || ''}
              onChange={(e) => setSelectedBook({ ...selectedBook, author: e.target.value })}
              className="w-full rounded-lg border border-gray-300 bg-white px-4 py-2 text-gray-900 focus:border-primary-500 focus:outline-none focus:ring-2 focus:ring-primary-500/20 dark:border-gray-700 dark:bg-gray-800 dark:text-white"
            />
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="mb-1.5 block text-sm font-medium text-gray-700 dark:text-gray-300">
                Year
              </label>
              <input
                type="number"
                value={selectedBook?.publishedYear || ''}
                onChange={(e) => setSelectedBook({ ...selectedBook, publishedYear: e.target.value })}
                className="w-full rounded-lg border border-gray-300 bg-white px-4 py-2 text-gray-900 focus:border-primary-500 focus:outline-none focus:ring-2 focus:ring-primary-500/20 dark:border-gray-700 dark:bg-gray-800 dark:text-white"
              />
            </div>

            <div>
              <label className="mb-1.5 block text-sm font-medium text-gray-700 dark:text-gray-300">
                Category *
              </label>
              <select
                required
                value={selectedBook?.category || ''}
                onChange={(e) => setSelectedBook({ ...selectedBook, category: e.target.value })}
                className="w-full rounded-lg border border-gray-300 bg-white px-4 py-2 text-gray-900 focus:border-primary-500 focus:outline-none focus:ring-2 focus:ring-primary-500/20 dark:border-gray-700 dark:bg-gray-800 dark:text-white"
              >
                <option value="">Select...</option>
                {categories.filter((c) => c !== 'All').map((cat) => (
                  <option key={cat} value={cat}>
                    {cat}
                  </option>
                ))}
              </select>
            </div>
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="mb-1.5 block text-sm font-medium text-gray-700 dark:text-gray-300">
                Total Copies *
              </label>
              <input
                type="number"
                required
                min="1"
                value={selectedBook?.totalCopies || 1}
                onChange={(e) => setSelectedBook({ ...selectedBook, totalCopies: parseInt(e.target.value) })}
                className="w-full rounded-lg border border-gray-300 bg-white px-4 py-2 text-gray-900 focus:border-primary-500 focus:outline-none focus:ring-2 focus:ring-primary-500/20 dark:border-gray-700 dark:bg-gray-800 dark:text-white"
              />
            </div>

            <div>
              <label className="mb-1.5 block text-sm font-medium text-gray-700 dark:text-gray-300">
                Publisher
              </label>
              <input
                type="text"
                value={selectedBook?.publisher || ''}
                onChange={(e) => setSelectedBook({ ...selectedBook, publisher: e.target.value })}
                className="w-full rounded-lg border border-gray-300 bg-white px-4 py-2 text-gray-900 focus:border-primary-500 focus:outline-none focus:ring-2 focus:ring-primary-500/20 dark:border-gray-700 dark:bg-gray-800 dark:text-white"
              />
            </div>
          </div>

          <div className="flex justify-end gap-3 pt-4">
            <Button
              type="button"
              variant="secondary"
              onClick={() => setIsAddModalOpen(false)}
            >
              Cancel
            </Button>
            <Button type="submit" variant="primary">
              Add Book
            </Button>
          </div>
        </form>
      </Modal>
    </div>
  );
}
