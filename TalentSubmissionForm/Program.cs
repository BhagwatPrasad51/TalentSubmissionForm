using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TalentSubmissionForm;
using TalentSubmissionForm.Data;
using TalentSubmissionForm.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configuring for Client Side Validation
builder.Services.AddControllersWithViews().AddViewOptions(options =>
    {
        options.HtmlHelperOptions.ClientValidationEnabled = true;
    });

//Configure DB Connection
builder.Services.AddDbContext<TalentContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")));

//Talent Service Scoped Declare To Create Object
builder.Services.AddScoped<TalentService>();

// Add Identity with roles
builder.Services.AddAuthentication("MyCookieAuth")
    .AddCookie("MyCookieAuth", options =>
    {
        options.LoginPath = "/Admin/Login"; 
    });
builder.Services.AddHttpContextAccessor();
//Session 

builder.Services.AddSession(options =>
{
    // Set a timeout for the session
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true; // Make the session cookie inaccessible to client-side script
    options.Cookie.IsEssential = true; // Make the session cookie essential for the application to function
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var dbContext = services.GetRequiredService<TalentContext>();

        dbContext.Database.Migrate();

        string adminEmail = "admin@example.com";
        string adminPassword = "Admin@123"; // Secure this in production
        string adminUserName = "Admin";

        var existingAdmin = dbContext.admins.FirstOrDefault(a => a.Email == adminEmail);

        if (existingAdmin == null)
        {
            var newAdmin = new Admin
            {
                Email = adminEmail,
                Password = adminPassword, 
                UserName = adminUserName,
                Createdon = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
            };

            dbContext.admins.Add(newAdmin);
            await dbContext.SaveChangesAsync();
            Console.WriteLine("✅ Admin user created successfully.");
        }
        else
        {
            Console.WriteLine("ℹ️ Admin user already exists.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Error seeding admin user: {ex.Message}");
        throw;
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Talent/Error");
    app.UseStatusCodePagesWithReExecute("/Talent/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

//use Session
app.UseSession();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Talent}/{action=Create}/{id?}")
    .WithStaticAssets();


app.UseStaticFiles();
app.Run();
