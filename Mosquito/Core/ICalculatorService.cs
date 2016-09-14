using System;
using Input.InputModels;
using OutputWPF.OutputWPFModels;

namespace Core
{
    public interface ICalculatorService
    {
        OutputWpfData GetDefault();

        OutputWpfData ChangeWidth(decimal width, OutputWpfData oldData);

        OutputWpfData ChangeHeight(decimal height, OutputWpfData oldData);

        OutputWpfData ChangeSystem(string systemName, OutputWpfData oldData);

        OutputWpfData ChangeProfile(string profileName, OutputWpfData oldData);

        OutputWpfData ChangeCrossProfile(string crossProfileName, OutputWpfData oldData);

        OutputWpfData ChangeNet(string netName, OutputWpfData oldData);

        OutputWpfData ChangeCord(string cordName, OutputWpfData oldData);

        OutputWpfData ChangeAngle(string angleName, OutputWpfData oldData);

        OutputWpfData ChangeAngleCount(decimal angleCount, OutputWpfData oldData);

        OutputWpfData ChangeMount(string mountName, OutputWpfData oldData);

        OutputWpfData ChangeMountCount(decimal mountCount, OutputWpfData oldData);

        OutputWpfData AddExtraDetail(OutputWpfData oldData);

        OutputWpfData RemoveExtraDetail(Guid id, OutputWpfData oldData);

        OutputWpfData UpdateExtraDetailName(Guid id, string newName, OutputWpfData oldData);

        OutputWpfData UpdateExtraDetailCount(Guid id, decimal newCount, OutputWpfData oldData);
    }
}
