using SQL.NoSQL.BLL.Common.DTO;
using SQL.NoSQL.BLL.SQL.DAL.Entity;
using SQL.NoSQL.Library.Interfaces;
using SQL.NoSQL.Library.SQL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SQL.NoSQL.BLL.SQL.Repository
{
    public class SQLLogRepository : IRepository<LogDto>
    {
        public override void Delete(LogDto dto)
        {
            using (UnitOfNhibernate op = new UnitOfNhibernate())
            {
                op.BeginTransaction();
                SQLLogEntity entity = op.Query<SQLLogEntity>().Where(x => x.Id.Equals(dto.Id)).FirstOrDefault();
                if (entity != null)
                    op.Delete(entity);
                op.Commit();
            }
        }

        public override List<LogDto> GetAll()
        {
            using (UnitOfNhibernate op = new UnitOfNhibernate())
            {
                op.BeginTransaction();
                List<SQLLogEntity> entity = op.Query<SQLLogEntity>().ToList();
                return ConvertEntityListToDtoList(entity);
            }
        }

        public override LogDto GetById(Guid Id)
        {
            using (UnitOfNhibernate op = new UnitOfNhibernate())
            {
                op.BeginTransaction();
                SQLLogEntity entity = op.Query<SQLLogEntity>().Where(x => x.Id.Equals(Id)).FirstOrDefault();
                return ConvertEntityToDto(entity);
            }
        }

        public override void Save(LogDto dto)
        {
            using (UnitOfNhibernate op = new UnitOfNhibernate())
            {
                op.BeginTransaction();
                SQLLogEntity entity = op.Query<SQLLogEntity>().Where(x => x.Id.Equals(dto.Id)).FirstOrDefault();
                if (entity == null)
                    entity = new SQLLogEntity();
                entity.AppId = dto.App.Id;
                entity.Level = dto.Level;
                entity.LogDate = dto.LogDate;
                entity.Message = dto.Message;
                op.SaveOrUpdate(entity);
                op.Commit();

            }
        }

        public List<LogDto> Search(Guid? SelectedApp, string TextToSearch)
        {
            using (UnitOfNhibernate op = new UnitOfNhibernate())
            {
                op.BeginTransaction();
                IEnumerable<SQLLogEntity> query = op.Query<SQLLogEntity>();
                if (!string.IsNullOrEmpty(TextToSearch))
                    query = query.Where(x => x.Message.Contains(TextToSearch));
                if (SelectedApp != null)
                    query = query.Where(x => x.AppId.Equals(SelectedApp));
                List<SQLLogEntity> entity = query.ToList();
                return ConvertEntityListToDtoList(entity);
            }
        }

        #region Entity to Dto
        internal static LogDto ConvertEntityToDto(SQLLogEntity entity)
        {
            LogDto result = new LogDto();
            if (entity != null)
            {
                result.Id = entity.Id;
                result.App = (new SQLAppRepository().GetById(entity.AppId));
                result.Level = entity.Level;
                result.LogDate = entity.LogDate;
                result.Message = entity.Message;
            }
            return result;
        }

        internal static List<LogDto> ConvertEntityListToDtoList(List<SQLLogEntity> entity)
        {
            List<LogDto> result = new List<LogDto>();
            if (entity != null && entity.Count > 0)
            {
                foreach (SQLLogEntity en in entity)
                {
                    result.Add(ConvertEntityToDto(en));
                }
            }
            return result;
        }
        #endregion
    }
}
