using Microsoft.EntityFrameworkCore;
using mobibank_test.db;
using mobibank_test.model;

namespace mobibank_test.repository.impl
{
    public class SessionRepository : ISessionRepository
    {
        private readonly ApplicationContext Db;

        public SessionRepository(ApplicationContext applicationContext)
        {
            Db = applicationContext;
        }

        public async Task<Session?> FindById(long id)
        {
            using (ApplicationContext db = new ApplicationContext(Db.ConnectionString))
            {
                await db.Sessions.LoadAsync();
                await db.Users.LoadAsync();
                await db.FieldCells.LoadAsync();

                return await db.Sessions.FindAsync(id);
            }
        }

        public async Task<List<Session>> FindAll()
        {
            using (ApplicationContext db = new ApplicationContext(Db.ConnectionString))
            {
                await db.Sessions.LoadAsync();
                await db.Users.LoadAsync();
                await db.FieldCells.LoadAsync();

                return await db.Sessions.ToListAsync();
            }
        }

        public async Task<Session> Save(Session entity)
        {
            using (ApplicationContext db = new ApplicationContext(Db.ConnectionString))
            {
                if (entity.Id <= 0 || await FindById(entity.Id) == null)
                {
                    await db.Sessions.AddAsync(entity);

                    await db.SaveChangesAsync();

                    await db.Sessions.LoadAsync();
                    await db.Users.LoadAsync();
                    await db.FieldCells.LoadAsync();

                    return entity;
                }
                else
                {
                    db.Sessions.Update(entity);

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
                await db.Sessions.LoadAsync();

                Session? session = await db.Sessions.FindAsync(id);

                if (session != null)
                {
                    db.Sessions.Remove(session);

                    await db.SaveChangesAsync();

                    await db.Sessions.LoadAsync();
                    await db.Users.LoadAsync();
                    await db.FieldCells.LoadAsync();

                    return await db.Sessions.FindAsync(id) == null;
                }
                else
                    return false;
            }
        }
    }
}