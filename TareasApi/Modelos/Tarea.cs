namespace TareasApi.Modelos;

public class Tarea
{
    public int Id { get; set; }
    public required string Nombre { get; set; }
    public bool EstaFinalizada { get; set; }
}

