using System;
using System.Collections.Generic;
using SQL.NoSQL.BLL.MixedAcces.DAL.Entity;
using SQL.NoSQL.Library.Interfaces;
using SQL.NoSQL.BLL.Common.DTO;
using SQL.NoSQL.Library.Mixed;
using System.Linq;

namespace SQL.NoSQL.BLL.MixedAcces.Repository
{
    public class AppRepository : IRepository<AppDto>
    {
        private static UnitFactory _UnitFactory;

        public AppRepository()
        {
            _UnitFactory = new UnitFactory();
        }
        public override void Delete(AppDto dto)
        {
            using (IUnitOfWork op = _UnitFactory.GetUnit(this))
            {
                op.BeginTransaction();
                AppEntity entity = op.Query<AppEntity>().Where(x => x.Id.Equals(dto.Id)).FirstOrDefault();
                if (entity != null)
                {
                    List<LogEntity> logList = op.Query<LogEntity>().Where(x => x.AppId.Equals(entity.Id)).ToList();
                    if (logList != null && logList.Count > 0)
                    {
                        foreach (LogEntity log in logList)
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
            using (IUnitOfWork op = _UnitFactory.GetUnit(this))
            {
                op.BeginTransaction();
                List<AppEntity> entity = op.Query<AppEntity>().ToList();
                return ConvertEntityListToDtoList(entity);
            }
        }

        public override AppDto GetById(Guid Id)
        {
            using (IUnitOfWork op = _UnitFactory.GetUnit(this))
            {
                op.BeginTransaction();
                AppEntity entity = op.Query<AppEntity>().Where(x => x.Id.Equals(Id)).FirstOrDefault();
                return ConvertEntityToDto(entity);
            }
        }

        public AppDto GetByName(string Name)
        {
            using (IUnitOfWork op = _UnitFactory.GetUnit(this))
            {
                op.BeginTransaction();
                AppEntity entity = op.Query<AppEntity>().Where(x => x.Name.Equals(Name)).FirstOrDefault();
                return ConvertEntityToDto(entity);
            }
        }

        public override void Save(AppDto dto)
        {
            using (IUnitOfWork op = _UnitFactory.GetUnit(this))
            {
                op.BeginTransaction();
                AppEntity entity = op.Query<AppEntity>().Where(x => x.Id.Equals(dto.Id)).FirstOrDefault();
                if (entity == null)
                    entity = new AppEntity();
                entity.Name = dto.Name;
                op.SaveOrUpdate(entity);
                op.Commit();

            }
        }

        #region Entity to Dto
        internal static AppDto ConvertEntityToDto(AppEntity entity)
        {
            AppDto result = new AppDto();
            if (entity != null)
            {
                result.Id = entity.Id;
                result.Name = entity.Name;
            }
            return result;
        }

        internal static List<AppDto> ConvertEntityListToDtoList(List<AppEntity> entity)
        {
            List<AppDto> result = new List<AppDto>();
            if (entity != null && entity.Count > 0)
            {
                foreach (AppEntity en in entity)
                {
                    result.Add(ConvertEntityToDto(en));
                }
            }
            return result;
        }
        #endregion
    }
}
