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

        public List<LogDto> Search(Guid? SelectedApp, string TextToSearch)
        {
            using (IUnitOfWork op = _UnitFactory.GetUnit(this))
            {
                op.BeginTransaction();
                IQueryable<LogEntity> query = op.Query<LogEntity>();
                if (!string.IsNullOrEmpty(TextToSearch))
                    query = query.Where(x => x.Message.Contains(TextToSearch));
                if (SelectedApp != null)
                    query = query.Where(x => x.AppId.Equals(SelectedApp));
                List<LogEntity> entity = query.ToList();
                return ConvertEntityListToDtoList(entity);
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
