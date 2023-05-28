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

namespace SE104___WPF
{
    /// <summary>
    /// Interaction logic for CC_MGrade.xaml
    /// </summary>
    public partial class CC_MGrade : UserControl
    {
        public CC_MGrade()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            EnterGrade enterGrade = new EnterGrade();
            enterGrade.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            UpdateGrade updateGrade = new UpdateGrade();
            updateGrade.Show();
        }
    }
}
