using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whipstaff.Mermaid.Configuration
{
    public sealed class SequenceConfig
    {
        public bool? ShowSequenceNumbers { get; set; }
        public bool? ActorMargin { get; set; }
        public bool? HideUnusedParticipants { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
    }
}
