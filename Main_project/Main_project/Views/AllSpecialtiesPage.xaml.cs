using Main_project.Controllers;
using Microsoft.EntityFrameworkCore;
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
    /// Логика взаимодействия для AllSpecialtiesPage.xaml
    /// </summary>
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
