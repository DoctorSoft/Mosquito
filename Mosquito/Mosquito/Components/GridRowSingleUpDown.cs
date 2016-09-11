using System;
using System.Windows;
using Xceed.Wpf.Toolkit;

namespace Mosquito.Components
{
    public class GridRowSingleUpDown : SingleUpDown
    {
        public static readonly DependencyProperty ItemIdProperty = DependencyProperty.Register("ItemId", typeof(Guid), typeof(GridRowSingleUpDown));

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
