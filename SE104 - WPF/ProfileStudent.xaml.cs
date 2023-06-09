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
using System.Windows.Shapes;
using static SE104___WPF.DBClass.clsHOCSINH;
using static SE104___WPF.CC_MStudent;

namespace SE104___WPF
{
    /// <summary>
    /// Interaction logic for ProfileStudent.xaml
    /// </summary>
    public partial class ProfileStudent : Window
    {
        private Student selectedStudent;
        clsHOCSINH hs = new clsHOCSINH();

        private void CenterWindowOnScreen()
        {
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2);
        }
        public ProfileStudent(Student student)
        {
            selectedStudent = student;
            InitializeComponent();
            CenterWindowOnScreen();
            txtboxID.Text = student.ID;
            txtboxName.Text = student.Fullname;
            txtboxEmail.Text = student.Email;
            txtboxAddress.Text = student.Address;

            datePickerNgsinh.Text = Convert.ToString(student.Birthday);
            if (student.Gender == "Male")
                cbbGioiTinh.SelectedIndex = 1;
            else
                cbbGioiTinh.SelectedIndex = 0;


        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void txtGioiTinh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            hs.gioitinh = cbbGioiTinh.SelectedIndex;
        }

        private void capNhatHocSinh()
        {
            if (txtboxID.Text != "")
                hs.hs_id = txtboxID.Text;
            if (datePickerNgsinh.Text != "")
                hs.ngsinh = Convert.ToDateTime(datePickerNgsinh.Text);
            if (txtboxName.Text != "")
                hs.tenhs = txtboxName.Text;
            if (txtboxAddress.Text != "")
                hs.diachi = txtboxAddress.Text;
            if (txtboxEmail.Text != "")
                hs.email = txtboxEmail.Text;

            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            capNhatHocSinh();
            hs.suaHocSinh(hs);
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure want to delete?",
            "Yes", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Thực hiện xóa đối tượng
                capNhatHocSinh();
                hs.xoaHocSinh(hs);
                this.Close();
            }
            else
            {
                return;
            }
            

        }
    }
}
