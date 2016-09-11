using System;
using System.Windows;
using System.Windows.Controls;

namespace Mosquito.Components
{
    public class GridRowButton : Button
    {
        public static readonly DependencyProperty ItemIdProperty = DependencyProperty.Register("ItemId", typeof(Guid), typeof(GridRowButton));

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
