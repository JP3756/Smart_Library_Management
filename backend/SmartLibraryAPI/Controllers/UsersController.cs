using Microsoft.AspNetCore.Mvc;
using SmartLibraryAPI.DTOs;
using SmartLibraryAPI.Factories;
using SmartLibraryAPI.Interfaces;
using SmartLibraryAPI.Models;

namespace SmartLibraryAPI.Controllers
{
    /// <summary>
    /// Users API Controller - Handles user management (Students and Faculty)
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserFactory _userFactory;

        public UsersController(IUserRepository userRepository, IUserFactory userFactory)
        {
            _userRepository = userRepository;
            _userFactory = userFactory;
        }

        /// <summary>
        /// Get all users
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            var users = await _userRepository.GetAllAsync();
            var userDtos = users.Select(u => MapToDto(u));
            return Ok(userDtos);
        }

        /// <summary>
        /// Get user by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound(new { message = $"User with ID {id} not found" });

            return Ok(MapToDto(user));
        }

        /// <summary>
        /// Search users by name or email
        /// </summary>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<UserDto>>> Search([FromQuery] string query)
        {
            if (string.IsNullOrEmpty(query))
                return BadRequest(new { message = "Query parameter is required" });

            var allUsers = await _userRepository.GetAllAsync();
            var filteredUsers = allUsers.Where(u => 
                u.Name.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                u.Email.Contains(query, StringComparison.OrdinalIgnoreCase)
            );
            var userDtos = filteredUsers.Select(u => MapToDto(u));
            return Ok(userDtos);
        }

        /// <summary>
        /// Get users by type (Student or Faculty)
        /// </summary>
        [HttpGet("type/{userType}")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetByType(string userType)
        {
            var allUsers = await _userRepository.GetAllAsync();
            var filteredUsers = allUsers.Where(u => u.GetUserType().Equals(userType, StringComparison.OrdinalIgnoreCase));
            var userDtos = filteredUsers.Select(u => MapToDto(u));
            return Ok(userDtos);
        }

        /// <summary>
        /// Get user borrow limit (demonstrates polymorphism)
        /// </summary>
        [HttpGet("{id}/borrow-limit")]
        public async Task<ActionResult<object>> GetBorrowLimit(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound(new { message = $"User with ID {id} not found" });

            return Ok(new { maxBorrowLimit = user.GetMaxBorrowLimit(), userType = user.GetUserType() });
        }

        /// <summary>
        /// Get all students
        /// </summary>
        [HttpGet("students")]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudents()
        {
            var students = await _userRepository.GetAllStudentsAsync();
            var studentDtos = students.Select(s => MapToStudentDto(s));
            return Ok(studentDtos);
        }

        /// <summary>
        /// Get all faculty members
        /// </summary>
        [HttpGet("faculty")]
        public async Task<ActionResult<IEnumerable<FacultyDto>>> GetFaculty()
        {
            var faculty = await _userRepository.GetAllFacultyAsync();
            var facultyDtos = faculty.Select(f => MapToFacultyDto(f));
            return Ok(facultyDtos);
        }

        /// <summary>
        /// Get user by email
        /// </summary>
        [HttpGet("email/{email}")]
        public async Task<ActionResult<UserDto>> GetUserByEmail(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
                return NotFound(new { message = $"User with email {email} not found" });

            return Ok(MapToDto(user));
        }

        /// <summary>
        /// Get user with loans
        /// </summary>
        [HttpGet("{id}/loans")]
        public async Task<ActionResult<UserDto>> GetUserWithLoans(int id)
        {
            var user = await _userRepository.GetUserWithLoansAsync(id);
            if (user == null)
                return NotFound(new { message = $"User with ID {id} not found" });

            return Ok(MapToDto(user));
        }

        /// <summary>
        /// Create new user (using Factory Pattern)
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser(CreateUserDto createUserDto)
        {
            try
            {
                // Check if email already exists
                var existingUser = await _userRepository.GetByEmailAsync(createUserDto.Email);
                if (existingUser != null)
                    return Conflict(new { message = "User with this email already exists" });

                // Determine user type (support both Type and UserType fields)
                var userType = !string.IsNullOrEmpty(createUserDto.Type) ? createUserDto.Type : createUserDto.UserType;
                
                // Use Factory Pattern to create user
                var user = _userFactory.CreateUser(userType, createUserDto.Name, createUserDto.Email);
                
                // Set common properties
                user.Phone = createUserDto.Phone;

                // Set type-specific properties
                if (user is Student student && userType.Equals("student", StringComparison.OrdinalIgnoreCase))
                {
                    student.StudentId = createUserDto.StudentId ?? $"STU{DateTime.UtcNow:yyyyMMddHHmmss}";
                    student.Program = createUserDto.Program ?? "Computer Science";
                    student.YearLevel = createUserDto.YearLevel ?? 1;
                    student.Department = createUserDto.Department ?? "Engineering";
                }
                else if (user is Faculty faculty && userType.Equals("faculty", StringComparison.OrdinalIgnoreCase))
                {
                    faculty.EmployeeId = createUserDto.EmployeeId ?? $"FAC{DateTime.UtcNow:yyyyMMddHHmmss}";
                    faculty.Department = createUserDto.Department ?? "Computer Science";
                    faculty.Position = createUserDto.Position ?? "Instructor";
                    faculty.Specialization = createUserDto.Specialization ?? "";
                }

                var createdUser = await _userRepository.AddAsync(user);
                return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, MapToDto(createdUser));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Update user
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<UserDto>> UpdateUser(int id, CreateUserDto updateUserDto)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id);
                if (user == null)
                    return NotFound(new { message = $"User with ID {id} not found" });

                // Update common properties
                if (!string.IsNullOrEmpty(updateUserDto.Name))
                    user.Name = updateUserDto.Name;
                if (!string.IsNullOrEmpty(updateUserDto.Email))
                    user.Email = updateUserDto.Email;
                user.Phone = updateUserDto.Phone;

                // Update type-specific properties
                if (user is Student student)
                {
                    if (!string.IsNullOrEmpty(updateUserDto.Program))
                        student.Program = updateUserDto.Program;
                    if (updateUserDto.YearLevel.HasValue)
                        student.YearLevel = updateUserDto.YearLevel.Value;
                    if (!string.IsNullOrEmpty(updateUserDto.Department))
                        student.Department = updateUserDto.Department;
                }
                else if (user is Faculty faculty)
                {
                    if (!string.IsNullOrEmpty(updateUserDto.Department))
                        faculty.Department = updateUserDto.Department;
                    if (!string.IsNullOrEmpty(updateUserDto.Position))
                        faculty.Position = updateUserDto.Position;
                    if (!string.IsNullOrEmpty(updateUserDto.Specialization))
                        faculty.Specialization = updateUserDto.Specialization;
                }

                var updatedUser = await _userRepository.UpdateAsync(user);
                return Ok(MapToDto(updatedUser));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Delete user
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var success = await _userRepository.DeleteAsync(id);
            if (!success)
                return NotFound(new { message = $"User with ID {id} not found" });

            return NoContent();
        }

        // Helper methods to map entities to DTOs
        private UserDto MapToDto(User user)
        {
            var dto = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                UserType = user.GetUserType(),
                IsActive = user.IsActive,
                BooksIssued = user.Loans.Count(l => l.Status == LoanStatus.Active),
                MaxBorrowLimit = user.GetMaxBorrowLimit()
            };

            return dto;
        }

        private StudentDto MapToStudentDto(Student student)
        {
            return new StudentDto
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email,
                Phone = student.Phone,
                UserType = student.GetUserType(),
                IsActive = student.IsActive,
                BooksIssued = student.Loans.Count(l => l.Status == LoanStatus.Active),
                MaxBorrowLimit = student.GetMaxBorrowLimit(),
                StudentId = student.StudentId,
                Program = student.Program,
                YearLevel = student.YearLevel,
                Department = student.Department
            };
        }

        private FacultyDto MapToFacultyDto(Faculty faculty)
        {
            return new FacultyDto
            {
                Id = faculty.Id,
                Name = faculty.Name,
                Email = faculty.Email,
                Phone = faculty.Phone,
                UserType = faculty.GetUserType(),
                IsActive = faculty.IsActive,
                BooksIssued = faculty.Loans.Count(l => l.Status == LoanStatus.Active),
                MaxBorrowLimit = faculty.GetMaxBorrowLimit(),
                EmployeeId = faculty.EmployeeId,
                Department = faculty.Department,
                Position = faculty.Position,
                Specialization = faculty.Specialization
            };
        }
    }
}
