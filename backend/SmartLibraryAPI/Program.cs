using Microsoft.EntityFrameworkCore;
using SmartLibraryAPI.Data;
using SmartLibraryAPI.Interfaces;
using SmartLibraryAPI.Repositories;
using SmartLibraryAPI.Services;
using SmartLibraryAPI.Factories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure PostgreSQL Database with Entity Framework Core
// Note: Update connection string in appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseNpgsql(connectionString));

// Register Repositories (REPOSITORY PATTERN - Dependency Injection)
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Register Services (SOLID - Dependency Inversion Principle)
builder.Services.AddScoped<ILoanService, LoanService>();

// Register Factories (FACTORY PATTERN)
builder.Services.AddScoped<IUserFactory, UserFactory>();

// Configure CORS for frontend integration and Swagger
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Enable CORS (must be before Authorization)
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
