using MedicalAPI.Models;
using MongoDB.Driver;

namespace MedicalAPI.Services
{
    public class LogService
    {
        private readonly IMongoCollection<LogActividad> _logs;

        public LogService(MongoService mongo)
        {
            _logs = mongo.GetCollection<LogActividad>("logs");
        }

        public async Task RegistrarAsync(string usuarioId, string accion)
        {
            var log = new LogActividad
            {
                UsuarioId = usuarioId,
                Accion = accion,
                FechaHora = DateTime.UtcNow
            };
            await _logs.InsertOneAsync(log);
        }

        public async Task<List<LogActividad>> ObtenerPorUsuario(string usuarioId) =>
            await _logs.Find(l => l.UsuarioId == usuarioId).SortByDescending(l => l.FechaHora).ToListAsync();
    }
}
