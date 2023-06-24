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
using static SE104___WPF.CC_MClass;

namespace SE104___WPF
{
    /// <summary>
    /// Interaction logic for UpdateSubject.xaml
    /// </summary>
    public partial class UpdateSubject : Window
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
        public UpdateSubject()
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
                sqlCom = new SqlCommand("SELECT MH_ID FROM MONHOC", sqlCon);
                using (SqlDataReader reader = sqlCom.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string khoiValue = reader["MH_ID"].ToString();
                        // Thêm giá trị vào ComboBox
                        SubjectCbb.Items.Add(khoiValue);
                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void GradeCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (sqlCon = new SqlConnection(bang))
            {
                sqlCon.Open();
                sqlCom = new SqlCommand("SELECT TENMH FROM MONHOC WHERE MH_ID = @MHID", sqlCon);
                sqlCom.Parameters.AddWithValue("@MHID", SubjectCbb.SelectedItem.ToString());
                using (SqlDataReader reader = sqlCom.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        txtboxSubject.Text = reader["TENMH"].ToString();
                    }
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string subjectName = txtboxSubject.Text;
            string subjectID = SubjectCbb.SelectedItem.ToString();
            if (subjectName == null)
            {
                MessageBox.Show("EROR: Subject name of Subject ID is null!");
            }
            else
            {
                using (SqlConnection sqlCon = new SqlConnection(bang))
                {
                    sqlCon.Open();
                    string selectQueryClassname = "SELECT COUNT(*) FROM MONHOC WHERE TENMH = @TenMH";
                    using (SqlCommand selectCmd2 = new SqlCommand(selectQueryClassname, sqlCon))
                    {
                        selectCmd2.Parameters.AddWithValue("@TenMH", subjectName);
                        int existingCount = (int)selectCmd2.ExecuteScalar();

                        if (existingCount > 0)
                        {
                            // Tên lớp đã tồn tại, báo lỗi hoặc thực hiện xử lý phù hợp
                            MessageBox.Show("Subject's already existed!!");
                            return;
                        }
                        else
                        {
                            string sql2 = "UPDATE MONHOC SET TENMH = @TenMH " +
                                "WHERE MH_ID = @MHID";
                            SqlCommand sql = new SqlCommand(sql2, sqlCon);
                            sql.Parameters.AddWithValue("@TenMH", subjectName);
                            sql.Parameters.AddWithValue("@MHID", subjectID);
                            sql.ExecuteNonQuery();
                            sqlCon.Close();
                            MessageBox.Show("Update subject name successfully!");
                        }

                        // Truy vấn INSERT để thêm giá trị mới vào bảng LOP
                        this.Close();

                    }
                }
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string subjectID = SubjectCbb.SelectedItem.ToString();
            if (subjectID == null)
            {
                MessageBox.Show("EROR!");
            }
            else
            {
                SqlConnection sqlConString = new SqlConnection(bang);
                sqlConString.Open();
                string sql2 = "DELETE FROM BANGDIEM WHERE MH_ID = @SubjectID " +
                    " DELETE FROM MONHOC WHERE MH_ID = @SubjectID";
                SqlCommand sql = new SqlCommand(sql2, sqlConString);
                sql.Parameters.AddWithValue("@SubjectID", subjectID);

                MessageBoxResult result = MessageBox.Show("Are you sure want to delete?",
                "Yes", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    sql.ExecuteNonQuery();
                    sqlConString.Close();
                    MessageBox.Show("All the grade belong to this subject have been removed! Delete subject successfully!");
                    this.Close();
                }
                else
                {
                    return;
                }
            }
        }
    }
}
