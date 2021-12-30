using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VBMS.Domain.Contracts;

namespace VBMS.Domain.Entities.vbms.diary
{
    public class DairyType : AuditableEntity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
    }
}
