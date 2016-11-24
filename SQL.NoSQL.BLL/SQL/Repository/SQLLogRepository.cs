using PagedList;
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

        public IPagedList<LogDto> Search(Guid? SelectedApp, string TextToSearch, int PageNum, int SizePage)
        {
            using (UnitOfNhibernate op = new UnitOfNhibernate())
            {
                op.BeginTransaction();
                IEnumerable<SQLLogEntity> query = op.Query<SQLLogEntity>();
                if (!string.IsNullOrEmpty(TextToSearch))
                    query = query.Where(x => x.Message.ToLower().Contains(TextToSearch.ToLower()));
                if (SelectedApp != null)
                    query = query.Where(x => x.AppId.Equals(SelectedApp));
                query = query.Skip((PageNum - 1) * SizePage);
                query = query.Take(SizePage);
                List<SQLLogEntity> entity = query.ToList();
                List<LogDto> dto = ConvertEntityListToDtoList(entity);

                return new StaticPagedList<LogDto>(dto.AsEnumerable<LogDto>(), PageNum, SizePage, CountLogs(SelectedApp,TextToSearch));
            }
        }

        public List<LogReportDto> GetLogsReport()
        {
            List<LogReportDto> result = new List<LogReportDto>();
            using (UnitOfNhibernate op = new UnitOfNhibernate())
            {
                op.BeginTransaction();
                List<AppDto> apps = (new SQLAppRepository()).GetAll();
                //result = op.Query<SQLLogEntity>().GroupBy(x => new { x.AppId, x.Level }).Select(y => new LogReportDto { Id = y.Key.AppId, Level = y.Key.Level, Count = y.Count() }).Join(apps, s => s.Id, t => t.Id, (s, t) => new LogReportDto { AppName = t.Name, Count = s.Count, Id = s.Id, Level = s.Level }).ToList();
                result = op.Query<SQLLogEntity>().GroupBy(x => new { x.AppId, x.Level }).Select(y => new LogReportDto { Id = y.Key.AppId, Level = y.Key.Level, Count = y.Count() }).ToList();//.Join(op.Query<SQLAppEntity>(), s => s.Id, t => t.Id, (s, t) => new LogReportDto { AppName = t.Name, Count = s.Count, Id = s.Id, Level = s.Level }).ToList();
            }
            return result;
        }

        public List<LogDto> GetLogsByAppId(Guid AppId)
        {
            List<LogDto> result = new List<LogDto>();
            using (UnitOfNhibernate op = new UnitOfNhibernate())
            {
                op.BeginTransaction();
                result = ConvertEntityListToDtoList(op.Query<SQLLogEntity>().Where(x => x.AppId.Equals(AppId)).OrderBy(x => x.LogDate).ToList());
            }
            return result;
        }

        private int CountLogs(Guid? SelectedApp, string TextToSearch)
        {
            using (UnitOfNhibernate op = new UnitOfNhibernate())
            {
                op.BeginTransaction();
                IEnumerable<SQLLogEntity> query = op.Query<SQLLogEntity>();
                if (!string.IsNullOrEmpty(TextToSearch))
                    query = query.Where(x => x.Message.Contains(TextToSearch));
                if (SelectedApp != null)
                    query = query.Where(x => x.AppId.Equals(SelectedApp));
                return query.Count();
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
