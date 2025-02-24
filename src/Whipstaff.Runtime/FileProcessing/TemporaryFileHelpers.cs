// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;

#if ARGUMENT_NULL_EXCEPTION_SHIM
using ArgumentNullException = Whipstaff.Runtime.Exceptions.ArgumentNullException;
#endif

namespace Whipstaff.Runtime.FileProcessing
{
    /// <summary>
    /// Helpers for working with temporary files.
    /// </summary>
    public static class TemporaryFileHelpers
    {
        /// <summary>
        /// Carries out an action using a temporary file.
        /// </summary>
        /// <param name="fileAsBytes">The file as a byte array.</param>
        /// <param name="fileExtension">The file extension.</param>
        /// <param name="fileAction">The action to carry out referencing the file.</param>
        /// <param name="skipDeleteException">Whether to skip the exception from attempting to delete the temporary file.</param>
        public static void WithTempFile(
            byte[] fileAsBytes,
            string fileExtension,
            Action<string> fileAction,
            bool skipDeleteException)
        {
            ArgumentNullException.ThrowIfNull(fileAsBytes);
            ArgumentNullException.ThrowIfNull(fileExtension);
            ArgumentNullException.ThrowIfNull(fileAction);

            var tempFileName = GetTempFile(fileAsBytes, fileExtension);

            var exceptions = new List<Exception>();
            try
            {
                fileAction(tempFileName);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                exceptions.Add(e);
            }

            DeleteTempFile(
                skipDeleteException,
                tempFileName,
                exceptions);

            if (exceptions.Count > 0)
            {
                throw new AggregateException(exceptions);
            }
        }

        /// <summary>
        /// Carries out an action using a temporary file.
        /// </summary>
        /// <typeparam name="TResult">The type for the result of the action.</typeparam>
        /// <param name="fileAsBytes">The file as a byte array.</param>
        /// <param name="fileExtension">The file extension.</param>
        /// <param name="fileAction">The action to carry out referencing the file.</param>
        /// <param name="skipDeleteException">Whether to skip the exception from attempting to delete the temporary file.</param>
        /// <returns>Result from the file action.</returns>
        public static TResult? WithTempFile<TResult>(
            byte[] fileAsBytes,
            string fileExtension,
            Func<string, TResult> fileAction,
            bool skipDeleteException)
        {
            ArgumentNullException.ThrowIfNull(fileAsBytes);
            ArgumentNullException.ThrowIfNull(fileExtension);
            ArgumentNullException.ThrowIfNull(fileAction);

            var tempFileName = GetTempFile(fileAsBytes, fileExtension);

            var exceptions = new List<Exception>();
            var result = default(TResult);
            try
            {
                result = fileAction(tempFileName);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                exceptions.Add(e);
            }

            DeleteTempFile(
                skipDeleteException,
                tempFileName,
                exceptions);

            if (exceptions.Count > 0)
            {
                throw new AggregateException(exceptions);
            }

            return result;
        }

        private static void DeleteTempFile(bool skipDeleteException, string tempFileName, List<Exception> exceptions)
        {
            try
            {
                File.Delete(tempFileName);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                if (!skipDeleteException)
                {
                    exceptions.Add(e);
                }
            }
        }

        private static string GetTempFile(
            byte[] fileAsBytes,
            string fileExtension)
        {
            var tempFileName = Path.Combine(
                Path.GetTempPath(),
                $"{Path.GetRandomFileName()}{fileExtension}");

            using (var stream = File.Create(tempFileName))
            {
                var fileLength = fileAsBytes.Length;
                stream.Write(fileAsBytes, 0, fileLength);
                stream.Flush(true);
            }

            return tempFileName;
        }
    }
}
