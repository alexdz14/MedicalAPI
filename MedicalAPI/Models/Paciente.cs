using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MedicalAPI.Models
{
    public class Paciente
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Nombre { get; set; } = "";
        public string Email { get; set; } = "";
        public string Telefono { get; set; } = "";
    }
}
