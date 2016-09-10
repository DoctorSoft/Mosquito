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

        public OutputWpfData ChangeNet(CurrentNet net, OutputWpfData oldData)
        {
            throw new System.NotImplementedException();
        }

        public OutputWpfData AddExtraDetail(CurrentExtraDetail currentExtraDetail, OutputWpfData oldData)
        {
            throw new System.NotImplementedException();
        }

        public OutputWpfData RemoveExtraDetail(CurrentExtraDetail currentExtraDetail, OutputWpfData oldData)
        {
            throw new System.NotImplementedException();
        }

        private OutputWpfData Calculate(OutputWpfData notPricedOutputData)
        {
            throw new System.NotImplementedException();
        }
    }
}
