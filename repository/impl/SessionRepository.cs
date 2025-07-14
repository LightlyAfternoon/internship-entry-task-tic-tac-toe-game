using mobibank_test.db;
using mobibank_test.model;

namespace mobibank_test.repository.impl
{
    public class SessionRepository : IRepository<Session>
    {
        public Session? FindById(long id)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                return db.Sessions.Find(id);
            }
        }

        public List<Session> FindAll()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                return db.Sessions.ToList();
            }
        }

        public Session Save(Session entity)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                if (entity.Id <= 0 || FindById(entity.Id) == null)
                    return db.Sessions.Add(entity).Entity;
                else
                    return db.Sessions.Update(entity).Entity;
            }
        }

        public bool DeleteById(long id)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Session? sessions = db.Sessions.Find(id);

                if (sessions != null)
                    return db.Sessions.Remove(sessions) != null;
                else
                    return false;
            }
        }
    }
}