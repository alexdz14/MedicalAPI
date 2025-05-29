using MedicalAPI.Helpers;
using MedicalAPI.Models;
using MongoDB.Driver;

namespace MedicalAPI.Services
{
    public class UsuarioService
    {
        private readonly IMongoCollection<Usuario> _usuarios;

        public UsuarioService(MongoService mongo)
        {
            _usuarios = mongo.GetCollection<Usuario>("usuarios");
        }

        public async Task<List<Usuario>> GetAllAsync() =>
            await _usuarios.Find(_ => true).ToListAsync();
    

    public async Task<Usuario?> BuscarPorEmailAsync(string email) =>
    await _usuarios.Find(u => u.Email == email).FirstOrDefaultAsync();

        public async Task CrearAsync(Usuario usuario) =>
            await _usuarios.InsertOneAsync(usuario);

        //metodo para editar perfil
        public async Task<Usuario?> ObtenerPorIdAsync(string id) =>
        await _usuarios.Find(u => u.Id == id).FirstOrDefaultAsync();

        public async Task ActualizarPerfilAsync(string id, string nombre, string email, string? nuevaPassword)
        {
            var update = Builders<Usuario>.Update
                .Set(u => u.Nombre, nombre)
                .Set(u => u.Email, email);

            if (!string.IsNullOrEmpty(nuevaPassword))
            {
                update = update.Set(u => u.PasswordHash, PasswordHelper.HashPassword(nuevaPassword));
            }

            await _usuarios.UpdateOneAsync(u => u.Id == id, update);
        }

    }
}
