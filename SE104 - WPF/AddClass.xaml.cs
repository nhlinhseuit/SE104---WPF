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
    /// Interaction logic for AddClass.xaml
    /// </summary>
    public partial class AddClass : Window
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
        public AddClass()
        {
            InitializeComponent();
            CenterWindowOnScreen();
            loadGradeCombobox();
        }

        string bang = "Data Source=LAPTOP-9Q9UCI39;Initial Catalog=SE104;Integrated Security=True";
        SqlConnection sqlCon;
        SqlCommand sqlCom;
        SqlDataReader read;
        SqlDataAdapter sqlDa;
        DataTable dtbClass;
        DataTable dtbStudent;

        private void loadGradeCombobox()
        {
            using (sqlCon = new SqlConnection(bang))
            {
                sqlCon.Open();
                sqlCom = new SqlCommand("SELECT TENKHOI FROM KHOI", sqlCon);
                using (SqlDataReader reader = sqlCom.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string khoiValue = reader["TENKHOI"].ToString();
                        // Thêm giá trị vào ComboBox
                        GradeCbb.Items.Add(khoiValue);
                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            String txtClass = txtboxClass.Text;
            String txtGrade = GradeCbb.SelectedItem.ToString();
            String KhoiID = "";
            string selectQuery = "SELECT KHOI_ID FROM KHOI WHERE TENKHOI = @Tenkhoi";
            string selectQueryClassname = "SELECT COUNT(*) FROM LOP WHERE TENLOP = @Tenlop";
            string insertQuery = "INSERT INTO LOP (TENLOP, KHOI_ID) VALUES (@Tenlop, @KhoiId)";

            using (SqlConnection sqlCon = new SqlConnection(bang))
            {
                sqlCon.Open();

                // Truy vấn SELECT để lấy KhoiID
                using (SqlCommand selectCmd = new SqlCommand(selectQuery, sqlCon))
                {
                    selectCmd.Parameters.AddWithValue("@Tenkhoi", txtGrade);
                    using (SqlDataReader reader = selectCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            KhoiID = reader["KHOI_ID"].ToString();
                        }
                    }
                }

                // Truy vấn SELECT để xem trong db đã có tên lớp đó chưa
                using (SqlCommand selectCmd2 = new SqlCommand(selectQueryClassname, sqlCon))
                {
                    selectCmd2.Parameters.AddWithValue("@Tenlop", txtClass);
                    int existingCount = (int)selectCmd2.ExecuteScalar();

                    if (existingCount > 0)
                    {
                        // Tên lớp đã tồn tại, báo lỗi hoặc thực hiện xử lý phù hợp
                        MessageBox.Show("Class's already existed!!");
                        return;
                    }
                    else
                    {
                        using (SqlCommand insertCmd = new SqlCommand(insertQuery, sqlCon))
                        {
                            insertCmd.Parameters.AddWithValue("@Tenlop", txtClass);
                            insertCmd.Parameters.AddWithValue("@KhoiId", KhoiID);
                            insertCmd.ExecuteNonQuery();
                        }
                    }

                    // Truy vấn INSERT để thêm giá trị mới vào bảng LOP

                }
                MessageBox.Show("Add class successfully!");
                this.Close();
            }
        }
        private void SemesterCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
