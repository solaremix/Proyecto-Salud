using System;

namespace Interface.Dto.Request
{
    public class EnviarNotificacionIMCRequestDto
    {
        public int PerfilPacienteId { get; set; }
        public float IMC { get; set; }
        public string Estado { get; set; } // "Anorexia", "Sobrepeso", etc.
    }
}
