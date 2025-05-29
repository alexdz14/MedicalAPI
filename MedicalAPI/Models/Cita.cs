using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace MedicalAPI.Models
{
    public class Cita
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("pacienteId")]
        public string PacienteId { get; set; } = string.Empty;

        [BsonElement("medicoId")]
        public string MedicoId { get; set; } = string.Empty;

        [BsonElement("fechaHora")]
        public DateTime FechaHora { get; set; }

        [BsonElement("motivo")]
        public string Motivo { get; set; } = string.Empty;

        [BsonElement("estado")]
        public string Estado { get; set; } = "programada"; // programada | atendida | cancelada
    }
}
