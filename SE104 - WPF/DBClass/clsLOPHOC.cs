using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SE104___WPF.DBClass
{
    class clsLOPHOC
    {
        public string hs_id { get; set; }
        public string tenhs { get; set; }
        public DateTime ngsinh { get; set; }
        public string diachi { get; set; }
        public string email { get; set; }
        public int gioitinh { get; set; }

        DBConnect dBConnect = new DBConnect();
        string bang = "Data Source=LAPTOP-9Q9UCI39;Initial Catalog=SE104;Integrated Security=True";
        SqlConnection sqlCon;
        SqlCommand sqlCom;
        SqlDataReader read;
        SqlDataAdapter sqlDa;
        DataTable dtbStudent;

        static int countID = 6;


        public int themHocSinh(clsHOCSINH hs)
        {
            try
            {
                SqlConnection sqlConString = new SqlConnection(bang);
                sqlConString.Open();
                SqlCommand sql = new SqlCommand("INSERT INTO HOCSINH VALUES('" + countID++ + "', N'" + hs.tenhs + "', '" + hs.ngsinh + "', '" + hs.diachi + "', '" + hs.email + "','" + hs.gioitinh + "' )", sqlConString);
                sql.ExecuteNonQuery();
                sqlConString.Close();
                MessageBox.Show("Add successfully");
                return 1;
            }
            catch
            {
                MessageBox.Show("Add failed");
                return 0;
            }
        }

        public int suaHocSinh(clsHOCSINH hs)
        {
            try
            {
                SqlConnection sqlConString = new SqlConnection(bang);
                sqlConString.Open();
                SqlCommand sql = new SqlCommand("UPDATE HOCSINH SET TENHS = @TenHS, NGSINH = @NgaySinh, DIACHI = @DiaChi, EMAIL = @Email, GIOITINH = @GioiTinh WHERE HS_ID = @SelectedID", sqlConString);
                sql.Parameters.AddWithValue("@TenHS", hs.tenhs);
                sql.Parameters.AddWithValue("@NgaySinh", hs.ngsinh);
                sql.Parameters.AddWithValue("@DiaChi", hs.diachi);
                sql.Parameters.AddWithValue("@Email", hs.email);
                sql.Parameters.AddWithValue("@GioiTinh", hs.gioitinh);
                sql.Parameters.AddWithValue("@SelectedID", hs.hs_id);
                sql.ExecuteNonQuery();
                sqlConString.Close();
                MessageBox.Show("Update successfully");
                return 1;
            }
            catch
            {
                MessageBox.Show("Update failed");
                return 0;
            }
        }

        public int xoaHocSinh(clsHOCSINH hs)
        {
            try
            {
                SqlConnection sqlConString = new SqlConnection(bang);
                sqlConString.Open();
                SqlCommand sql = new SqlCommand("DELETE FROM HOCSINH WHERE HS_ID = '" + hs.hs_id + "'", sqlConString);
                sql.ExecuteNonQuery();
                sqlConString.Close();
                MessageBox.Show("Delete successfully");
                return 1;
            }
            catch
            {
                MessageBox.Show("Delete failed");
                return 0;
            }
        }
    }
}
