using System.Configuration;

namespace SQL.NoSQL.Library.Config
{
    public class RepositoryConfiguration : ConfigurationSection
    {
        internal static RepositoryConfiguration GetConfig()
        {
            return (RepositoryConfiguration)ConfigurationManager.GetSection("RepositoryConfiguration") ?? new RepositoryConfiguration();
        }


        [ConfigurationProperty("RepositoryCollection")]
        [ConfigurationCollection(typeof(RepositoryCollection), AddItemName = "Repository")]
        internal RepositoryCollection RepositoryCollection
        {
            get
            {
                object o = this["RepositoryCollection"];
                return o as RepositoryCollection;
            }
        }

    }
}
