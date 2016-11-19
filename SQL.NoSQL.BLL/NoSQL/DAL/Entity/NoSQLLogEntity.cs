
using SQL.NoSQL.Library.Interfaces;
using System;

namespace SQL.NoSQL.BLL.NoSQL.DAL.Entity
{
    public class NoSQLLogEntity:IEntityBase
    {
        public virtual string Message { get; set; }
        public virtual string Level { get; set; }
        public virtual DateTime LogDate { get; set; }
        public virtual Guid AppId { get; set; }
        public virtual string AppName { get; set; }
    }
}
