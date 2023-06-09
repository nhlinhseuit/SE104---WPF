using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

namespace SE104___WPF
{
    /// <summary>
    /// Interaction logic for SigninWindow.xaml
    /// </summary>
    public partial class SigninWindow : Window
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
        public SigninWindow()
        {
            InitializeComponent();
            CenterWindowOnScreen();
            rememberMe = ReadRememberMeState();
            rememberCheckbox.IsChecked = rememberMe;
            txtboxUsername.Focus();
            if (rememberMe)
            {
                AutoLogin();
            }
            this.PreviewKeyDown += MainWindow_PreviewKeyDown;
        }

        SqlConnection sqlCon;
        SqlCommand sqlCom;
        SqlDataReader read;
        SqlDataAdapter sqlDa;
        DataTable dtbStudent;

        private bool rememberMe;
        private string remembertxtboxUsername;
        private string remembertxtboxPassword;

        private void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true; // Ngăn chặn sự kiện Enter tiếp tục lan ra các điều khiển khác
                Button_Click(sender, e); // Kích hoạt sự kiện của button
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string connectionString = "Data Source=LAPTOP-9Q9UCI39;Initial Catalog=SE104;Integrated Security=True";
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    string query = "SELECT COUNT(*) FROM NGDUNG WHERE USERNAME = @username AND MATKHAU = @password";
                    SqlCommand sqlCommand = new SqlCommand(query, sqlCon);
                    sqlCommand.Parameters.AddWithValue("@username", txtboxUsername.Text);
                    sqlCommand.Parameters.AddWithValue("@password", txtboxPassword.Text);

                    int count = Convert.ToInt32(sqlCommand.ExecuteScalar());
                    sqlCon.Close();

                    txtboxUsername.Text = "";
                    txtboxPassword.Text = "";

                    if (count > 0)
                    {
                        MainWindow mainWindow = new MainWindow();
                        this.Close();
                        mainWindow.Show();
                    }
                    else
                    {
                        MessageBox.Show("Username or password isn't correct!");
                    }
                }

            }
            catch
            {
                MessageBox.Show("Sign in failed!");
            }

            
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            bool loginSuccess = AuthenticateUser();

            if (loginSuccess)
            {
                rememberMe = rememberCheckbox.IsChecked ?? false;
                SaveRememberMeState(rememberMe);
                // Tiếp tục với các hoạt động sau khi đăng nhập thành công
                MessageBox.Show("Đăng nhập thành công!");
            }
            else
            {
                // Xử lý khi đăng nhập thất bại
                MessageBox.Show("Đăng nhập thất bại!");
            }
        }

        private bool AuthenticateUser()
        {
            // Triển khai mã để xác thực người dùng
            // Ví dụ: Kiểm tra tên người dùng và mật khẩu nhập vào
            string bang = "Data Source=LAPTOP-9Q9UCI39;Initial Catalog=SE104;Integrated Security=True";
            sqlCon = new SqlConnection(bang);
            sqlCon.Open();
            SqlCommand sql = new SqlCommand("SELECT * FROM NGDUNG WHERE USERNAME = @username AND MATKHAU = @password", sqlCon);

            sql.Parameters.AddWithValue("@username", txtboxUsername.Text);
            sql.Parameters.AddWithValue("@password", txtboxPassword.Text);
            sql.ExecuteNonQuery();
            int count = Convert.ToInt32(sql.ExecuteScalar());
            sqlCon.Close();

            txtboxUsername.Text = "";
            txtboxPassword.Text = "";

            if (count > 0)
            {
                remembertxtboxUsername = txtboxUsername.Text;
                remembertxtboxPassword = txtboxPassword.Text;
                return true;
            }
            else
            {
                return false;
            }
        }

        private void AutoLogin()
        {
            // Triển khai mã để đăng nhập tự động
            // Ví dụ: Gán tên người dùng và mật khẩu mặc định
            txtboxUsername.Text = remembertxtboxUsername;
            txtboxPassword.Text = remembertxtboxPassword;
        }

        private bool ReadRememberMeState()
        {
            // Triển khai mã để đọc trạng thái Remember Me từ lưu trữ
            // Ví dụ: Đọc giá trị từ tệp tin hoặc cơ sở dữ liệu
            // Return giá trị đọc được (true nếu đã chọn Remember Me, false nếu không)
            // Trong ví dụ này, tạm thời trả về true để mô phỏng việc đã chọn Remember Me trước đó
            return true;
        }

        private void SaveRememberMeState(bool rememberMe)
        {
            // Triển khai mã để lưu trạng thái Remember Me vào lưu trữ
            // Ví dụ: Ghi giá trị vào tệp tin hoặc cơ sở dữ liệu
            // Trong ví dụ này, chỉ hiển thị thông báo để mô phỏng việc lưu trạng thái Remember Me
            if (rememberMe)
            {
                remembertxtboxUsername = txtboxUsername.Text;
                remembertxtboxPassword = txtboxPassword.Text;
                MessageBox.Show("Remember Me đã được lưu.");
            }
            else
            {
                remembertxtboxUsername = "";
                remembertxtboxPassword = "";
                MessageBox.Show("Remember Me đã bị xóa.");
            }
        }


    private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SignupWindow signupWindow = new SignupWindow();
            this.Close();
            signupWindow.Show();
        }
    }
}
