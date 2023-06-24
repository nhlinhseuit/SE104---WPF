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

namespace SE104___WPF
{
    /// <summary>
    /// Interaction logic for CC_Report.xaml
    /// </summary>
    public partial class CC_Report : UserControl
    {
        public CC_Report()
        {
            InitializeComponent();
            loadTypeCombobox();
        }

        string bang = "Data Source=LAPTOP-9Q9UCI39;Initial Catalog=SE104;Integrated Security=True";
        SqlConnection sqlCon;
        SqlCommand sqlCom;
        SqlDataReader read;
        SqlDataAdapter sqlDa;
        DataTable dtbClass;
        DataTable dtbStudent;
         
        private int getNumberOfClass()
        {
            int count;
            using (SqlConnection connection = new SqlConnection(bang))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM LOP", connection);
                count = (int)command.ExecuteScalar();
                connection.Close();
                return count;
            }
        }

        private List<string> getListOfClass()
        {
            List<string> listClass = new List<string>(); ;
            using (SqlConnection connection = new SqlConnection(bang))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("SELECT TENLOP FROM LOP", connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string tenLop = reader.GetString(0);
                    listClass.Add(tenLop);
                }
                reader.Close();
            }
            return listClass;
        }

        private List<int> getListOfTotalStudents(List<string> listClass)
        {
            List<int> totalStudents = new List<int>();

            using (SqlConnection connection = new SqlConnection(bang))
            {
                connection.Open();

                foreach (string tenLop in listClass)
                {
                    SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM HOCSINH WHERE LOP_ID IN (SELECT LOP_ID FROM LOP WHERE TENLOP = @TenLop)", connection);
                    command.Parameters.AddWithValue("@TenLop", tenLop);
                    int count = (int)command.ExecuteScalar();
                    totalStudents.Add(count);
                }

            }
            return totalStudents;

        }


        // TUNG MON
        private List<int> getListOfStudentPassSubject(List<string> listClass, String slectedHK, String slectedMH)
        {
            List<int> listPassedStudents = new List<int>();

            using (SqlConnection connection = new SqlConnection(bang))
            {
                connection.Open();

                foreach (string tenLop in listClass)
                {
                    SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM HOCSINH" +
                        " INNER JOIN BANGDIEM ON HOCSINH.HS_ID = BANGDIEM.HS_ID" +
                        " WHERE BANGDIEM.DIEMTB >= (SELECT DIEMQUAMON FROM QUYDINH)" +
                        " AND HOCSINH.LOP_ID IN (SELECT LOP_ID FROM LOP WHERE TENLOP = @TenLop)" +
                        " AND BANGDIEM.HK_ID IN (SELECT HK_ID FROM HOCKY WHERE TENHK = @TenHK)" +
                        " AND BANGDIEM.MH_ID IN (SELECT MH_ID FROM MONHOC WHERE TENMH = @TenMH)", connection);
                    command.Parameters.AddWithValue("@TenLop", tenLop);
                    command.Parameters.AddWithValue("@TenHK", slectedHK);
                    command.Parameters.AddWithValue("@TenMH", slectedMH);
                    int count = (int)command.ExecuteScalar();
                    listPassedStudents.Add(count);
                }
            }
            return listPassedStudents;

        }

        private List<double> getListOfPassSubjectRate(List<int> listOfTotalStudents, List<int> listOfStudentPassSubject)
        {
            List<double> listPassRate = new List<double>();

            for (int i = 0; i < listOfTotalStudents.Count; i++)
            {
                int passed = listOfStudentPassSubject[i];
                int total = listOfTotalStudents[i];
                double passRate = (double)passed / total * 100;
                double roundedPassRate = Math.Round(passRate, 2);
                listPassRate.Add(roundedPassRate);
            }
            return listPassRate;

        }


        // CA HOC KY
        private List<int> getListOfStudentPass(List<string> listClass, String slectedHK)
        {
            List<int> listPassedStudents = new List<int>();

            using (SqlConnection connection = new SqlConnection(bang))
            {
                connection.Open();

                foreach (string tenLop in listClass)
                {
                    SqlCommand command = new SqlCommand(
                        "SELECT COUNT(*) AS SoLuongHocSinh" +
                        " FROM (" +
                        "     SELECT HOCSINH.HS_ID, AVG(BANGDIEM.DIEMTB) AS DIEMTB_TRUNGBINH" +
                        "     FROM HOCSINH" +
                        "     INNER JOIN BANGDIEM ON HOCSINH.HS_ID = BANGDIEM.HS_ID" +
                        "     WHERE HOCSINH.LOP_ID IN (SELECT LOP_ID FROM LOP WHERE TENLOP = @TenLop)" +
                        "     AND BANGDIEM.HK_ID IN (SELECT HK_ID FROM HOCKY WHERE TENHK = @TenHK)" +
                        "     GROUP BY HOCSINH.HS_ID" +
                        " ) AS HocSinh" +
                        " WHERE DIEMTB_TRUNGBINH > (SELECT DIEMQUAMON FROM QUYDINH)",
                        connection);
                    command.Parameters.AddWithValue("@TenLop", tenLop);
                    command.Parameters.AddWithValue("@TenHK", slectedHK);
                    int count = (int)command.ExecuteScalar();
                    listPassedStudents.Add(count);
                }
            }
            return listPassedStudents;

        }

        private List<double> getListOfPassRate(List<int> listOfTotalStudents, List<int> listOfStudentPassSubject)
        {
            List<double> listPassRate = new List<double>();

            for (int i = 0; i < listOfTotalStudents.Count; i++)
            {
                int passed = listOfStudentPassSubject[i];
                int total = listOfTotalStudents[i];
                double passRate = (double)passed / total * 100;
                double roundedPassRate = Math.Round(passRate, 2);
                listPassRate.Add(roundedPassRate);
            }
            return listPassRate;

        }

        private void loadThongTinTKMon()
        {
            String slectedHK = SemesterCombobox.SelectedItem.ToString();
            int numberOfClass = getNumberOfClass();
            // list STT
            List<int> listRowNumbers = new List<int>();
            for (int i = 1; i <= numberOfClass; i++)
            {
                listRowNumbers.Add(i);
            }

            // list class
            List<string> listOfClass = getListOfClass();

            // list totalStudents
            List<int> listOfTotalStudents = getListOfTotalStudents(listOfClass);

            // list studentPassSubject
            List<int> listOfStudentPass = getListOfStudentPass(listOfClass, slectedHK);

            // list studentPassRate
            List<double> listOfPassRate= getListOfPassSubjectRate(listOfTotalStudents, listOfStudentPass);


            // LOAD THONG TIN LEN DATAGRID
            DataTable dataTable = new DataTable();

            // Thêm các cột vào DataTable
            dataTable.Columns.Add("No", typeof(int));
            dataTable.Columns.Add("Class", typeof(string));
            dataTable.Columns.Add("Total students", typeof(int));
            dataTable.Columns.Add("Total students pass subject", typeof(int));
            dataTable.Columns.Add("Pass subject rate (%)", typeof(double));

            // Đổ dữ liệu từ các danh sách vào DataTable
            for (int i = 0; i < listRowNumbers.Count; i++)
            {
                int stt = listRowNumbers[i];
                string lop = listOfClass[i];
                int totalStudents = listOfTotalStudents[i];
                int studentPass = listOfStudentPass[i];
                double passRate = listOfPassRate[i];

                // Tạo DataRow và thêm vào DataTable
                DataRow row = dataTable.NewRow();
                row["No"] = stt;
                row["Class"] = lop;
                row["Total students"] = totalStudents;
                row["Total students pass subject"] = studentPass;
                row["Pass subject rate (%)"] = passRate;
                dataTable.Rows.Add(row);
            }

            // Sử dụng DataTable làm dữ liệu cho DataGridView
            dataGridViewReport.ItemsSource = dataTable.DefaultView;

        }

        private void loadThongTinTKHocKy()
        {
            String slectedHK = SemesterCombobox.SelectedItem.ToString();
            int numberOfClass = getNumberOfClass();
            // list STT
            List<int> listRowNumbers = new List<int>();
            for (int i = 1; i <= numberOfClass; i++)
            {
                listRowNumbers.Add(i);
            }

            // list class
            List<string> listOfClass = getListOfClass();

            // list totalStudents
            List<int> listOfTotalStudents = getListOfTotalStudents(listOfClass);

            // list studentPassSubject
            List<int> listOfStudentPassSubject = getListOfStudentPass(listOfClass, slectedHK);

            // list studentPassSubject
            List<double> listOfPassRate = getListOfPassRate(listOfTotalStudents, listOfStudentPassSubject);


            // LOAD THONG TIN LEN DATAGRID
            DataTable dataTable = new DataTable();

            // Thêm các cột vào DataTable
            dataTable.Columns.Add("No", typeof(int));
            dataTable.Columns.Add("Class", typeof(string));
            dataTable.Columns.Add("Total students", typeof(int));
            dataTable.Columns.Add("Total students pass subject", typeof(int));
            dataTable.Columns.Add("Pass subject rate", typeof(double));

            // Đổ dữ liệu từ các danh sách vào DataTable
            for (int i = 0; i < listRowNumbers.Count; i++)
            {
                int stt = listRowNumbers[i];
                string lop = listOfClass[i];
                int totalStudents = listOfTotalStudents[i];
                int studentPassSubject = listOfStudentPassSubject[i];
                double passRate = listOfPassRate[i];

                // Tạo DataRow và thêm vào DataTable
                DataRow row = dataTable.NewRow();
                row["No"] = stt;
                row["Class"] = lop;
                row["Total students"] = totalStudents;
                row["Total students pass subject"] = studentPassSubject;
                row["Pass subject rate"] = passRate;
                dataTable.Rows.Add(row);
            }

            // Sử dụng DataTable làm dữ liệu cho DataGridView
            dataGridViewReport.ItemsSource = dataTable.DefaultView;
        }



        private void loadTypeCombobox()
        {
            string type1 = "Báo cáo tổng kết môn";
            string type2 = "Báo cáo tổng kết học kỳ";
            TypeCombobox.Items.Add(type1);
            TypeCombobox.Items.Add(type2);
        }

        private void loadSubjectCombobox()
        {
            if (TypeCombobox.SelectedItem.ToString() == "Báo cáo tổng kết học kỳ")
            {
                SubjectCombobox.IsEnabled = false;
            }
            else
            {
                if (SemesterCombobox.SelectedItem != null)
                {
                    SubjectCombobox.SelectedItem = null;
                    SubjectCombobox.Items.Clear();
                }
                SubjectCombobox.IsEnabled = true;
                using (sqlCon = new SqlConnection(bang))
                {
                    sqlCon.Open();
                    sqlCom = new SqlCommand("SELECT TENMH FROM MONHOC", sqlCon);
                    using (SqlDataReader reader = sqlCom.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string monhocValue = reader["TENMH"].ToString();
                            SubjectCombobox.Items.Add(monhocValue);
                        }
                    }
                }
            }
        }

        private void loadSemesterCombobox()
        {
            if (TypeCombobox.SelectedItem != null)
            {
                if (SemesterCombobox.SelectedItem != null)
                {
                    SemesterCombobox.SelectedItem = null;
                    SemesterCombobox.Items.Clear();

                    dataGridViewReport.ItemsSource = null;
                    dataGridViewReport.ItemsSource = new List<object>();

                }
                using (sqlCon = new SqlConnection(bang))
                {
                    sqlCon.Open();
                    sqlCom = new SqlCommand("SELECT TENHK FROM HOCKY", sqlCon);
                    using (SqlDataReader reader = sqlCom.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string hockyValue = reader["TENHK"].ToString();
                            SemesterCombobox.Items.Add(hockyValue);
                        }
                    }
                }
            }
        }

        private DataGridRow previousSelectedRow = null;
        private void dataGridViewGrade_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DataGridRow row = FindVisualParent<DataGridRow>(e.OriginalSource as UIElement);
                if (row != null)
                {
                    if (previousSelectedRow != null && previousSelectedRow != row)
                    {
                        previousSelectedRow.IsSelected = false;
                    }

                    row.IsSelected = true;
                    previousSelectedRow = row;

                    e.Handled = true;
                }
            }
        }

        private T FindVisualParent<T>(UIElement element) where T : UIElement
        {
            UIElement parent = element;
            while (parent != null)
            {
                if (parent is T correctlyTyped)
                {
                    return correctlyTyped;
                }

                parent = VisualTreeHelper.GetParent(parent) as UIElement;
            }

            return null;
        }

        private void dataGridViewClass_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TypeCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
                loadSubjectCombobox();
                loadSemesterCombobox();
        }

        private void SubjectCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void SemesterCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SubjectCombobox.IsEnabled == true)
            {
                if (SemesterCombobox.SelectedItem == null)
                    return;
                loadThongTinTKMon();
            }
            else
            {
                if (SemesterCombobox.SelectedItem == null)
                    return;
                loadThongTinTKHocKy();
            }   

        }
    }
}
