using SQL.NoSQL.Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQL.NoSQL.BLL.Common.DTO
{
    public class LogReportDto:IDTOBase
    {
        public string AppName { get; set; }
        public string Level { get; set; }
        public int Count { get; set; }
    }
}
