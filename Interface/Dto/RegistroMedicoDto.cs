using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Dto
{
    public class RegistroMedicoDto
    {
        public int registroMedicoId { get; set; }
        public DateTime fecha { get; set; }
        public DatosMedicosDto datos { get; set; }
    }
}
