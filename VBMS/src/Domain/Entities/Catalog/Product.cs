using VBMS.Domain.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace VBMS.Domain.Entities.Catalog
{
    public class ProductTest : AuditableEntity<int>
    {
        public string Name { get; set; }
        public string Barcode { get; set; }

        [Column(TypeName = "text")]
        public string ImageDataURL { get; set; }

        public string Description { get; set; }
        public decimal Rate { get; set; }
        public int BrandTestId { get; set; }
        public virtual BrandTest BrandTest { get; set; }
    }
}