using Cassandra.Core;

namespace Cassandra.DataAccessLayer
{
    public class CassandraRepository<TEntity> : ICassandraRepository<TEntity>
    {
        private readonly ISession _session;
        private readonly Cluster _cluster;
        private string _keyspace;

        public CassandraRepository
        (
            string keyspace
            , string contactPoint = "localhost"
        )
        {
            _keyspace = keyspace;
            _cluster = Cluster
                          .Builder()
                          .AddContactPoint(contactPoint)
                          .Build();

            _session = _cluster
                       .Connect(keyspace);
        }

        public Task AddAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(string id, TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}