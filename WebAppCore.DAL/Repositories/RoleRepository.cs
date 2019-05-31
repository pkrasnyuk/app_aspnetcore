using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using WebAppCore.DAL.DataAccessHelpers;
using WebAppCore.Domain.Entities;

namespace WebAppCore.DAL.Repositories
{
    public class RoleRepository : EntityRepository<Role<ObjectId>, ObjectId>
    {
        public RoleRepository(IOptions<DataAccessConfiguration> dbOptions) : base(dbOptions, "Roles")
        {
        }

        public async Task<ICollection<Role<ObjectId>>> GetUserRolesAsync(string email)
        {
            var user = await Context.GetCollection<User<ObjectId>>("Users").Find(x => x.Email.Equals(email)).SingleOrDefaultAsync();
            if (user != null)
            {
                var userRoles = await Context.GetCollection<UserRole<ObjectId>>("UserRoles")
                    .Find(x => x.UserId.Equals(user.Id)).ToListAsync();
                if (userRoles != null && userRoles.Any())
                {
                    var userRoleIds = userRoles.Select(userRole => userRole.RoleId).ToList();
                    return await Collection.Find(x => userRoleIds.Contains(x.Id)).ToListAsync();
                }
            }

            return null;
        }

        public async Task<Role<ObjectId>> GetRoleByNameAsync(string name)
        {
            return await GetEntityAsync(x => x.Name.Equals(name));
        }
    }
}