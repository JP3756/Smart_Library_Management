import axios from 'axios';

// Axios instance with base configuration
const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL || 'http://localhost:5208/api',
  headers: {
    'Content-Type': 'application/json',
  },
});

// Request interceptor - add JWT token to headers
api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Response interceptor - handle errors globally
api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      localStorage.removeItem('token');
      localStorage.removeItem('user');
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

// ============================================
// MOCK DATA (until backend is ready)
// ============================================

export const mockBooks = [
  {
    id: 1,
    title: 'The Art of War',
    author: 'Sun Tzu',
    isbn: '978-1599869773',
    category: 'Strategy',
    status: 'Borrowed',
    publishedYear: 2005,
    copiesAvailable: 2,
    totalCopies: 3,
  },
  {
    id: 2,
    title: '48 Laws of Power',
    author: 'Robert Greene',
    isbn: '978-0140280197',
    category: 'Self-Help',
    status: 'Borrowed',
    publishedYear: 2000,
    copiesAvailable: 1,
    totalCopies: 2,
  },
  {
    id: 3,
    title: 'Clean Code',
    author: 'Robert C. Martin',
    isbn: '978-0132350884',
    category: 'Programming',
    status: 'Available',
    publishedYear: 2008,
    copiesAvailable: 3,
    totalCopies: 5,
  },
  {
    id: 4,
    title: 'Design Patterns',
    author: 'Gang of Four',
    isbn: '978-0201633610',
    category: 'Programming',
    status: 'Available',
    publishedYear: 1994,
    copiesAvailable: 2,
    totalCopies: 3,
  },
  {
    id: 5,
    title: 'Introduction to Algorithms',
    author: 'Thomas H. Cormen',
    isbn: '978-0262033848',
    category: 'Computer Science',
    status: 'Borrowed',
    publishedYear: 2009,
    copiesAvailable: 0,
    totalCopies: 4,
  },
  {
    id: 6,
    title: 'The Pragmatic Programmer',
    author: 'Andrew Hunt',
    isbn: '978-0135957059',
    category: 'Programming',
    status: 'Available',
    publishedYear: 2019,
    copiesAvailable: 5,
    totalCopies: 5,
  },
  {
    id: 7,
    title: 'Artificial Intelligence: A Modern Approach',
    author: 'Stuart Russell',
    isbn: '978-0134610993',
    category: 'AI',
    status: 'Reserved',
    publishedYear: 2020,
    copiesAvailable: 1,
    totalCopies: 2,
  },
];

export const mockUsers = [
  {
    id: 1,
    name: 'JP Cabaluna',
    email: 'jp.cabaluna@university.edu',
    role: 'Student',
    membershipId: 'STU2024100',
    joinedDate: '2024-09-15',
    status: 'Active',
    booksIssued: 1,
  },
  {
    id: 2,
    name: 'Jake Sucgang',
    email: 'jake.sucgang@university.edu',
    role: 'Student',
    membershipId: 'STU2024101',
    joinedDate: '2024-09-20',
    status: 'Active',
    booksIssued: 1,
  },
  {
    id: 3,
    name: 'John Doe',
    email: 'john.doe@university.edu',
    role: 'Student',
    membershipId: 'STU2024001',
    joinedDate: '2024-01-15',
    status: 'Active',
    booksIssued: 2,
  },
  {
    id: 4,
    name: 'Jane Smith',
    email: 'jane.smith@university.edu',
    role: 'Faculty',
    membershipId: 'FAC2023045',
    joinedDate: '2023-09-01',
    status: 'Active',
    booksIssued: 5,
  },
  {
    id: 5,
    name: 'Mike Johnson',
    email: 'mike.j@university.edu',
    role: 'Student',
    membershipId: 'STU2024012',
    joinedDate: '2024-02-10',
    status: 'Active',
    booksIssued: 1,
  },
  {
    id: 6,
    name: 'Sarah Williams',
    email: 'sarah.w@university.edu',
    role: 'Student',
    membershipId: 'STU2023098',
    joinedDate: '2023-08-20',
    status: 'Suspended',
    booksIssued: 0,
  },
];

export const mockBorrowRecords = [
  {
    id: 1,
    bookTitle: 'The Art of War',
    userName: 'JP Cabaluna',
    borrowDate: '2024-10-20',
    dueDate: '2024-11-20',
    returnDate: null,
    status: 'Active',
    fine: 0,
  },
  {
    id: 2,
    bookTitle: '48 Laws of Power',
    userName: 'Jake Sucgang',
    borrowDate: '2024-10-18',
    dueDate: '2024-11-18',
    returnDate: null,
    status: 'Active',
    fine: 0,
  },
  {
    id: 3,
    bookTitle: 'Clean Code',
    userName: 'John Doe',
    borrowDate: '2024-10-15',
    dueDate: '2024-11-15',
    returnDate: null,
    status: 'Active',
    fine: 0,
  },
  {
    id: 4,
    bookTitle: 'Introduction to Algorithms',
    userName: 'Jane Smith',
    borrowDate: '2024-10-01',
    dueDate: '2024-10-31',
    returnDate: null,
    status: 'Overdue',
    fine: 5.00,
  },
  {
    id: 5,
    bookTitle: 'Design Patterns',
    userName: 'Mike Johnson',
    borrowDate: '2024-09-20',
    dueDate: '2024-10-20',
    returnDate: '2024-10-18',
    status: 'Returned',
    fine: 0,
  },
];

export const mockReportStats = {
  totalBooks: 245,
  booksIssued: 87,
  booksAvailable: 158,
  totalUsers: 342,
  activeLoans: 87,
  overdueBooks: 12,
  totalFines: 245.50,
  newMembersThisMonth: 23,
};

// ============================================
// API FUNCTIONS
// ============================================

// Auth
export const authAPI = {
  login: async (credentials) => {
    // Mock login - replace with real API call
    return new Promise((resolve) => {
      setTimeout(() => {
        if (credentials.email && credentials.password) {
          const mockToken = 'mock-jwt-token-' + Date.now();
          const mockUser = {
            id: 1,
            name: 'Librarian Admin',
            email: credentials.email,
            role: 'Librarian',
          };
          resolve({ data: { token: mockToken, user: mockUser } });
        } else {
          throw new Error('Invalid credentials');
        }
      }, 1000);
    });
  },
  logout: () => {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
  },
};

// Books
export const booksAPI = {
  getAll: async (params) => {
    const queryParams = new URLSearchParams();
    if (params?.search) queryParams.append('search', params.search);
    if (params?.category && params.category !== 'All') queryParams.append('category', params.category);
    if (params?.status && params.status !== 'All') queryParams.append('status', params.status);
    
    const url = queryParams.toString() ? `/books?${queryParams}` : '/books';
    return api.get(url);
  },
  getById: async (id) => {
    return api.get(`/books/${id}`);
  },
  create: async (bookData) => {
    return api.post('/books', bookData);
  },
  update: async (id, bookData) => {
    return api.put(`/books/${id}`, bookData);
  },
  delete: async (id) => {
    return api.delete(`/books/${id}`);
  },
};

// Users
export const usersAPI = {
  getAll: async (params) => {
    const queryParams = new URLSearchParams();
    if (params?.search) queryParams.append('search', params.search);
    if (params?.role && params.role !== 'All') queryParams.append('type', params.role);
    
    const url = queryParams.toString() ? `/users?${queryParams}` : '/users';
    return api.get(url);
  },
  getById: async (id) => {
    return api.get(`/users/${id}`);
  },
  create: async (userData) => {
    // Map frontend fields to backend DTO
    const createDto = {
      name: userData.name,
      email: userData.email,
      type: userData.role, // Backend expects "type" (Student/Faculty)
      studentId: userData.role === 'Student' ? userData.membershipId : null,
      employeeId: userData.role === 'Faculty' ? userData.membershipId : null,
    };
    return api.post('/users', createDto);
  },
};

// Borrow/Return
export const borrowAPI = {
  getAll: async () => {
    return api.get('/loans');
  },
  borrowBook: async (data) => {
    // Map to backend CreateLoanDto
    const createDto = {
      bookId: data.bookId,
      userId: data.userId,
      dueDate: data.dueDate,
    };
    return api.post('/loans', createDto);
  },
  returnBook: async (id) => {
    return api.post(`/loans/${id}/return`);
  },
};

// Reports
export const reportsAPI = {
  getStats: async () => {
    return api.get('/reports/statistics');
  },
};

export default api;
