namespace LingoScape.Repository.Interfaces
{
    public interface IRepositoryBase<T>
    {
        IEnumerable<T> ReadAll();

        T Read(int id);

        T Create(T entity);

        T Update(T entity);

        void Delete(int id);
    }
}
