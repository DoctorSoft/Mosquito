using System.Collections.Generic;
using System.IO;
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
            var profiles = ParseWorkSheet<ProfileIm>(profilesSheet);

            var crossProfilesSheet = (Excel.Worksheet)workBook.Worksheets.Item[(int)SheetNumber.CrossProfiles];
            var crossProfiles = ParseWorkSheet<CrossProfileIm>(crossProfilesSheet);

            var cordsSheet = (Excel.Worksheet)workBook.Worksheets.Item[(int)SheetNumber.Cords];
            var cords = ParseWorkSheet<CordIm>(cordsSheet);

            var netsSheet = (Excel.Worksheet)workBook.Worksheets.Item[(int)SheetNumber.Nets];
            var nets = ParseWorkSheet<NetIm>(netsSheet);

            var extraDetailsSheet = (Excel.Worksheet)workBook.Worksheets.Item[(int)SheetNumber.ExtraDetails];
            var extraDetails = ParseWorkSheet<ExtraDetailIm>(extraDetailsSheet);

            var settingsSheet = (Excel.Worksheet)workBook.Worksheets.Item[(int)SheetNumber.Settings];
            var settings = ParseSettings(settingsSheet);

            return new InputData
            {
                ExtraDetails = extraDetails,
                CrossProfiles = crossProfiles,
                Cords = cords,
                Nets = nets,
                Profiles = profiles,
                Settings = settings
            };
        }

        public List<TProduct> ParseWorkSheet<TProduct>(Excel.Worksheet worksheet)
            where TProduct : ProductIm, new() 
        {
            var products = new List<TProduct>();
            if (worksheet == null)
            {
                return products;
            }

            var range = worksheet.UsedRange;
            if (range == null) return products;
            var rowCount = range.Rows.Count;

            for (var rowIndex = (int)RowName.StartData; rowIndex <= rowCount; rowIndex++)
            {
                var name = (worksheet.Cells[rowIndex, (int)ColumnName.Name] as Excel.Range).Value;
                var price = (worksheet.Cells[rowIndex, (int)ColumnName.PricePerCount] as Excel.Range).Value;

                if (name == null || price == null)
                {
                    continue;
                }

                products.Add(new TProduct{ Name = name.ToString(), PricePerCount = decimal.Parse(price.ToString()) });
            }

            return products;
        }

        public SettingsIm ParseSettings(Excel.Worksheet worksheet)
        {
            var settings = new SettingsIm();

            if (worksheet == null)
            {
                return settings;
            }

            settings.CrossProfileTolerance = decimal.Parse((worksheet.Cells[(int)RowName.CrossProfileTolerance, (int)ColumnName.Value] as Excel.Range).Value.ToString());
            settings.OtherSpendingPrice = decimal.Parse((worksheet.Cells[(int)RowName.OtherSpending, (int)ColumnName.Value] as Excel.Range).Value.ToString());
            settings.ProfileTolerance = decimal.Parse((worksheet.Cells[(int)RowName.ProfileTolerance, (int)ColumnName.Value] as Excel.Range).Value.ToString());
            settings.TrashPercent = decimal.Parse((worksheet.Cells[(int)RowName.TrashPercent, (int)ColumnName.Value] as Excel.Range).Value.ToString());
            settings.WorkPrice = decimal.Parse((worksheet.Cells[(int)RowName.WorkPrice, (int)ColumnName.Value] as Excel.Range).Value.ToString());

            return settings;
        }
    }
}
