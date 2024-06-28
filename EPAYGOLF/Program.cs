using EPAYGOLF;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Runtime.Intrinsics.X86;
using Microsoft.AspNetCore.Owin;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// Add services to the container.
builder.Services.AddSingleton<ViewRenderingService>();
builder.Services.AddHttpContextAccessor(); // Ensure IHttpContextAccessor is registered
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
            });



var startup = new Startup();
startup.ConfigureServices(builder.Services);

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//Use OWIN
// Use OWIN middleware

// Configure authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();



string enableAuthorization = ConfigurationHelper.GetConnectionString("EnableAuthorization");

var defaultController = "Report";
var defaultaction = "Index";
if (enableAuthorization == "true")
{
	defaultController = "Account";
    defaultaction = "Login";
}

    app.MapControllerRoute(
	name: "default",
	pattern: "{controller="+defaultController +"}/{action="+ defaultaction + "}/{id?}");




app.Run();
