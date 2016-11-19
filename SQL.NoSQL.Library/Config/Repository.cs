using System.Configuration;

namespace SQL.NoSQL.Library.Config
{
    public class Repository : ConfigurationElement
    {
        [ConfigurationProperty("RepositoryName", IsRequired = true)]
        public string RepositoryName
        {
            get { return this["RepositoryName"] as string; }
        }

        [ConfigurationProperty("DataBase", IsRequired = true)]
        public string DataBase
        {
            get { return this["DataBase"] as string; }
        }
    }
}
