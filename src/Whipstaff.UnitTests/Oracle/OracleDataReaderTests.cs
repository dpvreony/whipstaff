// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

#if NET48
using System;
using System.Data;
using NetTestRegimentation;
using Oracle.ManagedDataAccess.Client;
using Whipstaff.Oracle;
using Xunit;

namespace Dhgms.AspNetCoreContrib.UnitTests.Oracle
{
    /// <summary>
    /// Unit Tests for Oracle Data Reader Helpers.
    /// </summary>
    public static class OracleDataReaderHelperTests
    {
        /// <summary>
        /// Unit Tests for the Consume Data Reader Method.
        /// </summary>
        public sealed class ConsumeDataReaderMethod : ITestMethodWithNullableParameters<OracleConnection, IDataReader>
        {
            /// <summary>
            /// Test Source for ThrowsArgumentNullException Unit Test.
            /// </summary>
            public static TheoryData<OracleConnection, IDataReader> ThrowsArgumentNullExceptionTestSource =
                GetThrowsArgumentNullExceptionTestSource();

            [Theory]
            [MemberData(nameof(ThrowsArgumentNullExceptionTestSource))]
            public void ThrowsArgumentNullException(
                OracleConnection arg1,
                IDataReader arg2,
                string expectedParameterNameForException)
            {
                var exception = Assert.Throws<ArgumentNullException>(() => OracleDataReaderHelper.ConsumeDataReader(arg1, arg2));

                Assert.Equal(
                    expectedParameterNameForException,
                    exception.ParamName);
            }

            private static TheoryData<OracleConnection, IDataReader> GetThrowsArgumentNullExceptionTestSource()
            {
                throw new NotImplementedException();
            }
        }
    }
}
#endif
