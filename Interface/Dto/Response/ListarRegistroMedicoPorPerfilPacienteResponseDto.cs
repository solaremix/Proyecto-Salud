using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Dto.Response
{
    public class ListarRegistroMedicoPorPerfilPacienteResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<RegistroMedicoDto> RegistrosMedicos { get; set; }
    }
}
