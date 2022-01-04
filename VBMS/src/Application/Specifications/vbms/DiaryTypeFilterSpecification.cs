using VBMS.Application.Specifications.Base;
using VBMS.Domain.Entities.vbms.diary;

namespace VBMS.Application.Specifications.vbms
{
    public class DiaryTypeFilterSpecification : HeroSpecification<DiaryType>
    {
        public DiaryTypeFilterSpecification(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => p.Name.Contains(searchString) || p.Description.Contains(searchString);
            }
            else
            {
                Criteria = p => true;
            }
        }
    }
}
