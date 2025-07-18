namespace mobibank_test.service
{
    public interface IService<T>
    {
        public Task<T?> FindById(long id);
        public Task<List<T>> FindAll();
        public Task<T> Add(T entity);
        public Task<T> Update(long id, T entity);
        public Task<bool> DeleteById(long id);
    }
}