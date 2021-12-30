using VBMS.Domain.Contracts;

namespace VBMS.Domain.Entities.vbms.partners
{
    public class PartnerType : AuditableEntity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
