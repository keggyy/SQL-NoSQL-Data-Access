using System;
using System.Linq;

namespace SQL.NoSQL.Library.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        void BeginTransaction();
        void Commit();
        void SaveOrUpdate(IEntityBase entity);
        void Delete(IEntityBase entity);
        IQueryable<T> Query<T>();
    }
}
