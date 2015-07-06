using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Vale.Parsing
{
    class AbilityInfo
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public string BaseAbility { get; set; }
        public AbilityParameterInfo[] Parameters { get; set; }
    }
}
