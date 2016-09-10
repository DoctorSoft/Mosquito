using Input.InputModels;
using Input.Interfaces;
using OutputWPF.OutputWPFModels;

namespace Core
{
    public class CalculatorService : ICalculatorService
    {
        private readonly IInputDataProvider inputDataProvider;

        public CalculatorService(IInputDataProvider inputDataProvider)
        {
            this.inputDataProvider = inputDataProvider;
        }

        public OutputWpfData GetDefault()
        {
            var inputData = inputDataProvider.GetInputData();

            return new OutputWpfData
            {
                Profiles = inputData.Profiles,
                CrossProfiles = inputData.CrossProfiles,
                Nets = inputData.Nets,
                Cords = inputData.Cords,
                ExtraDetails = inputData.ExtraDetails,
                WorkPrice = inputData.Settings.WorkPrice,
                TrashPrice = inputData.Settings.TrashPercent,
                OtherSpendingPrice = inputData.Settings.OtherSpendingPrice,
            };
        }

        public OutputWpfData ChangeWidth(decimal width, OutputWpfData oldData)
        {
            oldData.Width = width;
            return oldData;
        }

        public OutputWpfData ChangeHeight(decimal heigth, OutputWpfData oldData)
        {
            oldData.Height = heigth;
            return oldData;
        }

        public OutputWpfData ChangeNet(NetIm net, OutputWpfData oldData)
        {
            throw new System.NotImplementedException();
        }

        public OutputWpfData AddExtraDetail(ExtraDetailIm extraDetail, int count, OutputWpfData oldData)
        {
            throw new System.NotImplementedException();
        }

        public OutputWpfData RemoveExtraDetail(ExtraDetailIm extraDetail, OutputWpfData oldData)
        {
            throw new System.NotImplementedException();
        }

        private OutputWpfData Calculate(OutputWpfData notPricedOutputData)
        {
            throw new System.NotImplementedException();
        }
    }
}
