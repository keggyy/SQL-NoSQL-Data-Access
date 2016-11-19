using MongoDB.Driver;
using SQL.NoSQL.Library.Interfaces;
using System;
using System.Linq;

namespace SQL.NoSQL.Library.NoSQL
{
    public  class UnitOfMongo : IUnitOfWork
    {
        private IMongoDatabase database;
        private IMongoCollection<IEntityBase> collection;

        public void Dispose()
        {

        }

        public UnitOfMongo()
        {
            database = MongoContext.Current.DataBase;
        }

        public void BeginTransaction()
        {
           
        }

        public void Commit()
        {

        }


        public void SaveOrUpdate(IEntityBase entity)
        {

            collection = GetCollection(entity);
            if (entity.Id == null || (new Guid("{00000000-0000-0000-0000-000000000000}").CompareTo(entity.Id) == 0))
            {
                collection.InsertOne(entity);
            }
            else
            {
                FilterDefinition<IEntityBase> filter = Builders<IEntityBase>.Filter.Eq(x => x.Id, entity.Id);
                ReplaceOneResult result = collection.ReplaceOne(filter, entity);
            }
        }

        public void Delete(IEntityBase entity)
        {
            collection = GetCollection(entity);
            collection.DeleteOne(x => x.Id == entity.Id);
        }

        public IQueryable<T> Query<T>()
        {

            return database.GetCollection<T>(typeof(T).Name).AsQueryable<T>();
        }

        private IMongoCollection<IEntityBase> GetCollection(IEntityBase entity)
        {
            Type type = entity.GetType();
            return collection = database.GetCollection<IEntityBase>(type.Name);
        }
    }
}
