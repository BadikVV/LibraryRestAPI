using LibraryRestAPI.Infrastructure.Contexts;
using LibraryRestAPI.Infrastructure.Repositories;
using LibraryRestAPI.Interfaces;
using LibraryRestAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Read all controllers based on ControllerBase class
builder.Services.AddControllers();

// Add DB Context to Dependency Injection 
builder.Services.AddDbContext<DataDbContext>(options => 
    options.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=MyDatabase;Trusted_Connection=True;"));
// Add Book repository to Dependency Injection 
builder.Services.AddTransient<IBookRepository, BookRepository>();
// Add Book service to Dependency Injection 
builder.Services.AddTransient<BookService>();

builder.Services.AddTransient<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddTransient<SubscriptionService>();

builder.Services.AddTransient<IBookInSubscriptionRepository, BookInSubscriptionRepository>();
builder.Services.AddTransient<BookInSubscriptionService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCorsPolicy", builder =>
    {
        builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithExposedHeaders();
    });
});

var app = builder.Build();

// Initialising Database (creating tables)
SeedDataDb.Initialize(app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope().ServiceProvider);

// Map addresses of controllers to the api
app.MapControllers();
app.UseCors("DevCorsPolicy");
app.Run();