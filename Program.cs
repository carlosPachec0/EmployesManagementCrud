using EmployeesManagement.Data;
using EmployeesManagement.Services;
using EmployeesManagement.Models.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Asp.Versioning;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("EmployeesManagementDBConnection")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
       options.Cookie.Name = "Login";
       options.Cookie.HttpOnly = true;
       options.Cookie.SecurePolicy = builder.Environment.IsDevelopment()
           ? CookieSecurePolicy.SameAsRequest
           : CookieSecurePolicy.Always;
       options.Cookie.SameSite = SameSiteMode.Lax;

       options.LoginPath = "/Account/Login";
       options.LogoutPath = "/Account/Logout";
       options.AccessDeniedPath = "/Account/AccessDenied";
       options.ExpireTimeSpan = TimeSpan.FromMinutes(60);

       options.Events = new CookieAuthenticationEvents
       {
           OnRedirectToLogin = context =>
           {
               if (context.Request.Path.StartsWithSegments("/api"))
               {
                   context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                   return Task.CompletedTask;
               }
               context.Response.Redirect(context.RedirectUri);
               return Task.CompletedTask;
           }
       };
    });

builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN";
});

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<PasswordHasher<User>>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
   app.UseExceptionHandler("/Home/Error");
   // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
   app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    var configuration = services.GetRequiredService<IConfiguration>();

    await context.Database.MigrateAsync();

    var passwordHasher = services.GetRequiredService<PasswordHasher<User>>();
    await DbSeeder.SeedAdminUserAsync(context, passwordHasher, configuration);
}

app.Run();
