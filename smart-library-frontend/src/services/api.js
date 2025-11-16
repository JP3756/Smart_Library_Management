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
    try {
      const response = await api.post('/auth/login', {
        email: credentials.email,
        password: credentials.password || '' // Password not required in simple auth
      });
      return response;
    } catch (error) {
      console.error('Login error:', error);
      throw error;
    }
  },
  
  logout: () => {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
  },
  
  getCurrentUser: async () => {
    return api.get('/auth/me');
  },
  
  // Development helper - get list of available test users
  getUsersList: async () => {
    return api.get('/auth/users-list');
  },
};

// Books
export const booksAPI = {
  getAll: async (params) => {
    try {
      let url = '/books';
      
      // Priority: search > category > status
      // Note: Backend doesn't support combined filters, so we fetch and filter client-side
      if (params?.search && params.search.trim()) {
        url = `/books/search?query=${encodeURIComponent(params.search.trim())}`;
      } else if (params?.category && params.category !== 'All') {
        url = `/books/category/${encodeURIComponent(params.category)}`;
      } else if (params?.status === 'Available') {
        url = '/books/available';
      }
      
      const response = await api.get(url);
      
      // Handle OData-style response with "value" wrapper
      let booksData = Array.isArray(response.data) ? response.data : (response.data.value || []);
      
      // Map backend response to frontend format
      let books = booksData.map(book => ({
        id: book.id,
        title: book.title,
        author: book.author,
        isbn: book.isbn,
        category: book.category,
        status: book.availableCopies > 0 ? 'Available' : 'Out of Stock',
        publishedYear: book.publicationYear,
        copiesAvailable: book.availableCopies,
        totalCopies: book.totalCopies,
        publisher: book.publisher,
        description: book.description,
      }));
      
      // Apply client-side filters if search was used but other filters are set
      if (params?.search && params.search.trim()) {
        // Apply category filter
        if (params?.category && params.category !== 'All') {
          books = books.filter(book => book.category === params.category);
        }
        // Apply status filter
        if (params?.status && params.status !== 'All') {
          books = books.filter(book => book.status === params.status);
        }
      }
      
      return { data: books };
    } catch (error) {
      console.error('Books API Error:', error);
      throw error;
    }
  },
  
  getById: async (id) => {
    const response = await api.get(`/books/${id}`);
    const book = response.data;
    return {
      data: {
        id: book.id,
        title: book.title,
        author: book.author,
        isbn: book.isbn,
        category: book.category,
        status: book.availableCopies > 0 ? 'Available' : 'Out of Stock',
        publishedYear: book.publicationYear,
        copiesAvailable: book.availableCopies,
        totalCopies: book.totalCopies,
        publisher: book.publisher,
        description: book.description,
      }
    };
  },
  
  create: async (bookData) => {
    // Map frontend data to backend DTO
    const createDto = {
      isbn: bookData.isbn,
      title: bookData.title,
      author: bookData.author,
      publisher: bookData.publisher || 'Unknown Publisher',
      publicationYear: parseInt(bookData.publicationYear) || new Date().getFullYear(),
      category: bookData.category,
      description: bookData.description || '',
      totalCopies: parseInt(bookData.totalCopies) || 1,
    };
    return api.post('/books', createDto);
  },
  
  update: async (id, bookData) => {
    // Map frontend data to backend UpdateBookDto
    const updateDto = {
      title: bookData.title,
      author: bookData.author,
      publisher: bookData.publisher,
      publicationYear: parseInt(bookData.publicationYear),
      category: bookData.category,
      description: bookData.description,
      totalCopies: parseInt(bookData.totalCopies),
    };
    return api.put(`/books/${id}`, updateDto);
  },
  
  delete: async (id) => {
    return api.delete(`/books/${id}`);
  },
};

// Users
export const usersAPI = {
  getAll: async (params) => {
    try {
      let url = '/users';
      
      // Priority: search > role filter
      if (params?.search && params.search.trim()) {
        url = `/users/search?query=${encodeURIComponent(params.search.trim())}`;
      } else if (params?.role && params.role !== 'All') {
        url = `/users/type/${params.role}`;
      }
      
      const response = await api.get(url);
      
      // Handle OData-style response with "value" wrapper
      let usersData = Array.isArray(response.data) ? response.data : (response.data.value || []);
      
      // Map backend response to frontend format
      let users = usersData.map(user => ({
        id: user.id,
        name: user.name,
        email: user.email,
        phone: user.phone,
        role: user.type || user.userType, // Backend uses "type"
        status: user.isActive ? 'Active' : 'Inactive',
        membershipId: user.studentId || user.employeeId || `USR-${user.id}`,
        booksIssued: user.booksIssued || 0,
        maxBorrowLimit: user.maxBorrowLimit,
      }));
      
      // Apply client-side filters if search was used but role filter is set
      if (params?.search && params.search.trim()) {
        if (params?.role && params.role !== 'All') {
          users = users.filter(user => user.role === params.role);
        }
      }
      
      return { data: users };
    } catch (error) {
      console.error('Users API Error:', error);
      throw error;
    }
  },
  
  getById: async (id) => {
    const response = await api.get(`/users/${id}`);
    const user = response.data;
    return {
      data: {
        id: user.id,
        name: user.name,
        email: user.email,
        phone: user.phone,
        role: user.type || user.userType,
        membershipId: user.studentId || user.employeeId || `USER-${user.id}`,
        status: user.isActive ? 'Active' : 'Inactive',
        maxBorrowLimit: user.maxBorrowLimit,
      }
    };
  },
  
  create: async (userData) => {
    // Map frontend fields to backend CreateUserDto
    const createDto = {
      name: userData.name,
      email: userData.email,
      phone: userData.phone || null,
      type: userData.role, // Backend expects "type" (Student/Faculty)
      studentId: userData.role === 'Student' ? userData.membershipId : null,
      enrollmentYear: userData.role === 'Student' ? new Date().getFullYear() : null,
      employeeId: userData.role === 'Faculty' ? userData.membershipId : null,
      department: userData.role === 'Faculty' ? (userData.department || 'General') : null,
    };
    return api.post('/users', createDto);
  },
  
  update: async (id, userData) => {
    const updateDto = {
      name: userData.name,
      email: userData.email,
      phone: userData.phone,
      isActive: userData.status === 'Active',
    };
    return api.put(`/users/${id}`, updateDto);
  },
  
  delete: async (id) => {
    return api.delete(`/users/${id}`);
  },
};

// Borrow/Return
export const borrowAPI = {
  getAll: async () => {
    try {
      const response = await api.get('/loans');
      
      // Map backend loan response to frontend format
      const loans = response.data.map(loan => ({
        id: loan.id,
        bookId: loan.bookId,
        userId: loan.userId,
        bookTitle: loan.bookTitle || 'Unknown Book',
        userName: loan.userName || 'Unknown User',
        borrowDate: loan.borrowDate,
        dueDate: loan.dueDate,
        returnDate: loan.returnDate,
        status: loan.status === 0 ? 'Active' : loan.status === 1 ? 'Returned' : 'Overdue',
        fine: loan.fine || 0,
        daysOverdue: loan.daysOverdue || 0,
      }));
      
      return { data: loans };
    } catch (error) {
      console.error('Loans API Error:', error);
      throw error;
    }
  },
  
  getActive: async () => {
    try {
      const response = await api.get('/loans/active');
      return { data: response.data };
    } catch (error) {
      console.error('Active Loans API Error:', error);
      throw error;
    }
  },
  
  getOverdue: async () => {
    try {
      const response = await api.get('/loans/overdue');
      return { data: response.data };
    } catch (error) {
      console.error('Overdue Loans API Error:', error);
      throw error;
    }
  },
  
  borrowBook: async (data) => {
    // Map to backend CreateLoanDto - expects bookId and userId
    const createDto = {
      bookId: parseInt(data.bookId),
      userId: parseInt(data.userId),
    };
    return api.post('/loans', createDto);
  },
  
  returnBook: async (id) => {
    return api.post(`/loans/${id}/return`);
  },
  
  deleteLoan: async (id) => {
    return api.delete(`/loans/${id}`);
  },
};

// Reports
export const reportsAPI = {
  getStats: async () => {
    try {
      const response = await api.get('/reports/stats');
      
      // Map backend statistics to frontend format
      return {
        data: {
          totalBooks: response.data.totalBooks || 0,
          booksIssued: response.data.activeLoans || 0,
          booksAvailable: response.data.totalBooks - response.data.activeLoans || 0,
          totalUsers: response.data.totalUsers || 0,
          activeLoans: response.data.activeLoans || 0,
          overdueBooks: response.data.overdueLoans || 0,
          totalFines: response.data.pendingFines || 0,
          newMembersThisMonth: 0,
        }
      };
    } catch (error) {
      console.error('Reports API Error:', error);
      // Return fallback data
      return {
        data: {
          totalBooks: 0,
          booksIssued: 0,
          booksAvailable: 0,
          totalUsers: 0,
          activeLoans: 0,
          overdueBooks: 0,
          totalFines: 0,
          newMembersThisMonth: 0,
        }
      };
    }
  },
  
  getBorrowingReport: async () => {
    try {
      const response = await api.get('/reports/borrowing');
      return { data: response.data };
    } catch (error) {
      console.error('Borrowing Report Error:', error);
      return { data: [] };
    }
  },
  
  getBooksByCategory: async () => {
    try {
      const response = await api.get('/reports/books-by-category');
      return { data: response.data };
    } catch (error) {
      console.error('Books by Category Error:', error);
      return { data: [] };
    }
  },
  
  getUserStatistics: async () => {
    try {
      const response = await api.get('/reports/user-statistics');
      return { data: response.data };
    } catch (error) {
      console.error('User Statistics Error:', error);
      return { data: [] };
    }
  },
  
  getFinesReport: async () => {
    try {
      const response = await api.get('/reports/fines');
      return { data: response.data };
    } catch (error) {
      console.error('Fines Report Error:', error);
      return { data: [] };
    }
  },
};

export default api;
