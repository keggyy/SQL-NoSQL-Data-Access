using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using SQL.NoSQL.BLL.SQL.DAL.Entity;

namespace SQL.NoSQL.BLL.SQL.DAL.Mapping
{
    public class SQLAppMap: ClassMapping<SQLAppEntity>
    {
        public SQLAppMap()
        {
            Table("App");
            Schema("dbo");
            Id(x => x.Id, map => { map.Generator(Generators.Guid); });
            Property(x => x.Name);
        }
    }
}
