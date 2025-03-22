using Microsoft.EntityFrameworkCore;
using ToDoApp.Models;  // Make sure to import your ApplicationDbContext and models
using ToDoApp.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Register the ApplicationDbContext with SQL Server connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add controllers and views
builder.Services.AddControllersWithViews();

builder.Services.AddTransient<IEmailService, EmailService>(); //

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Define default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
