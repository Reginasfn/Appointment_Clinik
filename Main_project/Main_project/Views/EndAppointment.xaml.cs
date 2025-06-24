using System.Windows;
using System.Windows.Controls;
namespace Main_project.Views
{
    public partial class EndAppointment : Page
    {
        public EndAppointment(string date, string time, string specialty, string fio, string? cabinet)
        {
            InitializeComponent();
            date_lbl.Content = date;
            time_lbl.Content = time;
            if(specialty == "Терапевт") { specialty = "Терапевт участковый"; }
            specialty_lbl.Content = specialty;
            fio_lbl.Content = GetShortName(fio);
            cabinet_lbl.Content = $"Кабинет № {cabinet}";
            ClinikMainWindow mainWindow = Application.Current.MainWindow as ClinikMainWindow;
            mainWindow.Title = "Успешная запись";
        }

        private static string GetShortName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName)) return fullName;
            string[] parts = fullName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 3){return $"{parts[0]} {parts[1][0]}. {parts[2][0]}.";}
            else if (parts.Length == 2){return $"{parts[0]} {parts[1][0]}.";}
            else {return fullName;}
        }
        private void on_main_Click(object sender, RoutedEventArgs e)
        {
            ClinikMainWindow mainWindow = Application.Current.MainWindow as ClinikMainWindow;
            mainWindow.mainframe.NavigationService.Navigate(new SpecialtiesPage());
        }
        private void print_talon_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
