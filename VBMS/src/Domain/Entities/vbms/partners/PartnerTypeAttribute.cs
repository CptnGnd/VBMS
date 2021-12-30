using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VBMS.Domain.Contracts;
using VBMS.Domain.Enums;

namespace VBMS.Domain.Entities.vbms.partners
{
    public class PartnerTypeAttribute : AuditableEntity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public AttributeType AttributeType { get; set; }
        public int PartnerTypeId { get; set; }
        public virtual PartnerType PartnerType { get; set; }
    }
}
