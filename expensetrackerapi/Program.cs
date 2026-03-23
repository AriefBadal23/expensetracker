using System.Text.Json.Serialization;
using expensetrackerapi.Models;
using expensetrackerapi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using NodaTime;
using NodaTime.Serialization.SystemTextJson;
using Serilog;
using Serilog.Events;
using Scalar.AspNetCore;


Log.Logger = new LoggerConfiguration()
    // Configure logs to see only certain type of logs by the namespace
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
    // To capture and include contextual information from the current logging scope.
    // logging scope; an completed operation and start an logging scope with different logs.
    .Enrich.FromLogContext()
    .WriteTo.Console()
    // Files will be created each day.
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day).CreateBootstrapLogger();
try
{
    Log.Information("Starting ExpenseTracker API");
    var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    
    // Inject Serilog as a service to the Dependancy Injection Container to use it in the application.
    // to configure it to work with the ILogger service
    builder.Host.UseSerilog((context, services, configuration) => configuration
        // Read from configuration file with the sinks it should use.
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
    );
    builder.Services.AddScoped<IExpenseService, ExpenseService>();
    builder.Services.AddScoped<IBucketService, BucketService>();

    // CORS setup between React FE and C# BE
    builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: MyAllowSpecificOrigins,
                policy =>
                {
                    policy.WithOrigins("http://localhost:5173")
                        .WithHeaders(HeaderNames.ContentType)
                        .WithMethods("PUT", "DELETE", "GET", "POST");
                });
        }

    );
    builder.Services.AddControllers()
        // Zorgt ervoor dat de enums correct worden getoond ipv van de id.
        .AddJsonOptions(options =>
        {
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


    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddOpenApi();

    var app = builder.Build();
    
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapControllers();
        app.MapScalarApiReference();
        app.MapOpenApi();
    }

    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<ExpenseTrackerContext>();
        context.Database.EnsureCreated();
        DbIntializer.Initialize(context);
    }

    app.UseHsts();
    app.UseHttpsRedirection();

    app.UseAuthorization();
    app.UseCors(MyAllowSpecificOrigins);


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


