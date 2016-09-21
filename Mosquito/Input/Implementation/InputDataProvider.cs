using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Input.Constants;
using Input.Decorators;
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

            using (var workBook = new ExcelWorkbook(path))
            {
                var systemsSheet = (Excel.Worksheet) workBook.Worksheets.Item[(int) SheetNumber.Systems];
                var systems = ParseSystems(systemsSheet);

                var profilesSheet = (Excel.Worksheet) workBook.Worksheets.Item[(int) SheetNumber.Profiles];
                var profiles = ParseWorkSheet<ProfileIm>(profilesSheet);

                var crossProfilesSheet = (Excel.Worksheet) workBook.Worksheets.Item[(int) SheetNumber.CrossProfiles];
                var crossProfiles = ParseCrossProfiles(crossProfilesSheet);

                var cordsSheet = (Excel.Worksheet) workBook.Worksheets.Item[(int) SheetNumber.Cords];
                var cords = ParseWorkSheet<CordIm>(cordsSheet);

                var netsSheet = (Excel.Worksheet) workBook.Worksheets.Item[(int) SheetNumber.Nets];
                var nets = ParseNets(netsSheet);

                var angelsSheet = (Excel.Worksheet) workBook.Worksheets.Item[(int) SheetNumber.Angels];
                var angels = ParseAngles(angelsSheet);

                var mountsSheet = (Excel.Worksheet) workBook.Worksheets.Item[(int) SheetNumber.Mounts];
                var mounts = ParseMounts(mountsSheet);

                var crossMountsSheet = (Excel.Worksheet) workBook.Worksheets.Item[(int) SheetNumber.CrossMounts];
                var crossMounts = ParseCrossMounts(crossMountsSheet);

                var knobsSheet = (Excel.Worksheet) workBook.Worksheets.Item[(int) SheetNumber.Knobs];
                var knobs = ParseKnobs(knobsSheet);

                var extraDetailsSheet = (Excel.Worksheet) workBook.Worksheets.Item[(int) SheetNumber.ExtraDetails];
                var extraDetails = ParseWorkSheet<ExtraDetailIm>(extraDetailsSheet);

                var packageDetailsSheet =
                    (Excel.Worksheet) workBook.Worksheets.Item[(int) SheetNumber.PackageDetails];
                var packageDetails = ParsePackageDetails(packageDetailsSheet);

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
                    Knobs = knobs,
                    ExtraDetails = extraDetails,
                    PackageDetails = packageDetails,
                    Settings = settings
                };
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

        public List<PackageDetailIm> ParsePackageDetails(Excel.Worksheet worksheet)
        {
            var packageDetails = new List<PackageDetailIm>();
            if (worksheet == null)
            {
                return packageDetails;
            }

            var range = worksheet.UsedRange;

            if (range == null)
            {
                return packageDetails;
            }

            var rowCount = range.Rows.Count;

            for (var rowIndex = (int)RowName.StartData; rowIndex <= rowCount; rowIndex++)
            {
                var id = (worksheet.Cells[rowIndex, (int)ColumnName.Id] as Excel.Range).Value;
                var name = (worksheet.Cells[rowIndex, (int)ColumnName.Name] as Excel.Range).Value;
                var price = (worksheet.Cells[rowIndex, (int)ColumnName.PricePerCount] as Excel.Range).Value;

                if (id == null || name == null || price == null)
                {
                    continue;
                }

                packageDetails.Add(new PackageDetailIm
                {
                    Name = name.ToString(),
                    PricePerCount = decimal.Parse(price.ToString()),
                    Id = int.Parse(id.ToString())
                });
            }

            return packageDetails;
        }

        public List<NetIm> ParseNets(Excel.Worksheet worksheet)
        {
            var nets = new List<NetIm>();
            if (worksheet == null)
            {
                return nets;
            }

            var range = worksheet.UsedRange;
            
            if (range == null)
            {
                return nets;
            }

            var rowCount = range.Rows.Count;

            for (var rowIndex = (int)RowName.StartData; rowIndex <= rowCount; rowIndex++)
            {
                var id = (worksheet.Cells[rowIndex, (int)ColumnName.Id] as Excel.Range).Value;
                var name = (worksheet.Cells[rowIndex, (int)ColumnName.Name] as Excel.Range).Value;
                var price = (worksheet.Cells[rowIndex, (int)ColumnName.PricePerCount] as Excel.Range).Value;
                var systems = (worksheet.Cells[rowIndex, (int)ColumnName.Systems] as Excel.Range).Value;
                var size = (worksheet.Cells[rowIndex, (int)ColumnName.Size] as Excel.Range).Value;

                if (id == null || name == null || price == null)
                {
                    continue;
                }

                nets.Add(new NetIm
                {
                    Name = name.ToString(), 
                    PricePerCount = decimal.Parse(price.ToString()), 
                    Id = int.Parse(id.ToString()),
                    Systems = systems == null ? new List<int>() : ((string)systems.ToString()).Split(',').Select(int.Parse).ToList(),
                    Size = int.Parse(size.ToString()),
                });
            }

            return nets;
        }

        public List<CrossProfileIm> ParseCrossProfiles(Excel.Worksheet worksheet)
        {
            var crossProfiles = new List<CrossProfileIm>();
            if (worksheet == null)
            {
                return crossProfiles;
            }

            var range = worksheet.UsedRange;

            if (range == null)
            {
                return crossProfiles;
            }

            var rowCount = range.Rows.Count;

            for (var rowIndex = (int)RowName.StartData; rowIndex <= rowCount; rowIndex++)
            {
                var id = (worksheet.Cells[rowIndex, (int)ColumnName.Id] as Excel.Range).Value;
                var name = (worksheet.Cells[rowIndex, (int)ColumnName.Name] as Excel.Range).Value;
                var price = (worksheet.Cells[rowIndex, (int)ColumnName.PricePerCount] as Excel.Range).Value;
                var systems = (worksheet.Cells[rowIndex, (int)ColumnName.Systems] as Excel.Range).Value;
                var joint = (worksheet.Cells[rowIndex, (int)ColumnName.Joint] as Excel.Range).Value;

                if (id == null || name == null || price == null)
                {
                    continue;
                }

                crossProfiles.Add(new CrossProfileIm
                {
                    Name = name.ToString(),
                    PricePerCount = decimal.Parse(price.ToString()),
                    Id = int.Parse(id.ToString()),
                    Systems = systems == null ? new List<int>() : ((string)systems.ToString()).Split(',').Select(int.Parse).ToList(),
                    JointExists = joint != null
                });
            }

            return crossProfiles;
        }

        public List<CrossMountIm> ParseCrossMounts(Excel.Worksheet worksheet)
        {
            var crossMounts = new List<CrossMountIm>();
            if (worksheet == null)
            {
                return crossMounts;
            }

            var range = worksheet.UsedRange;

            if (range == null)
            {
                return crossMounts;
            }

            var rowCount = range.Rows.Count;

            for (var rowIndex = (int)RowName.StartData; rowIndex <= rowCount; rowIndex++)
            {
                var id = (worksheet.Cells[rowIndex, (int)ColumnName.Id] as Excel.Range).Value;
                var name = (worksheet.Cells[rowIndex, (int)ColumnName.Name] as Excel.Range).Value;
                var price = (worksheet.Cells[rowIndex, (int)ColumnName.PricePerCount] as Excel.Range).Value;
                var systems = (worksheet.Cells[rowIndex, (int)ColumnName.Systems] as Excel.Range).Value;
                var joint = (worksheet.Cells[rowIndex, (int)ColumnName.Joint] as Excel.Range).Value;
                var count = (worksheet.Cells[rowIndex, (int)ColumnName.Count] as Excel.Range).Value;

                if (id == null || name == null || price == null)
                {
                    continue;
                }

                crossMounts.Add(new CrossMountIm
                {
                    Name = name.ToString(),
                    PricePerCount = decimal.Parse(price.ToString()),
                    Id = int.Parse(id.ToString()),
                    Systems = systems == null ? new List<int>() : ((string)systems.ToString()).Split(',').Select(int.Parse).ToList(),
                    Count = int.Parse(count.ToString()),
                    JointExists = joint != null
                });
            }

            return crossMounts;
        }

        public List<KnobIm> ParseKnobs(Excel.Worksheet worksheet)
        {
            var knobs = new List<KnobIm>();
            if (worksheet == null)
            {
                return knobs;
            }

            var range = worksheet.UsedRange;

            if (range == null)
            {
                return knobs;
            }

            var rowCount = range.Rows.Count;

            for (var rowIndex = (int)RowName.StartData; rowIndex <= rowCount; rowIndex++)
            {
                var id = (worksheet.Cells[rowIndex, (int)ColumnName.Id] as Excel.Range).Value;
                var name = (worksheet.Cells[rowIndex, (int)ColumnName.Name] as Excel.Range).Value;
                var price = (worksheet.Cells[rowIndex, (int)ColumnName.PricePerCount] as Excel.Range).Value;
                var systems = (worksheet.Cells[rowIndex, (int)ColumnName.Systems] as Excel.Range).Value;
                var count = (worksheet.Cells[rowIndex, (int)ColumnName.Count] as Excel.Range).Value;

                if (id == null || name == null || price == null)
                {
                    continue;
                }

                knobs.Add(new KnobIm
                {
                    Name = name.ToString(),
                    PricePerCount = decimal.Parse(price.ToString()),
                    Id = int.Parse(id.ToString()),
                    Systems = systems == null ? new List<int>() : ((string)systems.ToString()).Split(',').Select(int.Parse).ToList(),
                    Count = int.Parse(count.ToString()),
                });
            }

            return knobs;
        }

        public List<MountIm> ParseMounts(Excel.Worksheet worksheet)
        {
            var mounts = new List<MountIm>();
            if (worksheet == null)
            {
                return mounts;
            }

            var range = worksheet.UsedRange;

            if (range == null)
            {
                return mounts;
            }

            var rowCount = range.Rows.Count;

            for (var rowIndex = (int)RowName.StartData; rowIndex <= rowCount; rowIndex++)
            {
                var id = (worksheet.Cells[rowIndex, (int)ColumnName.Id] as Excel.Range).Value;
                var name = (worksheet.Cells[rowIndex, (int)ColumnName.Name] as Excel.Range).Value;
                var price = (worksheet.Cells[rowIndex, (int)ColumnName.PricePerCount] as Excel.Range).Value;
                var systems = (worksheet.Cells[rowIndex, (int)ColumnName.Systems] as Excel.Range).Value;
                var count = (worksheet.Cells[rowIndex, (int)ColumnName.Count] as Excel.Range).Value;
                var clincherCount = (worksheet.Cells[rowIndex, (int)ColumnName.ClincherCount] as Excel.Range).Value;
                var felt = (worksheet.Cells[rowIndex, (int)ColumnName.Felt] as Excel.Range).Value;

                if (id == null || name == null || price == null)
                {
                    continue;
                }

                mounts.Add(new MountIm
                {
                    Name = name.ToString(),
                    PricePerCount = decimal.Parse(price.ToString()),
                    Id = int.Parse(id.ToString()),
                    Systems = systems == null ? new List<int>() : ((string)systems.ToString()).Split(',').Select(int.Parse).ToList(),
                    Count = int.Parse(count.ToString()),
                    ClincherCount = int.Parse(clincherCount.ToString()),
                    Felt = felt != null
                });
            }

            return mounts;
        }

        public List<AngleIm> ParseAngles(Excel.Worksheet worksheet)
        {
            var nets = new List<AngleIm>();
            if (worksheet == null)
            {
                return nets;
            }

            var range = worksheet.UsedRange;

            if (range == null)
            {
                return nets;
            }

            var rowCount = range.Rows.Count;

            for (var rowIndex = (int)RowName.StartData; rowIndex <= rowCount; rowIndex++)
            {
                var id = (worksheet.Cells[rowIndex, (int)ColumnName.Id] as Excel.Range).Value;
                var name = (worksheet.Cells[rowIndex, (int)ColumnName.Name] as Excel.Range).Value;
                var price = (worksheet.Cells[rowIndex, (int)ColumnName.PricePerCount] as Excel.Range).Value;
                var systems = (worksheet.Cells[rowIndex, (int)ColumnName.Systems] as Excel.Range).Value;
                var count = (worksheet.Cells[rowIndex, (int)ColumnName.Count] as Excel.Range).Value;
                var clincherCount = (worksheet.Cells[rowIndex, (int) ColumnName.ClincherCount] as Excel.Range).Value;
                var inner = (worksheet.Cells[rowIndex, (int)ColumnName.Inner] as Excel.Range).Value;

                if (id == null || name == null || price == null)
                {
                    continue;
                }

                nets.Add(new AngleIm
                {
                    Name = name.ToString(),
                    PricePerCount = decimal.Parse(price.ToString()),
                    Id = int.Parse(id.ToString()),
                    Systems = systems == null ? new List<int>() : ((string)systems.ToString()).Split(',').Select(int.Parse).ToList(),
                    Count = int.Parse(count.ToString()),
                    ClincherCount = int.Parse(clincherCount.ToString()),
                    Inner = inner != null
                });
            }

            return nets;
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
            settings.GluePrice = decimal.Parse((worksheet.Cells[(int)RowName.GluePrice, (int)ColumnName.Value] as Excel.Range).Value.ToString());
            settings.AmountNetsOnTheOneGlue = decimal.Parse((worksheet.Cells[(int)RowName.AmountNetsOnTheOneGlue, (int)ColumnName.Value] as Excel.Range).Value.ToString());

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
