using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Dto
{
    public class UsuarioDto
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Dni { get; set; }
        public string Email { get; set; }
        public string Contrasena { get; set; }
        public string Genero { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public ListasDto Listas { get; set; }

        public UsuarioDto()
        {
            Listas = new ListasDto();
        }
    }
}


