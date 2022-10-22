using System.Net;
using Microsoft.OpenApi.Models;
using r7.Services;
using r7.Repositories;
using Serilog;
using Serilog.Events;

try
{
    var builder = WebApplication.CreateBuilder(args);

    var appName = builder.Configuration.GetValue<string>("ApiConfig:Name");
    var appVersion = builder.Configuration.GetValue<string>("ApiConfig:Version");
    
    var loggerConfig = CreateLoggerConfig(appName);
    Log.Logger = loggerConfig.CreateLogger();

    Log.Information($"Starting up {appName}...");
    Log.Information("Logging Initialised...");

    builder.WebHost.ConfigureKestrel(serverOptions =>
    {
        serverOptions.Listen(IPAddress.Any, Convert.ToInt32(Environment.GetEnvironmentVariable("PORT")));
    });

    // Setup Serilog logging
    builder.Host.UseSerilog((_, loggingConfig) => { loggingConfig.WriteTo.Console(); });

    // Add services to the container.
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddControllers();
    builder.Services.AddSwaggerGen(x=> x.SwaggerDoc("v1", new OpenApiInfo {Title = $"{appName}", Version = $"{appVersion}"}));

    // Ensure we add health checks!
    builder.Services.AddHealthChecks();
    builder.Services.AddCors(options =>
        options.AddPolicy(name: "CorsPolicy", policy =>
        {
            policy
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin();
        }));

    // Add AWS Services here

    // Add Dependencies, ideally via modules to avoid this file becoming too large
    builder.Services.AddScoped<IUserService, UserService>();

    // Repo
    builder.Services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
    builder.Services.AddScoped<IQuery, DapperQuery>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();

    builder.Services.AddScoped<IUserRepository, UserRepository>();

    // Build the app
    var app = builder.Build();

    // Adds the Serilog middleware
    app.UseSerilogRequestLogging();

    // Configure the HTTP request pipeline.
    app.UseSwagger();
    app.UseSwaggerUI();

    // Use secure redirection automatically
    app.UseHttpsRedirection();
    app.UseCors("CorsPolicy");
    
    // Map endpoints
    app.MapHealthChecks("/health");
    app.MapControllers();
    
    // Uncomment here to add authorisation
    // app.UseAuthorization();

    // Run app
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}


static LoggerConfiguration CreateLoggerConfig(string appName)
{
    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

    var loggerConfiguration = new LoggerConfiguration()
        .MinimumLevel.Information()
        .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
        .Filter.ByExcluding("RequestPath = '/health'")
        .Enrich.WithProperty("type", appName.ToLower())
        .Enrich.FromLogContext()
        .WriteTo.Console();

    return loggerConfiguration;
}