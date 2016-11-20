using System;
using System.Collections.Generic;

namespace SQL.NoSQL.Library.Interfaces
{
    public interface IRepository { }
    public abstract class IRepository<T>:IRepository where T: IDTOBase
    {
        public abstract T GetById(Guid Id);
        public abstract List<T> GetAll();
        public abstract void Save(T dto);
        public abstract void Delete(T dto);
    }
}
