using NHibernate.Dialect;
using NHibernate.SqlCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//SQL.NoSQL.BLL.Common.SQLDialettic
namespace SQL.NoSQL.BLL.Common
{
    public class SQLDialettic: MsSql2012Dialect
    {
        public override SqlString GetLimitString(SqlString querySqlString, SqlString offset, SqlString limit)
        {
            var result = base.GetLimitString(querySqlString, offset, limit);

            return result.Replace("ORDER BY CURRENT_TIMESTAMP", "ORDER BY 1");
        }
    }
}
