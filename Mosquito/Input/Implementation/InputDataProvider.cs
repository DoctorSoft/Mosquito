using System;
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
            Excel.Application excel;
            Excel.Workbook workBook;

            var currentDirectory = Directory.GetCurrentDirectory();
            var path = Path.Combine(currentDirectory, fileName);

            excel = new Excel.Application();
            workBook = excel.Workbooks.Open(path);

            try
            {
                var systemsSheet = (Excel.Worksheet)workBook.Worksheets.Item[(int)SheetNumber.Systems];
                var systems = ParseSystems(systemsSheet);

                var profilesSheet = (Excel.Worksheet) workBook.Worksheets.Item[(int) SheetNumber.Profiles];
                var profiles = ParseWorkSheet<ProfileIm>(profilesSheet);

                var crossProfilesSheet = (Excel.Worksheet) workBook.Worksheets.Item[(int) SheetNumber.CrossProfiles];
                var crossProfiles = ParseWorkSheet<CrossProfileIm>(crossProfilesSheet);

                var cordsSheet = (Excel.Worksheet) workBook.Worksheets.Item[(int) SheetNumber.Cords];
                var cords = ParseWorkSheet<CordIm>(cordsSheet);

                var netsSheet = (Excel.Worksheet) workBook.Worksheets.Item[(int) SheetNumber.Nets];
                var nets = ParseWorkSheet<NetIm>(netsSheet);

                var angelsSheet = (Excel.Worksheet)workBook.Worksheets.Item[(int)SheetNumber.Angels];
                var angels = ParseWorkSheet<AngleIm>(angelsSheet);
                
                var mountsSheet = (Excel.Worksheet)workBook.Worksheets.Item[(int)SheetNumber.Mounts];
                var mounts = ParseWorkSheet<MountIm>(mountsSheet);

                var crossMountsSheet = (Excel.Worksheet)workBook.Worksheets.Item[(int)SheetNumber.CrossMounts];
                var crossMounts = ParseWorkSheet<CrossMountIm>(crossMountsSheet);

                var extraDetailsSheet = (Excel.Worksheet) workBook.Worksheets.Item[(int) SheetNumber.ExtraDetails];
                var extraDetails = ParseWorkSheet<ExtraDetailIm>(extraDetailsSheet);

                var settingsSheet = (Excel.Worksheet) workBook.Worksheets.Item[(int) SheetNumber.Settings];
                var settings = ParseSettings(settingsSheet);

                return new InputData
                {
                    Systems = systems,
                    Profiles = profiles,
                    CrossProfiles = crossProfiles,
                    Nets = nets,
                    Cords = cords,
                    Angles = angels,
                    Mounts = mounts,
                    CrossMounts = crossMounts,
                    ExtraDetails = extraDetails,
                    Settings = settings
                };
            }
            finally
            {
                workBook.Close(false);
            }
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
            
            if (range == null)
            {
                return products;
            }

            var rowCount = range.Rows.Count;

            for (var rowIndex = (int)RowName.StartData; rowIndex <= rowCount; rowIndex++)
            {
                var id = (worksheet.Cells[rowIndex, (int)ColumnName.Id] as Excel.Range).Value;
                var name = (worksheet.Cells[rowIndex, (int)ColumnName.Name] as Excel.Range).Value;
                var price = (worksheet.Cells[rowIndex, (int)ColumnName.PricePerCount] as Excel.Range).Value;
                var systems = (worksheet.Cells[rowIndex, (int)ColumnName.Systems] as Excel.Range).Value;

                if (id == null || name == null || price == null)
                {
                    continue;
                }

                products.Add(new TProduct
                {
                    Name = name.ToString(), 
                    PricePerCount = decimal.Parse(price.ToString()), 
                    Id = int.Parse(id.ToString()),
                    Systems = systems == null ? new List<int>() : ((string)systems.ToString()).Split(',').Select(int.Parse).ToList() 
                });
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

        public List<SystemIm> ParseSystems(Excel.Worksheet worksheet)
        {
            var systems = new List<SystemIm>();
            if (worksheet == null)
            {
                return systems;
            }

            var range = worksheet.UsedRange;

            if (range == null)
            {
                return systems;
            }

            var rowCount = range.Rows.Count;

            for (var rowIndex = (int)RowName.StartData; rowIndex <= rowCount; rowIndex++)
            {
                var id = (worksheet.Cells[rowIndex, (int)ColumnName.Id] as Excel.Range).Value;
                var name = (worksheet.Cells[rowIndex, (int)ColumnName.Name] as Excel.Range).Value;

                if (id == null || name == null)
                {
                    continue;
                }

                systems.Add(new SystemIm { Name = name.ToString(), Id = int.Parse(id.ToString()) });
            }

            return systems;
        }
    }
}
