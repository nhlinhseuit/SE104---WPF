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
    /// Interaction logic for CC_MRegulation.xaml
    /// </summary>
    public partial class CC_MRegulation : UserControl
    {
        public CC_MRegulation()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddClass addClass = new AddClass();
            addClass.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            AddSubject addSubject = new AddSubject();
            addSubject.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            UpdateClass updateClass = new UpdateClass();
            updateClass.Show();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            UpdateSubject updateSubject = new UpdateSubject();
            updateSubject.Show();
        }
    }
}
