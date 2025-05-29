using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MedicalAPI.Models
{
    public class Usuario
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [BsonElement("email")]
        public string Email { get; set; } = string.Empty;

        [BsonElement("passwordHash")]
        public string PasswordHash { get; set; } = string.Empty;

        [BsonElement("rol")]
        public string Rol { get; set; } = "paciente"; // medico | recepcionista | paciente

        [BsonElement("activo")]
        public bool Activo { get; set; } = true;
    }
}
