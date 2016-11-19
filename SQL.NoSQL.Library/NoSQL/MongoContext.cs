using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQL.NoSQL.Library.NoSQL
{
    internal class MongoContext
    {
        private MongoClient _mongoClient;
        public IMongoDatabase DataBase { get { return this._mongoClient.GetDatabase(ConfigurationManager.AppSettings["MongoDbNAme"]); } }
        private static MongoContext CreateNewContext()
        {

            MongoContext ctx = new MongoContext();
            ctx._mongoClient = new MongoClient(ConfigurationManager.ConnectionStrings["Mongodb"].ConnectionString);
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
