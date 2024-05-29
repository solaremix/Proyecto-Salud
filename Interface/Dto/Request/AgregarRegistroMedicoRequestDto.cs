using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Dto.Request
{
    public class AgregarRegistroMedicoRequestDto
    {
        public int perfilPacienteId { get; set; }
        public RegistroMedicoDto registroMedico { get; set; }
    }
}
