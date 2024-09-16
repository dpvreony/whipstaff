// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Whipstaff.Runtime.JobSequencing
{
    /// <summary>
    /// Represents a job that runs with a start and end index.
    /// </summary>
    /// <typeparam name="T">The type for the sequence.</typeparam>
    public sealed class JobSequenceByRange<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JobSequenceByRange{T}"/> class.
        /// </summary>
        /// <param name="start">The start point for the job.</param>
        /// <param name="end">The end point for the job.</param>
        public JobSequenceByRange(T start, T end)
        {
            Start = start;
            End = end;
        }

        /// <summary>
        /// Gets the start point for the job.
        /// </summary>
        public T Start
        {
            get;
        }

        /// <summary>
        /// Gets the end point for the job.
        /// </summary>
        public T End
        {
            get;
        }

        /// <summary>
        /// Gets the a job sequence with a byte data type.
        /// </summary>
        /// <param name="start">The start point for the job.</param>
        /// <param name="end">The end point for the job.</param>
        /// <returns>Job Sequence.</returns>
        public static JobSequenceByRange<byte> GetJobSequenceByRange(byte start, byte end)
        {
#pragma warning disable CA1512
            if (end < start)
            {
                throw new ArgumentOutOfRangeException(nameof(end));
            }
#pragma warning restore CA1512

            return new JobSequenceByRange<byte>(start, end);
        }

        /// <summary>
        /// Gets the a job sequence with a short data type.
        /// </summary>
        /// <param name="start">The start point for the job.</param>
        /// <param name="end">The end point for the job.</param>
        /// <returns>Job Sequence.</returns>
        public static JobSequenceByRange<short> GetJobSequenceByRange(short start, short end)
        {
#pragma warning disable CA1512
            if (start < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(start));
            }

            if (end < start)
            {
                throw new ArgumentOutOfRangeException(nameof(end));
            }
#pragma warning restore CA1512

            return new JobSequenceByRange<short>(start, end);
        }

        /// <summary>
        /// Gets the a job sequence with a int data type.
        /// </summary>
        /// <param name="start">The start point for the job.</param>
        /// <param name="end">The end point for the job.</param>
        /// <returns>Job Sequence.</returns>
        public static JobSequenceByRange<int> GetJobSequenceByRange(int start, int end)
        {
#pragma warning disable CA1512
            if (start < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(start));
            }

            if (end < start)
            {
                throw new ArgumentOutOfRangeException(nameof(end));
            }
#pragma warning restore CA1512

            return new JobSequenceByRange<int>(start, end);
        }

        /// <summary>
        /// Gets the a job sequence with a long data type.
        /// </summary>
        /// <param name="start">The start point for the job.</param>
        /// <param name="end">The end point for the job.</param>
        /// <returns>Job Sequence.</returns>
        public static JobSequenceByRange<long> GetJobSequenceByRange(long start, long end)
        {
#pragma warning disable CA1512
            if (start < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(start));
            }

            if (end < start)
            {
                throw new ArgumentOutOfRangeException(nameof(end));
            }
#pragma warning restore CA1512

            return new JobSequenceByRange<long>(start, end);
        }
    }
}
