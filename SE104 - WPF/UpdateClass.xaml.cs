using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SE104___WPF
{
    /// <summary>
    /// Interaction logic for UpdateClass.xaml
    /// </summary>
    public partial class UpdateClass : Window
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


        public UpdateClass()
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

        private void loadClassCombobox()
        {
            using (sqlCon = new SqlConnection(bang))
            {
                sqlCon.Open();
                sqlCom = new SqlCommand("SELECT LOP_ID FROM LOP WHERE KHOI_ID = (SELECT KHOI_ID FROM KHOI WHERE TENKHOI = @TenKhoi)", sqlCon);
                sqlCom.Parameters.AddWithValue("@TenKhoi", GradeCbb.SelectedItem.ToString());
                using (SqlDataReader reader = sqlCom.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string lopIdValue = reader["LOP_ID"].ToString();
                        // Thêm giá trị vào ComboBox
                        ClassCbb.Items.Add(lopIdValue);
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
            loadClassCombobox();
        }

        private void ClassCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (sqlCon = new SqlConnection(bang))
            {
                sqlCon.Open();
                sqlCom = new SqlCommand("SELECT TENLOP FROM LOP WHERE LOP_ID = @LopID", sqlCon);
                sqlCom.Parameters.AddWithValue("@LopID", ClassCbb.SelectedItem.ToString());
                using (SqlDataReader reader = sqlCom.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        txtClassname.Text = reader["TENLOP"].ToString();
                    }
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string className = txtClassname.Text;
            string classId = ClassCbb.SelectedItem.ToString();
            if (className == null || classId == null)
            {
                MessageBox.Show("EROR!");
            }
            else
            {
                SqlConnection sqlConString = new SqlConnection(bang);
                sqlConString.Open();
                string sql2 = "UPDATE LOP SET TENLOP = @Tenlop " +
                    "WHERE LOP_ID = @LopID";
                SqlCommand sql = new SqlCommand(sql2, sqlConString);
                sql.Parameters.AddWithValue("@Tenlop", className);
                sql.Parameters.AddWithValue("@LopID", classId);
                sql.ExecuteNonQuery();
                sqlConString.Close();
                MessageBox.Show("Update class name successfully!");
                this.Close();
            }
            
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string classId = ClassCbb.SelectedItem.ToString();
            if (classId == null)
            {
                MessageBox.Show("EROR!");
            }
            else
            {
                SqlConnection sqlConString = new SqlConnection(bang);
                sqlConString.Open();
                string sql2 = "UPDATE HOCSINH SET LOP_ID = NULL WHERE LOP_ID = @ClassId " +
                    " DELETE FROM LOP WHERE LOP_ID = @ClassId";
                SqlCommand sql = new SqlCommand(sql2, sqlConString);
                sql.Parameters.AddWithValue("@ClassId", classId);

                MessageBoxResult result = MessageBox.Show("Are you sure want to delete?",
                "Yes", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    sql.ExecuteNonQuery();
                    sqlConString.Close();
                    MessageBox.Show("All the students belong to this class have been removed from this class! Delete class successfully!");
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
