using Main_project.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Main_project.Views
{
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
