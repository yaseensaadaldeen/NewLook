using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NEWLOOK.Models.NewLook;
using static NEWLOOK.Models.NewLook.NewLookContext;

var builder = WebApplication.CreateBuilder(args);

// ? Register services before building the app
builder.Services.AddControllersWithViews();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<NewLookContext>(options =>
options.UseSqlServer(connectionString));

// ? Add session services
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.Configure<ImageSettings>(
    builder.Configuration.GetSection("ImageSettings"));

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
