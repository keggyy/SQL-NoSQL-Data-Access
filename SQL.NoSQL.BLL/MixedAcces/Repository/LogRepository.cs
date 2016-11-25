using PagedList;
using SQL.NoSQL.BLL.Common.DTO;
using SQL.NoSQL.BLL.MixedAcces.DAL.Entity;
using SQL.NoSQL.Library.Interfaces;
using SQL.NoSQL.Library.Mixed;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SQL.NoSQL.BLL.MixedAcces.Repository
{
    public class LogRepository : IRepository<LogDto>
    {
        private static UnitFactory _UnitFactory;

        public LogRepository()
        {
            _UnitFactory = new UnitFactory();
        }
        public override void Delete(LogDto dto)
        {
            using (IUnitOfWork op = _UnitFactory.GetUnit(this))
            {
                op.BeginTransaction();
                LogEntity entity = op.Query<LogEntity>().Where(x => x.Id.Equals(dto.Id)).FirstOrDefault();
                if (entity != null)
                    op.Delete(entity);
                op.Commit();
            }
        }

        public override List<LogDto> GetAll()
        {
            using (IUnitOfWork op = _UnitFactory.GetUnit(this))
            {
                op.BeginTransaction();
                List<LogEntity> entity = op.Query<LogEntity>().ToList();
                return ConvertEntityListToDtoList(entity);
            }
        }

        public override LogDto GetById(Guid Id)
        {
            using (IUnitOfWork op = _UnitFactory.GetUnit(this))
            {
                op.BeginTransaction();
                LogEntity entity = op.Query<LogEntity>().Where(x => x.Id.Equals(Id)).FirstOrDefault();
                return ConvertEntityToDto(entity);
            }
        }

        public override void Save(LogDto dto)
        {
            using (IUnitOfWork op = _UnitFactory.GetUnit(this))
            {
                op.BeginTransaction();
                LogEntity entity = op.Query<LogEntity>().Where(x => x.Id.Equals(dto.Id)).FirstOrDefault();
                if (entity == null)
                    entity = new LogEntity();
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
            using (IUnitOfWork op = _UnitFactory.GetUnit(this))
            {
                op.BeginTransaction();
                IQueryable<LogEntity> query = op.Query<LogEntity>();
                if (!string.IsNullOrEmpty(TextToSearch))
                    query = query.Where(x => x.Message.ToLower().Contains(TextToSearch.ToLower()));
                if (SelectedApp != null)
                    query = query.Where(x => x.AppId.Equals(SelectedApp));

                query = query.Skip((PageNum - 1) * SizePage);
                query = query.Take(SizePage);
                List<LogEntity> entity = query.ToList();
                List<LogDto> dto = ConvertEntityListToDtoList(entity);

                return new StaticPagedList<LogDto>(dto.AsEnumerable<LogDto>(), PageNum, SizePage, CountLogs(SelectedApp, TextToSearch));
            }
        }

        public List<LogReportDto> GetLogsReport()
        {
            List<LogReportDto> result = new List<LogReportDto>();
            using (IUnitOfWork op = _UnitFactory.GetUnit(this))
            {
                op.BeginTransaction();
                List<AppDto> apps = (new AppRepository()).GetAll();
                List<LogEntity> entity= op.Query<LogEntity>().ToList();
                result = entity.Join(apps.AsEnumerable(), x => x.AppId, y => y.Id, (x, y) => new { x, y }).GroupBy(z => new { z.x.AppId, z.y.Name, z.x.Level }).Select(k => new LogReportDto { Id = k.Key.AppId, Level = k.Key.Level, AppName = k.Key.Name, Count = k.Count() }).ToList();
            }
            return result;
        }

        public List<LogDto> GetLogsByAppId(Guid AppId)
        {
            List<LogDto> result = new List<LogDto>();
            using (IUnitOfWork op = _UnitFactory.GetUnit(this))
            {
                op.BeginTransaction();
                result = ConvertEntityListToDtoList(op.Query<LogEntity>().Where(x => x.AppId.Equals(AppId)).Take(50000).OrderBy(x => x.LogDate).ToList());
            }
            return result;
        }

        private int CountLogs(Guid? SelectedApp, string TextToSearch)
        {
            using (IUnitOfWork op = _UnitFactory.GetUnit(this))
            {
                op.BeginTransaction();
                IEnumerable<LogEntity> query = op.Query<LogEntity>();
                if (!string.IsNullOrEmpty(TextToSearch))
                    query = query.Where(x => x.Message.Contains(TextToSearch));
                if (SelectedApp != null)
                    query = query.Where(x => x.AppId.Equals(SelectedApp));
                return query.Count();
            }
        }
    

        #region Entity to Dto
        internal static LogDto ConvertEntityToDto(LogEntity entity)
        {
            LogDto result = new LogDto();
            if (entity != null)
            {
                result.Id = entity.Id;
                result.App = new AppDto { Id = entity.AppId };
                result.Level = entity.Level;
                result.LogDate = entity.LogDate;
                result.Message = entity.Message;
            }
            return result;
        }

        internal static List<LogDto> ConvertEntityListToDtoList(List<LogEntity> entity)
        {
            List<LogDto> result = new List<LogDto>();
            if (entity != null && entity.Count > 0)
            {
                foreach (LogEntity en in entity)
                {
                    result.Add(ConvertEntityToDto(en));
                }
            }
            return result;
        }
        #endregion
    }
}
