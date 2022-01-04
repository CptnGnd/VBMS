using VBMS.Domain.Contracts;

namespace VBMS.Domain.Entities.vbms.diary
{
    public class DiaryType : AuditableEntity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Colour { get; set; }
    }
}
