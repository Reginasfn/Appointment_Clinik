using Microsoft.EntityFrameworkCore;
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
    /// Логика взаимодействия для SpecialtyControl.xaml
    /// </summary>
    public partial class SpecialtyControl : UserControl
    {
        private Specialty currSpec;
        public SpecialtyControl(Specialty specialty)
        {
            currSpec = specialty;
            InitializeComponent();
            DataContext = currSpec;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Click_toAppointment(object sender, RoutedEventArgs e)
        {
            using (var db = new DbAppontmentClinikContext())
            {
                currSpec = db.Specialties.FirstOrDefault(spec => spec.NameSpecialty == selectedSpecialtyTxtbx.Text);
                if (currSpec == null)
                {
                    MessageBox.Show("Специальность не найдена!");
                    return;
                }
            }

            ClinikMainWindow mainWindow = Application.Current.MainWindow as ClinikMainWindow;
            if (mainWindow != null && mainWindow.mainframe != null)
            {
                mainWindow.mainframe.NavigationService.Navigate(new AllSpecialtiesPage(currSpec));
            }
        }
    }
}
