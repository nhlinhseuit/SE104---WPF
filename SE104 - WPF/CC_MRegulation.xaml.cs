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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static SE104___WPF.CC_MStudent;

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
            loadMinAgeCombobox();
            loadMaxAgeCombobox();
            loadMaxStudentCombobox();
            loadGradePassingtCombobox();
            loadClass();
            loadSubject();

        }

        string bang = "Data Source=LAPTOP-9Q9UCI39;Initial Catalog=SE104;Integrated Security=True";
        SqlConnection sqlCon;
        SqlCommand sqlCom;
        SqlDataReader read;
        SqlDataAdapter sqlDa;
        DataTable dtbClass;
        DataTable dtbStudent;

        private void loadSubject()
        {
            int totalSubject = 0;
            using (sqlCon = new SqlConnection(bang))
            {
                sqlCon.Open();
                SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM MONHOC", sqlCon);
                totalSubject = (int)command.ExecuteScalar();
                sqlCon.Close();
            }
            subjectCbb.Text = totalSubject.ToString();
        }

        private void loadClass()
        {
            int totalClass = 0;
            using (sqlCon = new SqlConnection(bang))
            {
                sqlCon.Open();
                SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM LOP", sqlCon);
                totalClass = (int)command.ExecuteScalar();
                sqlCon.Close();
            }
            classCbb.Text = totalClass.ToString();
        }

        private void loadGradePassingtCombobox()
        {
            using (sqlCon = new SqlConnection(bang))
            {
                sqlCon.Open();
                sqlCom = new SqlCommand("SELECT DIEMQUAMON FROM QUYDINH", sqlCon);
                using (SqlDataReader reader = sqlCom.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string khoiValue = reader["DIEMQUAMON"].ToString();
                        // Thêm giá trị vào ComboBox
                        gradePassCbb.Text = khoiValue;
                    }
                }
            }
        }

        private void loadMaxStudentCombobox()
        {
            using (sqlCon = new SqlConnection(bang))
            {
                sqlCon.Open();
                sqlCom = new SqlCommand("SELECT SLMAX FROM QUYDINH", sqlCon);
                using (SqlDataReader reader = sqlCom.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string khoiValue = reader["SLMAX"].ToString();
                        // Thêm giá trị vào ComboBox
                        maxStudentCbb.Text = khoiValue;
                    }
                }
            }
        }

        private void loadMinAgeCombobox()
        {
            using (sqlCon = new SqlConnection(bang))
            {
                sqlCon.Open();
                sqlCom = new SqlCommand("SELECT TUOIMIN FROM QUYDINH", sqlCon);
                using (SqlDataReader reader = sqlCom.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string khoiValue = reader["TUOIMIN"].ToString();
                        // Thêm giá trị vào ComboBox
                        minAgeCbb.Text = khoiValue;
                    }
                }
            }
        }
        private void loadMaxAgeCombobox()
        {
            using (sqlCon = new SqlConnection(bang))
            {
                sqlCon.Open();
                sqlCom = new SqlCommand("SELECT TUOIMAX FROM QUYDINH", sqlCon);
                using (SqlDataReader reader = sqlCom.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string khoiValue = reader["TUOIMAX"].ToString();
                        // Thêm giá trị vào ComboBox
                        maxAgeCbb.Text = khoiValue;
                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddClass addClass = new AddClass();
            addClass.Show();
            addClass.Closed += (sender, args) => loadClass();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            AddSubject addSubject = new AddSubject();
            addSubject.Show();
            addSubject.Closed += (sender, args) => loadSubject();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            UpdateClass updateClass = new UpdateClass();
            updateClass.Show();
            updateClass.Closed += (sender, args) => loadClass();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            UpdateSubject updateSubject = new UpdateSubject();
            updateSubject.Show();
            updateSubject.Closed += (sender, args) => loadSubject();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            string selectedMaxtAge = maxAgeCbb.Text;
            string sql = "UPDATE QUYDINH SET TUOIMAX = @TUOIMAX";

            using (SqlConnection connection = new SqlConnection(bang))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@TUOIMAX", selectedMaxtAge);
                    command.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Update max age successfully!");
            maxAgeCbb.Text = selectedMaxtAge;
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            string selectedMintAge = minAgeCbb.Text;
            string sql = "UPDATE QUYDINH SET TUOIMIN = @TUOIMIN";

            using (SqlConnection connection = new SqlConnection(bang))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@TUOIMIN", selectedMintAge);
                    command.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Update min age successfully!");
            minAgeCbb.Text = selectedMintAge;
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            string selectedMaxStudent = maxStudentCbb.Text;
            string Sqltext;
            //float slmax = float.Parse(selectedMaxStudent);
            string sql = "UPDATE QUYDINH SET SLMAX = @soluongMax WHERE QD_ID = 1";

            using (SqlConnection connection = new SqlConnection(bang))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@soluongMax", selectedMaxStudent);
                    command.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Update max student successfully!");
            maxStudentCbb.Text = selectedMaxStudent;
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            string selectedGradePass = gradePassCbb.Text;
            string sql = "UPDATE QUYDINH SET DIEMQUAMON = @DIEMQUAMON";

            using (SqlConnection connection = new SqlConnection(bang))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@DIEMQUAMON", selectedGradePass);
                    command.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Update grade passing the test successfully!");
            gradePassCbb.Text = selectedGradePass;
        }
    }
}
