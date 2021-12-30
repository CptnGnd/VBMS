using System;
using VBMS.Domain.Contracts;

namespace VBMS.Domain.Entities.vbms.partners
{
    public class PartnerAttribute : AuditableEntity<int>
    {
        public int PartnerId { get; set; }
        public virtual Partner Partner { get; set; }
        public int PartnerTypeAttributeId { get; set; }
        public virtual PartnerTypeAttribute PartnerTypeAttribute { get; set; }
        public DateTime ValidFrom { get; set; } = DateTime.Now;
        public DateTime ValidTo { get; set; } = DateTime.MaxValue;
        public string AttributeValue { get; set; }
    }
}
