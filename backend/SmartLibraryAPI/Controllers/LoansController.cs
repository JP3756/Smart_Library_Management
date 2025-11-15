using Microsoft.AspNetCore.Mvc;
using SmartLibraryAPI.DTOs;
using SmartLibraryAPI.Interfaces;
using SmartLibraryAPI.Models;

namespace SmartLibraryAPI.Controllers
{
    /// <summary>
    /// Loans API Controller - Handles borrowing and returning books
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class LoansController : ControllerBase
    {
        private readonly ILoanService _loanService;
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;

        public LoansController(
            ILoanService loanService,
            IBookRepository bookRepository,
            IUserRepository userRepository)
        {
            _loanService = loanService;
            _bookRepository = bookRepository;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Get all loans
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoanDto>>> GetAllLoans()
        {
            var loans = await _loanService.GetAllLoansAsync();
            var loanDtos = await Task.WhenAll(loans.Select(l => MapToDtoAsync(l)));
            return Ok(loanDtos);
        }

        /// <summary>
        /// Get loan by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<LoanDto>> GetLoan(int id)
        {
            var loan = await _loanService.GetLoanByIdAsync(id);
            if (loan == null)
                return NotFound(new { message = $"Loan with ID {id} not found" });

            return Ok(await MapToDtoAsync(loan));
        }

        /// <summary>
        /// Get all active loans
        /// </summary>
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<LoanDto>>> GetActiveLoans()
        {
            var loans = await _loanService.GetActiveLoansAsync();
            var loanDtos = await Task.WhenAll(loans.Select(l => MapToDtoAsync(l)));
            return Ok(loanDtos);
        }

        /// <summary>
        /// Get all overdue loans
        /// </summary>
        [HttpGet("overdue")]
        public async Task<ActionResult<IEnumerable<LoanDto>>> GetOverdueLoans()
        {
            var loans = await _loanService.GetOverdueLoansAsync();
            var loanDtos = await Task.WhenAll(loans.Select(l => MapToDtoAsync(l)));
            return Ok(loanDtos);
        }

        /// <summary>
        /// Get loans for a specific user
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<LoanDto>>> GetUserLoans(int userId)
        {
            var loans = await _loanService.GetUserLoansAsync(userId);
            var loanDtos = await Task.WhenAll(loans.Select(l => MapToDtoAsync(l)));
            return Ok(loanDtos);
        }

        /// <summary>
        /// Create new loan (Borrow book) - Alternative endpoint
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<LoanDto>> CreateLoan(CreateLoanDto createLoanDto)
        {
            try
            {
                var loan = await _loanService.BorrowBookAsync(createLoanDto.UserId, createLoanDto.BookId);
                var loanDto = await MapToDtoAsync(loan);
                return CreatedAtAction(nameof(GetLoan), new { id = loan.Id }, loanDto);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Borrow a book
        /// </summary>
        [HttpPost("borrow")]
        public async Task<ActionResult<LoanDto>> BorrowBook(CreateLoanDto createLoanDto)
        {
            try
            {
                var loan = await _loanService.BorrowBookAsync(createLoanDto.UserId, createLoanDto.BookId);
                var loanDto = await MapToDtoAsync(loan);
                return Ok(loanDto);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Return a book
        /// </summary>
        [HttpPost("{loanId}/return")]
        public async Task<ActionResult<LoanDto>> ReturnBook(int loanId)
        {
            try
            {
                var loan = await _loanService.ReturnBookAsync(loanId);
                var loanDto = await MapToDtoAsync(loan);
                return Ok(loanDto);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Check if user can borrow
        /// </summary>
        [HttpGet("can-borrow/{userId}")]
        public async Task<ActionResult<object>> CanUserBorrow(int userId)
        {
            var canBorrow = await _loanService.CanUserBorrowAsync(userId);
            var user = await _userRepository.GetByIdAsync(userId);
            
            if (user == null)
                return NotFound(new { message = "User not found" });

            return Ok(new
            {
                canBorrow,
                userId,
                userName = user.Name,
                currentLoans = user.Loans.Count(l => l.Status == LoanStatus.Active),
                maxLimit = user.GetMaxBorrowLimit()
            });
        }

        /// <summary>
        /// Calculate fine for a loan
        /// </summary>
        [HttpGet("{loanId}/fine")]
        public async Task<ActionResult<object>> CalculateFine(int loanId)
        {
            try
            {
                var fine = await _loanService.CalculateFineAsync(loanId);
                
                if (fine == null)
                    return Ok(new { message = "No fine applicable", amount = 0 });

                return Ok(new
                {
                    fineId = fine.Id,
                    loanId = fine.LoanId,
                    amount = fine.Amount,
                    status = fine.Status.ToString(),
                    dateAssessed = fine.DateAssessed,
                    remarks = fine.Remarks
                });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // Helper method to map Loan to LoanDto
        private async Task<LoanDto> MapToDtoAsync(Loan loan)
        {
            // Ensure navigation properties are loaded
            if (loan.User == null)
            {
                var user = await _userRepository.GetByIdAsync(loan.UserId);
                loan.User = user!;
            }
            if (loan.Book == null)
            {
                var book = await _bookRepository.GetByIdAsync(loan.BookId);
                loan.Book = book!;
            }

            return new LoanDto
            {
                Id = loan.Id,
                UserId = loan.UserId,
                UserName = loan.User.Name,
                BookId = loan.BookId,
                BookTitle = loan.Book.Title,
                BorrowDate = loan.BorrowDate,
                DueDate = loan.DueDate,
                ReturnDate = loan.ReturnDate,
                Status = loan.Status.ToString(),
                DaysOverdue = loan.DaysOverdue,
                FineAmount = loan.Fine?.Amount
            };
        }
    }
}
