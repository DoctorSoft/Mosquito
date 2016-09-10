using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Core;
using Input.Implementation;
using Input.InputModels;
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
            InitializeComponent();
            data = calculatorService.GetDefault();
            RefreshFormValues(data);
        }

        private void RefreshFormValues(OutputWpfData outputData)
        {
            ProfileComboBox.ItemsSource = outputData.Profiles.Select(model => model.Name);
            //if(currentProfile != null)
            ProfileComboBox.SelectedIndex = 0;
            var currentProfile = outputData.Profiles.FirstOrDefault(model => model.Name != null);
            if (currentProfile != null)
                outputData.CurrentProfile = new CurrentProfile
                {
                    Name = currentProfile.Name,
                    Price = currentProfile.PricePerCount
                };

            CrossProfileComboBox.ItemsSource = outputData.CrossProfiles.Select(model => model.Name);
            CrossProfileComboBox.SelectedIndex = 0;
            var currentCrossProfile = outputData.Profiles.FirstOrDefault(model => model.Name != null);
            if (currentCrossProfile != null)
                outputData.CurrentCrossProfile = new CurrentCrossProfile
                {
                    Name = currentCrossProfile.Name,
                    Price = currentCrossProfile.PricePerCount
                };

            NetComboBox.ItemsSource = outputData.Nets.Select(model => model.Name);
            NetComboBox.SelectedIndex = 0;
            var currentNet = outputData.Profiles.FirstOrDefault(model => model.Name != null);
            if (currentNet != null)
                outputData.CurrentNet = new CurrentNet
                {
                    Name = currentNet.Name,
                    Price = currentNet.PricePerCount
                };

            CordComboBox.ItemsSource = outputData.Cords.Select(model => model.Name);
            CordComboBox.SelectedIndex = 0;
            var currentCord = outputData.Profiles.FirstOrDefault(model => model.Name != null);
            if (currentCord != null)
                outputData.CurrentCord = new CurrentCord
                {
                    Name = currentCord.Name,
                    Price = currentCord.PricePerCount
                };

            WorkPriceTextBox.Text = outputData.WorkPrice.ToString("G");

            TrashPriceTextBox.Text = outputData.TrashPrice.ToString("G");

            OtherPendingTextBox.Text = outputData.OtherSpendingPrice.ToString("G");

            if (outputData.ExtraDetails != null)
            {
                int i = 0;
                foreach (var extraDetail in outputData.ExtraDetails)
                {
                    CreateNewExtraDetailField(extraDetail, i);
                    i++;
                }
            }
        }

        private void CreateNewExtraDetailField(ExtraDetailIm extraDetail, int count)
        {
            const int indent = 30;
            double otherPendingTextBoxPositionTop = OtherPendingTextBox.Margin.Top + indent;
            if (count != 0)
            {
                otherPendingTextBoxPositionTop += count * indent;
            }
            double textBoxPositionLeft = TrashPriceTextBox.Margin.Left;
            double textBoxHeight = TrashPriceTextBox.Height;
            double textBoxWidth = TrashPriceTextBox.Width;
            double labelPositionLeft = HeighLabel.Margin.Left;
            double AdditionallabelPositionLeft = AdditionalHeighLabel.Margin.Left;
            
            var extraDetailNameLabel = new Label
            {
                Content = extraDetail.Name,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(labelPositionLeft, otherPendingTextBoxPositionTop, 0, 0)
            };

            var extraDetailTextBox= new TextBox
            {
                Name = "ExtraDetailTextBox_" + count,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Height = textBoxHeight,
                Width = textBoxWidth,
                Margin = new Thickness(textBoxPositionLeft, otherPendingTextBoxPositionTop, 0, 0)
            };

            var extraDetailMeasureLabel = new Label
            {
                Content = "шт.",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(AdditionallabelPositionLeft, otherPendingTextBoxPositionTop, 0, 0)
            };

            MailGrid.Children.Add(extraDetailNameLabel);
            MailGrid.Children.Add(extraDetailTextBox);
            MailGrid.Children.Add(extraDetailMeasureLabel);
        }
        private void HeighTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            decimal currentHeight = 0;
            if (HeighTextBox.Text != null)
                currentHeight = Convert.ToDecimal(HeighTextBox.Text);
            calculatorService.ChangeHeight(currentHeight, data);
        }

        private void WeigthTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            decimal currentWeigth = 0;
            if (WeigthTextBox.Text != null)
                currentWeigth = Convert.ToDecimal(WeigthTextBox.Text);
            calculatorService.ChangeHeight(currentWeigth, data);
        }
    }
}
