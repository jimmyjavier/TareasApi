using Microsoft.EntityFrameworkCore;
using TareasApi.Modelos;

namespace TareasApi.Datos;

public class TareaBd : DbContext
{
    public TareaBd(DbContextOptions<TareaBd> options)
        : base(options) { }

    public DbSet<Tarea> Tareas => Set<Tarea>();
}

