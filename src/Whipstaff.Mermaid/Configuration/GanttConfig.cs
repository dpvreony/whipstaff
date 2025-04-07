using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whipstaff.Mermaid.Configuration
{
    public sealed class GanttConfig
    {
        public string AxisFormat { get; set; } // e.g. '%Y-%m-%d'
        public int? BarHeight { get; set; }
        public int? FontSize { get; set; }
        public int? SectionFontSize { get; set; }
    }
}
