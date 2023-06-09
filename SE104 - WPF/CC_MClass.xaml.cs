using SE104___WPF.DBClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
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
    /// Interaction logic for CC_MClass.xaml
    /// </summary>
    public partial class CC_MClass : UserControl
    {
        public CC_MClass()
        {
            InitializeComponent();
            loadGradeCombobox();

        }
        string bang = "Data Source=LAPTOP-9Q9UCI39;Initial Catalog=SE104;Integrated Security=True";
        SqlConnection sqlCon;
        SqlCommand sqlCom;
        SqlDataReader read;
        SqlDataAdapter sqlDa;
        DataTable dtbClass;
        DataTable dtbStudent;

        String selectedID;
        clsHOCSINH hs = new clsHOCSINH();

        public class Class
        {
            public string ID { get; set; }
            public string Fullname { get; set; }
            public DateTime Birthday { get; set; }
            public string Address { get; set; }
            public string Email { get; set; }
            public string Gender { get; set; }

            // Các trường dữ liệu khác
        }

        Class selectedStudent;


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
                        GradeCombobox.Items.Add(khoiValue);
                    }
                }
            }
        }

  

        private void loadTotalAndGriddata()
        {
            // total
            
            if (ClassCombobox.SelectedItem != null)
            {
                string sql = "SELECT COUNT(HOCSINH.LOP_ID) AS TotalStudents " +
                    "FROM HOCSINH " +
                    "JOIN LOP ON HOCSINH.LOP_ID = LOP.LOP_ID " +
                    "WHERE LOP.TENLOP = @Tenlop";
                string selectedTenLop = ClassCombobox.SelectedItem.ToString(); // Giả sử selectedKhoiID lưu trữ KHOI_ID được chọn từ ComboBox
                using (SqlConnection connection = new SqlConnection(bang))
                {
                    // load total
                    connection.Open();
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@Tenlop", selectedTenLop);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // them gia tri lop vao combobox
                        if (reader.Read())
                        {
                            string lopValue = reader["TotalStudents"].ToString();
                            // Thực hiện các hoạt động với giá trị TENLOP lấy được
                            txtTotal.Text = lopValue;
                        }

                    }

                    //LOAD THÔNG TIN MẶC ĐỊNH TỪ DATAGRID
                    string sql2 = "SELECT HS_ID, TENHS, NGSINH, DIACHI, EMAIL, GIOITINH " +
                            "FROM HOCSINH " +
                            "WHERE LOP_ID IN (SELECT LOP_ID FROM LOP WHERE TENLOP = '" + selectedTenLop + "')";

                    using (sqlCon = new SqlConnection(bang))
                    {
                        sqlCon.Open();
                        sqlDa = new SqlDataAdapter(sql2, sqlCon);
                        dtbStudent = new DataTable();
                        sqlDa.Fill(dtbStudent);
                        sqlCon.Close();
                        dataGridViewClass.ItemsSource = dtbStudent.DefaultView;
                    }
                }
            }
            else
            {
                txtTotal.Text = "";
                return;
            }
                

            //griddata
        }

        private void loadClassCombobox()
        {
            if (GradeCombobox.SelectedItem != null)
            {
                string selectedKhoiID = GradeCombobox.SelectedItem.ToString(); // Giả sử selectedKhoiID lưu trữ KHOI_ID được chọn từ ComboBox

                if (ClassCombobox.SelectedItem != null)
                {
                    ClassCombobox.SelectedItem = null;
                    ClassCombobox.Items.Clear();

                    dataGridViewClass.ItemsSource = null;
                    dataGridViewClass.ItemsSource = new List<object>();

                }

                using (SqlConnection connection = new SqlConnection(bang))
                {
                    connection.Open();
                    string sql = "SELECT LOP.TENLOP " +
                        "FROM LOP " +
                        "JOIN KHOI ON LOP.KHOI_ID = KHOI.KHOI_ID " +
                        "WHERE KHOI.TENKHOI = @KhoiID";
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@KhoiID", selectedKhoiID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // them gia tri lop vao combobox
                        while (reader.Read())
                        {
                            string lopValue = reader["TENLOP"].ToString();
                            // Thực hiện các hoạt động với giá trị TENLOP lấy được
                            ClassCombobox.Items.Add(lopValue);
                        }

                    }
                }
            }
            else
            {
                MessageBox.Show("You have to choose the grade first!");
            }
        }

        private void loadThongTin()
        {
            string selectedTenLop = ClassCombobox.SelectedItem.ToString(); // Giả sử selectedKhoiID lưu trữ KHOI_ID được chọn từ ComboBox
            string bang = "Data Source=LAPTOP-9Q9UCI39;Initial Catalog=SE104;Integrated Security=True";
            // LOAD THÔNG TIN CUSTOM THEO CLASS STUDENT
            string sql2 = "SELECT HS_ID, TENHS, NGSINH, DIACHI, EMAIL, GIOITINH " +
                            "FROM HOCSINH " +
                            "WHERE LOP_ID IN (SELECT LOP_ID FROM LOP WHERE TENLOP = '" + selectedTenLop + "')";

            //using (sqlCon = new SqlConnection(bang))
            //{
            //    sqlCon.Open();
            //    sqlDa = new SqlDataAdapter(sql2, sqlCon);
            //    dtbStudent = new DataTable();
            //    sqlDa.Fill(dtbStudent);
            //    sqlCon.Close();
            //    dataGridViewClass.ItemsSource = dtbStudent.DefaultView;
            //}
            using (SqlConnection sqlCon = new SqlConnection(bang))
            {
                sqlCon.Open();
                using (SqlCommand cmd = new SqlCommand(sql2, sqlCon))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Class> students = new List<Class>();
                        while (reader.Read())
                        {
                            Class student = new Class();
                            student.ID = reader["HS_ID"].ToString();
                            student.Fullname = reader["TENHS"].ToString();
                            student.Birthday = Convert.ToDateTime(reader["NGSINH"]);
                            student.Address = reader["DIACHI"].ToString();
                            student.Email = reader["EMAIL"].ToString();
                            //student.Gender = reader["GIOITINH"].ToString();
                            bool gioiTinhBit = Convert.ToBoolean(reader["GIOITINH"]);
                            student.Gender = gioiTinhBit ? "Male" : "Female";
                            students.Add(student);
                        }

                        // Gán danh sách students làm nguồn dữ liệu cho DataGrid
                        dataGridViewClass.ItemsSource = students;
                    }
                }
            }


        }

        private bool isClassMaxStudent()
        {
            int maxStudentOfClass = 0;
            int totalStudentOfClass = 0;
            using (SqlConnection connection = new SqlConnection(bang))
            {
                connection.Open();

                string query = "SELECT SLMAX FROM QUYDINH";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        maxStudentOfClass = Convert.ToInt32(reader["SLMAX"]);
                    }

                    reader.Close();
                }

                connection.Close();
            }
            //using (SqlConnection connection = new SqlConnection(bang))
            //{
            //    connection.Open();
            //    SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM HOCSINH JOIN LOP ON HOCSINH.LOP_ID = LOP.LOP_ID WHERE LOP.LOP_ID IN (SELECT LOP_ID FROM LOP WHERE TENLOP = @TENLOP)", connection);
            //    command.Parameters.AddWithValue("@TENLOP", ClassCombobox.SelectedItem.ToString());

            //    totalStudentOfClass = (int)command.ExecuteScalar();
            //    connection.Close();
            //}
            totalStudentOfClass = loadTotalAgain();
            return totalStudentOfClass >= maxStudentOfClass;
        }

        private int loadTotalAgain()
        {
            int total = 0;
            string selectedTenLop = ClassCombobox.SelectedItem.ToString();

            using (SqlConnection connection = new SqlConnection(bang))
            {
                connection.Open();

                string sql2 = "SELECT COUNT(HOCSINH.LOP_ID) AS TotalStudents " +
                    "FROM HOCSINH " +
                    "JOIN LOP ON HOCSINH.LOP_ID = LOP.LOP_ID " +
                    "WHERE LOP.TENLOP = @Tenlopchon";
                SqlCommand command = new SqlCommand(sql2, connection);
                command.Parameters.AddWithValue("@Tenlopchon", selectedTenLop);

                using (SqlDataReader reader2 = command.ExecuteReader())
                {
                    // them gia tri lop vao combobox
                    if (reader2.Read())
                    {
                        string lopValue = reader2["TotalStudents"].ToString();
                        // Thực hiện các hoạt động với giá trị TENLOP lấy được
                        txtTotal.Text = lopValue;
                        total = Convert.ToInt32(lopValue);
                    }

                }
            }
            return total;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
            if (ClassCombobox.SelectedIndex == -1)
            {
                MessageBox.Show("Have to choose a class to add student!");
                return;
            }
            if (isClassMaxStudent())
            {
                MessageBox.Show("Max students in class!!");
                return;
            }

            int LopID;

            using (SqlConnection connection = new SqlConnection(bang))
            {
                connection.Open();

                string sql = "SELECT LOP.LOP_ID FROM LOP WHERE LOP.TENLOP = @Tenlop";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Tenlop", ClassCombobox.SelectedItem.ToString());

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    // them gia tri lop vao combobox
                    while (reader.Read())
                    {
                        LopID = Convert.ToInt32(reader["LOP_ID"]);
                        AddStudentToClass addStudentToClass = new AddStudentToClass(LopID);
                        addStudentToClass.Show();
                        //LOAD LẠI THÔNG TIN
                        addStudentToClass.Closed += (sender, args) =>
                        {
                            loadTotalAgain();
                            loadThongTin();
                        };
                        return; 
                    }
                }

            }
            }

        private void GradeCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            loadClassCombobox();
        }

        private void ClassCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            loadTotalAndGriddata();
        }



        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string selectedTenLop = ClassCombobox.SelectedItem.ToString(); // Giả sử selectedKhoiID lưu trữ KHOI_ID được chọn từ ComboBox
            MessageBoxResult result = MessageBox.Show("Are you sure want to delete?",
            "Yes", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Thực hiện xóa đối tượng
                hs.hs_id = selectedID;
                hs.xoaHocSinhKhoiLop(hs);

                using (SqlConnection connection = new SqlConnection(bang))
                {
                    // load total
                    connection.Open();
                    //LOAD THÔNG TIN MẶC ĐỊNH TỪ DATAGRID
                    string sql2 = "SELECT HS_ID, TENHS, NGSINH, DIACHI, EMAIL, GIOITINH " +
                            "FROM HOCSINH " +
                            "WHERE LOP_ID IN (SELECT LOP_ID FROM LOP WHERE TENLOP = '" + selectedTenLop + "')";
                    SqlCommand command = new SqlCommand(sql2, connection);
                    using (sqlCon = new SqlConnection(bang))
                    {
                        sqlCon.Open();
                        sqlDa = new SqlDataAdapter(sql2, sqlCon);
                        dtbStudent = new DataTable();
                        sqlDa.Fill(dtbStudent);
                        sqlCon.Close();
                        dataGridViewClass.ItemsSource = dtbStudent.DefaultView;
                    }
                }
                loadTotalAgain();
            }
            else
            {
                return;
            }
        }

        private DataGridRow previousSelectedRow = null;
        private void dataGridViewClass_PreviewMouseDown(object sender, MouseButtonEventArgs e)
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
            if (dataGridViewClass.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)dataGridViewClass.SelectedItem;

                selectedID = selectedRow["HS_ID"].ToString();


            }
        }
    }
}
