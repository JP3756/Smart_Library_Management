using Microsoft.AspNetCore.Mvc;
using SmartLibraryAPI.Data;
using SmartLibraryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace SmartLibraryAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly LibraryDbContext _context;
    private readonly ILogger<AuthController> _logger;

    public AuthController(LibraryDbContext context, ILogger<AuthController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Login endpoint - Simple email-based authentication
    /// </summary>
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Email))
            {
                return BadRequest(new { message = "Email is required" });
            }

            // Find user by email (case-insensitive)
            var user = await _context.Users
                .FirstOrDefaultAsync(u => EF.Functions.ILike(u.Email, request.Email));

            if (user == null)
            {
                return Unauthorized(new { message = "Invalid email. User not found." });
            }

            if (!user.IsActive)
            {
                return Unauthorized(new { message = "Account is inactive. Please contact the librarian." });
            }

            // Determine user role based on type
            string role = user switch
            {
                Student => "Student",
                Faculty => "Faculty",
                Librarian => "Librarian",
                _ => "User"
            };

            // Create simple token (in production, use JWT)
            var token = GenerateSimpleToken(user.Id, user.Email);

            // Get additional user info
            var userInfo = new
            {
                id = user.Id,
                name = user.Name,
                email = user.Email,
                role = role,
                phone = user.Phone,
                isActive = user.IsActive,
                membershipId = user is Student student ? student.StudentId : 
                               user is Faculty faculty ? faculty.EmployeeId : 
                               user is Librarian librarian ? librarian.EmployeeId :
                               $"USR-{user.Id}",
                department = user is Faculty fac ? fac.Department : 
                             user is Librarian lib ? lib.Department : null,
                maxBorrowLimit = user.GetMaxBorrowLimit(),
                maxBorrowDays = user.GetMaxBorrowDays()
            };

            _logger.LogInformation("User {Email} logged in successfully as {Role}", user.Email, role);

            return Ok(new LoginResponse
            {
                Token = token,
                User = userInfo
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for email: {Email}", request.Email);
            return StatusCode(500, new { message = "An error occurred during login" });
        }
    }

    /// <summary>
    /// Get current user info (if we had real JWT, we'd validate token here)
    /// </summary>
    [HttpGet("me")]
    public ActionResult GetCurrentUser()
    {
        // In production, extract user ID from JWT token
        // For now, just return success
        return Ok(new { message = "User authentication endpoint" });
    }

    /// <summary>
    /// Get all users for login testing (Development only)
    /// </summary>
    [HttpGet("users-list")]
    public async Task<ActionResult> GetUsersList()
    {
        var librarians = await _context.Users.OfType<Librarian>()
            .Select(l => new { 
                email = l.Email, 
                name = l.Name, 
                role = "Librarian",
                employeeId = l.EmployeeId,
                department = l.Department 
            })
            .ToListAsync();

        var students = await _context.Users.OfType<Student>()
            .Select(s => new { 
                email = s.Email, 
                name = s.Name, 
                role = "Student",
                studentId = s.StudentId 
            })
            .ToListAsync();

        var faculty = await _context.Users.OfType<Faculty>()
            .Select(f => new { 
                email = f.Email, 
                name = f.Name, 
                role = "Faculty",
                employeeId = f.EmployeeId,
                department = f.Department 
            })
            .ToListAsync();

        return Ok(new 
        { 
            librarians,
            students, 
            faculty,
            note = "Use any of these emails to login. Password is not required in this simple auth system."
        });
    }

    /// <summary>
    /// Create librarian user (Development helper)
    /// </summary>
    [HttpPost("create-librarian")]
    public async Task<ActionResult> CreateLibrarian()
    {
        // Check if librarian already exists
        var existingLibrarian = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == "admin@library.edu");

        if (existingLibrarian != null)
        {
            return Ok(new { message = "Librarian already exists", email = "admin@library.edu" });
        }

        // Create new librarian
        var librarian = new Librarian
        {
            Name = "Sarah Johnson",
            Email = "admin@library.edu",
            EmployeeId = "LIB2023001",
            Phone = "09161234567",
            IsActive = true,
            Department = "Library Administration",
            Position = "Head Librarian"
        };

        _context.Users.Add(librarian);
        await _context.SaveChangesAsync();

        return Ok(new { 
            message = "Librarian created successfully!",
            email = librarian.Email,
            name = librarian.Name,
            role = "Librarian"
        });
    }

    /// <summary>
    /// Generate a simple token (in production, use proper JWT)
    /// </summary>
    private string GenerateSimpleToken(int userId, string email)
    {
        // Simple token format: base64(userId:email:timestamp)
        var tokenData = $"{userId}:{email}:{DateTime.UtcNow.Ticks}";
        var tokenBytes = System.Text.Encoding.UTF8.GetBytes(tokenData);
        return Convert.ToBase64String(tokenBytes);
    }
}

// DTOs
public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty; // Not used in simple auth, but kept for frontend compatibility
}

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public object User { get; set; } = null!;
}
