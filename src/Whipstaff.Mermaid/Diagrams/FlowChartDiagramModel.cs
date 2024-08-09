using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whipstaff.Mermaid.Diagrams
{
    public sealed class FlowChartDiagramModel
    {
        public IList<int> Nodes { get; }

        public IList<LinkModel> Links { get; }
    }
}
