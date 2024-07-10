namespace Interface.Dto.Response
{
    public class AgregarVacunaResponseDto
    {
        public int VacunaId { get; set; } // Asegúrate de que esta propiedad exista
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
