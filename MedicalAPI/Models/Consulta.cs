using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace MedicalAPI.Models
{
    public class Consulta
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

        [BsonElement("sintomas")]
        public string Sintomas { get; set; } = string.Empty;

        [BsonElement("diagnostico")]
        public string Diagnostico { get; set; } = string.Empty;

        [BsonElement("tratamiento")]
        public string Tratamiento { get; set; } = string.Empty;

        [BsonElement("citaId")]
        [BsonIgnoreIfNull]
        public string? CitaId { get; set; }
    }
}
