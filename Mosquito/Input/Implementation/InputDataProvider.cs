using System.Collections.Generic;
using System.IO;
using System.Linq;
using Input.Constants;
using Input.InputModels;
using Input.Interfaces;
using Excel = Microsoft.Office.Interop.Excel;

namespace Input.Implementation
{
    public class InputDataProvider : IInputDataProvider
    {
        private readonly string fileName;

        public InputDataProvider(string fileName)
        {
            this.fileName = fileName;
        }

        public InputData GetInputData()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var path = Path.Combine(currentDirectory, fileName);

            var excel = new Excel.Application();
            var workBook = excel.Workbooks.Open(path);

            var profilesSheet = (Excel.Worksheet)workBook.Worksheets.Item[(int)SheetNumber.Profiles];

            var profiles = ParseWorkSheet(profilesSheet).Select(im => new ProductIm
            {
                Name = im.Name,
                PricePerCount = im.PricePerCount
            }).ToList();

            return null;
        }

        public List<ProductIm> ParseWorkSheet(Excel.Worksheet worksheet)
        {
            if (worksheet != null)
            {
                var range = worksheet.UsedRange;
                if (range != null)
                {
                    var nRows = range.Rows.Count;
                    var nCols = range.Columns.Count;
                    foreach (Excel.Range row in range.Rows)
                    {
                        string value = row.Cells[1].FormattedValue as string;
                    }
                }
            }

            return null;
        } 
    }
}
