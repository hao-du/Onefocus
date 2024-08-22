using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onefocus.Common.Utilities.Attributes
{
    public class GroupAttribute<T> : Attribute
    {
        public T Key { get; private set; }

        public GroupAttribute(T key)
        {
            Key = key;
        }
    }
}
