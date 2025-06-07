namespace MedicalAPI.DTOs
{
    public class CitaDto
    {
        public string PacienteId { get; set; }
        public string MedicoId { get; set; }
        public DateTime FechaHora { get; set; }
        public string Motivo { get; set; }
    }
}
