﻿using System;
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
            RefreshFormValues();
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

        private void SetProductData<TProductIm>(ComboBox comboBox, Label sizeLabel, Label priceLabel, List<TProductIm> products, CurrentProduct currentProduct)
            where TProductIm : ProductIm
        {
            comboBox.ItemsSource = products.Select(im => im.Name);
            comboBox.SelectedIndex = comboBox.Items.IndexOf(currentProduct.Name);

            if (sizeLabel == null)
            {
                return;
            }

            sizeLabel.Content = currentProduct.Count;
            priceLabel.Content = currentProduct.Price;
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

            SetProductData(ProfileComboBox, ProfileCountLabel, ProfilePriceLabel, data.Profiles, data.CurrentProfile);
            SetProductData(CrossProfileComboBox, CrossProfileCountLabel, CrossProfilePriceLabel, data.CrossProfiles, data.CurrentCrossProfile);
            SetProductData(NetComboBox, NetCountLabel, NetPriceLabel, data.Nets, data.CurrentNet);
            SetProductData(CordComboBox, CordCountLabel, CordPriceLabel, data.Cords, data.CurrentCord);

            InitExtraDetailsGridBinding();
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
            if (data == null)
            {
                return;
            }
            var newValue = e.AddedItems[0] as string;
            data = calculatorService.ChangeProfile(newValue, data);
            RefreshFormValues();
        }

        private void CrossProfileComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (data == null)
            {
                return;
            }
            var newValue = e.AddedItems[0] as string;
            data = calculatorService.ChangeCrossProfile(newValue, data);
            RefreshFormValues();
        }

        private void NetComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (data == null)
            {
                return;
            }
            var newValue = e.AddedItems[0] as string;
            data = calculatorService.ChangeNet(newValue, data);
            RefreshFormValues();
        }

        private void CordComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (data == null)
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
            if (data == null)
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
    }
}
