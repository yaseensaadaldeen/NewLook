using Microsoft.EntityFrameworkCore;
using NEWLOOK.Models.NewLook;

var builder = WebApplication.CreateBuilder(args);

// ? Register services before building the app
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<NewLookContext>(options =>
    options.UseSqlServer("Server=VM0D7D1F9\\SQLSERVER;Database=NewLook;Trusted_Connection=True;TrustServerCertificate=true;"));

// ? Add session services
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// ? Configure middleware in the correct order
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ? Enable session before authorization and endpoints
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
