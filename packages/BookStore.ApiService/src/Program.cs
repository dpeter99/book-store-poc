using System.Reflection;
using Asp.Versioning;
using Asp.Versioning.Routing;
using BookStore.ApiService;
using BookStore.ApiService.Database;
using BookStore.ApiService.Database.Entities;
using BookStore.ApiService.Infrastructure;
using BookStore.ApiService.Infrastructure.Auth;
using BookStore.ApiService.Infrastructure.MuliTenant;
using BookStore.ApiService.Infrastructure.Policies;
using BookStore.ApiService.Modules;
using BookStore.ApiService.Modules.BookManager;
using BookStore.ApiService.MuliTenant;
using BookStore.JobContracts.Contracts;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using OpenTelemetry.Trace;
using Scalar.AspNetCore;
using SimpleActivityExportProcessor = OpenTelemetry.SimpleActivityExportProcessor;

var openApiBuildTime = Assembly.GetEntryAssembly()?.GetName().Name == "GetDocument.Insider";


var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
if(!openApiBuildTime)
	builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

builder.Services.AddVersionedOpenApi([
	("", new ApiVersion(1)),
	("public", new ApiVersion(1)),
	("public", new ApiVersion(2))
]);

builder.Services.AddControllers();


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("bookdb")));
builder.EnrichNpgsqlDbContext<AppDbContext>();

builder.Services.AddDbContext<TenantDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("bookdb")));
builder.EnrichNpgsqlDbContext<TenantDbContext>();

builder.Services.AddScoped<ITenantService, TenantService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddAuthentication()
    .AddScheme<DummyAuthenticationSchemeOptions, DummyAuthenticationHandler>(
        DummyAuthenticationHandler.AuthenticationScheme,
        o => { }
    );

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    
    options.AddPolicy("User", policy => 
        policy
            .RequireAuthenticatedUser()
            .AddRequirements([new TenantAccessAuthorizationRequirement()])
        );
    
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
});
builder.Services.AddScoped<IAuthorizationHandler, TenantAccessAuthorizationRequirementHandler>();




builder.Services.AddHangfire((sp,config) => 
    config.UsePostgreSqlStorage(c =>
        //c.UseConnectionFactory(new HangfireDbFactory(sp))
        c.UseNpgsqlConnection(builder.Configuration.GetConnectionString("bookdb"))
        ) 
    );

if(!openApiBuildTime)
	builder.Services.AddHangfireServer(c => { c.Queues = ["local"]; });

builder.Services.AddOpenTelemetry()
	.WithTracing(c => c.AddProcessor(new HangfirePostgresTraceFilteringProcessor()));


builder.AddBookModule();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.Migrate();
        
        var scheduler = scope.ServiceProvider.GetRequiredService<IBackgroundJobClient>();
        scheduler.Enqueue<IOptimizationRunner>((r)=> r.Run(new("Opt task")));
    }   
}

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

app.UseMiddleware<TenantResolverMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

if(!openApiBuildTime)
	app.UseHangfireDashboard();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi().AllowAnonymous();
    app.MapScalarApiReference(c =>
	    {
		    c.AddDocuments(["v1","public-v1","public-v2"]);
	    })
	    .AllowAnonymous();
}

app.MapDefaultEndpoints();
// app.MapControllers();

// app.Map


app.Run();
