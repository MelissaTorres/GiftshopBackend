using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using SS.GiftShop.Core;

namespace SS.GiftShop.Infrastructure
{
    public sealed class ExcelReader : IExcelReader
    {
        public Table Read(Stream stream)
        {
            Table table = null;
            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Load(stream);
                var workbook = excelPackage.Workbook;

                foreach (var worksheet in workbook.Worksheets)
                {
                    var dimension = worksheet.Dimension;

                    for (int row = dimension.Start.Row; row <= dimension.End.Row; row++)
                    {
                        var rowCells = worksheet.Cells[row, dimension.Start.Column, row, dimension.End.Column];

                        if (table == null)
                        {
                            table = new Table(rowCells.Select(x => Convert.ToString(x.Value)));
                            if (table.Columns.Count == 0)
                            {
                                throw new ExcelReaderException("No header columns found.");
                            }

                            continue;
                        }

                        var values = new object[table.Columns.Count];
                        for (int i = 0; i < values.Length; i++)
                        {
                            values[i] = rowCells[row, dimension.Start.Column + i].Value;
                        }
                        table.AddRow(values);
                    }
                }
            }

            return table ?? new Table(Enumerable.Empty<string>());
        }
    }
}
