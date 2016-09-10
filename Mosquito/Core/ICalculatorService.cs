using Input.InputModels;
using OutputWPF.OutputWPFModels;

namespace Core
{
    public interface ICalculatorService
    {
        OutputWpfData GetDefault();

        OutputWpfData ChangeWidth(decimal width, OutputWpfData oldData);

        OutputWpfData ChangeHeight(decimal width, OutputWpfData oldData);

        OutputWpfData ChangeNet(NetIm net, OutputWpfData oldData);

        OutputWpfData AddExtraDetail(ExtraDetailIm extraDetail, int count, OutputWpfData oldData);

        OutputWpfData RemoveExtraDetail(ExtraDetailIm extraDetail, OutputWpfData oldData);
    }
}
