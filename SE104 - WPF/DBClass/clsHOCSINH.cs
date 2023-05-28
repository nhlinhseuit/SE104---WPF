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
    class clsHOCSINH
    {
        public int hs_id { get; set; }
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

        int countID = 5;


        public int themHocSinh(clsHOCSINH hs)
        {
            try
            {
                SqlConnection sqlConString = new SqlConnection(bang);
                sqlConString.Open();
                SqlCommand sql = new SqlCommand("INSERT INTO HOCSINH VALUES('" + ++countID + "', '" + hs.tenhs + "', '" + hs.ngsinh + "', '" + hs.diachi + "', '" + hs.email + "','" + hs.gioitinh + "' )", sqlConString);
                sql.ExecuteNonQuery();
                sqlConString.Close();
                MessageBox.Show("Them thanh cong");
                return 1;
            }
            catch
            {
                MessageBox.Show("Them bi loi");
                return 0;
            }
        }
    }

    
}
