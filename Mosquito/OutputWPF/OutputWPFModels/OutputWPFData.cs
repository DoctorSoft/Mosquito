using System.Collections.Generic;
using Input.InputModels;

namespace OutputWPF.OutputWPFModels
{
    public class OutputWpfData
    {
        public decimal Width { get; set; }

        public decimal Height { get; set; }

        public List<ProfileIm> Profiles { get; set; }

        public CurrentProfile CurrentProfile { get; set; }

        public List<CrossProfileIm> CrossProfiles { get; set; }

        public CurrentCrossProfile CurrentCrossProfile { get; set; }

        public List<NetIm> Nets { get; set; }

        public CurrentNet CurrentNet { get; set; }

        public List<CordIm> Cords { get; set; }

        public CurrentCord CurrentCord { get; set; }

        public List<ExtraDetailIm> ExtraDetails { get; set; }

        public List<CurrentExtraDetail> CurrentExtraDetails { get; set; }

        public decimal WorkPrice { get; set; }

        public decimal TrashPrice { get; set; }

        public decimal OtherSpendingPrice { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
