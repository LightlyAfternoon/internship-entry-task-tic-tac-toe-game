namespace mobibank_test.repository
{
    public interface IRepository<T>
    {
        public T? FindById(long id);
        public List<T> FindAll();
        public T Save(T entity);
        public bool DeleteById(long id);
    }
}