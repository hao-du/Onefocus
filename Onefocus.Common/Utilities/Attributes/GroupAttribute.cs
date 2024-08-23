using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onefocus.Common.Utilities.Attributes
{
    public class GroupAttribute : Attribute
    {
        public Type Key { get; private set; }

        public GroupAttribute(Type key)
        {
            Key = key;
        }
    }
}
