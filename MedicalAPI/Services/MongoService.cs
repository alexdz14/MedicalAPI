using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MedicalAPI.Models;

namespace MedicalAPI.Services
{
    public class MongoService
    {
        public readonly IMongoDatabase Database;

        public MongoService(IOptions<ClinicaDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            Database = client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string name) =>
            Database.GetCollection<T>(name);
    }
}
