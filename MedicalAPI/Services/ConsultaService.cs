using MedicalAPI.Models;
using MongoDB.Driver;

namespace MedicalAPI.Services
{
    public class ConsultaService
    {
        private readonly IMongoCollection<Consulta> _consultas;

        public ConsultaService(MongoService mongo)
        {
            _consultas = mongo.GetCollection<Consulta>("consultas");
        }

        public async Task CrearAsync(Consulta consulta) =>
            await _consultas.InsertOneAsync(consulta);

        public async Task<List<Consulta>> HistorialPorPacienteAsync(string pacienteId) =>
            await _consultas.Find(c => c.PacienteId == pacienteId)
                            .SortByDescending(c => c.FechaHora)
                            .ToListAsync();

        //Reporte de consultas por médico y fechas
        public async Task<List<Consulta>> FiltrarPorMedicoYFechas(string medicoId, DateTime desde, DateTime hasta) =>
            await _consultas.Find(c =>
                c.MedicoId == medicoId &&
                c.FechaHora >= desde &&
                c.FechaHora <= hasta)
            .SortByDescending(c => c.FechaHora)
            .ToListAsync();
    }
}
