using Microsoft.Extensions.Options;
using MongoDB.Bson;
using WebAppCore.DAL.DataAccessHelpers;
using WebAppCore.Domain.Entities;

namespace WebAppCore.DAL.Repositories
{
    public class ErrorRepository : EntityRepository<Error<ObjectId>, ObjectId>
    {
        public ErrorRepository(IOptions<DataAccessConfiguration> dbOptions) : base(dbOptions, "Errors")
        {
        }
    }
}