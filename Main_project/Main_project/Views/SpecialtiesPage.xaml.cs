using Main_project.Controllers;
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
    /// Логика взаимодействия для SpecialtiesPage.xaml
    /// </summary>
    public partial class SpecialtiesPage : Page
    {
        public List<Specialty> allSpecialties { get; set; } // Оригинальный список
        public List<Specialty> selectedSpecialty { get; set; } // Список для отображения
        public SpecialtiesPage()
        {
            InitializeComponent();

            ClinikMainWindow mainWindow = Application.Current.MainWindow as ClinikMainWindow;
            mainWindow.Title = "Запись на приём";

            LoadSpecialty();
            UpdateSpecialtyListView();
        }

        private void LoadSpecialty()
        {
            using (var db = new DbAppontmentClinikContext())
            {
                allSpecialties = db.Specialties.ToList();
            }
            selectedSpecialty = allSpecialties.ToList();
            UpdateSpecialtyListView();
        }

        private void UpdateSpecialtyListView()
        {
            specialtyListView.Items.Clear();
            foreach (Specialty specialty in selectedSpecialty)
            {
                SpecialtyControl specItem = new SpecialtyControl(specialty);
                specialtyListView.Items.Add(specItem);
            }
        }

        private void searchtxtbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var search = searchtbox.Text?.ToLower();
            if (!string.IsNullOrEmpty(search))
            {
                selectedSpecialty = allSpecialties.Where(s =>
                    !string.IsNullOrEmpty(s.NameSpecialty) && s.NameSpecialty.ToLower().Contains(search)).ToList();
            }
            else
            {
                selectedSpecialty = allSpecialties.ToList();
            }

            UpdateSpecialtyListView();
        }
    }
}
