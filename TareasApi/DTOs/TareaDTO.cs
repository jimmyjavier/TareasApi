using TareasApi.Modelos;

namespace TareasApi.DTOs;

public class TareaDTO
{
    
        public int Id { get; set; }
        public  string Nombre { get; set; }
        public bool EstaFinalizada { get; set; }

        public TareaDTO() { }
        public TareaDTO(Tarea tarea) =>
        (Id, Nombre, EstaFinalizada) = (tarea.Id, tarea.Nombre, tarea.EstaFinalizada);
    
}
