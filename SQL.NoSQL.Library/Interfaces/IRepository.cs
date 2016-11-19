using System;
using System.Collections.Generic;

namespace SQL.NoSQL.Library.Interfaces
{
    public abstract class IRepository<T> where T: IDTOBase
    {
        public abstract T GetById(Guid Id);
        public abstract List<T> GetAll();
        public abstract void Save(T dto);
        public abstract void Delete(T dto);
    }
}
