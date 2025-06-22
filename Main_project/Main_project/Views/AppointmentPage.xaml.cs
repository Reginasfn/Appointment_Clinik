using Main_project.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Логика взаимодействия для AppointmentPage.xaml
    /// </summary>
    public partial class AppointmentPage : Page
    {
        Specialty selectedSpecialty;
        Doctor currDoctor;
        //Schedule currSchedule;
        public AppointmentPage(Specialty spec, Doctor doct)
        {
            
            if (spec == null || doct == null) return;
                    
            currDoctor = doct;
            selectedSpecialty = spec;

            InitializeComponent();
            DataContext = currDoctor;

            StartInitialize();
        }

        private void StartInitialize()
        {
            CalendarLimit(); // ограничения календаря
        }

        private void CalendarLimit()
        {
            try
            {
                DateTime startDate = DateTime.Today;
                DateTime endDate = DateTime.Today.AddDays(14); //запись на ближ 2 недель

                calendarSelectedDateTxt.Text = "Дата: " + DateTime.Today.ToShortDateString();

                appointmentCalendar.DisplayDateStart = startDate;
                appointmentCalendar.DisplayDateEnd = endDate;

                if (currDoctor != null) //если врач выбран
                {
                    NoAppointmentsText.Visibility = Visibility.Visible;

                    using (var db = new DbAppontmentClinikContext())
                    {
                        var scheduleDays = db.Schedules.Where(s => s.IdDoctor == currDoctor.IdDoctor).Select(sc => sc.DayWeek).ToList();

                        for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                        {
                            string dayOfWeekString = date.ToString("ddd", new CultureInfo("ru-RU"));
                            dayOfWeekString = dayOfWeekString.Substring(0, 1).ToUpper() + dayOfWeekString.Substring(1).ToLower();

                            if (!scheduleDays.Contains(dayOfWeekString)) //блок если дня нет в расписании
                            {
                                appointmentCalendar.BlackoutDates.Add(new CalendarDateRange(date));
                            }
                            if (date.DayOfWeek == DayOfWeek.Sunday) //блок воскресенья
                            {
                                appointmentCalendar.BlackoutDates.Add(new CalendarDateRange(date));
                            }
                        }
                    }
                }
                else //если врач не выбран
                {
                    appointmentCalendar.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка в ограничении календаря: {ex.Message}");
                appointmentCalendar.IsEnabled = false;
            }
        }

        private void Back_button_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is not ClinikMainWindow mainWindow)
            {
                MessageBox.Show("Ошибка навигации");
                return;
            }
            mainWindow.mainframe.NavigationService?.Navigate(new AllSpecialtiesPage(selectedSpecialty));
        }

        private void appointmentCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadAvailableAppointments(); //загрузка записей
        }

        private void LoadAvailableAppointments()
        {
            if (!appointmentCalendar.SelectedDate.HasValue) return;
            if (currDoctor == null) return;

            try
            {
                var selectedDate = appointmentCalendar.SelectedDate.Value.Date;
                var dayOfWeek = CultureInfo.GetCultureInfo("ru-RU").DateTimeFormat.GetAbbreviatedDayName(selectedDate.DayOfWeek);
                dayOfWeek = char.ToUpper(dayOfWeek[0]) + dayOfWeek.Substring(1).ToLower();

                calendarSelectedDateTxt.Text = $"Дата: {selectedDate:d}";

                using var db = new DbAppontmentClinikContext();
                if (db == null) { MessageBox.Show("Ошибка в подключении к базе данных"); return; }

                var schedule = db.Schedules.FirstOrDefault(s => s.IdDoctor == currDoctor.IdDoctor && s.DayWeek == dayOfWeek);

                //если нет расписания для этого дня
                if (schedule == null)
                {
                    availableAppointmentsList.ItemsSource = null; //пустой список
                    NoAppointmentsText.Visibility = Visibility.Visible; //записи отсутсвуют
                    return;
                }

                //длит-ть приёма, по умолчанию 15
                var time_accept = db.Specialties
                    .Where(d => d.IdSpecialty == currDoctor.IdSpecialty)
                    .Select(t => t.TimeAccept)
                    .FirstOrDefault() ?? 15;

                //записи в опред день
                var existingApp = db.Appointments
                    .Where(a => a.IdDoctor == currDoctor.IdDoctor &&
                                a.DateAppointment == DateOnly.FromDateTime(selectedDate) && a.TimeAppointment.HasValue)
                    .Select(a => a.TimeAppointment!.Value)
                    .ToList();

                var availableAppList = GenerateTimeApp(selectedDate.Add(schedule.TimeStart.ToTimeSpan()),
                                                        selectedDate.Add(schedule.TimeEnd.ToTimeSpan()),
                                                        TimeSpan.FromMinutes((double)time_accept),
                                                        existingApp);

                availableAppointmentsList.ItemsSource = availableAppList;
                NoAppointmentsText.Visibility = availableAppList.Count == 0 ? Visibility.Visible : Visibility.Hidden;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки записей: {ex.Message}");
                availableAppointmentsList.ItemsSource = null;
                NoAppointmentsText.Visibility = Visibility.Visible;
            }
        }

        private List<Schedule> GenerateTimeApp(DateTime startTime, DateTime endTime, TimeSpan interval, List<TimeOnly> bookedTimes)
        {
            var appList = new List<Schedule>();
            if (startTime >= endTime) return appList;
            if (interval <= TimeSpan.Zero) return appList;

            //генерация слотов с интервалом и проверкой на занятость
            for (var i = startTime; i <= endTime; i = i + interval)
            {
                if (!bookedTimes.Contains(TimeOnly.FromDateTime(i)))
                {
                    appList.Add(new Schedule
                    {
                        TimeStart = TimeOnly.FromDateTime(i)
                    });
                }
            }
            return appList;
        }

        private void bookAppointmentButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void appointmentCalendar_DisplayDateChanged(object sender, CalendarDateChangedEventArgs e)
        {
            if (appointmentCalendar.DisplayDate.Month != DateTime.Now.Month || appointmentCalendar.DisplayDate.Year != DateTime.Now.Year)
            {
                appointmentCalendar.DisplayDate = DateTime.Now;
            }
        }
    }
}
