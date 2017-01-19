using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Dijkstra
{
    class CustomLabel : Label
    {
        public string LabelName { get; private set; }
        public CustomLabel(string labelName)
        {
            LabelName = labelName;
        }
    }
}
