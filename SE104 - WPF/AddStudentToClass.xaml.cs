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
using System.Xml.Linq;
using static Azure.Core.HttpHeader;
using static SE104___WPF.CC_MStudent;

namespace SE104___WPF
{
    /// <summary>
    /// Interaction logic for AddStudentToClass.xaml
    /// </summary>
    public partial class AddStudentToClass : Window
    {
        private int selectedLopID;
        private void CenterWindowOnScreen()
        {
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2);
        }
        public AddStudentToClass(int LopID)
        {
            InitializeComponent();
            CenterWindowOnScreen();
            loadThongTin();
            selectedLopID = LopID;
        }

        Student selectedStudent;

        private void loadThongTin()
        {
            string bang = "Data Source=LAPTOP-9Q9UCI39;Initial Catalog=SE104;Integrated Security=True";
            // LOAD THÔNG TIN CUSTOM THEO CLASS STUDENT
            string sql = "SELECT HS_ID, TENHS, NGSINH, DIACHI, EMAIL, GIOITINH FROM HOCSINH WHERE LOP_ID IS NULL";
            using (SqlConnection sqlCon = new SqlConnection(bang))
            {
                sqlCon.Open();
                using (SqlCommand cmd = new SqlCommand(sql, sqlCon))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Student> students = new List<Student>();
                        while (reader.Read())
                        {
                            Student student = new Student();
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
                        studentNotHaveClassDatagrid.ItemsSource = students;
                    }
                }
            }


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private DataGridRow previousSelectedRow = null;
        private void studentNotHaveClassDatagrid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string bang = "Data Source=LAPTOP-9Q9UCI39;Initial Catalog=SE104;Integrated Security=True";
            string studentID = selectedStudent.ID;

            // Thực hiện câu lệnh
            // để cập nhật LOP_ID
            string sql = "UPDATE HOCSINH SET LOP_ID = @LopID WHERE HS_ID = @StudentID";

            using (SqlConnection connection = new SqlConnection(bang))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@LopID", selectedLopID);
                    command.Parameters.AddWithValue("@StudentID", studentID);

                    // Thực thi câu lệnh UPDATE
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Add student to class successfully!");
                        this.Close();
                    }
                }
            }
        }

        private void studentNotHaveClassDatagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (studentNotHaveClassDatagrid.SelectedItem != null)
            {
                Student student = (Student)studentNotHaveClassDatagrid.SelectedItem;
                selectedStudent = student;
            }
        }

        private void txtboxNameStd_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtboxnamestd.Text))
            {
                label.Visibility = Visibility.Visible; // Hiển thị label nếu textBox trống
            }
            else
            {
                label.Visibility = Visibility.Collapsed; // Ẩn label nếu có giá trị trong textBox
            }
        }

        // BUTTON SEARCH
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //SqlConnection sqlCon;
            //SqlDataReader read;
            //SqlDataAdapter sqlDa;
            //DataTable dtbStudent;
            string nameStd = txtboxnamestd.Text.ToLower();
            if (nameStd == "")
            {
                MessageBox.Show("Please enter the name of student!");
                return;
            }

            string bang = "Data Source=LAPTOP-9Q9UCI39;Initial Catalog=SE104;Integrated Security=True";
            string sql = "SELECT * FROM HOCSINH WHERE LOWER(TENHS) LIKE '%' + @Name + '%'AND LOP_ID IS NULL";
            using (SqlConnection sqlCon = new SqlConnection(bang))
            {
                sqlCon.Open();
                using (SqlCommand cmd = new SqlCommand(sql, sqlCon))
                {
                    cmd.Parameters.AddWithValue("@Name", nameStd);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Student> students = new List<Student>();
                        while (reader.Read())
                        {
                            Student student = new Student();
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
                        studentNotHaveClassDatagrid.ItemsSource = students;
                    }
                }
            }
        }
    }
}
