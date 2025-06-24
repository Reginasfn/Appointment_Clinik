using Main_project.Controllers;
using Microsoft.EntityFrameworkCore;
using Main_project.Models;
using System.Windows;
using System.Windows.Controls;
namespace Main_project.Views
{
    public partial class AllSpecialtiesPage : Page
    {
        Specialty specialtyName;
        public AllSpecialtiesPage(Specialty selectedSpecialty)
        {
            specialtyName = selectedSpecialty;
            InitializeComponent();
            selectedSpecialtyTxtbx.Text = "Специализация: " + specialtyName.NameSpecialty;
            EmptySpecTxt.Visibility = Visibility.Hidden;
            LoadDoctors();
        }
        private void LoadDoctors()
        {
            using (var db = new DbAppontmentClinikContext())
            {
                var doctors = db.Doctors.Include(spec => spec.IdSpecialtyNavigation).Where(d => d.IdSpecialtyNavigation.NameSpecialty == specialtyName.NameSpecialty);
                if (!doctors.Any())
                {
                    EmptySpecTxt.Visibility = Visibility.Visible;
                    EmptySpecTxt.Text = "К сожалению специалистов данного направления нет...";
                }
                else
                {
                    foreach (var d in doctors)
                    {
                        doctorsListView.Items.Add(new DoctorsControl(d, specialtyName));
                    }
                }
            }
        }
        private void Back_button_Click(object sender, RoutedEventArgs e)
        {
            ClinikMainWindow mainWindow = Application.Current.MainWindow as ClinikMainWindow;
            mainWindow.mainframe.NavigationService.Navigate(new SpecialtiesPage());
        }
    }
}
