using Microsoft.EntityFrameworkCore;
using TareasApi.Datos;
using TareasApi.Modelos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TareaBd>(opt => 

opt.UseSqlite(builder.Configuration.GetConnectionString("TareasBd"))

);
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

app.MapGet("/", () => "¡Hola Mundo!");

var tareasGroup = app.MapGroup("/tareas");

tareasGroup.MapPost("/", async (Tarea tarea, TareaBd db) =>
{
    db.Tareas.Add(tarea);
    await db.SaveChangesAsync();

    return Results.Created($"/tareas/{tarea.Id}", tarea);
});

tareasGroup.MapGet("/", async (TareaBd db) =>
    await db.Tareas.ToListAsync());

tareasGroup.MapGet("/finalizadas", async (TareaBd db) =>
    await db.Tareas.Where(u => u.EstaFinalizada).ToListAsync());

tareasGroup.MapGet("/{id}", async (int id, TareaBd db) =>
    await db.Tareas.FindAsync(id)
        is Tarea tarea
            ? Results.Ok(tarea)
            : Results.NotFound());


tareasGroup.MapPut("/{id}", async (int id, Tarea inputTarea, TareaBd db) =>
{
    var todo = await db.Tareas.FindAsync(id);

    if (todo is null) return Results.NotFound();

    todo.Nombre = inputTarea.Nombre;
    todo.EstaFinalizada = inputTarea.EstaFinalizada;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

tareasGroup.MapDelete("/{id}", async (int id, TareaBd db) =>
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
