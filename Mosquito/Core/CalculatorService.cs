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
            throw new System.NotImplementedException();
        }

        public OutputWpfData ChangeWidth(decimal width, OutputWpfData oldData)
        {
            throw new System.NotImplementedException();
        }

        public OutputWpfData ChangeHeight(decimal width, OutputWpfData oldData)
        {
            throw new System.NotImplementedException();
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
