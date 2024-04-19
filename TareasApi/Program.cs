using Microsoft.EntityFrameworkCore;
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

app.MapGet("/tareas", async (TareaBd db) =>
    await db.Tareas.ToListAsync());

app.MapGet("/tareas/{id}", async (int id, TareaBd db) =>
    await db.Tareas.FindAsync(id)
        is Tarea tarea
            ? Results.Ok(tarea)
            : Results.NotFound());



app.Run();
