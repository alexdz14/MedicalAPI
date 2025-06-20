using MedicalAPI.DTOs;
using MedicalAPI.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MedicalAPI.Services
{
    public class CitaService
    {
        private readonly IMongoCollection<Cita> _citas;
        private readonly IMongoCollection<Paciente> _pacientes;
        private readonly IMongoCollection<Usuario> _usuarios;

        public CitaService(MongoService mongo)
        {
            _citas = mongo.GetCollection<Cita>("citas");
            _pacientes = mongo.GetCollection<Paciente>("pacientes");
            _usuarios = mongo.GetCollection<Usuario>("usuarios");

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

        //Obtiene todas las citas
        public async Task<List<Cita>> ObtenerTodasAsync() =>
             await _citas.Find(_ => true).ToListAsync();

        public async Task EliminarAsync(string id) =>
            await _citas.DeleteOneAsync(c => c.Id == id);

        //Obtiene un resumen de citas por médico
        public async Task<List<CitaResumen>> ObtenerResumenPorMedicoAsync(string medicoId)
        {
            var citas = await _citas.Find(c => c.MedicoId == medicoId).ToListAsync();
            var resumen = new List<CitaResumen>();

            foreach (var cita in citas)
            {
                var paciente = await _pacientes.Find(p => p.Id == cita.PacienteId).FirstOrDefaultAsync();

                string nombrePaciente;

                if (paciente != null)
                {
                    nombrePaciente = paciente.Nombre;
                }
                else
                {
                    var usuario = await _usuarios.Find(u => u.Id == cita.PacienteId && u.Rol == "paciente").FirstOrDefaultAsync();
                    nombrePaciente = usuario != null ? usuario.Nombre : "Paciente desconocido";
                }

                resumen.Add(new CitaResumen
                {
                    Paciente = nombrePaciente,
                    FechaHora = cita.FechaHora,
                    Motivo = cita.Motivo,
                    Estado = cita.Estado
                });
            }

            return resumen;
        }


        public async Task<List<object>> ObtenerTodasConNombresAsync()
        {
            var citas = await _citas.Find(_ => true).ToListAsync();
            var resultado = new List<object>();

            foreach (var cita in citas)
            {
                string nombrePaciente = "Paciente desconocido";
                string nombreMedico = "Médico desconocido";

                if (ObjectId.TryParse(cita.PacienteId, out _))
                {
                    var paciente = await _pacientes.Find(p => p.Id == cita.PacienteId).FirstOrDefaultAsync();
                    if (paciente != null)
                        nombrePaciente = paciente.Nombre;
                    else
                    {
                        var usuario = await _usuarios.Find(u => u.Id == cita.PacienteId && u.Rol == "paciente").FirstOrDefaultAsync();
                        if (usuario != null)
                            nombrePaciente = usuario.Nombre;
                    }
                }

                if (ObjectId.TryParse(cita.MedicoId, out _))
                {
                    var medico = await _usuarios.Find(u => u.Id == cita.MedicoId && u.Rol == "medico").FirstOrDefaultAsync();
                    if (medico != null)
                        nombreMedico = medico.Nombre;
                }

                resultado.Add(new
                {
                    id = cita.Id,
                    pacienteId = cita.PacienteId,
                    pacienteNombre = nombrePaciente,
                    medicoId = cita.MedicoId,
                    medicoNombre = nombreMedico,
                    fechaHora = cita.FechaHora,
                    motivo = cita.Motivo,
                    estado = cita.Estado
                });
            }

            return resultado;
        }

        //Consultar por estado y fecha
        public async Task<List<Cita>> ReportePorEstadoYFecha(string estado, DateTime fecha) =>
        await _citas.Find(c =>
            c.Estado == estado &&
            c.FechaHora.Date == fecha.Date)
        .SortBy(c => c.FechaHora)
        .ToListAsync();


    }
}
