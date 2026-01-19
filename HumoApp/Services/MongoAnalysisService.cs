using MongoDB.Driver;
using MongoDB.Bson;
using HumoApp.Dtos;

namespace HumoApp.Services
{
    public interface IMongoAnalysisService
    {
        Task SaveAnalysisAsync(AnalysisResponseDto analysis);
        Task<AnalysisResponseDto?> GetAnalysisByIdAsync(string id);
        Task<List<AnalysisResponseDto>> GetAllAnalysisAsync();
    }

    public class MongoAnalysisService : IMongoAnalysisService
    {
        private readonly IMongoCollection<AnalysisResponseDto> _collection;

        public MongoAnalysisService(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            _collection = database.GetCollection<AnalysisResponseDto>("Analysis");
        }

        public async Task SaveAnalysisAsync(AnalysisResponseDto analysis)
        {
            analysis.Id = ObjectId.GenerateNewId().ToString();
            analysis.CreatedAt = DateTime.UtcNow;
            await _collection.InsertOneAsync(analysis);
        }

        public async Task<AnalysisResponseDto?> GetAnalysisByIdAsync(string id)
        {
            return await _collection.Find(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<AnalysisResponseDto>> GetAllAnalysisAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }
    }
}
