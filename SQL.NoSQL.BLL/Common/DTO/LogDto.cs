using SQL.NoSQL.Library.Interfaces;
using System;

namespace SQL.NoSQL.BLL.Common.DTO
{
    public class LogDto:IDTOBase
    {
        public string Message { get; set; }
        public string Level { get; set; }
        public DateTime LogDate { get; set; } 
        public AppDto App { get; set; }
    }
}
