using MedicalAPI.Models;
using MongoDB.Driver;
using MedicalAPI.Models;

namespace MedicalAPI.Services
{
    public class PacienteService
    {
        private readonly IMongoCollection<Paciente> _pacientes;

        public PacienteService(IConfiguration config)
        {
            var connectionString = config["ClinicaDbSettings:ConnectionString"];
            var databaseName = config["ClinicaDbSettings:DatabaseName"];

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            _pacientes = database.GetCollection<Paciente>("pacientes");
        }

        public async Task CrearAsync(Paciente paciente)
        {
            if (string.IsNullOrWhiteSpace(paciente.Id))
                paciente.Id = null;

            await _pacientes.InsertOneAsync(paciente);
        }

        public async Task<List<Paciente>> GetAllAsync() =>
            await _pacientes.Find(_ => true).ToListAsync();

        public async Task<Paciente?> ObtenerPorIdAsync(string id) =>
            await _pacientes.Find(p => p.Id == id).FirstOrDefaultAsync();

        public async Task ActualizarAsync(string id, Paciente paciente) =>
            await _pacientes.ReplaceOneAsync(p => p.Id == id, paciente);

        public async Task EliminarAsync(string id) =>
            await _pacientes.DeleteOneAsync(p => p.Id == id);

    }

}
