using SE104___WPF.DBClass;
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
    /// Interaction logic for CC_AddNewStudent.xaml
    /// </summary>
    public partial class CC_AddNewStudent : UserControl
    {
        clsHOCSINH hs = new clsHOCSINH();
        private void CenterWindowOnScreen()
        {
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            //this.Left = (screenWidth / 2) - (windowWidth / 2);
            //this.Top = (screenHeight / 2) - (windowHeight / 2);
        }
        public CC_AddNewStudent()
        {
            InitializeComponent();
            CenterWindowOnScreen();
        }

        // btn Cancel
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void khoiTaoHocSinh()
        {
            if (datePickerNgsinh.Text != "")
                hs.ngsinh = Convert.ToDateTime(datePickerNgsinh.Text);
            if (txtTenHs.Text != "")
                hs.tenhs = txtTenHs.Text;
            if (txtDiaChi.Text != "")
                hs.diachi = txtDiaChi.Text;
            if (txtEmail.Text != "")
                hs.email = txtEmail.Text;
        }


        private void txtGioiTinh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            hs.gioitinh = cbbGioiTinh.SelectedIndex;
        }


        //btn Add Student
        private void btnAddStudent_Click(object sender, RoutedEventArgs e)
        {
            khoiTaoHocSinh();
            if (hs.themHocSinh(hs) == 1)
                this.Close();
        }
    }
}
