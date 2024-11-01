using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onefocus.Common.Dtos
{
    public class PagingDto
    {
        int Size { get; }
        int Index { get; }
        int Take { get; }
    }
}
