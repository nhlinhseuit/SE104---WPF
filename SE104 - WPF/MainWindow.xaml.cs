using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Xml.Linq;
using System.Data.SqlClient;

namespace SE104___WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        private readonly List<Button> buttons = new List<Button>();
        private readonly List<TextBlock> textBlocks = new List<TextBlock>();
        private readonly List<MaterialDesignThemes.Wpf.PackIcon> packIcons = new List<MaterialDesignThemes.Wpf.PackIcon>();

        //private readonly List<buttonObject> buttonObjects = new List<buttonObject>();
        private void CenterWindowOnScreen()
        {
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2);
        }
        public MainWindow()
        {
            InitializeComponent();
            CC.Content = new CC_MStudent();
            handleButtonClickChangeColor(buttons, textBlocks, packIcons, btnMStudent, textMStudent, iconMStudent);
            CenterWindowOnScreen();
            buttons.AddRange(new[] { btnMStudent, btnMClass, btnMGrade, btnMRegulation, btnReport });
            textBlocks.AddRange(new[] { textMStudent, textMClass, textMGrade, textMRegulation, textReport });
            packIcons.AddRange(new[] { iconMStudent, iconMClass, iconMGrade, iconMRegulation, iconReport });


        }


        public void setClicked(Button x, TextBlock y, MaterialDesignThemes.Wpf.PackIcon z)
        {
            var converter = new System.Windows.Media.BrushConverter();
            x.Background = (Brush)converter.ConvertFromString("#7E9ABB");
            x.BorderBrush = (Brush)converter.ConvertFromString("#7E9ABB");
            y.Foreground = (Brush)converter.ConvertFromString("#FFF");
            z.Foreground = (Brush)converter.ConvertFromString("#FFF");
        }

        public void setDefault(Button x, TextBlock y, MaterialDesignThemes.Wpf.PackIcon z)
        {
            var converter = new System.Windows.Media.BrushConverter();
            x.Background = (Brush)converter.ConvertFromString("#BBD1EB");
            x.BorderBrush = (Brush)converter.ConvertFromString("#BBD1EB");
            y.Foreground = (Brush)converter.ConvertFromString("#54667A");
            z.Foreground = (Brush)converter.ConvertFromString("#54667A");
        }

        public void handleButtonClickChangeColor(List<Button> buttons, List<TextBlock> textBlocks, List<MaterialDesignThemes.Wpf.PackIcon> packIcons, Button buttonClicked, TextBlock textClicked, MaterialDesignThemes.Wpf.PackIcon iconClicked)
        {
            foreach (var b in buttons.Where(b => b != buttonClicked))
                foreach (var t in textBlocks.Where(t => t != textClicked))
                    foreach (var p in packIcons.Where(p => p != iconClicked))
                        setDefault(b, t, p);
            setClicked(buttonClicked, textClicked, iconClicked);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CC.Content = new CC_MStudent();
            handleButtonClickChangeColor(buttons, textBlocks, packIcons, btnMStudent, textMStudent, iconMStudent);
        }

        private void btnMClass_Click(object sender, RoutedEventArgs e)
        {
            CC.Content = new CC_MClass();
            handleButtonClickChangeColor(buttons, textBlocks, packIcons, btnMClass, textMClass, iconMClass);
        }

        private void btnMGrade_Click(object sender, RoutedEventArgs e)
        {
            CC.Content = new CC_MGrade();
            handleButtonClickChangeColor(buttons, textBlocks, packIcons, btnMGrade, textMGrade, iconMGrade);
        }

        private void btnMRegulation_Click(object sender, RoutedEventArgs e)
        {
            CC.Content = new CC_MRegulation();
            handleButtonClickChangeColor(buttons, textBlocks, packIcons, btnMRegulation, textMRegulation, iconMRegulation);
        }

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            CC.Content = new CC_Report();
            handleButtonClickChangeColor(buttons, textBlocks, packIcons, btnReport, textReport, iconReport);
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            SigninWindow loginWindow = new SigninWindow();
            this.Hide();
            loginWindow.Show ();
        }
    }
}
