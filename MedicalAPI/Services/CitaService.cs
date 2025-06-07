using MedicalAPI.Models;
using MongoDB.Driver;

namespace MedicalAPI.Services
{
    public class CitaService
    {
        private readonly IMongoCollection<Cita> _citas;

        public CitaService(MongoService mongo)
        {
            _citas = mongo.GetCollection<Cita>("citas");
        }

        public async Task CrearAsync(Cita cita) =>
            await _citas.InsertOneAsync(cita);

        public async Task<List<Cita>> ObtenerPorMedicoAsync(string medicoId) =>
            await _citas.Find(c => c.MedicoId == medicoId).ToListAsync();

        public async Task<List<Cita>> ObtenerPorPacienteAsync(string pacienteId) =>
            await _citas.Find(c => c.PacienteId == pacienteId).ToListAsync();
    
        //Actualizar citas
        public async Task ActualizarAsync(string id, Cita citaActualizada) =>
        await _citas.ReplaceOneAsync(c => c.Id == id, citaActualizada);

        public async Task<Cita?> ObtenerPorIdAsync(string id) =>
            await _citas.Find(c => c.Id == id).FirstOrDefaultAsync();

        //Cancelar citas
        public async Task CancelarAsync(string id)
        {
            var update = Builders<Cita>.Update.Set("estado", "cancelada");
            await _citas.UpdateOneAsync(c => c.Id == id, update);
        }

        public async Task<List<Cita>> ObtenerTodasAsync() =>
             await _citas.Find(_ => true).ToListAsync();

        public async Task EliminarAsync(string id) =>
            await _citas.DeleteOneAsync(c => c.Id == id);


        //Consultar por estado y fecha
        public async Task<List<Cita>> ReportePorEstadoYFecha(string estado, DateTime fecha) =>
        await _citas.Find(c =>
            c.Estado == estado &&
            c.FechaHora.Date == fecha.Date)
        .SortBy(c => c.FechaHora)
        .ToListAsync();


    }
}
