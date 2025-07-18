using Microsoft.EntityFrameworkCore;
using mobibank_test.db;
using mobibank_test.model;

namespace mobibank_test.repository.impl
{
    public class FieldCellRepository : IFieldCellRepository
    {
        private readonly ApplicationContext Db;

        public FieldCellRepository(ApplicationContext applicationContext)
        {
            Db = applicationContext;
        }

        public async Task<FieldCell?> FindById(long id)
        {
            using (ApplicationContext db = new ApplicationContext(Db.ConnectionString))
            {
                await db.Sessions.LoadAsync();
                await db.Users.LoadAsync();
                await db.FieldCells.LoadAsync();

                return await db.FieldCells.FindAsync(id);
            }
        }

        public async Task<List<FieldCell>> FindAll()
        {
            using (ApplicationContext db = new ApplicationContext(Db.ConnectionString))
            {
                await db.Sessions.LoadAsync();
                await db.Users.LoadAsync();
                await db.FieldCells.LoadAsync();

                return await db.FieldCells.ToListAsync();
            }
        }

        public async Task<List<FieldCell>> FindAllBySessionId(long sessionId)
        {
            using (ApplicationContext db = new ApplicationContext(Db.ConnectionString))
            {
                await db.Sessions.LoadAsync();
                await db.Users.LoadAsync();
                await db.FieldCells.LoadAsync();

                return await db.FieldCells.Where(c => c.SessionId == sessionId).ToListAsync();
            }
        }

        public async Task<List<FieldCell>> FindAllByOccupiedUserId(long userId)
        {
            using (ApplicationContext db = new ApplicationContext(Db.ConnectionString))
            {
                await db.Sessions.LoadAsync();
                await db.Users.LoadAsync();
                await db.FieldCells.LoadAsync();

                return await db.FieldCells.Where(c => c.OccupiedByUserId == userId).ToListAsync();
            }
        }

        public async Task<List<FieldCell>> FindAllBySessionIdAndOccupiedUserId(long sessionId, long userId)
        {
            using (ApplicationContext db = new ApplicationContext(Db.ConnectionString))
            {
                await db.Sessions.LoadAsync();
                await db.Users.LoadAsync();
                await db.FieldCells.LoadAsync();

                return await db.FieldCells.Where(c => c.SessionId == sessionId && c.OccupiedByUserId == userId).ToListAsync();
            }
        }

        public async Task<FieldCell> Save(FieldCell entity)
        {
            using (ApplicationContext db = new ApplicationContext(Db.ConnectionString))
            {
                if (entity.Id <= 0 || await FindById(entity.Id) == null)
                {
                    await db.FieldCells.AddAsync(entity);

                    await db.SaveChangesAsync();

                    await db.Sessions.LoadAsync();
                    await db.Users.LoadAsync();
                    await db.FieldCells.LoadAsync();

                    return entity;
                }
                else
                {
                    db.FieldCells.Update(entity);

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
                await db.FieldCells.LoadAsync();

                FieldCell? fieldCell = await db.FieldCells.FindAsync(id);

                if (fieldCell != null)
                {
                    db.FieldCells.Remove(fieldCell);

                    await db.SaveChangesAsync();

                    await db.Sessions.LoadAsync();
                    await db.Users.LoadAsync();
                    await db.FieldCells.LoadAsync();

                    return await db.FieldCells.FindAsync(id) == null;
                }
                else
                    return false;
            }
        }
    }
}