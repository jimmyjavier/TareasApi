using Microsoft.EntityFrameworkCore;
using TareasApi.Datos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TareaBd>(opt => opt.UseInMemoryDatabase("TareasBd"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

app.MapGet("/", () => "¡Hola Mundo!");

app.Run();
