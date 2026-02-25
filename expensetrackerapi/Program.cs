using System.Text.Json.Serialization;
using expensetrackerapi.Models;
using expensetrackerapi.Services;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using NodaTime.Serialization.SystemTextJson;
using System.Data.SqlClient;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddScoped<IExpenseService, ExpenseService>();
builder.Services.AddScoped<IBucketService, BucketService>();
builder.Services.AddCors(
options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
    policy =>
    {
        policy.WithOrigins("http://localhost:5173").AllowAnyMethod().AllowAnyHeader();
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

Npgsql.NpgsqlConnectionStringBuilder npgsqlConnectionStringBuilder = new (builder.Configuration.GetConnectionString("ExpenseTrackerContext"))
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
    app.MapOpenApi();

}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ExpenseTrackerContext>();
    context.Database.EnsureCreated();
    DbIntializer.Initialize(context);
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors(MyAllowSpecificOrigins);

app.MapControllers();

app.Run();
