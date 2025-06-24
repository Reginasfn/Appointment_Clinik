using Main_project.Controllers;
using Main_project.Models;
using System.Windows;
using System.Windows.Controls;
namespace Main_project.Views
{
    public partial class SpecialtiesPage : Page
    {
        public List<Specialty> allSpecialties { get; set; }
        public List<Specialty> selectedSpecialty { get; set; }
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
