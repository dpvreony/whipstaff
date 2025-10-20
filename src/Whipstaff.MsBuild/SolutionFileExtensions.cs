// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Build.Construction;

namespace Whipstaff.MsBuild
{
    /// <summary>
    /// Extension methods for <see cref="SolutionFile"/>.
    /// </summary>
    public static class SolutionFileExtensions
    {
        /// <summary>
        /// Gets known MSBuild projects from the solution.
        /// </summary>
        /// <param name="solutionFile">The solution to pull projects from.</param>
        /// <returns>Enumerable of MSBuild projects.</returns>
        public static IEnumerable<ProjectInSolution> GetKnownMsBuildProjects(this SolutionFile solutionFile)
        {
            ArgumentNullException.ThrowIfNull(solutionFile);

            return solutionFile.ProjectsInOrder.Where(x => x.ProjectType == SolutionProjectType.KnownToBeMSBuildFormat);
        }
    }
}
