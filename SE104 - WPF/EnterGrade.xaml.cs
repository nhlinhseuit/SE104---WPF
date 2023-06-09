using SE104___WPF.DBClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
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
    /// Interaction logic for EnterGrade.xaml
    /// </summary>
    public partial class EnterGrade : Window
    {
        string selectedClass;
        private void CenterWindowOnScreen()
        {
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2);
        }
        public EnterGrade(String selectedCbbClass)
        {
            selectedClass = selectedCbbClass;
            InitializeComponent();
            CenterWindowOnScreen();
            loadIDCombobox();
            loadSemesterCombobox();
            loadSubjectCombobox();
        }

        string bang = "Data Source=LAPTOP-9Q9UCI39;Initial Catalog=SE104;Integrated Security=True";
        SqlConnection sqlCon;
        SqlCommand sqlCom;
        SqlDataReader read;
        SqlDataAdapter sqlDa;
        DataTable dtbClass;
        DataTable dtbStudent;

        clsHOCSINH hs = new clsHOCSINH();

        private void loadSemesterCombobox()
        {
                using (SqlConnection connection = new SqlConnection(bang))
                {
                    connection.Open();
                    string sql = "SELECT TENHK FROM HOCKY";
                    SqlCommand command = new SqlCommand(sql, connection);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // them gia tri lop vao combobox
                        while (reader.Read())
                        {
                            string lopValue = reader["TENHK"].ToString();
                            // Thực hiện các hoạt động với giá trị TENLOP lấy được
                            SemesterCombobox.Items.Add(lopValue);
                        }

                    }
                }
        }

        private void loadIDCombobox()
        {
            if (selectedClass == null)
            {
                MessageBox.Show("Choose class first");
                return;
            }
               
            using (SqlConnection connection = new SqlConnection(bang))
            {
                connection.Open();
                string txt = selectedClass;
                string sql = "SELECT HOCSINH.TENHS FROM HOCSINH JOIN LOP ON HOCSINH.LOP_ID = LOP.LOP_ID WHERE LOP.LOP_ID IN " +
                    "(SELECT LOP_ID FROM LOP WHERE TENLOP = '" + selectedClass + "')";


                SqlCommand command = new SqlCommand(sql, connection);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    // them gia tri lop vao combobox
                    while (reader.Read())
                    {
                        string lopValue = reader["TENHS"].ToString();
                        // Thực hiện các hoạt động với giá trị TENLOP lấy được
                        IDCombobox.Items.Add(lopValue);
                    }

                }
            }
        }

        private void loadSubjectCombobox()
        {
                using (SqlConnection connection = new SqlConnection(bang))
                {
                    connection.Open();
                    string sql = "SELECT TENMH FROM MONHOC";
                    SqlCommand command = new SqlCommand(sql, connection);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // them gia tri lop vao combobox
                        while (reader.Read())
                        {
                            string lopValue = reader["TENMH"].ToString();
                            // Thực hiện các hoạt động với giá trị TENLOP lấy được
                            SubjectCombobox.Items.Add(lopValue);
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
            String selectedSemesterString = SemesterCombobox.SelectedItem.ToString();
            String selectedIDString = IDCombobox.SelectedItem.ToString();
            String selectedSubjectString = SubjectCombobox.SelectedItem.ToString();
            float txtDiem15 = float.Parse(txt15.Text);
            float txtDiem45 = float.Parse(txt45.Text);
            float txtDiemCK = float.Parse(txtCK.Text);

            SqlConnection sqlConString = new SqlConnection(bang);
            sqlConString.Open();


            String sql2 = "MERGE INTO BANGDIEM AS target " +
                "USING (" +
                "    VALUES (" +
                "        (SELECT HK_ID FROM HOCKY WHERE TENHK = @SelectedSemester)," +
                "        (SELECT HS_ID FROM HOCSINH WHERE TENHS = @SelectedID)," +
                "        (SELECT MH_ID FROM MONHOC WHERE TENMH = @SelectedSubject)," +
                "        @Diem15," +
                "        @Diem45," +
                "        @DiemHK" +
                "    )" +
                ") AS source (HK_ID, HS_ID, MH_ID, DIEM15, DIEM45, DIEMHK)" +
                "ON (target.HK_ID = source.HK_ID AND target.HS_ID = source.HS_ID AND target.MH_ID = source.MH_ID) " +
                "WHEN MATCHED THEN" +
                "    UPDATE SET" +
                "        DIEM15 = source.DIEM15," +
                "        DIEM45 = source.DIEM45," +
                "        DIEMHK = source.DIEMHK, " +
                "        DIEMTB = ROUND((source.DIEM15 + source.DIEM45*2 + source.DIEMHK*3)/6, 2) " +
                "WHEN NOT MATCHED THEN" +
                "    INSERT (HK_ID, HS_ID, MH_ID, DIEM15, DIEM45, DIEMHK, DIEMTB) " +
                "    VALUES (source.HK_ID, source.HS_ID, source.MH_ID, source.DIEM15, source.DIEM45, source.DIEMHK, ROUND((source.DIEM15 + source.DIEM45*2 + source.DIEMHK*3)/6, 2));";
            SqlCommand sql = new SqlCommand(sql2, sqlConString);
            sql.Parameters.AddWithValue("@Diem15", txtDiem15);
            sql.Parameters.AddWithValue("@Diem45", txtDiem45);
            sql.Parameters.AddWithValue("@DiemHK", txtDiemCK);
            sql.Parameters.AddWithValue("@SelectedSemester", selectedSemesterString);
            sql.Parameters.AddWithValue("@SelectedSubject", selectedSubjectString);
            sql.Parameters.AddWithValue("@SelectedID", selectedIDString);
            sql.ExecuteNonQuery();

            String sql3 = "UPDATE BANGDIEM SET DIEMTB = ROUND((@Diem15 + @Diem45*2 + @DiemHK*3)/6, 2) " +
                "WHERE HS_ID = (SELECT MAX(HS_ID) FROM HOCSINH)";
            SqlCommand sqla = new SqlCommand(sql3, sqlConString);
            sqla.Parameters.AddWithValue("@Diem15", txtDiem15);
            sqla.Parameters.AddWithValue("@Diem45", txtDiem45);
            sqla.Parameters.AddWithValue("@DiemHK", txtDiemCK);
            sqla.ExecuteNonQuery();
            sqlConString.Close();

            this.Close();
        }
    }
}
