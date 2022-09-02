using CursoIdentity.Datos;
using CursoIdentity.Servicios;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options => 
options.UseSqlServer(builder.Configuration.GetConnectionString("CursoIdentityDB")));

//Agregar servicion Identity a la aplicación
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

//Esta linea es para la url de retorno al acceder
builder.Services.ConfigureApplicationCookie(
    options =>
    {
        options.LoginPath = new PathString("/Cuentas/Acceso");
        options.AccessDeniedPath = new PathString("/Cuentas/Acceso/Bloqueado");

    });

//Se agrega IEmailSeder
builder.Services.AddTransient<IEmailSender, MailJetEmailSender>();

//Estas son opciones de configuracion del identity

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireLowercase = true;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
    options.Lockout.MaxFailedAccessAttempts = 3;
});

//Agregar autenticación de facebook
builder.Services.AddAuthentication().AddFacebook(options =>
{
    options.AppId = "1222871738560977";
    options.AppSecret = "6084e2799108f36a5c3c85dd07e37f21";
});

builder.Services.AddAuthentication().AddGoogle(options =>
{
    options.ClientId = "1074993027512-cglkgi9m0t15coldoi1inqqqtia9qiic.apps.googleusercontent.com";
    options.ClientSecret = "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";
});



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

//se agrega la autenticación
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
