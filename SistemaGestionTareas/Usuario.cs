namespace SistemaGestionTareas
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Contraseña { get; set; } 
        public string CorreoElectronico { get; set; }
        public string Rol { get; set; } 

        public List<Tarea>? Tareas { get; set; } 
    }
}
