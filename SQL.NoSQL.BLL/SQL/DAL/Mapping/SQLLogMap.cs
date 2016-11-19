﻿using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using SQL.NoSQL.BLL.SQL.DAL.Entity;

namespace SQL.NoSQL.BLL.SQL.DAL.Mapping
{
    public class SQLLogMap: ClassMapping<SQLLogEntity>
    {
        public SQLLogMap()
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
