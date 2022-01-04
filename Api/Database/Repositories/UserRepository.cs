using System;
using System.Threading.Tasks;
using Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Database.Repositories
{
    public class UserRepository
    {
        private readonly IDbContextFactory<ApplicationContext> _contextFactory;

        public UserRepository(IDbContextFactory<ApplicationContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task CreateUser(User user)
        {
            user.Id = Guid.NewGuid();
            var dbContext = _contextFactory.CreateDbContext();
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
        }
    }
}