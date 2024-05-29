using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Dto.Request
{
    public class AgregarPerfilPacienteRequestDto
    {
        public int UsuarioId { get; set; }
        public PerfilPacienteDto PerfilPaciente { get; set; }
    }
}
