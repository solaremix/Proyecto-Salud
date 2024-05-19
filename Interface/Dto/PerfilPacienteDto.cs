using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Dto
{
    public class PerfilPacienteDto
    {
        public string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Dni { get; set; }
        public string Genero { get; set; }
        public int EdadAnios { get; set; }
        public int EdadMeses { get; set; }
        public List<RegistroMedicoDto> RegistrosMedicos { get; set; }

        public PerfilPacienteDto()
        {
            RegistrosMedicos = new List<RegistroMedicoDto>();
        }
    }
}

