namespace SistemaGestionTareas
{
    public class Tarea
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public EstadoTarea Estado { get; set; } 
        public DateTime FechaLimite { get; set; } 
        public int Prioridad { get; set; } 

        public int ProyectoId { get; set; } 
        public Proyecto? Proyecto { get; set; } 

        public int UsuarioId { get; set; } 
        public Usuario? Usuario { get; set; } 
    }

   
    public enum EstadoTarea
    {
        Pendiente,
        EnProgreso,
        Completada
    }
}
