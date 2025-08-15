using Microsoft.EntityFrameworkCore;
using To_Do.API.Data;
using To_Do.API.Repositories.Implementations;
using To_Do.API.Repositories.Interfaces;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog Configuration
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// builder.Logging.ClearProviders();
// builder.Logging.AddConsole();

// Add DbContext and Connection String
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Other Services (Repositories, Controllers, etc.)
builder.Services.AddScoped<IToDoRepositories, ToDoRepository>();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddSwaggerGen();


// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000") // React dev server address
                  .AllowAnyHeader()
                  .AllowAnyMethod(); // This includes OPTIONS, GET, POST, PUT, DELETE
        });
});

var app = builder.Build();

app.UseCors("AllowReactApp");

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        // options.SwaggerEndpoint("/swagger/v1/swagger.json", "NZWalks API v1");
        // options.RoutePrefix = string.Empty;
    });
}

app.UseAuthorization();

app.UseHttpsRedirection();

app.Run();


