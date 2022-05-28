// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace Whipstaff.Oracle
{
    /// <summary>
    /// Oracle Data Reader helpers.
    /// </summary>
    public static class OracleDataReaderHelper
    {
        /// <summary>
        /// Bulk Copy a Data Reader to an Oracle Database.
        /// </summary>
        /// <param name="oracleConnection">Oracle Database Connection.</param>
        /// <param name="dataReader">Data Reader to consumer.</param>
        public static void ConsumeDataReader(
            OracleConnection oracleConnection,
            IDataReader dataReader)
        {
            using (var bulkCopy = new OracleBulkCopy(oracleConnection))
            {
                bulkCopy.WriteToServer(dataReader);
            }
        }
    }
}
