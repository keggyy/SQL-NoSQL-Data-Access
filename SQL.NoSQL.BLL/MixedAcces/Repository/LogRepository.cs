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
                    query = query.Where(x => x.Message.Contains(TextToSearch));
                if (SelectedApp != null)
                    query = query.Where(x => x.AppId.Equals(SelectedApp));

                query = query.Skip((PageNum - 1) * SizePage);
                query = query.Take(SizePage);
                List<LogEntity> entity = query.ToList();
                List<LogDto> dto = ConvertEntityListToDtoList(entity);

                return dto.AsEnumerable<LogDto>().ToPagedList<LogDto>(PageNum, SizePage);
            }
        }

        public List<LogReportDto> GetLogsReport()
        {
            List<LogReportDto> result = new List<LogReportDto>();
            using (IUnitOfWork op = _UnitFactory.GetUnit(this))
            {
                op.BeginTransaction();
                List<AppDto> apps = (new AppRepository()).GetAll();
                //result = op.Query<SQLLogEntity>().GroupBy(x => new { x.AppId, x.Level }).Select(y => new LogReportDto { Id = y.Key.AppId, Level = y.Key.Level, Count = y.Count() }).Join(apps, s => s.Id, t => t.Id, (s, t) => new LogReportDto { AppName = t.Name, Count = s.Count, Id = s.Id, Level = s.Level }).ToList();
                result = op.Query<LogEntity>().GroupBy(x => new { x.AppId, x.Level }).Select(y => new LogReportDto { Id = y.Key.AppId, Level = y.Key.Level, Count = y.Count() }).ToList();//.Join(op.Query<SQLAppEntity>(), s => s.Id, t => t.Id, (s, t) => new LogReportDto { AppName = t.Name, Count = s.Count, Id = s.Id, Level = s.Level }).ToList();
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
                result.App = (new AppRepository().GetById(entity.AppId));
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
