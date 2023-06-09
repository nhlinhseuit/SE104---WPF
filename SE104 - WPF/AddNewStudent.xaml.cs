using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SE104___WPF.DBClass;

namespace SE104___WPF
{
    /// <summary>
    /// Interaction logic for AddNewStudent.xaml
    /// </summary>

    public partial class AddNewStudent : Window
    {
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
        public AddNewStudent( )
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

        public bool ValidateEmail(string email)
        {
            // Biểu thức chính quy để kiểm tra tính hợp lệ của địa chỉ email
            string emailPattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

            // Sử dụng Regex để kiểm tra địa chỉ email theo biểu thức chính quy
            Regex regex = new Regex(emailPattern);
            return regex.IsMatch(email);
        }
        public bool ValidateAge(int tuoiMin, int tuoiMax)
        {
            DateTime ngaySinh = datePickerNgsinh.SelectedDate ?? DateTime.MinValue; // Lấy giá trị ngày sinh từ DatePicker
            DateTime ngayHienTai = DateTime.Now;
            int tuoi = ngayHienTai.Year - ngaySinh.Year;
            if (ngaySinh > ngayHienTai.AddYears(-tuoi))
            {
                tuoi--; // Giảm tuổi nếu chưa đến ngày sinh trong năm hiện tại
            }
            if (tuoi >= tuoiMin && tuoi < tuoiMax)
                return true;
            return false;
        }
        //btn Add Student
        private void btnAddStudent_Click(object sender, RoutedEventArgs e)
        {
            //check email
            string email = txtEmail.Text;
            if (ValidateEmail(email) == false)
            {
                MessageBox.Show("Email is invalid!");
                return;
            }

            //check tuoi
            int tuoiMin = 0;
            int tuoiMax = 0;
            string bang = "Data Source=LAPTOP-9Q9UCI39;Initial Catalog=SE104;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(bang))
            {
                connection.Open();

                string query = "SELECT TUOIMIN, TUOIMAX FROM QUYDINH";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        tuoiMin = Convert.ToInt32(reader["TUOIMIN"]);
                        tuoiMax = Convert.ToInt32(reader["TUOIMAX"]);
                    }

                    reader.Close();
                }

                connection.Close();
            }
            if (ValidateAge(tuoiMin, tuoiMax) == false)
            {
                MessageBox.Show("Age must be grater than " + tuoiMin + " and smaller than " + tuoiMax);
                return;
            }

            khoiTaoHocSinh();
            hs.themHocSinh(hs);
            this.Close();
               
        }
    }
}
