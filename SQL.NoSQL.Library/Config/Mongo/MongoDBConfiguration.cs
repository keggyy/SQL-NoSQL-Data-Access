using System.Configuration;

namespace SQL.NoSQL.Library.Config
{
    internal class MongoDBConfiguration: ConfigurationSection
    {
        internal static MongoDBConfiguration GetConfig()
        {
            return (MongoDBConfiguration)ConfigurationManager.GetSection("MongoDBConfiguration") ?? new MongoDBConfiguration();
        }

        [ConfigurationProperty("MongoDBCollection")]
        [ConfigurationCollection(typeof(MongoDBCollection), AddItemName = "property")]
        internal MongoDBCollection MongoDBCollection
        {
            get
            {
                object o = this["MongoDBCollection"];
                return o as MongoDBCollection;
            }
        }

    }
}
