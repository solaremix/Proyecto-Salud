using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Dto.Request
{
    public class CambiarContrasenaRequestDto
    {
        public string email { get; set; }
        public string nuevaContrasena { get; set; }
    }
}