using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MisCanchas.Contracts.Services;
using MisCanchas.Data;
using MisCanchas.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;


var builder = WebApplication.CreateBuilder(args);


var AunthenticatedUserPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();


// Add services to the container.
builder.Services.AddControllersWithViews(opciones =>
{
    opciones.Filters.Add(new AuthorizeFilter(AunthenticatedUserPolicy));
});
builder.Services.AddDbContext<MisCanchasDbContext>(options =>
    options.UseSqlServer(builder.Configuration
    .GetConnectionString("MisCanchasConnectionString")));

builder.Services.AddTransient<IClientService, ClientService>();
builder.Services.AddTransient<ITurnService, TurnService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IFieldService, FieldService>();
builder.Services.AddTransient<IReportService, ReportService>();
//aqui van los servicios

//agrego autenticacion, db y roles
builder.Services.AddAuthentication();

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
}).AddEntityFrameworkStores<MisCanchasDbContext>()
.AddDefaultTokenProviders();


// utilizo las vistas login personalizadas
builder.Services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme,
    opciones =>
    {
        opciones.LoginPath = "/users/login";
        opciones.AccessDeniedPath = "/users/login";
    });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MisCanchasDbContext>();
    context.Database.Migrate();
}

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
app.MapControllerRoute(
name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
