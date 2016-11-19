using System.Configuration;

namespace SQL.NoSQL.Library.Config
{
    public class RepositoryCollection : ConfigurationElementCollection
    {
        public Repository this[int index]
        {
            get { return base.BaseGet(index) as Repository; }
        }

        public new Repository this[string responseString]
        {
            get { return (Repository)BaseGet(responseString); }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new Repository();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Repository)element).RepositoryName;
        }
    }
}
