namespace Cassandra.Core
{
    public interface ICassandraRepository<TEntity>
    {
        /// <summary>
        /// It asynchronously performs the operation of adding a new record.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task AddAsync(TEntity entity);

        /// <summary>
        /// It performs the operation of adding a new record synchronously.
        /// </summary>
        /// <param name="entity"></param>
        void Add(TEntity entity);

        /// <summary>
        /// It asynchronously performs the delete operation.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAsync(string id);

        /// <summary>
        /// It performs the operation of deleting a record synchronously.
        /// </summary>
        /// <param name="id"></param>
        void Delete(string id);

        /// <summary>
        /// It asynchronously returns all records as IEnumerable<TEntity>
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAllAsync();

        /// <summary>
        /// It returns all records synchronously as an IEnumerable<TEntity>.
        /// </summary>
        /// <returns></returns>
        IEnumerable<TEntity> GetAll();

        /// <summary>
        /// It asynchronously returns the model based on the ID value
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> GetByIdAsync(string id);

        /// <summary>
        /// It asynchronously performs the update operation.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task UpdateAsync(string id, TEntity entity);

        /// <summary>
        /// It performs the operation of updating a record synchronously.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        void Update(string id, TEntity entity);

        /// <summary>
        /// After the operations are completed, the _session and _cluster processes are terminated.
        /// </summary>
        void Dispose();
    }
}
