using Microsoft.AspNetCore.Builder;
using DarazApp.Helpers;
using DarazApp.Services;
using DarazApp.Entities;
using DarazApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using static DarazApp.Services.UserService;
using NuGet.Protocol.Core.Types;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Session;


var builder = WebApplication.CreateBuilder(args);
{
    var services = builder.Services;
    builder.Services.AddDbContext<CommerceDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("Server=DESKTOP-GTT98CG\\SQLEXPRESS; Database=CommerceDb; Trusted_Connection=True;"));
        options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    });
    builder.Services.AddDirectoryBrowser();
    builder.Services.AddRazorPages();
    services.AddCors();
    services.AddControllersWithViews().AddRazorRuntimeCompilation();
    services.AddMvc();
    services.AddSession();
    services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
    services.AddScoped<IUserService, UserService>();
    services.AddRouting();
    services.AddAuthentication();
    services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });
}
var app = builder.Build();
{
    app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
    app.UseStaticFiles();
    app.UseMiddleware<JwtMiddleware>();

    if (!app.Environment.IsDevelopment())
        app.UseExceptionHandler("Home/Error");
    app.UseSession();
    app.UseRouting();
    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseAuthentication();
    app.UseAuthorization();


    app.MapControllers();
    app.UseEndpoints(Endpoint =>
    {
        Endpoint.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
    );
    });
}
app.Run();
