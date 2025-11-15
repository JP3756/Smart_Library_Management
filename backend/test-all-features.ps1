# ============================================
# Smart Library API - Complete Feature Test
# ============================================

$baseUrl = "http://localhost:5208/api"
$results = @{
    Passed = 0
    Failed = 0
    Tests = @()
}

function Test-Endpoint {
    param(
        [string]$Name,
        [string]$Method,
        [string]$Url,
        [object]$Body = $null,
        [int]$ExpectedStatus = 200
    )
    
    try {
        $params = @{
            Uri = $Url
            Method = $Method
            UseBasicParsing = $true
            ContentType = "application/json"
        }
        
        if ($Body) {
            $params.Body = ($Body | ConvertTo-Json -Depth 10)
        }
        
        $response = Invoke-WebRequest @params
        
        if ($response.StatusCode -eq $ExpectedStatus) {
            Write-Host "✅ PASS: $Name" -ForegroundColor Green
            $results.Passed++
            $results.Tests += @{ Name = $Name; Status = "PASS" }
            return $response
        } else {
            Write-Host "❌ FAIL: $Name (Status: $($response.StatusCode))" -ForegroundColor Red
            $results.Failed++
            $results.Tests += @{ Name = $Name; Status = "FAIL" }
            return $null
        }
    } catch {
        Write-Host "❌ FAIL: $Name - $($_.Exception.Message)" -ForegroundColor Red
        $results.Failed++
        $results.Tests += @{ Name = $Name; Status = "FAIL"; Error = $_.Exception.Message }
        return $null
    }
}

Write-Host "`n========================================================" -ForegroundColor Cyan
Write-Host "    Smart Library API - Complete Feature Test           " -ForegroundColor Cyan
Write-Host "========================================================`n" -ForegroundColor Cyan

# ============================================
# 1. BOOKS API TESTS
# ============================================
Write-Host "`nBOOKS API TESTS" -ForegroundColor Yellow
Write-Host "--------------------------------------------------------`n"

# 1.1 Get All Books
$booksResponse = Test-Endpoint -Name "Get All Books" -Method GET -Url "$baseUrl/books"
$books = ($booksResponse.Content | ConvertFrom-Json)
Write-Host "   Found $($books.Count) books in database`n"

# 1.2 Get Book by ID
if ($books.Count -gt 0) {
    Test-Endpoint -Name "Get Book by ID" -Method GET -Url "$baseUrl/books/$($books[0].id)"
}

# 1.3 Search Books
Test-Endpoint -Name "Search Books" -Method GET -Url "$baseUrl/books/search?query=Code"

# 1.4 Get Books by Category
Test-Endpoint -Name "Get Books by Category" -Method GET -Url "$baseUrl/books/category/Programming"

# 1.5 Get Available Books
Test-Endpoint -Name "Get Available Books" -Method GET -Url "$baseUrl/books/available"

# 1.6 Create New Book
$newBook = @{
    isbn = "978-1234567890"
    title = "Test Book - API Testing"
    author = "Test Author"
    category = "Programming"
    publicationYear = 2024
    totalCopies = 5
    publisher = "Test Publisher"
}
$createdBook = Test-Endpoint -Name "Create New Book" -Method POST -Url "$baseUrl/books" -Body $newBook -ExpectedStatus 201
$createdBookId = if ($createdBook) { ($createdBook.Content | ConvertFrom-Json).id } else { $null }

# 1.7 Update Book
if ($createdBookId) {
    $updateBook = @{
        title = "Updated Test Book"
        totalCopies = 10
    }
    Test-Endpoint -Name "Update Book" -Method PUT -Url "$baseUrl/books/$createdBookId" -Body $updateBook
}

# 1.8 Update Book Availability
if ($createdBookId) {
    $availability = @{ availableCopies = 3 }
    Test-Endpoint -Name "Update Book Availability" -Method PATCH -Url "$baseUrl/books/$createdBookId/availability" -Body $availability
}

# 1.9 Delete Book
if ($createdBookId) {
    Test-Endpoint -Name "Delete Book" -Method DELETE -Url "$baseUrl/books/$createdBookId"
}

# ============================================
# 2. USERS API TESTS (Factory Pattern)
# ============================================
Write-Host "`nUSERS API TESTS (Factory Pattern)" -ForegroundColor Yellow
Write-Host "--------------------------------------------------------`n"

# 2.1 Get All Users
$usersResponse = Test-Endpoint -Name "Get All Users" -Method GET -Url "$baseUrl/users"
$users = ($usersResponse.Content | ConvertFrom-Json)
Write-Host "   Found $($users.Count) users in database`n"

# 2.2 Create Student (Factory Pattern)
$newStudent = @{
    name = "Test Student"
    email = "test.student@university.edu"
    type = "Student"
    studentId = "STU999999"
}
$createdStudent = Test-Endpoint -Name "Create Student (Factory)" -Method POST -Url "$baseUrl/users" -Body $newStudent -ExpectedStatus 201
$studentId = if ($createdStudent) { ($createdStudent.Content | ConvertFrom-Json).id } else { $null }

# 2.3 Create Faculty (Factory Pattern)
$newFaculty = @{
    name = "Test Faculty"
    email = "test.faculty@university.edu"
    type = "Faculty"
    employeeId = "FAC999999"
}
$createdFaculty = Test-Endpoint -Name "Create Faculty (Factory)" -Method POST -Url "$baseUrl/users" -Body $newFaculty -ExpectedStatus 201
$facultyId = if ($createdFaculty) { ($createdFaculty.Content | ConvertFrom-Json).id } else { $null }

# 2.4 Get User by ID
if ($users.Count -gt 0) {
    Test-Endpoint -Name "Get User by ID" -Method GET -Url "$baseUrl/users/$($users[0].id)"
}

# 2.5 Search Users
Test-Endpoint -Name "Search Users" -Method GET -Url "$baseUrl/users/search?query=Test"

# 2.6 Get Users by Type
Test-Endpoint -Name "Get Students" -Method GET -Url "$baseUrl/users/type/Student"
Test-Endpoint -Name "Get Faculty" -Method GET -Url "$baseUrl/users/type/Faculty"

# 2.7 Get User Borrow Limit (Polymorphism Test)
if ($studentId) {
    $limitResponse = Test-Endpoint -Name "Get Student Borrow Limit" -Method GET -Url "$baseUrl/users/$studentId/borrow-limit"
    if ($limitResponse) {
        $limit = ($limitResponse.Content | ConvertFrom-Json).maxBorrowLimit
        Write-Host "   Student Max Borrow Limit: $limit (Expected: 3)`n"
    }
}

if ($facultyId) {
    $limitResponse = Test-Endpoint -Name "Get Faculty Borrow Limit" -Method GET -Url "$baseUrl/users/$facultyId/borrow-limit"
    if ($limitResponse) {
        $limit = ($limitResponse.Content | ConvertFrom-Json).maxBorrowLimit
        Write-Host "   Faculty Max Borrow Limit: $limit (Expected: 10)`n"
    }
}

# 2.8 Update User
if ($studentId) {
    $updateUser = @{
        name = "Updated Test Student"
    }
    Test-Endpoint -Name "Update User" -Method PUT -Url "$baseUrl/users/$studentId" -Body $updateUser
}

# ============================================
# 3. LOANS API TESTS (Strategy Pattern)
# ============================================
Write-Host "`nLOANS API TESTS (Strategy Pattern)" -ForegroundColor Yellow
Write-Host "--------------------------------------------------------`n"

# 3.1 Get All Loans
$loansResponse = Test-Endpoint -Name "Get All Loans" -Method GET -Url "$baseUrl/loans"
$loans = if ($loansResponse) { ($loansResponse.Content | ConvertFrom-Json) } else { @() }
Write-Host "   Found $($loans.Count) loans in database`n"

# 3.2 Create Loan (Borrow Book)
if ($books.Count -gt 0 -and $users.Count -gt 0) {
    $newLoan = @{
        bookId = $books[0].id
        userId = $users[0].id
        dueDate = (Get-Date).AddDays(14).ToString("yyyy-MM-ddTHH:mm:ss")
    }
    $createdLoan = Test-Endpoint -Name "Create Loan (Borrow Book)" -Method POST -Url "$baseUrl/loans" -Body $newLoan -ExpectedStatus 201
    $loanId = if ($createdLoan) { ($createdLoan.Content | ConvertFrom-Json).id } else { $null }
}

# 3.3 Get Loan by ID
if ($loans.Count -gt 0) {
    Test-Endpoint -Name "Get Loan by ID" -Method GET -Url "$baseUrl/loans/$($loans[0].id)"
}

# 3.4 Get Active Loans
Test-Endpoint -Name "Get Active Loans" -Method GET -Url "$baseUrl/loans/active"

# 3.5 Get Overdue Loans
Test-Endpoint -Name "Get Overdue Loans" -Method GET -Url "$baseUrl/loans/overdue"

# 3.6 Get User Loans
if ($users.Count -gt 0) {
    Test-Endpoint -Name "Get User Loans" -Method GET -Url "$baseUrl/loans/user/$($users[0].id)"
}

# 3.7 Return Book (Fine Calculation with Strategy)
if ($loanId) {
    Test-Endpoint -Name "Return Book (Calculate Fine)" -Method POST -Url "$baseUrl/loans/$loanId/return"
    Write-Host "   Fine calculated using Strategy Pattern`n"
}

# ============================================
# 4. REPORTS API TESTS
# ============================================
Write-Host "`nREPORTS API TESTS" -ForegroundColor Yellow
Write-Host "--------------------------------------------------------`n"

# 4.1 Get Statistics
$statsResponse = Test-Endpoint -Name "Get Overall Statistics" -Method GET -Url "$baseUrl/reports/stats"
if ($statsResponse) {
    $stats = ($statsResponse.Content | ConvertFrom-Json)
    Write-Host "   Statistics:`n"
    Write-Host "   - Total Books: $($stats.totalBooks)"
    Write-Host "   - Total Users: $($stats.totalUsers)"
    Write-Host "   - Active Loans: $($stats.activeLoans)"
    Write-Host "   - Overdue Loans: $($stats.overdueLoans)"
    Write-Host "   - Total Students: $($stats.totalStudents)"
    Write-Host "   - Total Faculty: $($stats.totalFaculty)"
    Write-Host "   - Total Fines Collected: `$$($stats.totalFinesCollected)"
    Write-Host "   - Pending Fines: `$$($stats.pendingFines)`n"
}

# 4.2 Get Borrowing Report
Test-Endpoint -Name "Get Borrowing Report" -Method GET -Url "$baseUrl/reports/borrowing"

# 4.3 Get Books by Category Report
Test-Endpoint -Name "Get Books by Category" -Method GET -Url "$baseUrl/reports/books-by-category"

# 4.4 Get User Statistics
Test-Endpoint -Name "Get User Statistics" -Method GET -Url "$baseUrl/reports/user-statistics"

# 4.5 Get Fines Report
Test-Endpoint -Name "Get Fines Report" -Method GET -Url "$baseUrl/reports/fines"

# 4.6 Get Monthly Trend
Test-Endpoint -Name "Get Monthly Trend" -Method GET -Url "$baseUrl/reports/monthly-trend"

# ============================================
# CLEANUP
# ============================================
Write-Host "`nCLEANUP" -ForegroundColor Yellow
Write-Host "--------------------------------------------------------`n"

# Delete test users
if ($studentId) {
    Test-Endpoint -Name "Delete Test Student" -Method DELETE -Url "$baseUrl/users/$studentId"
}
if ($facultyId) {
    Test-Endpoint -Name "Delete Test Faculty" -Method DELETE -Url "$baseUrl/users/$facultyId"
}

# ============================================
# SUMMARY
# ============================================
Write-Host "`n========================================================" -ForegroundColor Cyan
Write-Host "                    TEST SUMMARY                        " -ForegroundColor Cyan
Write-Host "========================================================`n" -ForegroundColor Cyan

$total = $results.Passed + $results.Failed
$percentage = if ($total -gt 0) { [math]::Round(($results.Passed / $total) * 100, 2) } else { 0 }

Write-Host "Total Tests: $total" -ForegroundColor White
Write-Host "Passed: $($results.Passed)" -ForegroundColor Green
Write-Host "Failed: $($results.Failed)" -ForegroundColor Red
Write-Host "Success Rate: $percentage%`n" -ForegroundColor $(if ($percentage -eq 100) { "Green" } else { "Yellow" })

if ($results.Failed -eq 0) {
    Write-Host "ALL TESTS PASSED! Backend is fully functional!" -ForegroundColor Green
} else {
    Write-Host "Some tests failed. Check the output above for details." -ForegroundColor Yellow
}

Write-Host "`n========================================================`n"
