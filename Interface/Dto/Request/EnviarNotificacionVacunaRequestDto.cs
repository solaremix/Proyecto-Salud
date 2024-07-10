using System;

namespace Interface.Dto.Request
{
    public class EnviarNotificacionVacunaRequestDto
    {
        public int PerfilPacienteId { get; set; }
        public string NombreVacuna { get; set; }
        public DateTime FechaAplicacion { get; set; }
    }
}
