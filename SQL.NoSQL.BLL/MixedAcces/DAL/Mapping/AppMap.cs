using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using SQL.NoSQL.BLL.MixedAcces.DAL.Entity;

namespace SQL.NoSQL.BLL.MixedAcces.DAL.Mapping
{
    public class AppMap:ClassMapping<AppEntity>
    {
        public AppMap()
        {
            Table("App");
            Schema("dbo");
            Id(x => x.Id, map => { map.Generator(Generators.Guid); });
            Property(x => x.Name);
        }
    }
}
