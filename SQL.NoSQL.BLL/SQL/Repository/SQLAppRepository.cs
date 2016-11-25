using SQL.NoSQL.BLL.Common.DTO;
using SQL.NoSQL.BLL.SQL.DAL.Entity;
using SQL.NoSQL.Library.Interfaces;
using SQL.NoSQL.Library.SQL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SQL.NoSQL.BLL.SQL.Repository
{
    public class SQLAppRepository : IRepository<AppDto>
    {
        public override void Delete(AppDto dto)
        {
            using (UnitOfNhibernate op = new UnitOfNhibernate())
            {
                op.BeginTransaction();
                SQLAppEntity entity = op.Query<SQLAppEntity>().Where(x => x.Id.Equals(dto.Id)).FirstOrDefault();
                if (entity != null)
                {
                    List<SQLLogEntity> logList = op.Query<SQLLogEntity>().Where(x => x.App.Id.Equals(entity.Id)).ToList();
                    if (logList != null && logList.Count > 0)
                    {
                        foreach (SQLLogEntity log in logList)
                        {
                            op.Delete(log);
                        }
                    }
                    op.Delete(entity);

                    op.Commit();
                }
            }
        }

        public override List<AppDto> GetAll()
        {
            using (UnitOfNhibernate op = new UnitOfNhibernate())
            {
                op.BeginTransaction();
                List<SQLAppEntity> entity = op.Query<SQLAppEntity>().ToList();
                return ConvertEntityListToDtoList(entity);
            }
        }

        public override AppDto GetById(Guid Id)
        {
            using (UnitOfNhibernate op = new UnitOfNhibernate())
            {
                op.BeginTransaction();
                SQLAppEntity entity = op.Query<SQLAppEntity>().Where(x => x.Id.Equals(Id)).FirstOrDefault();
                return ConvertEntityToDto(entity);
            }
        }

        public AppDto GetByName(string  Name)
        {
            using (UnitOfNhibernate op = new UnitOfNhibernate())
            {
                op.BeginTransaction();
                SQLAppEntity entity = op.Query<SQLAppEntity>().Where(x => x.Name.Equals(Name)).FirstOrDefault();
                return ConvertEntityToDto(entity);
            }
        }

        public override void Save(AppDto dto)
        {
            using (UnitOfNhibernate op = new UnitOfNhibernate())
            {
                op.BeginTransaction();
                SQLAppEntity entity = op.Query<SQLAppEntity>().Where(x => x.Id.Equals(dto.Id)).FirstOrDefault();
                if (entity == null)
                    entity = new SQLAppEntity();
                entity.Name = dto.Name;
                op.SaveOrUpdate(entity);
                op.Commit();
                
            }
        }

        #region Entity to Dto
        internal static AppDto ConvertEntityToDto(SQLAppEntity entity)
        {
            AppDto result = new AppDto();
            if(entity!=null)
            {
                result.Id = entity.Id;
                result.Name = entity.Name;
            }
            return result;
        } 

        internal static List<AppDto> ConvertEntityListToDtoList(List<SQLAppEntity> entity)
        {
            List<AppDto> result = new List<AppDto>();
            if(entity!= null && entity.Count>0)
            {
                foreach(SQLAppEntity en in entity)
                {
                    result.Add(ConvertEntityToDto(en));
                }
            }
            return result;
        }
        #endregion

    }
}
