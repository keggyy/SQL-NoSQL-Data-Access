using SQL.NoSQL.Library.Interfaces;
using System;

namespace SQL.NoSQL.BLL.SQL.DAL.Entity
{
    public class SQLLogEntity:IEntityBase
    {
        public virtual string Message { get; set; }
        public virtual string Level { get; set; }
        public virtual DateTime LogDate { get; set; }
        public virtual SQLAppEntity App { get; set; }
    }
}
