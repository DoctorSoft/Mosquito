using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Core;
using Input.Implementation;
using Input.InputModels;
using Mosquito.Components;
using Mosquito.Models;
using OutputWPF.OutputWPFModels;
using Xceed.Wpf.Toolkit;
using MessageBox = System.Windows.MessageBox;

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
            try
            {
                data = calculatorService.GetDefault();
                RefreshFormValues();
            }
            catch (Exception)
            {
                MessageBox.Show("Something went wrong. Check you excel file");
                Close();
            }
        }

        private void InitRequiredExtraDetailsGridBinding()
        {
            var items = data.RequiredExtraDetails.Select(im => new ExtraDetailViewModel
            {
                Name = im.Name,
                Price = im.Price,
                Count = im.Count,
                ItemId = im.Id
            });

            RequiredExtraDetailsDataGrid.Items.Clear();
            foreach (var item in items)
            {
                RequiredExtraDetailsDataGrid.Items.Add(item);
            }
        }

        private void InitExtraDetailsGridBinding()
        {
            var items = data.CurrentExtraDetails.Select(im => new ExtraDetailViewModel
            {
                Name = im.Name,
                Price = im.Price,
                Count = im.Count,
                NameList = data.ExtraDetails.Select(detailIm => detailIm.Name).ToList(),
                ItemId = im.Id
            });

            ExtraDetailsDataGrid.Items.Clear();
            foreach (var item in items)
            {
                ExtraDetailsDataGrid.Items.Add(item);
            }
        }

        private void SetProductData<TProductIm>(ComboBox comboBox, SingleUpDown sizeSingleUpDown, Label sizeLabel, Label priceLabel, List<TProductIm> products, CurrentProduct currentProduct)
            where TProductIm : ProductIm
        {
            comboBox.ItemsSource = products.Select(im => im.Name);
            comboBox.SelectedIndex = comboBox.Items.IndexOf(currentProduct.Name);

            if (sizeLabel == null && sizeSingleUpDown == null)
            {
                return;
            }

            if (sizeSingleUpDown == null)
            {
                sizeLabel.Content = currentProduct.Count;
            }
            else
            {
                sizeSingleUpDown.Value = currentProduct.Count == 0 ? 1 : (float) currentProduct.Count;
            }
            priceLabel.Content = currentProduct.Price;

        }

        private void SetAdditionalCrossProfileData(ComboBox comboBox, SingleUpDown sizeSingleUpDown, Label sizeLabel,
            Label priceLabel, List<CrossProfileIm> products, CurrentProduct currentProduct, CheckBox checkBox)
        {
            checkBox.IsChecked = data.ExtraCrossProfileEnabled;

            if (!data.ExtraCrossProfileAllowed)
            {
                comboBox.IsEnabled = false;
                checkBox.IsEnabled = false;
                checkBox.IsChecked = false;
            }
            else
            {
                comboBox.IsEnabled = true;
                checkBox.IsEnabled = true;
            }

            SetProductData(comboBox, sizeSingleUpDown, sizeLabel, sizeLabel, products, currentProduct);

        }

        private void SetAdditionalCrossProfileWithGrooveData(ComboBox comboBox, SingleUpDown sizeSingleUpDown, Label sizeLabel,
            Label priceLabel, List<CrossProfileIm> products, CurrentProduct currentProduct, CheckBox checkBox)
        {
            checkBox.IsChecked = data.ExtraCrossProfileWithGrooveEnabled;

            if (!data.ExtraCrossProfileWithGrooveAllowed)
            {
                comboBox.IsEnabled = false;
                checkBox.IsEnabled = false;
                checkBox.IsChecked = false;
            }
            else
            {
                comboBox.IsEnabled = true;
                checkBox.IsEnabled = true;
            }

            SetProductData(comboBox, sizeSingleUpDown, sizeLabel, sizeLabel, products, currentProduct);

        }

        private void SetSystemData(ComboBox comboBox, List<SystemIm> systems, CurrentSystem currentSystem)
        {
            comboBox.ItemsSource = systems.Select(im => im.Name);
            comboBox.SelectedIndex = comboBox.Items.IndexOf(currentSystem.Name);
        }

        private void RefreshFormValues()
        {
            HeightSingleUpDown.Value = (float?)data.Height;
            WeigthSingleUpDown.Value = (float?)data.Width;
            WorkPriceLabel.Content = (float?)data.WorkPrice;
            TrashPercentLabel.Content = (float?) data.TrashPercent;
            TrashPriceLabel.Content = (float?) data.TrashPrice;
            OtherPendingLabel.Content = (float?) data.OtherSpendingPrice;
            TotalPriceLabel.Content = (float?) data.TotalPrice;

            SetSystemData(SystemComboBox, data.Systems, data.CurrentSystem);

            SetProductData(ProfileComboBox, null, ProfileCountLabel, ProfilePriceLabel, data.Profiles, data.CurrentProfile);
            SetProductData(CrossProfileComboBox, null, CrossProfileCountLabel, CrossProfilePriceLabel, data.CrossProfiles, data.CurrentCrossProfile);
            SetAdditionalCrossProfileData(AdditionalCrossWithoutGrooveComboBox, null, null, null, data.CrossProfiles.Where(im => im.Name == data.CurrentCrossProfile.Name).ToList(), data.CurrentCrossProfile, AdditionalCrossWithoutGrooveCheckBox);
            SetAdditionalCrossProfileWithGrooveData(AdditionalCrossWithGrooveComboBox, null, null, null, data.CrossProfilesWithGroove, data.CurrentCrossProfileWithGroove, AdditionalCrossWithGrooveCheckBox);
            SetProductData(NetComboBox, null, NetCountLabel, NetPriceLabel, data.Nets, data.CurrentNet);
            SetProductData(CordComboBox, null, CordCountLabel, CordPriceLabel, data.Cords, data.CurrentCord);
            SetProductData(AngleComboBox, AngleCountSingleUpDown, null, AnglePriceLabel, data.Angles, data.CurrentAngle);
            SetProductData(MountComboBox, MountCountSingleUpDown, null, MountPriceLabel, data.Mounts, data.CurrentMount);
            SetProductData(CrossMountComboBox, CrossMountCountSingleUpDown, null, CrossMountPriceLabel, data.CrossMounts, data.CurrentCrossMount);
            SetProductData(KnobComboBox, KnobCountSingleUpDown, null, KnobPriceLabel, data.Knobs, data.CurrentKnob);
            SetProductData(ExtraMountComboBox, ExtraMountCountSingleUpDown, null, ExtraMountPriceLabel, data.ExtraMounts, data.CurrentExtraMount);

            InitExtraDetailsGridBinding();
            InitRequiredExtraDetailsGridBinding();
        }

        private void HeightSingleUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (data == null)
            {
                return;
            }

            var newValue = e.NewValue;
            data = calculatorService.ChangeHeight(decimal.Parse(newValue.ToString()), data);
            RefreshFormValues();
        }

        private void WeigthSingleUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (data == null)
            {
                return;
            }

            var newValue = e.NewValue;
            data = calculatorService.ChangeWidth(decimal.Parse(newValue.ToString()), data);
            RefreshFormValues();
        }

        private void ProfileComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (data == null || e.AddedItems.Count == 0)
            {
                return;
            }
            var newValue = e.AddedItems[0] as string;
            data = calculatorService.ChangeProfile(newValue, data);
            RefreshFormValues();
        }

        private void CrossProfileComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (data == null || e.AddedItems.Count == 0)
            {
                return;
            }
            var newValue = e.AddedItems[0] as string;
            data = calculatorService.ChangeCrossProfile(newValue, data);
            RefreshFormValues();
        }

        private void NetComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (data == null || e.AddedItems.Count == 0)
            {
                return;
            }
            var newValue = e.AddedItems[0] as string;
            data = calculatorService.ChangeNet(newValue, data);
            RefreshFormValues();
        }

        private void CordComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (data == null || e.AddedItems.Count == 0)
            {
                return;
            }
            var newValue = e.AddedItems[0] as string;
            data = calculatorService.ChangeCord(newValue, data);
            RefreshFormValues();
        }

        private void AddExtraDetailButton_Click(object sender, RoutedEventArgs e)
        {
            if (data == null)
            {
                return;
            }
            data = calculatorService.AddExtraDetail(data);
            RefreshFormValues();
        }

        private void RemoveExtraDetailButton_Click(object sender, RoutedEventArgs e)
        {
            if (data == null)
            {
                return;
            }

            var button = (GridRowButton) sender;
            var itemId = button.ItemId;

            data = calculatorService.RemoveExtraDetail(itemId, data);
            RefreshFormValues();
        }

        private void ExtraDetailCount_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (data == null)
            {
                return;
            }

            var newValue = e.NewValue;
            var element = (GridRowSingleUpDown) sender;
            var itemId = element.ItemId;

            if (itemId == Guid.Empty)
            {
                return;
            }

            data = calculatorService.UpdateExtraDetailCount(itemId, decimal.Parse(newValue.ToString()), data);
            RefreshFormValues();
        }

        private void ExtraDetailName_ValueChanged(object sender, SelectionChangedEventArgs e)
        {
            if (data == null || e.AddedItems.Count == 0)
            {
                return;
            }

            var newValue = e.AddedItems[0] as string;
            var element = (GridRowComboBox)sender;
            var itemId = element.ItemId;

            if (itemId == Guid.Empty)
            {
                return;
            }

            data = calculatorService.UpdateExtraDetailName(itemId, newValue, data);
            RefreshFormValues();
        }

        private void AngleCountSingleUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (data == null)
            {
                return;
            }

            var newValue = e.NewValue;
            data = calculatorService.ChangeAngleCount(decimal.Parse(newValue.ToString()), data);
            RefreshFormValues();
        }

        private void AngleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (data == null || e.AddedItems.Count == 0)
            {
                return;
            }
            var newValue = e.AddedItems[0] as string;
            data = calculatorService.ChangeAngle(newValue, data);
            RefreshFormValues();
        }

        private void MountComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (data == null || e.AddedItems.Count == 0)
            {
                return;
            }
            var newValue = e.AddedItems[0] as string;
            data = calculatorService.ChangeMount(newValue, data);
            RefreshFormValues();
        }

        private void MountCountSingleUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (data == null)
            {
                return;
            }

            var newValue = e.NewValue;
            data = calculatorService.ChangeMountCount(decimal.Parse(newValue.ToString()), data);
            RefreshFormValues();
        }

        private void CrossMountComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (data == null || e.AddedItems.Count == 0)
            {
                return;
            }
            var newValue = e.AddedItems[0] as string;
            data = calculatorService.ChangeCrossMount(newValue, data);
            RefreshFormValues();
        }
        
        private void CrossMountCountSingleUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (data == null)
            {
                return;
            }

            var newValue = e.NewValue;
            data = calculatorService.ChangeCrossMountCount(decimal.Parse(newValue.ToString()), data);
            RefreshFormValues();
        }

        private void KnobComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (data == null || e.AddedItems.Count == 0)
            {
                return;
            }
            var newValue = e.AddedItems[0] as string;
            data = calculatorService.ChangeKnob(newValue, data);
            RefreshFormValues();
        }

        private void KnobCountSingleUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (data == null)
            {
                return;
            }

            var newValue = e.NewValue;
            data = calculatorService.ChangeKnobCount(decimal.Parse(newValue.ToString()), data);
            RefreshFormValues();
        }

        private void SystemComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (data == null || e.AddedItems.Count == 0)
            {
                return;
            }
            var newValue = e.AddedItems[0] as string;
            data = calculatorService.ChangeSystem(newValue, data);
            RefreshFormValues();
        }
        private void ExtraMountComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (data == null || e.AddedItems.Count == 0)
            {
                return;
            }
            var newValue = e.AddedItems[0] as string;
            data = calculatorService.ChangeExtraMount(newValue, data);
            RefreshFormValues();
        }

        private void ExtraMountCountSingleUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (data == null)
            {
                return;
            }

            var newValue = e.NewValue;
            data = calculatorService.ChangeExtraMountCount(decimal.Parse(newValue.ToString()), data);
            RefreshFormValues();
        }
        
        private void ExtraDetailsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AdditionalCrossWithoutGrooveCheckBox_Click(object sender, RoutedEventArgs e)
        {
            var checkedValue = ((CheckBox) sender).IsChecked;

            data = calculatorService.CheckExtraCrossProfile(checkedValue.Value, data);
            RefreshFormValues();
        }

        private void AdditionalCrossWithGrooveCheckBox_Click(object sender, RoutedEventArgs e)
        {
            var checkedValue = ((CheckBox)sender).IsChecked;

            data = calculatorService.CheckExtraCrossProfileWithGroove(checkedValue.Value, data);
            RefreshFormValues();
        }

        private void AdditionalCrossWithGrooveComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (data == null || e.AddedItems.Count == 0)
            {
                return;
            }
            var newValue = e.AddedItems[0] as string;
            data = calculatorService.ChangeExtraCrossProfileWithGroove(newValue, data);
            RefreshFormValues();
        }

    }
}
