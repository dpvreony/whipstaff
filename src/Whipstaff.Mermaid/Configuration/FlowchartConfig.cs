using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whipstaff.Mermaid.Configuration
{
    public sealed class FlowchartConfig
    {
        public bool? UseMaxWidth { get; set; }
        public string HtmlLabels { get; set; }
        public string Curve { get; set; } // 'linear', 'basis', etc.
        public int? Padding { get; set; }
        public int? DiagramPadding { get; set; }
    }
}
