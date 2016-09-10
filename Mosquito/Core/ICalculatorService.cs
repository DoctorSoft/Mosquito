using OutputWPF.OutputWPFModels;

namespace Core
{
    public interface ICalculatorService
    {
        OutputWpfData GetDefault();

        OutputWpfData ChangeWidth(decimal width, OutputWpfData oldData);

        OutputWpfData ChangeHeight(decimal width, OutputWpfData oldData);

        OutputWpfData ChangeNet(CurrentNet net, OutputWpfData oldData);

        OutputWpfData AddExtraDetail(CurrentExtraDetail currentExtraDetail, OutputWpfData oldData);

        OutputWpfData RemoveExtraDetail(CurrentExtraDetail currentExtraDetail, OutputWpfData oldData);
    }
}
