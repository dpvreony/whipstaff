using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Dhgms.AspNetCoreContrib.UnitTests.Features.EntityFramework
{
    /// <summary>
    /// Initial test logic for generating DGML
    /// </summary>
    public class DgmlGeneration
    {
        /// <summary>
        /// Generates a DGML diagram.
        /// </summary>
        /// <remarks>
        /// Taken from https://github.com/ErikEJ/EFCorePowerTools/wiki/Inspect-your-DbContext-model
        /// made re-usable to run in build and tests.
        /// </remarks>
        public void GeneratesDgml<T>()
            where T : DbContext, new()
        {
            using (var myContext = new T())
            {
                var path = Path.GetTempFileName() + ".dgml";
                File.WriteAllText(path, myContext.AsDgml(), Encoding.UTF8);
                Process.Start(path);
            }
        }
    }
}
