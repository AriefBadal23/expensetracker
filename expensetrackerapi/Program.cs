using System.Text.Json.Serialization;
using expensetrackerapi.Models;
using Microsoft.EntityFrameworkCore;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

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
});

// Make use of the PostgreSQL database as a service we inject in the DI container.
builder.Services.AddDbContext<ExpenseTrackerContext>(options => options
        .UseNpgsql(builder.Configuration.GetConnectionString("ExpenseTrackerContext"),
         o => o.MapEnum<Buckets>("buckets")));


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
