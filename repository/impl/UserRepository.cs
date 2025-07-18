using Microsoft.EntityFrameworkCore;
using mobibank_test.db;
using mobibank_test.model;

namespace mobibank_test.repository.impl
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext Db;

        public UserRepository(ApplicationContext applicationContext)
        {
            Db = applicationContext;
        }

        public async Task<User?> FindById(long id)
        {
            using (ApplicationContext db = new ApplicationContext(Db.ConnectionString))
            {
                await db.Sessions.LoadAsync();
                await db.Users.LoadAsync();
                await db.FieldCells.LoadAsync();

                return await db.Users.FindAsync(id);
            }
        }

        public async Task<List<User>> FindAll()
        {
            using (ApplicationContext db = new ApplicationContext(Db.ConnectionString))
            {
                await db.Sessions.LoadAsync();
                await db.Users.LoadAsync();
                await db.FieldCells.LoadAsync();

                return await db.Users.ToListAsync();
            }
        }

        public async Task<User> Save(User entity)
        {
            using (ApplicationContext db = new ApplicationContext(Db.ConnectionString))
            {
                if (entity.Id <= 0 || await FindById(entity.Id) == null)
                {
                    await db.Users.AddAsync(entity);

                    await db.SaveChangesAsync();

                    await db.Sessions.LoadAsync();
                    await db.Users.LoadAsync();
                    await db.FieldCells.LoadAsync();

                    return entity;
                }
                else
                {
                    db.Users.Update(entity);

                    await db.SaveChangesAsync();

                    await db.Sessions.LoadAsync();
                    await db.Users.LoadAsync();
                    await db.FieldCells.LoadAsync();

                    return entity;
                }
            }
        }

        public async Task<bool> DeleteById(long id)
        {
            using (ApplicationContext db = new ApplicationContext(Db.ConnectionString))
            {
                await db.Users.LoadAsync();

                User? user = await db.Users.FindAsync(id);

                if (user != null)
                {
                    db.Users.Remove(user);

                    await db.SaveChangesAsync();

                    await db.Sessions.LoadAsync();
                    await db.Users.LoadAsync();
                    await db.FieldCells.LoadAsync();

                    return await db.Users.FindAsync(id) == null;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}