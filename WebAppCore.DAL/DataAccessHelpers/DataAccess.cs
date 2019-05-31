using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace WebAppCore.DAL.DataAccessHelpers
{
    public class DataAccess
    {
        public DataAccess(IOptions<DataAccessConfiguration> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            DbContext = client.GetDatabase(options.Value.DbName);
        }

        public IMongoDatabase DbContext { get; }
    }
}