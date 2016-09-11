using System;
using System.Windows;
using System.Windows.Controls;

namespace Mosquito.Components
{
    public class GridRowComboBox: ComboBox
    {
        public static readonly DependencyProperty ItemIdProperty = DependencyProperty.Register("ItemId", typeof(Guid), typeof(GridRowComboBox));

        public Guid ItemId
        {
            get
            {
                return Guid.Parse(this.GetValue(ItemIdProperty).ToString());
            }
            set
            {
                this.SetValue(ItemIdProperty, value);
            }
        }
    }
}
