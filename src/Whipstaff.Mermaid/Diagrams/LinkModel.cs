using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whipstaff.Mermaid.Diagrams
{
    public sealed record LinkModel
    {
        public LinkModel Invisble(string? text = null) => new();

        public LinkModel StandardLine(string? text = null) => new();

        public LinkModel DottedLine(string? text = null) => new();

        public LinkModel ThickLine(string? text = null) => new();
    }
}
