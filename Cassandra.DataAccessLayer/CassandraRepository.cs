﻿using Cassandra.Core;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

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

        /// <summary>
        /// It asynchronously performs the operation of adding a new record.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task AddAsync(TEntity entity) => await _session.ExecuteAsync(AddBase(entity));

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

        #region Base
        //The Base region contains common operations that both synchronous and asynchronous operations will execute. 

        /// <summary>
        /// The Query is created with _keyspace and TEntity, and converted to a SimpleStatement object.
        /// </summary>
        /// <param name="entity">The values to be inserted are added in the generic model TEntity.</param>
        /// <returns></returns>
        private SimpleStatement AddBase(TEntity entity)
        {
            return new SimpleStatement(
                                        $"INSERT INTO {_keyspace}.{typeof(TEntity).Name.ToLower()} JSON ?"
                                        , JObject.FromObject(entity).ToString()
                                      );
        }
        #endregion
    }
}