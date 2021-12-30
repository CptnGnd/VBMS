using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VBMS.Domain.Contracts;

namespace VBMS.Domain.Entities.vbms.partners
{
    public class Partner : AuditableEntity<int>
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public int PartnerTypeId { get; set; }
        public string ImageDataURL { get; set; }
        public virtual PartnerType PartnerType { get; set; }
    }
}
