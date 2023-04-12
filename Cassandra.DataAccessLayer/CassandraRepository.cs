using Cassandra.Core;
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

        /// <summary>
        /// It performs the operation of adding a new record synchronously.
        /// </summary>
        /// <param name="entity"></param>
       public void Add(TEntity entity) => _session.Execute(AddBase(entity));

        /// <summary>
        /// It asynchronously performs the delete operation.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(string id) => await _session.ExecuteAsync(DeleteBase(id));

        /// <summary>
        /// It performs the operation of deleting a record synchronously.
        /// </summary>
        /// <param name="id"></param>
        public void Delete(string id) => _session.Execute(DeleteBase(id));

        /// <summary>
        /// It asynchronously returns all records as IEnumerable<TEntity>
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            var result = await _session.ExecuteAsync(ListBase());
            return ListSelectBase(result);
        }

        /// <summary>
        /// It returns all records synchronously as an IEnumerable<TEntity>.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TEntity> GetAll() => ListSelectBase(_session.Execute(ListBase()));

        /// <summary>
        /// It asynchronously performs the operation of adding a new record.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<TEntity> GetByIdAsync(string id) => SingleOrDefaultBase(await _session.ExecuteAsync(GetBase(id)));


        /// <summary>
        /// It asynchronously performs the update operation.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task UpdateAsync(string id, TEntity entity) => _session.ExecuteAsync(UpdateBase(id, entity));

        /// <summary>
        /// It performs the operation of updating a record synchronously.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        public void Update(string id, TEntity entity) => _session.Execute(UpdateBase(id, entity));

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

        /// <summary>
        /// To delete the record with the given ID value, it is converted to a SimpleStatement object and sent to the ISession.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private SimpleStatement DeleteBase(string id)
        {
            return new SimpleStatement(
                                        $"DELETE FROM {_keyspace}.{typeof(TEntity).Name.ToLower()} WHERE id = ?"
                                        , id
                                      );
        }

        /// <summary>
        /// It returns all values in the table with the given _keyspace and TEntity.
        /// </summary>
        /// <returns></returns>
        private SimpleStatement ListBase()
        {
            return new SimpleStatement($"SELECT * FROM {_keyspace}.{typeof(TEntity).Name.ToLower()}");
        }

        /// <summary>
        /// It returns a list of TEntity from the RowSet? object returned after the ISession execute.
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        private IEnumerable<TEntity> ListSelectBase(RowSet? rows)
        {
            return rows.Select(
                                    r =>
                                    JsonConvert
                                    .DeserializeObject<TEntity>
                                    (
                                        r.GetValue<string>("json")
                                    )
                              );
        }

        /// <summary>
        /// A query is created that returns the record with the given ID value, and then it is converted to a SimpleStatement object and returned.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private SimpleStatement GetBase(string id)
        {
            return new SimpleStatement(
                                        $"SELECT * FROM {_keyspace}.{typeof(TEntity).Name.ToLower()} WHERE id = ?"
                                        , id
                                      );
        }

        /// <summary>
        /// It returns a TEntity model from the RowSet? object returned after the ISession execute.
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        private TEntity SingleOrDefaultBase(RowSet? rows)
        {
            return JsonConvert
                   .DeserializeObject<TEntity>
                   (
                      rows.SingleOrDefault().GetValue<string>("json")
                   );
        }

        /// <summary>
        /// Query _keyspace, Id ve TEntity ile beraber oluşturulur ve SimpleStatement nesnesine dönüştürülür.
        /// </summary>
        /// <param name="id">Update işlemi yapılacak olan row(guid)</param>
        /// <param name="entity">Update edilmesi istenen değerler TEntity jenerik modelinde eklenir</param>
        /// <returns></returns>
        private SimpleStatement UpdateBase(string id, TEntity entity)
        {
            return new SimpleStatement(
                                        $"UPDATE {_keyspace}.{typeof(TEntity).Name.ToLower()} SET JSON ? WHERE id = ?"
                                        , JObject.FromObject(entity).ToString()
                                        , id
                                      );
        }
        #endregion
    }
}