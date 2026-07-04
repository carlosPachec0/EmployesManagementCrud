using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EmployeesManagement.Models.Entities;

namespace EmployeesManagement.Data;

public static class DbSeeder
{
    public static async Task SeedAdminUserAsync(
        ApplicationDbContext context,
        PasswordHasher<User> passwordHasher,
        IConfiguration configuration)
    {
        bool adminExists = await context.Users
            .IgnoreQueryFilters() // include inactive users too, as a safety check
            .AnyAsync(u => u.Role == UserRole.Admin);

        if (adminExists)
            return;

        var email = configuration["SeedAdmin:Email"];
        var password = configuration["SeedAdmin:Password"];

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            throw new InvalidOperationException(
                "SeedAdmin:Email and SeedAdmin:Password must be configured to seed the initial admin user.");

        var admin = new User
        {
            Email = email,
            FirstName = "Admin",
            LastName = "User",
            Role = UserRole.Admin,
            IsActive = true
        };

        admin.Password = passwordHasher.HashPassword(admin, password);

        context.Users.Add(admin);
        await context.SaveChangesAsync();
    }
}
