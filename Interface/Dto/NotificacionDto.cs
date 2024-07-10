using System;

namespace Interface.Dto
{
    public class NotificacionDto
    {
        public int Id { get; set; }
        public string Mensaje { get; set; }
        public DateTime FechaEnvio { get; set; }
        public int UsuarioId { get; set; }
    }
}
