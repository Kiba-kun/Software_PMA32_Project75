using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SoftwarePractice_10.CustomControls.DialogWindows
{
    /// <summary>
    /// Interaction logic for Prompt.xaml
    /// </summary>
    public partial class Prompt : Window
    {
        private IEnumerable<string> _items;

        public Prompt(IEnumerable<string> listItems)
        {
            InitializeComponent();
            _items = listItems;
            foreach (var item in _items)
            {
                chooseItem_ListBox.Items.Add(item);
            }

            DataContext = this;
        }

        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// Identified the Label dependency property
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(string),
              typeof(Prompt), new PropertyMetadata("Exception"));

        private void confirmChoose_Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
