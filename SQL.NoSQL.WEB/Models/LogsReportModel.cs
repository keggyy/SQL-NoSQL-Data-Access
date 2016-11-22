using SQL.NoSQL.BLL.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SQL.NoSQL.WEB.Models
{
    public class LogsReportModel
    {
        public List<LogReportDto> Items { get; set; }
        public long TimeReport { get; set; }

        public LogsReportModel()
        {
            Items = new List<LogReportDto>();
        }
    }
}