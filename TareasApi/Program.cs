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


tareasGroup.MapGet("/", ObtenerTareas);

tareasGroup.MapGet("/finalizadas", ObtenerTareasFinalizadas);

tareasGroup.MapGet("/{id}", ObtenerTarea);

tareasGroup.MapPost("/", AgregarTarea);

tareasGroup.MapPut("/{id}", ActualizarTarea);

tareasGroup.MapDelete("/{id}", EliminarTarea);

app.Run();


static async Task<IResult> ObtenerTareas(TareaBd db)
{
    return TypedResults.Ok(await db.Tareas.ToListAsync());
}

static async Task<IResult> ObtenerTareasFinalizadas(TareaBd db)
{
    return TypedResults.Ok(await db.Tareas.Where(u => u.EstaFinalizada).ToListAsync());
}

static async Task<IResult> ObtenerTarea(int id, TareaBd db)
{
    return await db.Tareas.FindAsync(id)
        is Tarea tarea
            ? TypedResults.Ok(tarea)
            : TypedResults.NotFound();
}

static async Task<IResult> AgregarTarea(Tarea tarea, TareaBd db)
{
    db.Tareas.Add(tarea);
    await db.SaveChangesAsync();

    return TypedResults.Created($"/tareas/{tarea.Id}", tarea);
}

static async Task<IResult> ActualizarTarea(int id, Tarea inputTarea, TareaBd db)
{
    var tarea = await db.Tareas.FindAsync(id);

    if (tarea is null) return TypedResults.NotFound();

    tarea.Nombre = inputTarea.Nombre;
    tarea.EstaFinalizada = inputTarea.EstaFinalizada;

    await db.SaveChangesAsync();

    return TypedResults.NoContent();
}


static async Task<IResult> EliminarTarea(int id, TareaBd db)
{
    if (await db.Tareas.FindAsync(id) is Tarea tarea)
    {
        db.Tareas.Remove(tarea);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }

    return TypedResults.NotFound();
}