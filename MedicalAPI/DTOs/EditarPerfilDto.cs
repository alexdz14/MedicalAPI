namespace MedicalAPI.DTOs
{
    public class EditarPerfilDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? NuevaPassword { get; set; }
    }
}
