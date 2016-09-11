using System;
using System.Collections.Generic;
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

        private OutputWpfData data;

        public MainWindow()
        {
            calculatorService = new CalculatorService(new InputDataProvider("mosdb.xlsx"));
            InitializeComponent();
            data = calculatorService.GetDefault();
            RefreshFormValues(data);
        }

        private void SetProductData<TProductIm>(ComboBox comboBox, List<TProductIm> products, CurrentProduct currentProduct)
            where TProductIm : ProductIm
        {
            comboBox.ItemsSource = products.Select(im => im.Name);
            comboBox.SelectedIndex = comboBox.Items.IndexOf(currentProduct.Name);
        }

        private void RefreshFormValues(OutputWpfData outputData)
        {
            SetProductData(ProfileComboBox, outputData.Profiles, outputData.CurrentProfile);
            SetProductData(CrossProfileComboBox, outputData.CrossProfiles, outputData.CurrentCrossProfile);
            SetProductData(CordComboBox, outputData.Cords, outputData.CurrentCord);
            SetProductData(NetComboBox, outputData.Nets, outputData.CurrentNet);

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
