using Integration_Sales_Order_Test.Helpers;
using Integration_Sales_Order_Test.Repository;
using System.Configuration;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Integration_Sales_Order_Test.Repository.ServicesEmail;
using Integration_Sales_Order_Test.Middleware;
using System.Data.Entity;
using Integration_Sales_Order_Test.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Integration_Sales_Order_Test.Repository.ProductServices;
using Integration_Sales_Order_Test.Repository.CategoryServices;
using Integration_Sales_Order_Test.Repository.ClientServices;
using Integration_Sales_Order_Test.Repository.Orders;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
var config = builder.Configuration;

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>();

builder.Services.AddCors();
builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.IgnoreNullValues = true);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.IgnoreNullValues = true);

builder.Services.Configure<AppSettings>(config.GetSection("AppSettings"));

builder.Services.AddTransient<ISalesOrder, SalesOrderRepo>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IProductServices, ProductServices>();
builder.Services.AddScoped<ICategory, CategoryServices>();
builder.Services.AddScoped<IClientServices, ClientServices>();
builder.Services.AddScoped<IOrders, OrderServices>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;

}).AddCookie();



builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy =>
    {
        policy.RequireRole(nameof(Role.Admin));
        policy.AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme);

    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DataContext>();
    // DataContext dataContext;
    
    //db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    //  app.UseSwaggerUI();
    app.UseSwaggerUI(x => x.SwaggerEndpoint("/swagger/v1/swagger.json", "ASP.NET Core Sign-up and Verification API"));

}


app.UseCors(x => x
             .SetIsOriginAllowed(origin => true)
             .AllowAnyMethod()
             .AllowAnyHeader()
             .AllowCredentials());


app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseMiddleware<JwtMiddleware>();

//app.UseEndpoints(x => x.MapControllers());

app.UseHttpsRedirection();

app.UseFileServer();

app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
