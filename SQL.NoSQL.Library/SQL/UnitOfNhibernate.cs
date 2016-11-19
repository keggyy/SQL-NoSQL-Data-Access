using NHibernate;
using NHibernate.Linq;
using SQL.NoSQL.Library.Interfaces;
using System.Linq;

namespace SQL.NoSQL.Library.SQL
{
    public  class UnitOfNhibernate : IUnitOfWork
    {

        private ITransaction _transaction;

        public ISession Session { get; private set; }

        bool _uncommitted = true;

        public UnitOfNhibernate()
        {

            Session = NHIbernateContext.Current.SessionFactory.OpenSession();
            BeginTransaction();
        }


        public void BeginTransaction()
        {
            _transaction = Session.BeginTransaction();
            _uncommitted = true;
        }

        public void Commit()
        {
            try
            {
                _transaction.Commit();
            }
            catch
            {
                _transaction.Rollback();
                throw;
            }
            finally
            {

            }

            _uncommitted = false;
        }

        public void Dispose()
        {
            try
            {
                _transaction.Rollback();
            }
            catch
            {
            }
            finally
            {
                try
                {
                    Session.Close();
                }
                catch { }
            }
        }

        public void SaveOrUpdate(IEntityBase entity)
        {
            Session.SaveOrUpdate(entity);
        }

        public IQueryable<T> Query<T>()
        {
            return Session.Query<T>();
        }

        public void Delete(IEntityBase entity)
        {
            Session.Delete(entity);
        }
    }
}
