namespace mobibank_test.service
{
    public interface IService<T>
    {
        public T? FindById(long id);
        public List<T> FindAll();
        public T Add(T entity);
        public T Update(long id, T entity);
        public bool DeleteById(long id);
    }
}