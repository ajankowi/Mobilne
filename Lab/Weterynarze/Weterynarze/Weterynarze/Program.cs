using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Weterynarze.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    //.AddDefaultTokenProviders().AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

using (var scope = app.Services.CreateScope()) {

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var roles = new[] { "Admin", "Dystrybutor", "Weterynarz" };

    foreach (var role in roles){

        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));

    }

}


using (var scope = app.Services.CreateScope())
{

    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    string email = "admin@admin.com";
    string password = "Admin1234,";

    string email_1 = "dyst@dyst.com";
    string password_1 = "Dyst1234,";

    string email_2 = "wet@wet.com";
    string password_2 = "Wet1234,";


    //Tworzenie konta admina
    if (await userManager.FindByEmailAsync(email) == null) {

        var user = new IdentityUser();

        user.UserName = email;
        user.Email = email;
        user.EmailConfirmed = true;
    
        await userManager.CreateAsync (user,password);

        await userManager.AddToRoleAsync(user,"Admin");

    }


    //Tworzenie konta dystrybutora
    

    if (await userManager.FindByEmailAsync(email_1) == null) {

        var user = new IdentityUser();

        user.UserName = email_1;
        user.Email = email_1;
        user.EmailConfirmed = true;
    
        await userManager.CreateAsync (user,password_1);

        await userManager.AddToRoleAsync(user, "Dystrybutor");

    }


    //Tworzenie konta dystrybutora


    if (await userManager.FindByEmailAsync(email_2) == null)
    {

        var user = new IdentityUser();

        user.UserName = email_2;
        user.Email = email_2;
        user.EmailConfirmed = true;

        await userManager.CreateAsync(user, password_2);

        await userManager.AddToRoleAsync(user, "Weterynarz");

    }


}


app.Run();



