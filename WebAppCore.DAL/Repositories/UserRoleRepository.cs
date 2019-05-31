using Microsoft.Extensions.Options;
using MongoDB.Bson;
using WebAppCore.DAL.DataAccessHelpers;
using WebAppCore.Domain.Entities;

namespace WebAppCore.DAL.Repositories
{
    public class UserRoleRepository : EntityRepository<UserRole<ObjectId>, ObjectId>
    {
        public UserRoleRepository(IOptions<DataAccessConfiguration> dbOptions) : base(dbOptions, "UserRoles")
        {
        }
    }
}