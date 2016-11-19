using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQL.NoSQL.Library.Interfaces
{
    public abstract class IDTOBase
    {
        public virtual Guid Id { get; set; }
    }
}
