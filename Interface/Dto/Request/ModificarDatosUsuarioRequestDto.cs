﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Dto.Request
{
    public class ModificarDatosUsuarioRequestDto
    {
        public int usuarioId { get; set; }
        public string email { get; set; }
        public string contrasena { get; set; }
        public PadreDto padre { get; set; }
    }
}
