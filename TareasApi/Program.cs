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


app.MapPut("/tareas/{id}", async (int id, Tarea inputTarea, TareaBd db) =>
{
    var todo = await db.Tareas.FindAsync(id);

    if (todo is null) return Results.NotFound();

    todo.Nombre = inputTarea.Nombre;
    todo.EstaFinalizada = inputTarea.EstaFinalizada;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/tareas/{id}", async (int id, TareaBd db) =>
{
    if (await db.Tareas.FindAsync(id) is Tarea tarea)
    {
        db.Tareas.Remove(tarea);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});

app.Run();
