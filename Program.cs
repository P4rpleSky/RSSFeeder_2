var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();

app.MapControllerRoute(
    name: "feeder",
    pattern: "",
    defaults: new { controller = "Rss", action = "Main" });

app.Run();