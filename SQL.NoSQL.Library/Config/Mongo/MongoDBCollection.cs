using System.Configuration;

namespace SQL.NoSQL.Library.Config
{
    internal class MongoDBCollection: ConfigurationElementCollection
    {
        public Property this[int index]
        {
            get { return base.BaseGet(index) as Property; }
        }

        public new Property this[string responseString]
        {
            get { return (Property)BaseGet(responseString); }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new Property();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Property)element).Name;
        }
    }
}
