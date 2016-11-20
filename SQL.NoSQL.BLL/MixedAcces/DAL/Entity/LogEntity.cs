using SQL.NoSQL.Library.Interfaces;
using System;

namespace SQL.NoSQL.BLL.MixedAcces.DAL.Entity
{
    public class LogEntity:IEntityBase
    {
        public virtual string Message { get; set; }
        public virtual string Level { get; set; }
        public virtual DateTime LogDate { get; set; }
        public virtual Guid AppId { get; set; }
    }
}
