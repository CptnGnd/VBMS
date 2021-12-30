using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBMS.Domain.Enums
{
    public enum AttributeComparer
    {
        Equals,
        NotEquals,
        Contains,
        GreaterThan,
        LessThan,
        GreaterThanOrEqual, 
        LessThanOrEqual,
        Exist
    }
}
