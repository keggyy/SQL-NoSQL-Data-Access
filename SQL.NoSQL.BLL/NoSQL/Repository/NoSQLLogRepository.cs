using PagedList;
using SQL.NoSQL.BLL.Common.DTO;
using SQL.NoSQL.BLL.NoSQL.DAL.Entity;
using SQL.NoSQL.Library.Interfaces;
using SQL.NoSQL.Library.NoSQL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SQL.NoSQL.BLL.NoSQL.Repository
{
    public class NoSQLLogRepository : IRepository<LogDto>
    {
        public override void Delete(LogDto dto)
        {
            using (UnitOfMongo op = new UnitOfMongo())
            {
                NoSQLLogEntity entity = op.Query<NoSQLLogEntity>().Where(x => x.Id.Equals(dto.Id)).FirstOrDefault();
                if (entity != null)
                    op.Delete(entity);
            }
        }

        public override List<LogDto> GetAll()
        {
            using (UnitOfMongo op = new UnitOfMongo())
            {
                List<NoSQLLogEntity> entity = op.Query<NoSQLLogEntity>().ToList();
                return ConvertEntityListToDtoList(entity);
            }
        }

        public override LogDto GetById(Guid Id)
        {
            using (UnitOfMongo op = new UnitOfMongo())
            {
                NoSQLLogEntity entity = op.Query<NoSQLLogEntity>().Where(x => x.Id.Equals(Id)).FirstOrDefault();
                return ConvertEntityToDto(entity);
            }
        }

        public override void Save(LogDto dto)
        {
            using (UnitOfMongo op = new UnitOfMongo())
            {
                NoSQLLogEntity entity = op.Query<NoSQLLogEntity>().Where(x => x.Id.Equals(dto.Id)).FirstOrDefault();
                if (entity == null)
                    entity = new NoSQLLogEntity();
                entity.AppId = dto.App.Id;
                entity.AppName = dto.App.Name;
                entity.Level = dto.Level;
                entity.LogDate = dto.LogDate;
                entity.Message = dto.Message;
                op.SaveOrUpdate(entity);

            }
        }

        public IPagedList<LogDto> Search(Guid? SelectedApp, string TextToSearch,int PageNum, int SizePage)
        {
            using (UnitOfMongo op = new UnitOfMongo())
            {
                IQueryable<NoSQLLogEntity> query = op.Query<NoSQLLogEntity>();
                if (!string.IsNullOrEmpty(TextToSearch))
                    query = query.Where(x => x.Message.ToLower().Contains(TextToSearch.ToLower()));
                if (SelectedApp != null)
                    query = query.Where(x => x.AppId.Equals(SelectedApp));

                query = query.Skip((PageNum - 1) * SizePage);
                query = query.Take(SizePage);
                List<NoSQLLogEntity> entity = query.ToList();
                List<LogDto> dto = ConvertEntityListToDtoList(entity);

                return new StaticPagedList<LogDto>(dto.AsEnumerable<LogDto>(), PageNum, SizePage, CountLogs(SelectedApp, TextToSearch));
            }
        }

        public List<LogReportDto> GetLogsReport()
        {
            List<LogReportDto> result = new List<LogReportDto>();
            using (UnitOfMongo op = new UnitOfMongo())
            {
                List<AppDto> apps = (new NoSQLAppRepository()).GetAll();
                result = op.Query<NoSQLLogEntity>().GroupBy(x => new { x.AppId,x.AppName, x.Level }).Select(y => new LogReportDto { Id = y.Key.AppId,AppName = y.Key.AppName, Level = y.Key.Level, Count = y.Count() }).ToList();
            }
            return result;
        }

        public List<LogDto> GetLogsByAppId(Guid AppId)
        {
            List<LogDto> result = new List<LogDto>();
            using (UnitOfMongo op = new UnitOfMongo())
            {
                result = ConvertEntityListToDtoList(op.Query<NoSQLLogEntity>().Where(x => x.AppId.Equals(AppId)).Take(10000).OrderBy(x => x.LogDate).ToList());
            }
            return result;
        }

        private int CountLogs(Guid? SelectedApp, string TextToSearch)
        {
            using (UnitOfMongo op = new UnitOfMongo())
            {
                op.BeginTransaction();
                IEnumerable<NoSQLLogEntity> query = op.Query<NoSQLLogEntity>();
                if (!string.IsNullOrEmpty(TextToSearch))
                    query = query.Where(x => x.Message.Contains(TextToSearch));
                if (SelectedApp != null)
                    query = query.Where(x => x.AppId.Equals(SelectedApp));
                return query.Count();
            }
        }

        #region Entity to Dto
        internal static LogDto ConvertEntityToDto(NoSQLLogEntity entity)
        {
            LogDto result = new LogDto();
            if (entity != null)
            {
                result.Id = entity.Id;
                result.App = new AppDto { Id = entity.AppId, Name = entity.AppName };
                result.Level = entity.Level;
                result.LogDate = entity.LogDate;
                result.Message = entity.Message;
            }
            return result;
        }

        internal static List<LogDto> ConvertEntityListToDtoList(List<NoSQLLogEntity> entity)
        {
            List<LogDto> result = new List<LogDto>();
            if (entity != null && entity.Count > 0)
            {
                foreach (NoSQLLogEntity en in entity)
                {
                    result.Add(ConvertEntityToDto(en));
                }
            }
            return result;
        }
        #endregion
    }
}
