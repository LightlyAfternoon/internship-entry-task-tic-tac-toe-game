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

        public User? FindById(long id)
        {
            using (ApplicationContext db = new ApplicationContext(Db.ConnectionString))
            {
                db.Sessions.Load();
                db.Users.Load();
                db.FieldCells.Load();

                return db.Users.Find(id);
            }
        }

        public List<User> FindAll()
        {
            using (ApplicationContext db = new ApplicationContext(Db.ConnectionString))
            {
                db.Sessions.Load();
                db.Users.Load();
                db.FieldCells.Load();

                return db.Users.ToList();
            }
        }

        public User Save(User entity)
        {
            using (ApplicationContext db = new ApplicationContext(Db.ConnectionString))
            {
                if (entity.Id <= 0 || FindById(entity.Id) == null)
                {
                    db.Users.Add(entity);

                    db.SaveChanges();

                    db.Sessions.Load();
                    db.Users.Load();
                    db.FieldCells.Load();

                    return entity;
                }
                else
                {
                    db.Users.Update(entity);

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
                db.Users.Load();

                User? user = db.Users.Find(id);

                if (user != null)
                {
                    db.Users.Remove(user);

                    db.SaveChanges();

                    db.Sessions.Load();
                    db.Users.Load();
                    db.FieldCells.Load();

                    return db.Users.Find(id) == null;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}