using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Dto
{
    public class ListasDto
    {
        public List<PerfilPacienteDto> Perfiles { get; set; }
        public List<RegistroMedicoDto> RegistrosMedicos { get; set; }

        public ListasDto()
        {
            Perfiles = new List<PerfilPacienteDto>();
            RegistrosMedicos = new List<RegistroMedicoDto>();
        }
    }
}

