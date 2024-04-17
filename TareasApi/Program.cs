using Microsoft.EntityFrameworkCore;
using System.Threading;
using TareasApi.Datos;
using TareasApi.Modelos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TareaBd>(opt => opt.UseInMemoryDatabase("TareasBd"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

app.MapGet("/", () => "¡Hola Mundo!");

app.MapPost("/tareas", async (Tarea tarea, TareaBd db) =>
{
    db.Tareas.Add(tarea);
    await db.SaveChangesAsync();

    return Results.Created($"/tareas/{tarea.Id}", tarea);
});

app.Run();
