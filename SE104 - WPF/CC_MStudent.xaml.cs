using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using SE104___WPF.DBClass;

using System.Data;

using System.Data.SqlClient;
using MaterialDesignThemes.Wpf;
using System;
using System.Data.SqlTypes;
using System.Windows.Documents;
using static SE104___WPF.CC_MStudent;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media;

namespace SE104___WPF
{
    /// <summary>
    /// Interaction logic for CC_MStudent.xaml
    /// </summary>
    public partial class CC_MStudent : UserControl
    {
        clsHOCSINH hs = new clsHOCSINH();
        DBConnect dBConnect = new DBConnect();
        public CC_MStudent()
        {
            InitializeComponent();
            loadThongTin();
        }
        string bang = "Data Source=LAPTOP-9Q9UCI39;Initial Catalog=SE104;Integrated Security=True";
        string nameStd = "init";

        SqlConnection sqlCon;
        SqlCommand sqlCom;
        SqlDataReader read;
        SqlDataAdapter sqlDa;
        DataTable dtbStudent;

        public class Student
        {
            public string ID { get; set; }
            public string Fullname { get; set; }
            public DateTime Birthday { get; set; }
            public string Address { get; set; }
            public string Email { get; set; }
            public string Gender { get; set; }

            // Các trường dữ liệu khác
        }

        Student selectedStudent;

        // Lấy dữ liệu từ cơ sở dữ liệu và tạo danh sách đối tượng Student



        private void loadThongTin()
        {

            //LOAD THÔNG TIN CUSTOM THEO CLASS STUDENT
            string sql = "SELECT HS_ID, TENHS, NGSINH, DIACHI, EMAIL, GIOITINH FROM HOCSINH";
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
                        dataGridViewStudent.ItemsSource = students;
                    }
                }
            }


        }

        private DataGridRow previousSelectedRow = null;

        private void dataGridViewStudent_PreviewMouseDown(object sender, MouseButtonEventArgs e)
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



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddNewStudent addNewStudent = new AddNewStudent();

            addNewStudent.Show();
            addNewStudent.Closed += AddOrUpdateNewStudent_Closed;
        }

        private void AddOrUpdateNewStudent_Closed(object sender, EventArgs e)
        {
            loadThongTin();
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (dataGridViewStudent.SelectedItem == null)
            {
                MessageBox.Show("Need select a student first");
                return;
            }
            ProfileStudent profileStudent = new ProfileStudent(selectedStudent);
            profileStudent.Show();
            profileStudent.Closed += AddOrUpdateNewStudent_Closed;
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            nameStd = txtboxNameStd.Text.ToLower();
            if (nameStd == "")
            {
                MessageBox.Show("Please enter the name of student!");
                return;
            }

            string sql = "SELECT * FROM HOCSINH WHERE LOWER(TENHS) LIKE '%' + @Name + '%'";
            using (sqlCon = new SqlConnection(bang))
            {
                sqlCon.Open();
                sqlCom = new SqlCommand(sql, sqlCon);
                sqlCom.Parameters.AddWithValue("@Name", nameStd);
                sqlDa = new SqlDataAdapter(sqlCom);
                dtbStudent = new DataTable();
                sqlDa.Fill(dtbStudent);
                dataGridViewStudent.ItemsSource = dtbStudent.DefaultView;
                sqlCon.Close();
            }
        }


        private void txtboxNameStd_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtboxNameStd.Text))
            {
                label.Visibility = Visibility.Visible; // Hiển thị label nếu textBox trống
            }
            else
            {
                label.Visibility = Visibility.Collapsed; // Ẩn label nếu có giá trị trong textBox
            }
        }

        private void txtboxNameStd_FocusableChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }

        public Student getStudent(Student student2)
        {
            return student2;
        }

        private void dataGridViewStudent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridViewStudent.SelectedItem != null)
            {
                Student student = (Student)dataGridViewStudent.SelectedItem;
                selectedStudent = student;
            }
        }


    }
}
