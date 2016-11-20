using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQL.NoSQL.Library.Config
{
    public class Property: ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return this["name"] as string; }
        }

        [ConfigurationProperty("value", IsRequired = true)]
        public string Value
        {
            get { return this["value"] as string; }
        }
    }
}
