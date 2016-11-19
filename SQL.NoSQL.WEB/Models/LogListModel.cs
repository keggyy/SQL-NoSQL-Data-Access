using SQL.NoSQL.BLL.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SQL.NoSQL.WEB.Models
{
    public class LogListModel
    {
        public List<AppDto> App { get; set; }
        public List<LogDto> Logs { get; set; }

        public LogListModel()
        {
            App = new List<AppDto>();
            Logs = new List<LogDto>();
        }
    }
}