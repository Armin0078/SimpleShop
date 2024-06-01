using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MyFirstShop.Data;
using MyFirstShop.Data.Repositories;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
#region Db Context
builder.Services.AddDbContext<MyFirstShopContext>(options =>
{
	options.UseSqlServer("Data Source =.\\SQLSERVER2022;Initial Catalog = MyFirstShop_DB; Integrated Security = true;TrustServerCertificate=true");
});
#endregion

#region Ioc
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
#endregion

#region Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	.AddCookie(option =>
	{
		option.LoginPath = "/Account/Login";
		option.LogoutPath = "/Account/Logout";
		option.ExpireTimeSpan = TimeSpan.FromDays(10);
	});


#endregion

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

app.UseAuthentication();
app.UseAuthorization();

// MiddleWare for admin Panel

//app.Use(async (context, next) =>
//{
//	if (context.Request.Path.StartsWithSegments("/Admin"))
//	{
//		if (!context.User.Identity.IsAuthenticated)
//		{
//			context.Response.Redirect("/Account/Login");
//		}
//		else if (!bool.Parse(context.User.FindFirstValue("IsAdmin")))
//		{
//			context.Response.Redirect("/Account/Login");
//		}
//		await next();
//	}
//});


app.MapRazorPages();
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
