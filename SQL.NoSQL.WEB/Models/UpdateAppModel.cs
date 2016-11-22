using SQL.NoSQL.BLL.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SQL.NoSQL.WEB.Models
{
    public class UpdateAppModel
    {
        public List<AppDto> Apps { get; set; }
        public Guid SelectedApp { get; set; }
    }
}