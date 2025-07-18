using mobibank_test.model;

namespace mobibank_test.repository
{
    public interface IRepository<T>
    {
        public Task<T?> FindById(long id);
        public Task<List<T>> FindAll();
        public Task<T> Save(T entity);
        public Task<bool> DeleteById(long id);
    }
}