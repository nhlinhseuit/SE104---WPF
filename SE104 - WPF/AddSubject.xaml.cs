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
    /// Interaction logic for AddSubject.xaml
    /// </summary>
    public partial class AddSubject : Window
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
        public AddSubject()
        {
            InitializeComponent();
            CenterWindowOnScreen();
        }

        string bang = "Data Source=LAPTOP-9Q9UCI39;Initial Catalog=SE104;Integrated Security=True";
        SqlConnection sqlCon;
        SqlCommand sqlCom;
        SqlDataReader read;
        SqlDataAdapter sqlDa;
        DataTable dtbClass;
        DataTable dtbStudent;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            String txtSubject = txtboxSubject.Text;
            string selectQuerySubjectname = "SELECT COUNT(*) FROM MONHOC WHERE TENMH = @TenMH";
            string insertQuery = "INSERT INTO MONHOC (TENMH) VALUES (@TenMH)";

            using (SqlConnection sqlCon = new SqlConnection(bang))
            {
                sqlCon.Open();
                // Truy vấn SELECT để xem trong db đã có tên lớp đó chưa
                using (SqlCommand selectCmd2 = new SqlCommand(selectQuerySubjectname, sqlCon))
                {
                    selectCmd2.Parameters.AddWithValue("@TenMH", txtSubject);
                    int existingCount = (int)selectCmd2.ExecuteScalar();

                    if (existingCount > 0)
                    {
                        // Tên lớp đã tồn tại, báo lỗi hoặc thực hiện xử lý phù hợp
                        MessageBox.Show("Subject's already existed!!");
                        return;
                    }
                    else
                    {
                        // Truy vấn INSERT để thêm giá trị mới vào bảng LOP
                        using (SqlCommand insertCmd = new SqlCommand(insertQuery, sqlCon))
                        {
                            insertCmd.Parameters.AddWithValue("@TenMH", txtSubject);
                            insertCmd.ExecuteNonQuery();
                        }
                    }


                }
                MessageBox.Show("Add subject successfully!");
                this.Close();
            }
        }
    }
}
