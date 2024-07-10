using System;

namespace Interface.Dto
{
    public class CitaVacunacionDto
    {
        public int PerfilPacienteId { get; set; }
        public DateTime FechaCita { get; set; }
        public string NombreClinica { get; set; }
    }
}
