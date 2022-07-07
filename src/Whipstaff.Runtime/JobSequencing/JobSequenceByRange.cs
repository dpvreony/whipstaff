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
        private readonly T _start;
        private readonly T _end;

        /// <summary>
        /// Initializes a new instance of the <see cref="JobSequenceByRange{T}"/> class.
        /// </summary>
        /// <param name="start">The start point for the job.</param>
        /// <param name="end">The end point for the job.</param>
        public JobSequenceByRange(T start, T end)
        {
            _start = start;
            _end = end;
        }

        /// <summary>
        /// Gets the a job sequence with a byte data type.
        /// </summary>
        /// <param name="start">The start point for the job.</param>
        /// <param name="end">The end point for the job.</param>
        /// <returns>Job Sequence.</returns>
        public static JobSequenceByRange<byte> GetJobSequenceByRange(byte start, byte end)
        {
            if (end < start)
            {
                throw new ArgumentOutOfRangeException(nameof(end));
            }

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
            if (end < start)
            {
                throw new ArgumentOutOfRangeException(nameof(end));
            }

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
            if (start < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(start));
            }

            if (end < start)
            {
                throw new ArgumentOutOfRangeException(nameof(end));
            }

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
            if (start < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(start));
            }

            if (end < start)
            {
                throw new ArgumentOutOfRangeException(nameof(end));
            }

            return new JobSequenceByRange<long>(start, end);
        }
    }
}