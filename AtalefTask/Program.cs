using AtalefTask.Infrastructure;
using AtalefTask.Models;
using AtalefTask.Services;
using AtalefTask.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var Services = builder.Services;
var Configuration = builder.Configuration;
Services.AddControllers();
Services.AddAutoMapper(typeof(Program));
Services.AddDbContext<ApplicationContext>(options => 
    options.UseSqlServer(Configuration.GetConnectionString("SqlConnection"), o =>
    {
        o.EnableRetryOnFailure(
                maxRetryCount: 20,
                maxRetryDelay: TimeSpan.FromSeconds(2),
                errorNumbersToAdd: null);
    }));
Services.AddTransient<ISmartMatchService, SmartMatchService>();
Services.AddEndpointsApiExplorer();
Services.AddSwaggerGen(c =>
{
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
    $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRestExceptionHandler();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
