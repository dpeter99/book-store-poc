using BookStore.ApiService.Database;
using BookStore.ApiService.Database.Entities;
using BookStore.ApiService.Infrastructure.Auth;
using BookStore.ApiService.Infrastructure.MuliTenant;
using BookStore.ApiService.Modules;
using BookStore.ApiService.Modules.BookManager;
using BookStore.ApiService.MuliTenant;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("bookdb")));
builder.EnrichNpgsqlDbContext<AppDbContext>();

builder.Services.AddDbContext<TenantDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("bookdb")));
builder.EnrichNpgsqlDbContext<TenantDbContext>();

builder.Services.AddScoped<ITenantService, TenantService>();
builder.Services.AddScoped<ICurrentTenantService, CurrentTenantServiceImpl>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddAuthentication()
    .AddScheme<DummyAuthenticationSchemeOptions, DummyAuthenticationHandler>(
        DummyAuthenticationHandler.AuthenticationScheme,
        o => { }
    );

builder.AddBookModule();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.Migrate();
    }   
}

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

app.UseMiddleware<TenantResolver>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapDefaultEndpoints();
app.MapControllers();

app.Run();