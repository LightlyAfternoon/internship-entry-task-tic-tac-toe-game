using mobibank_test.db;
using mobibank_test.model;

namespace mobibank_test.repository.impl
{
    public class FieldCellRepository : IRepository<FieldCell>
    {
        public FieldCell? FindById(long id)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                return db.FieldCells.Find(id);
            }
        }

        public List<FieldCell> FindAll()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                return db.FieldCells.ToList();
            }
        }

        public List<FieldCell> FindAllBySessionId(long sessionId)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                return db.FieldCells.Where(c => c.SessionId == sessionId).ToList();
            }
        }
        
        public List<FieldCell> FindAllByOccupiedUserId(long userId)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                return db.FieldCells.Where(c => c.OccupiedByUserId == userId).ToList();
            }
        }

        public List<FieldCell> FindAllBySessionIdAndOccupiedUserId(long sessionId, long userId)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                return db.FieldCells.Where(c => c.SessionId == sessionId && c.OccupiedByUserId == userId).ToList();
            }
        }
        
        public FieldCell Save(FieldCell entity)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                if (entity.Id <= 0 || FindById(entity.Id) == null)
                    return db.FieldCells.Add(entity).Entity;
                else
                    return db.FieldCells.Update(entity).Entity;
            }
        }

        public bool DeleteById(long id)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                FieldCell? fieldCells = db.FieldCells.Find(id);

                if (fieldCells != null)
                    return db.FieldCells.Remove(fieldCells) != null;
                else
                    return false;
            }
        }
    }
}