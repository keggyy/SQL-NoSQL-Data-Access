using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using SQL.NoSQL.BLL.MixedAcces.DAL.Entity;

namespace SQL.NoSQL.BLL.MixedAcces.DAL.Mapping
{
    public class LogMap: ClassMapping<LogEntity>

    { 
        public LogMap()
        {
            Table("Log");
            Schema("dbo");
            Id(x => x.Id, map => { map.Generator(Generators.Guid); });
            Property(x => x.AppId, map => { map.NotNullable(true); });
            Property(x => x.Level);
            Property(x => x.LogDate);
            Property(x => x.Message);
        }
    }
}
