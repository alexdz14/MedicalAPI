namespace MedicalAPI.DTOs
{
    public class CitaResumen
    {
        public string Paciente { get; set; } = "";
        public DateTime FechaHora { get; set; }
        public string Motivo { get; set; } = "";
        public string Estado { get; set; } = "";
    }
}
