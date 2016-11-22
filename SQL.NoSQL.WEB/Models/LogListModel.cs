using PagedList;
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
        public IPagedList<LogDto> Logs { get; set; }
        public System.Nullable<Guid> SelectedApp { get; set; }
        public string TextToSearch { get; set; }
        public long PaginationTime { get; set; }

        public LogListModel()
        {
            App = new List<AppDto>();
        }
    }
}