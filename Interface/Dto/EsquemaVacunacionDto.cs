using System;
using System.Collections.Generic;

namespace Interface.Dto
{
    public class EsquemaVacunacionDto
    {
        public int Id { get; set; }
        public int VacunaId { get; set; }
        public int PerfilPacienteId { get; set; }
        public DateTime FechaAplicacion { get; set; }
        public int Estado { get; set; } // 0: Pendiente, 1: Aceptado, 2: Atrasado

    }
}
