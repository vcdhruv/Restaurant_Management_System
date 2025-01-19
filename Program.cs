using Restaurant_Management_System;
using Restaurant_Management_System.DAL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10); // for custom time span by default minutes will be 20
}
);
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); // for registering iHttpContextAccessor for accessing session varaibles in views directly...
builder.Services.AddSignalR(); // for signalR services

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseSession(); // for session
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "myArea",
    pattern: "{area:exists}/{controller=Register}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<StatusUpdateHub>("/statusUpdateHub"); // for signalR mapping


app.Run();
