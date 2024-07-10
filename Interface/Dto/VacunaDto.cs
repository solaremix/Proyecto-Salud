using System;

namespace Interface.Dto
{
    public class VacunaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Status { get; set; }
        public DateTime FechaAplicacion { get; set; }
        public int PerfilPacienteId { get; set; }
        public string PerfilPacienteNombre { get; set; }
    }
}
