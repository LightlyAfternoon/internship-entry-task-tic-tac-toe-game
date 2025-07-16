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

        public Session? FindById(long id)
        {
            using (ApplicationContext db = new ApplicationContext(Db.ConnectionString))
            {
                db.Sessions.Load();
                db.Users.Load();
                db.FieldCells.Load();

                return db.Sessions.Find(id);
            }
        }

        public List<Session> FindAll()
        {
            using (ApplicationContext db = new ApplicationContext(Db.ConnectionString))
            {
                db.Sessions.Load();
                db.Users.Load();
                db.FieldCells.Load();

                return db.Sessions.ToList();
            }
        }

        public Session Save(Session entity)
        {
            using (ApplicationContext db = new ApplicationContext(Db.ConnectionString))
            {
                if (entity.Id <= 0 || FindById(entity.Id) == null)
                {
                    db.Sessions.Add(entity);

                    db.SaveChanges();

                    db.Sessions.Load();
                    db.Users.Load();
                    db.FieldCells.Load();

                    return entity;
                }
                else
                {
                    db.Sessions.Update(entity);

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
                db.Sessions.Load();

                Session? session = db.Sessions.Find(id);

                if (session != null)
                {
                    db.Sessions.Remove(session);

                    db.SaveChanges();

                    db.Sessions.Load();
                    db.Users.Load();
                    db.FieldCells.Load();

                    return db.Sessions.Find(id) == null;
                }
                else
                    return false;
            }
        }
    }
}