using Microsoft.EntityFrameworkCore;
using mobibank_test.db;
using mobibank_test.mobi_test_test_project.test;
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

        public FieldCell? FindById(long id)
        {
            using (ApplicationContext db = new ApplicationContext(Db.ConnectionString))
            {
                db.Sessions.Load();
                db.Users.Load();
                db.FieldCells.Load();

                return db.FieldCells.Find(id);
            }
        }

        public List<FieldCell> FindAll()
        {
            using (ApplicationContext db = new ApplicationContext(Db.ConnectionString))
            {
                db.Sessions.Load();
                db.Users.Load();
                db.FieldCells.Load();

                return db.FieldCells.ToList();
            }
        }

        public List<FieldCell> FindAllBySessionId(long sessionId)
        {
            using (ApplicationContext db = new ApplicationContext(Db.ConnectionString))
            {
                db.Sessions.Load();
                db.Users.Load();
                db.FieldCells.Load();

                return db.FieldCells.Where(c => c.SessionId == sessionId).ToList();
            }
        }

        public List<FieldCell> FindAllByOccupiedUserId(long userId)
        {
            using (ApplicationContext db = new ApplicationContext(Db.ConnectionString))
            {
                db.Sessions.Load();
                db.Users.Load();
                db.FieldCells.Load();

                return db.FieldCells.Where(c => c.OccupiedByUserId == userId).ToList();
            }
        }

        public List<FieldCell> FindAllBySessionIdAndOccupiedUserId(long sessionId, long userId)
        {
            using (ApplicationContext db = new ApplicationContext(Db.ConnectionString))
            {
                db.Sessions.Load();
                db.Users.Load();
                db.FieldCells.Load();

                return db.FieldCells.Where(c => c.SessionId == sessionId && c.OccupiedByUserId == userId).ToList();
            }
        }

        public FieldCell Save(FieldCell entity)
        {
            using (ApplicationContext db = new ApplicationContext(Db.ConnectionString))
            {
                if (entity.Id <= 0 || FindById(entity.Id) == null)
                {
                    db.FieldCells.Add(entity);

                    db.SaveChanges();

                    db.Sessions.Load();
                    db.Users.Load();
                    db.FieldCells.Load();

                    return entity;
                }
                else
                {
                    db.FieldCells.Update(entity);

                    db.SaveChanges();

                    db.Sessions.Load();
                    db.Users.Load();
                    db.FieldCells.Load();

                    return entity;
                }
            }
        }

        public bool DeleteById(long id)
        {
            using (ApplicationContext db = new ApplicationContext(Db.ConnectionString))
            {
                db.FieldCells.Load();

                FieldCell? fieldCell = db.FieldCells.Find(id);

                if (fieldCell != null)
                {
                    db.FieldCells.Remove(fieldCell);

                    db.SaveChanges();

                    db.Sessions.Load();
                    db.Users.Load();
                    db.FieldCells.Load();

                    return db.FieldCells.Find(id) == null;
                }
                else
                    return false;
            }
        }
    }
}