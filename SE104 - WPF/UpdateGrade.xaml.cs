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
    /// Interaction logic for UpdateGrade.xaml
    /// </summary>
    public partial class UpdateGrade : Window
    {
        string bang = "Data Source=LAPTOP-9Q9UCI39;Initial Catalog=SE104;Integrated Security=True";
        SqlConnection sqlCon;
        SqlCommand sqlCom;
        SqlDataReader read;
        SqlDataAdapter sqlDa;
        DataTable dtbClass;
        DataTable dtbStudent;

        String boxHocky;
        String boxIDHS;
        String boxMonhoc;
        private void CenterWindowOnScreen()
        {
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2);
        }
        public UpdateGrade(String txtboxHocky,
            String txtboxIDHS,
            String txtboxMonhoc,
            String txtboxDiem15,
            String txtboxDime45,
            String txtboxDiemCK)
        {
            InitializeComponent();
            CenterWindowOnScreen();
            boxHocky = txtboxHocky;
            txtHK.Text = txtboxHocky;
            boxIDHS = txtboxIDHS;
            txtHS.Text = txtboxIDHS;
            boxMonhoc = txtboxMonhoc;
            txtMH.Text = txtboxMonhoc;
            txt15.Text = txtboxDiem15;
            txt45.Text = txtboxDime45;
            txtCK.Text = txtboxDiemCK;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            float txtDiem15 = float.Parse(txt15.Text);
            float txtDiem45 = float.Parse(txt45.Text);
            float txtDiemCK = float.Parse(txtCK.Text);

            SqlConnection sqlConString = new SqlConnection(bang);
            sqlConString.Open();


            string sql2 = "UPDATE BANGDIEM SET DIEM15 = @Diem15, DIEM45 = @Diem45, DIEMHK = @DiemCK " +
                "WHERE HS_ID = @HSID " +
                "AND HK_ID IN (SELECT HK_ID FROM HOCKY WHERE TENHK = @HKID) " +
                "AND MH_ID IN (SELECT MH_ID FROM MONHOC WHERE TENMH = @MHID)";

            SqlCommand sql = new SqlCommand(sql2, sqlConString);
            sql.Parameters.AddWithValue("@Diem15", txtDiem15);
            sql.Parameters.AddWithValue("@Diem45", txtDiem45);
            sql.Parameters.AddWithValue("@DiemCK", txtDiemCK);
            sql.Parameters.AddWithValue("@HKID", boxHocky);
            sql.Parameters.AddWithValue("@MHID", boxMonhoc);
            sql.Parameters.AddWithValue("@HSID", boxIDHS);
            sql.ExecuteNonQuery();
            sqlConString.Close();

            this.Close();
        }
    }
}
