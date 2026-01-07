// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Whipstaff.OpenXml.Excel
{
    /// <summary>
    /// Extension methods for working with a open xml worksheet.
    /// </summary>
    public static class WorksheetPartExtensions
    {
        /// <summary>
        /// Inserts a cell into a worksheet with a specified value.
        /// </summary>
        /// <param name="worksheetPart">The worksheet part.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="rowIndex">The row index. 1 based.</param>
        /// <param name="value">The value to enter.</param>
        /// <returns>The created or updated cell.</returns>
        public static Cell InsertCellInWorksheet(
            this WorksheetPart worksheetPart,
            string columnName,
            uint rowIndex,
            string value)
        {
            return worksheetPart.InsertCellInWorksheet(
                columnName,
                rowIndex,
                value,
                CellValues.String);
        }

        /// <summary>
        /// Inserts a cell into a worksheet with a specified value.
        /// </summary>
        /// <param name="worksheetPart">The worksheet part.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="rowIndex">The row index. 1 based.</param>
        /// <param name="value">The value to enter.</param>
        /// <returns>The created or updated cell.</returns>
        public static Cell InsertCellInWorksheet(
            this WorksheetPart worksheetPart,
            string columnName,
            uint rowIndex,
            bool value)
        {
            return worksheetPart.InsertCellInWorksheet(
                columnName,
                rowIndex,
                value ? "1" : "0",
                CellValues.Boolean);
        }

        /// <summary>
        /// Inserts a cell into a worksheet with a specified value.
        /// </summary>
        /// <param name="worksheetPart">The worksheet part.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="rowIndex">The row index. 1 based.</param>
        /// <param name="value">The value to enter.</param>
        /// <returns>The created or updated cell.</returns>
        public static Cell InsertCellInWorksheet(
            this WorksheetPart worksheetPart,
            string columnName,
            uint rowIndex,
            int value)
        {
            return worksheetPart.InsertCellInWorksheet(
                columnName,
                rowIndex,
                value.ToString(NumberFormatInfo.InvariantInfo),
                CellValues.Number);
        }

        /// <summary>
        /// Inserts a cell into a worksheet with a specified value.
        /// </summary>
        /// <param name="worksheetPart">The worksheet part.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="rowIndex">The row index. 1 based.</param>
        /// <param name="value">The value to enter.</param>
        /// <returns>The created or updated cell.</returns>
        public static Cell InsertCellInWorksheet(
            this WorksheetPart worksheetPart,
            string columnName,
            uint rowIndex,
            DateTimeOffset value)
        {
            return worksheetPart.InsertCellInWorksheet(
                columnName,
                rowIndex,
                value.ToString(NumberFormatInfo.InvariantInfo),
                CellValues.Date);
        }

        private static Cell InsertCellInWorksheet(
            this WorksheetPart worksheetPart,
            string columnName,
            uint rowIndex,
            string value,
            CellValues cellValues)
        {
            var cell = worksheetPart.InsertCellInWorksheet(columnName, rowIndex);
            cell.CellValue = new CellValue(value);
            cell.DataType = new EnumValue<CellValues>(cellValues);

            return cell;
        }

        // Given a column name, a row index, and a WorksheetPart, inserts a cell into the worksheet.
        // If the cell already exists, returns it.
        private static Cell InsertCellInWorksheet(
            this WorksheetPart worksheetPart,
            string columnName,
            uint rowIndex)
        {
            var worksheet = worksheetPart.Worksheet;
            if (worksheet == null)
            {
                throw new InvalidOperationException("No worksheet");
            }

            var sheetData = worksheet.GetFirstChild<SheetData>();
            if (sheetData == null)
            {
                throw new InvalidOperationException("No sheet data in worksheet");
            }

            var cellReference = columnName + rowIndex;

            // If the worksheet does not contain a row with the specified row index, insert one.
            var row = sheetData.Elements<Row>().FirstOrDefault(r => r.RowIndex?.Value == rowIndex);
            if (row != null)
            {
                var cell = row.Elements<Cell>().FirstOrDefault(c => c.CellReference?.Value == cellReference);
                if (cell != null)
                {
                    return cell;
                }
            }
            else
            {
                row = new Row { RowIndex = rowIndex };

                _ = sheetData.AddChild(row);
            }

            Cell? refCell = null;
            foreach (var cell in row.Elements<Cell>())
            {
                if (cell.CellReference?.Value?.Length == cellReference.Length && string.Compare(cell.CellReference.Value, cellReference, StringComparison.Ordinal) > 0)
                {
                    refCell = cell;
                    break;
                }
            }

            var newCell = new Cell { CellReference = cellReference };
            _ = row.InsertBefore(newCell, refCell);

            worksheet.Save();
            return newCell;
        }
    }
}
