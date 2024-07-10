using System.Collections.Generic;

namespace Interface.Dto.Request
{
    public class AgregarVacunaRequestDto
    {
        public VacunaDto Vacuna { get; set; }
        public List<EsquemaVacunacionDto> EsquemaVacunacion { get; set; } // Asegúrate de que esta propiedad exista
    }
}
