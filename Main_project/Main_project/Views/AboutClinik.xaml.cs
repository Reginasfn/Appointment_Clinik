using System;
using System.Collections.Generic;
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

namespace Main_project.Views
{
    /// <summary>
    /// Логика взаимодействия для AboutClinik.xaml
    /// </summary>
    public partial class AboutClinik : Page
    {
        public AboutClinik()
        {
            ClinikMainWindow mainWindow = Application.Current.MainWindow as ClinikMainWindow;
            mainWindow.Title = "О клинике";

            InitializeComponent();
        }
    }
}
