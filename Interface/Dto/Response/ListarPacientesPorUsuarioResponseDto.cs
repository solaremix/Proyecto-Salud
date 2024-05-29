using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Dto.Response
{
    public class ListarPacientesPorUsuarioResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<PerfilPacienteDto> Perfiles { get; set; } // Lista de perfiles de pacientes
    }
}
