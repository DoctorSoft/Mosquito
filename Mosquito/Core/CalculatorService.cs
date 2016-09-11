using System.Collections.Generic;
using System.Linq;
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
                CurrentProfile = new CurrentProfile
                {
                    Name = inputData.Profiles.FirstOrDefault().Name,  
                },
                CrossProfiles = inputData.CrossProfiles,
                CurrentCrossProfile = new CurrentCrossProfile
                {
                    Name = inputData.CrossProfiles.FirstOrDefault().Name
                },
                Nets = inputData.Nets,
                CurrentNet = new CurrentNet
                {
                    Name = inputData.Nets.FirstOrDefault().Name
                },
                Cords = inputData.Cords,
                CurrentCord = new CurrentCord
                {
                    Name = inputData.Cords.FirstOrDefault().Name
                },
                ExtraDetails = inputData.ExtraDetails,
                CurrentExtraDetails = new List<CurrentExtraDetail>(),
                WorkPrice = inputData.Settings.WorkPrice,
                OtherSpendingPrice = inputData.Settings.OtherSpendingPrice,
                Height = 600,
                Width = 1700
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

        public OutputWpfData ChangeCord(CordIm cord, OutputWpfData oldData)
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
