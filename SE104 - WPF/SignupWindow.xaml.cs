using System;
using System.Collections.Generic;
using System.Data;
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

namespace SE104___WPF
{
    /// <summary>
    /// Interaction logic for SignupWindow.xaml
    /// </summary>
    public partial class SignupWindow : Window
    {
        private void CenterWindowOnScreen()
        {
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2);
        }

        public SignupWindow()
        {
            InitializeComponent();
            CenterWindowOnScreen();
        }

        SqlConnection sqlCon;
        SqlCommand sqlCom;
        SqlDataReader read;
        SqlDataAdapter sqlDa;
        DataTable dtbStudent;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SigninWindow loginWindow = new SigninWindow();
            this.Close();
            loginWindow.Show();
        }

        public bool ValidateEmail(string email)
        {
            // Biểu thức chính quy để kiểm tra tính hợp lệ của địa chỉ email
            string emailPattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

            // Sử dụng Regex để kiểm tra địa chỉ email theo biểu thức chính quy
            Regex regex = new Regex(emailPattern);
            return regex.IsMatch(email);
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string email = txtboxEmail.Text;
            if (ValidateEmail(email) == false)
            {
                MessageBox.Show("Email is invalid!");
                return;
            }
            if (txtboxPassword.Text != txtboxRePassword.Text)
            {
                MessageBox.Show("Re-password is not correct!");
                return;
            }

            try
            {
                string bang = "Data Source=LAPTOP-9Q9UCI39;Initial Catalog=SE104;Integrated Security=True";
                sqlCon = new SqlConnection(bang);
                sqlCon.Open();

                string selectQueryClassname = "SELECT COUNT(*) FROM NGDUNG WHERE USERNAME = @Ngdung";
                SqlCommand sqlCheck = new SqlCommand(selectQueryClassname, sqlCon);
                sqlCheck.Parameters.AddWithValue("@Ngdung", txtboxName.Text);
                int existingCount = (int)sqlCheck.ExecuteScalar();

                if (existingCount > 0)
                {
                    // Tên lớp đã tồn tại, báo lỗi hoặc thực hiện xử lý phù hợp
                    MessageBox.Show("Username's already existed!!");
                    return;
                }
                else
                {
                    SqlCommand sql = new SqlCommand("INSERT INTO NGDUNG(USERNAME,EMAIL,MATKHAU) VALUES(@username, @email, @password)", sqlCon);

                    sql.Parameters.AddWithValue("@username", txtboxName.Text);
                    sql.Parameters.AddWithValue("@email", txtboxEmail.Text);
                    sql.Parameters.AddWithValue("@password", txtboxPassword.Text);
                    sql.ExecuteNonQuery();
                    sqlCon.Close();

                    txtboxName.Text = "";
                    txtboxEmail.Text = "";
                    txtboxPassword.Text = "";
                }
                
                MessageBox.Show("Sign up successfully!");
                SigninWindow loginWindow = new SigninWindow();
                this.Close();
                loginWindow.Show();
            }
             catch
            {
                MessageBox.Show("Sign up failed!");
            }

        }
    }
}
