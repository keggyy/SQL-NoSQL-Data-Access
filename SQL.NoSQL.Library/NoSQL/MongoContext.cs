using MongoDB.Driver;
using SQL.NoSQL.Library.Config;
using System.Configuration;

namespace SQL.NoSQL.Library.NoSQL
{
    internal class MongoContext
    {
        private MongoClient _mongoClient;
        public IMongoDatabase DataBase { get { return this._mongoClient.GetDatabase(MongoDBConfiguration.GetConfig().MongoDBCollection["CollectionName"].Value); } }
        private static MongoContext CreateNewContext()
        {

            MongoContext ctx = new MongoContext();
            ctx._mongoClient = new MongoClient(MongoDBConfiguration.GetConfig().MongoDBCollection["Connection"].Value);
            return ctx;
        }

        private static MongoContext _Current;
        public static MongoContext Current
        {
            get
            {
                return _Current ?? (_Current = CreateNewContext());
            }
        }
    }
}
