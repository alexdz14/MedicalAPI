using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace MedicalAPI.Models
{
    public class LogActividad
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("usuarioId")]
        public string UsuarioId { get; set; } = string.Empty;

        [BsonElement("accion")]
        public string Accion { get; set; } = string.Empty;

        [BsonElement("fechaHora")]
        public DateTime FechaHora { get; set; } = DateTime.UtcNow;
    }
}
