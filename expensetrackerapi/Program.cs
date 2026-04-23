using System.Text;
using System.Text.Json.Serialization;
using expensetrackerapi.Contracts;
using expensetrackerapi.Models;
using expensetrackerapi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NodaTime;
using NodaTime.Serialization.SystemTextJson;
using Serilog;
using Serilog.Events;
using Scalar.AspNetCore;
using Serilog.Context;


Log.Logger = new LoggerConfiguration()
    // Configure logs to see only certain type of logs by the namespace
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
    // To capture and include contextual information from the current logging scope.
    // logging scope; an completed operation and start an logging scope with different logs.
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate:
        "{Timestamp:HH:mm:ss} [{Level}] {Message} (IP: {ClientIp}){NewLine}{Exception}")
    // Files will be created each day.
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateBootstrapLogger();
try
{
    Log.Information("Starting ExpenseTracker API");
    var myAllowSpecificOrigins = "_myAllowSpecificOrigins";
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    
    // Inject Serilog as a service to the Dependancy Injection Container to use it in the application.
    // to configure it to work with the ILogger service
    builder.Host.UseSerilog((context, services, configuration) => configuration
        // Read from configuration file with the sinks it should use.
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .Enrich.WithClientIp()
    
    );
    
    // Als iemand IDbInitializer vraagt → geef een DbInitializer
    builder.Services.AddScoped<IDbInitializer, DbIntializer>();
    builder.Services.AddScoped<IExpenseService, ExpenseService>();
    builder.Services.AddScoped<IBucketService, BucketService>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddIdentityCore<ApplicationUser>(options =>
        {
        })
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<ExpenseTrackerContext>();
    builder.Services.AddIdentityApiEndpoints<ApplicationUser>();

    // CORS setup between React FE and C# BE
    builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: myAllowSpecificOrigins,
                policy =>
                {
                    policy.WithOrigins("http://localhost:5173")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
        });
    
    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            // Zorgt ervoor dat de enums correct worden getoond ipv van de id.
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            // !Required for nodatime deserialization otherwise 400 bad request
            options.JsonSerializerOptions.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
        });

    // Make use of the PostgreSQL database as a service we inject in the DI container.
    var password = builder.Configuration["DbPassword"];

    Npgsql.NpgsqlConnectionStringBuilder npgsqlConnectionStringBuilder =
        new(builder.Configuration.GetConnectionString("ExpenseTrackerContext"))
        {
            Password = password
        };

    var connectionstring = npgsqlConnectionStringBuilder.ConnectionString;

    builder.Services.AddDbContext<ExpenseTrackerContext>(options => options
        .UseNpgsql(connectionstring,
            o =>
            {
                o.MapEnum<Buckets>("buckets");
                o.UseNodaTime();
            }

        ));

    // Add authorization service
    builder.Services.AddAuthorization();

    builder.Services.AddAuthentication((options) =>
    {
        // Wat doet dit precies???!?!
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
        // add new scheme
        .AddJwtBearer(options =>
        {
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    context.Token = context.Request.Cookies["jwt"];
                    var cookie = context.Request.Cookies["jwt"];
                    return Task.CompletedTask;
                },
                OnAuthenticationFailed = context =>
                {
                    return Task.CompletedTask;
                }
            };
            
        // handler to handle authentication
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            // validates our secret key
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            // encrypting our (encoded) secret key
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"])),
            // 5-min window (by default) between duration, we change it here to 0.
            ClockSkew = TimeSpan.Zero
        };
    });
    
    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddOpenApi();

    var app = builder.Build();
    app.Use(async (context, next) =>
    {
        var clientIp = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
        
        // Add of source IP to the LogContext
        using (LogContext.PushProperty("ClientIp", clientIp))
        {
            await next.Invoke();
        }
    });
    app.UseCors(myAllowSpecificOrigins);
    
    app.UseAuthentication();
    app.UseAuthorization();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapControllers();
        app.MapScalarApiReference();
        app.MapOpenApi();
    }

    // Add Identity API for the authN and authZ endpoints
    app.MapGroup("api/v1/defaultauth").MapIdentityApi<ApplicationUser>();

    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<ExpenseTrackerContext>();
        context.Database.EnsureCreated();
        // seed the database.
        var initializer = services.GetRequiredService<IDbInitializer>();
        await initializer.SeedAsync(context);

    }

    app.UseHsts();
    app.UseHttpsRedirection();



    Log.Information("Expense Tracker API started successfully");

    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}

finally
{
    Log.CloseAndFlush();
}


