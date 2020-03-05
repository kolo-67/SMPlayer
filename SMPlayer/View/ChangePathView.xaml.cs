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
using System.Windows.Shapes;

namespace SMPlayer.View
{
    /// <summary>
    /// Логика взаимодействия для ChangePathView.xaml
    /// </summary>
    public partial class ChangePathView : Window
    {
        public ChangePathView()
        {
            InitializeComponent();
        }
        //-------------------------------------------------------------------------------------------------------------------
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
