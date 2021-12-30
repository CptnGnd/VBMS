using VBMS.Domain.Contracts;

namespace VBMS.Domain.Entities.Catalog
{
    public class BrandTest : AuditableEntity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Tax { get; set; }
    }
}