using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Dto
{
    public class UsuarioDto
    {
        public string email { get; set; }
        public string contrasena { get; set; }
        public List<PerfilPacienteDto> perfiles { get; set; }
        public PadreDto padre { get; set; }
    }
}
