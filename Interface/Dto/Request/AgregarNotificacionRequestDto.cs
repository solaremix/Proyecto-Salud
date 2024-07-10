using System;

namespace Interface.Dto.Request
{
    public class AgregarNotificacionRequestDto
    {
        public int UsuarioId { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaHora { get; set; }
    }
}