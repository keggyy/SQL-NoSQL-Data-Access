using SQL.NoSQL.BLL.Common.DTO;
using SQL.NoSQL.BLL.NoSQL.DAL.Entity;
using SQL.NoSQL.Library.Interfaces;
using SQL.NoSQL.Library.NoSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQL.NoSQL.BLL.NoSQL.Repository
{
    public class NoSQLAppRepository : IRepository<AppDto>
    {
        public override void Delete(AppDto dto)
        {
            using (UnitOfMongo op = new UnitOfMongo())
            {
                List<NoSQLLogEntity> entity = op.Query<NoSQLLogEntity>().Where(x => x.AppId.Equals(dto.Id)).ToList();
                if (entity != null && entity.Count > 0)
                {
                    foreach (NoSQLLogEntity log in entity)
                    {
                        op.Delete(log);
                    }
                }
            }
        }

        public override List<AppDto> GetAll()
        {
            using (UnitOfMongo op = new UnitOfMongo())
            {
                return op.Query<NoSQLLogEntity>().Select(x => new AppDto { Id = x.AppId, Name = x.AppName }).Distinct().ToList();
            }
        }

        public override AppDto GetById(Guid Id)
        {
            using (UnitOfMongo op = new UnitOfMongo())
            {
                return op.Query<NoSQLLogEntity>().Where(x => x.AppId.Equals(Id)).Select(x => new AppDto { Id = x.AppId, Name = x.AppName }).FirstOrDefault();
            }
        }

        public override void Save(AppDto dto)
        {
            using (UnitOfMongo op = new UnitOfMongo())
            {
                List<NoSQLLogEntity> logs = op.Query<NoSQLLogEntity>().Where(x => x.AppId.Equals(dto.Id)).ToList();
                if(logs!= null && logs.Count> 0)
                {
                    foreach(NoSQLLogEntity entity in logs)
                    {
                        entity.AppName = dto.Name;
                        op.SaveOrUpdate(entity);
                    }
                }
            }
        }
    }
}
