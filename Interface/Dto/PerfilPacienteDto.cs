using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Dto
{
    public class PerfilPacienteDto
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public DateTime fechaNacimiento { get; set; }
        public string dni { get; set; }
        public string genero { get; set; }
        public int edadAnios { get; set; }
        public int edadMeses { get; set; }

    }
}