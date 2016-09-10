using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Core;
using Input.Implementation;
using OutputWPF.OutputWPFModels;

namespace Mosquito
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ICalculatorService calculatorService;

        private readonly OutputWpfData data;

        public MainWindow()
        {
            calculatorService = new CalculatorService(new InputDataProviderStub());
            data = calculatorService.GetDefault();
            InitializeComponent();
        }
    }
}
