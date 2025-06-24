using Main_project.Models;
using Main_project.Scripts;
using System.Windows;
using System.Windows.Input;

namespace Main_project.Views
{
    public partial class ClinikMainWindow : Window
    {
        public ClinikMainWindow()
        {
            InitializeComponent();
            UpdateAppointmentStatuses();
            mainframe.NavigationService.Navigate(new SpecialtiesPage());
            PDF.CreateTodaysAppointmentsPdf();
        }

        private void UpdateAppointmentStatuses()
        {
            try
            {
                using (var db = new DbAppontmentClinikContext())
                {
                    var currentDate = DateOnly.FromDateTime(DateTime.Now);
                    var currentTime = TimeOnly.FromDateTime(DateTime.Now);
                    var appointmentsToUpdate = db.Appointments.Where(a => (a.DateAppointment < currentDate ||(a.DateAppointment == currentDate && a.TimeAppointment <= currentTime)) &&(a.StatusAppointment == null || a.StatusAppointment != "Завершён")).ToList();
                    foreach (var appointment in appointmentsToUpdate)
                    {
                        appointment.StatusAppointment = "Завершён";
                    }

                    if (appointmentsToUpdate.Any())
                    {
                        int changes = db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении статусов записей: {ex.Message}\n\n{ex.InnerException?.Message}",
                              "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Click_aboutClinik_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            mainframe.NavigationService.Navigate(new AboutClinik());
        }
        private void Click_contactsClinik_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            mainframe.NavigationService.Navigate(new ContactsClinik());
        }
        private void Click_appointmentClinik_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            mainframe.NavigationService.Navigate(new SpecialtiesPage());
        }
        private void mainframe_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

        }
        private void Admin_panel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            mainframe.NavigationService.Navigate(new AdminAuth());
        }
    }
}
