using System;
using System.Net;
using System.Threading.Tasks;
using Cloud_Lab.Entities;
using Cloud_Lab.Entities.DTO;
using Microsoft.EntityFrameworkCore;

namespace Cloud_Lab.DataAccess.Database.Repositories
{
    public class UserRepository
    {
        private readonly IDbContextFactory<ApplicationContext> _contextFactory;

        public UserRepository(IDbContextFactory<ApplicationContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<OperationResult> CreateUser(User user)
        {
            try
            {
                user.Id = Guid.NewGuid();
                var dbContext = _contextFactory.CreateDbContext();

                var dbUser = await dbContext.Users.FirstOrDefaultAsync(elem => elem.Username == user.Username);
                if (dbUser != null)
                    return new OperationResult(HttpStatusCode.BadRequest, "Такой пользователь уже существует");

                dbContext.Users.Add(user);
                await dbContext.SaveChangesAsync();
                return new OperationResult();
            }
            catch (Exception)
            {
                return new OperationResult<User>(HttpStatusCode.InternalServerError, "Не удалось выполнить операцию");
            }
        }

        public async Task<OperationResult<User>> GetUser(User user)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext();
                var dbUser = await dbContext.Users.FirstOrDefaultAsync(elem => elem.Username == user.Username);
                if (dbUser == null)
                    return new OperationResult<User>(HttpStatusCode.NotFound, "Такого пользователя не существует");
                return dbUser.Password != user.Password
                    ? new OperationResult<User>(HttpStatusCode.BadRequest, "Неверный пароль")
                    : new OperationResult<User>(dbUser);
            }
            catch (Exception)
            {
                return new OperationResult<User>(HttpStatusCode.InternalServerError, "Не удалось выполнить операцию");
            }
        }

        public async Task<OperationResult<User>> GetUser(string username)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext();
                var dbUser = await dbContext.Users.FirstOrDefaultAsync(elem =>
                    elem.Username == username);
                return dbUser == null
                    ? new OperationResult<User>(HttpStatusCode.NotFound, "Такого пользователя не существует")
                    : new OperationResult<User>(dbUser);
            }
            catch (Exception)
            {
                return new OperationResult<User>(HttpStatusCode.InternalServerError, "Не удалось выполнить операцию");
            }
        }
    }
}