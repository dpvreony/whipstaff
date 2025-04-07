// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Whipstaff.Mermaid.Configuration
{
    /// <summary>
    /// Configuration options for Mermaid.
    /// </summary>
    public sealed class MermaidConfig
    {
        /// <summary>
        /// Gets or sets the theme.
        /// </summary>
        public string Theme { get; set; } = "default"; // 'default', 'forest', 'dark', 'neutral'

        /// <summary>
        /// Gets or sets whether to start the diagram rendering on load.
        /// </summary>
        public bool? StartOnLoad { get; set; }

        /// <summary>
        /// Gets or sets the security level for rendering.
        /// </summary>
        public string SecurityLevel { get; set; } = "strict"; // 'strict', 'loose', 'antiscript', 'sandbox'

        /// <summary>
        /// Gets or sets whether to use HTML labels.
        /// </summary>
        public bool? HtmlLabels { get; set; }

        /// <summary>
        /// Gets or sets whether to make arrow markers absolute.
        /// </summary>
        public bool? ArrowMarkerAbsolute { get; set; }

        /// <summary>
        /// Gets or sets the log level.
        /// </summary>
        public int? LogLevel { get; set; } // 0 = debug, 1 = info, 2 = warn, 3 = error, 4 = fatal

        /// <summary>
        /// Gets or sets the theme variables.
        /// </summary>
        public ThemeVariables? ThemeVariables { get; set; }

        /// <summary>
        /// Gets or sets the configuration for flowchart diagrams.
        /// </summary>
        public FlowchartConfig? Flowchart { get; set; }

        /// <summary>
        /// Gets or sets the configuration for sequence diagrams.
        /// </summary>
        public SequenceConfig? Sequence { get; set; }

        /// <summary>
        /// Gets or sets the configuration for gantt chart diagrams.
        /// </summary>
        public GanttConfig? Gantt { get; set; }

        /// <summary>
        /// Gets or sets the configuration for class diagrams.
        /// </summary>
        public ClassConfig? Class { get; set; }

        /// <summary>
        /// Gets or sets the configuration for journey diagrams.
        /// </summary>
        public JourneyConfig? Journey { get; set; }
    }
}
