using Main_project.Models;
using Main_project.Views;
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

namespace Main_project.Controllers
{
    /// <summary>
    /// Логика взаимодействия для DoctorsControl.xaml
    /// </summary>
    public partial class DoctorsControl : UserControl
    {
        public Doctor currDoct { get; set; }
        Specialty specialtyName;
        public DoctorsControl(Doctor doctor, Specialty spec)
        {
            specialtyName = spec;
            currDoct = doctor;
            InitializeComponent();
            DataContext = doctor;

            specialtyNameTxt.Text = "Врач - " + spec.NameSpecialty.ToString().ToLower();
        }

        private void toAppointment_button_Click(object sender, RoutedEventArgs e)
        {
            ClinikMainWindow mainWindow = Application.Current.MainWindow as ClinikMainWindow;
            mainWindow.mainframe.NavigationService.Navigate(new AppointmentPage(specialtyName, currDoct));
        }

        private void informationDoctor_Click(object sender, RoutedEventArgs e)
        {
            ClinikMainWindow mainWindow = Application.Current.MainWindow as ClinikMainWindow;
            mainWindow.mainframe.NavigationService.Navigate(new InformationDoctorPage(specialtyName, currDoct));
        }
    }
}
