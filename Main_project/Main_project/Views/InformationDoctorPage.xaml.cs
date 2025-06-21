using Main_project.Models;
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
    /// Логика взаимодействия для InformationDoctorPage.xaml
    /// </summary>
    public partial class InformationDoctorPage : Page
    {
        Specialty specialtyName;
        Doctor currDoctor1 { get; set; }
        public InformationDoctorPage(Specialty specialty, Doctor currDoctor)
        {
            specialtyName = specialty;
            currDoctor1 = currDoctor;

            InitializeComponent();
            DataContext = currDoctor1;

            specialtyNameTxt.Text = $"Врач - {specialtyName.NameSpecialty.ToLower()}";
            IconDoctorView.Source = new BitmapImage(new Uri(currDoctor.DisplayIconDoctor));
        }
        private void Back_button_Click(object sender, RoutedEventArgs e)
        {
            ClinikMainWindow mainWindow = Application.Current.MainWindow as ClinikMainWindow;
            mainWindow.mainframe.NavigationService.Navigate(new AllSpecialtiesPage(specialtyName));
        }
    }
}
