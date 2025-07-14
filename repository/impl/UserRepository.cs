using mobibank_test.db;
using mobibank_test.model;

namespace mobibank_test.repository.impl
{
    public class UserRepository : IUserRepository
    {
        public User? FindById(long Id)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                return db.Users.Find(Id);
            }
        }

        public List<User> FindAll()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                return db.Users.ToList();
            }
        }

        public User Save(User entity)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                if (entity.Id <= 0 || FindById(entity.Id) == null)
                    return db.Users.Add(entity).Entity;
                else
                    return db.Users.Update(entity).Entity;
            }
        }

        public bool DeleteById(long Id)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                User? user = db.Users.Find(Id);

                if (user != null)
                    return db.Users.Remove(user) != null;
                else
                    return false;
            }
        }
    }
}