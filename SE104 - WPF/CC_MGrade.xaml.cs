using SE104___WPF.DBClass;
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
    /// Interaction logic for CC_MGrade.xaml
    /// </summary>
    public partial class CC_MGrade : UserControl
    {
        public CC_MGrade()
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
        Student selectedStudent;
        clsHOCSINH hs = new clsHOCSINH();

        String txtboxHocky;
        String txtboxIDHS;
        String txtboxMonhoc;
        String txtboxDiem15;
        String txtboxDime45;
        String txtboxDiemCK;


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

        private void loadClassCombobox()
        {
                string selectedKhoiID = GradeCombobox.SelectedItem.ToString(); // Giả sử selectedKhoiID lưu trữ KHOI_ID được chọn từ ComboBox

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

        private void loadSubjectCombobox()
        {
            if (SemesterCombobox.SelectedItem != null)
            {
                string selectedKhoiID = SemesterCombobox.SelectedItem.ToString(); // Giả sử selectedKhoiID lưu trữ KHOI_ID được chọn từ ComboBox

                if (SubjectCombobox.SelectedItem != null)
                {
                    SubjectCombobox.SelectedItem = null;
                    SubjectCombobox.Items.Clear();

                    dataGridViewGrade.ItemsSource = null;
                    dataGridViewGrade.ItemsSource = new List<object>();

                }

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
            else
            {
            }
        }

        private void loadThongTin()
        {
            using (SqlConnection connection = new SqlConnection(bang))
            {
                String selectedKhoiID = GradeCombobox.SelectedItem.ToString();
                String selectedClassID = ClassCombobox.SelectedItem.ToString();
                String selectedSemesterID = SemesterCombobox.SelectedItem.ToString();
                String selectedSubjectID = SubjectCombobox.SelectedItem.ToString();
                connection.Open();
                string sql = "SELECT HOCSINH.HS_ID, HOCSINH.TENHS, BANGDIEM.DIEM15, BANGDIEM.DIEM45, BANGDIEM.DIEMHK, BANGDIEM.DIEMTB " +
                    "FROM HOCSINH " +
                    "JOIN LOP ON HOCSINH.LOP_ID = LOP.LOP_ID " +
                    "JOIN BANGDIEM ON HOCSINH.HS_ID = BANGDIEM.HS_ID " +
                    "JOIN HOCKY ON BANGDIEM.HK_ID = HOCKY.HK_ID " +
                    "JOIN MONHOC ON BANGDIEM.MH_ID = MONHOC.MH_ID " +
                    "WHERE LOP.KHOI_ID IN (SELECT KHOI_ID FROM KHOI WHERE TENKHOI = '" + selectedKhoiID + "' ) " +
                    "AND LOP.LOP_ID IN (SELECT LOP_ID FROM LOP WHERE TENLOP = '" + selectedClassID + "' ) " +
                    "AND HOCKY.HK_ID IN (SELECT HK_ID FROM HOCKY WHERE TENHK = '" + selectedSemesterID + "' ) " +
                    "AND MONHOC.MH_ID IN (SELECT MH_ID FROM MONHOC WHERE TENMH = '" + selectedSubjectID + "' )";
                    
                SqlCommand command = new SqlCommand(sql, connection);
                using (sqlCon = new SqlConnection(bang))
                {
                    
                    sqlCon.Open();
                    
                    sqlDa = new SqlDataAdapter(sql, sqlCon);
                    dtbStudent = new DataTable();
                    sqlDa.Fill(dtbStudent);
                    sqlCon.Close();
                    dataGridViewGrade.ItemsSource = dtbStudent.DefaultView;
                }
                
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string selectedCbbClass = ClassCombobox.SelectedItem.ToString();
            EnterGrade enterGrade = new EnterGrade( selectedCbbClass);
            enterGrade.Show();
            enterGrade.Closed += (sender, args) => loadThongTin();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (dataGridViewGrade.SelectedItem == null)
            {
                MessageBox.Show("Need select a student first");
                return;
            }
            UpdateGrade updateGrade = new UpdateGrade(txtboxHocky, txtboxIDHS, txtboxMonhoc, txtboxDiem15, txtboxDime45, txtboxDiemCK);
            updateGrade.Show();
            updateGrade.Closed += (sender, args) => loadThongTin();
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
            if (dataGridViewGrade.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)dataGridViewGrade.SelectedItem;
                String selectedSemesterID = SemesterCombobox.SelectedItem.ToString();
                String selectedSubjectID = SubjectCombobox.SelectedItem.ToString();

                string hsID = selectedRow["HS_ID"].ToString();
                string diem45 = selectedRow["DIEM45"].ToString();
                string diem15 = selectedRow["DIEM15"].ToString();
                string diemHK = selectedRow["DIEMHK"].ToString();

                // Gán các giá trị vào các textbox hoặc nơi bạn muốn hiển thị
                txtboxMonhoc = selectedSubjectID;
                txtboxIDHS = hsID;
                txtboxHocky = selectedSemesterID;
                txtboxDime45 = diem45;
                txtboxDiem15 = diem15;
                txtboxDiemCK = diemHK;
            }    
                

        }

        private bool isFirstSelectionGrade = true;

        private void GradeCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isFirstSelectionGrade)
            {
                isFirstSelectionGrade = false;
                loadClassCombobox();
            }
            else
            {
                ClassCombobox.SelectedItem = null;
                ClassCombobox.Items.Clear();
                loadClassCombobox();

                dataGridViewGrade.ItemsSource = null;
                dataGridViewGrade.ItemsSource = new List<object>();
            }

        }

        private bool isFirstSelectionClass = true;
        private void ClassCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isFirstSelectionClass)
            {
                isFirstSelectionClass = false;
                loadSemesterCombobox();
            }
            else
            {
                SemesterCombobox.SelectedItem = null;
                SemesterCombobox.Items.Clear();
                loadSemesterCombobox();

                dataGridViewGrade.ItemsSource = null;
                dataGridViewGrade.ItemsSource = new List<object>();
            }
        }

        private bool isFirstSelectionSemester = true;
        private void SemesterCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            if (isFirstSelectionSemester)
            {
                isFirstSelectionSemester = false;
                loadSubjectCombobox();
            }
            else
            {
                SubjectCombobox.SelectedItem = null;
                SubjectCombobox.Items.Clear();
                loadSubjectCombobox();

                dataGridViewGrade.ItemsSource = null;
                dataGridViewGrade.ItemsSource = new List<object>();
            }
        }

        private void SubjectCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            loadThongTin();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            String selectedHSID;
            if (dataGridViewGrade.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)dataGridViewGrade.SelectedItem;
                selectedHSID = selectedRow["HS_ID"].ToString();

                String selectedSemesterID = SemesterCombobox.SelectedItem.ToString();
                String selectedSubjectID = SubjectCombobox.SelectedItem.ToString();
                MessageBoxResult result = MessageBox.Show("Are you sure want to delete?",
                "Yes", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // Thực hiện xóa đối tượng
                    hs.selectedID = selectedHSID;
                    hs.selectedSubject = selectedSubjectID;
                    hs.selectedSemester = selectedSemesterID;
                    hs.xoaBangDiem(hs);
                    loadThongTin();
                }
                else
                {
                    return;
                }
            }
        }

    }
}
