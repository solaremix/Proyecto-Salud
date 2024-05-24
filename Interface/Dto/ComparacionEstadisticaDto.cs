using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Dto
{
    public class ComparacionEstadisticaDto
    {
        public List<DatosMedicosDto> datos { get; set; }
        public int edad { get; set; }
        public string genero { get; set; }
    }
}
