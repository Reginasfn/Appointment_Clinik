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
        Schedule currSchedule;
        public AppointmentPage(Specialty spec, Doctor doct)
        {
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

        private void Back_button_Click(object sender, RoutedEventArgs e)
        {
            ClinikMainWindow mainWindow = Application.Current.MainWindow as ClinikMainWindow;
            mainWindow.mainframe.NavigationService.Navigate(new AllSpecialtiesPage(selectedSpecialty));
        }

        private void appointmentCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadAvailableAppointments(); //загрузка записей
        }

        private void LoadAvailableAppointments()
        {
            if (appointmentCalendar.SelectedDate.HasValue) //если выбрана дата
            {
                DateTime selectedDate = appointmentCalendar.SelectedDate.Value;
                string dayOfWeek = selectedDate.ToString("ddd", new CultureInfo("ru-RU"));
                dayOfWeek = dayOfWeek.Substring(0, 1).ToUpper() + dayOfWeek.Substring(1).ToLower(); //пн => Пн

                calendarSelectedDateTxt.Text = "Дата: " + appointmentCalendar.SelectedDate.Value.ToShortDateString();
                
                using (var db = new DbAppontmentClinikContext())
                {
                    var schedule = db.Schedules.Where(s => s.IdDoctor == currDoctor.IdDoctor && s.DayWeek == dayOfWeek).FirstOrDefault();

                    //если нет расписания для этого дня
                    if (schedule == null) 
                    {
                        availableAppointmentsList.ItemsSource = new List<Schedule>(); //пустой список
                        NoAppointmentsText.Visibility = Visibility.Visible; //записи отсутсвуют
                        return;
                    }

                    TimeOnly startTimeOnly = schedule.TimeStart;
                    TimeOnly endTimeOnly = schedule.TimeEnd;

                    DateTime startTime1 = selectedDate.Add(startTimeOnly.ToTimeSpan());
                    DateTime endTime1 = selectedDate.Add(endTimeOnly.ToTimeSpan());

                    DateTime currentTime = startTime1;

                    //длит-ть приёма
                    var time_accept = db.Specialties.Where(d => d.IdSpecialty == currDoctor.IdSpecialty).Select(t => t.TimeAccept).FirstOrDefault();

                    var appointmentsAll = new List<Schedule>();

                    //создание слотов с интервалом
                    while (currentTime <= endTime1)
                    {
                        //bool appointmentExist = db.Appointments.Any(d => 
                        //        d.IdDoctor == currDoctor.IdDoctor && 
                        //        appointmentCalendar.SelectedDate.Value.ToShortDateString() == d.DateAppointment.ToString() &&
                        //        d.TimeAppointment == TimeOnly.FromDateTime(currentTime) &&
                        //        (d.StatusAppointment == null || d.StatusAppointment != "Завершён"));

                        //// Если записи нет, добавляем
                        //if (!appointmentExist)
                        //{
                        appointmentsAll.Add(new Schedule
                        {
                            TimeStart = TimeOnly.FromDateTime(currentTime)
                        });
                        currentTime = currentTime.AddMinutes(Convert.ToInt64(time_accept));
                        //}
                    }
                    availableAppointmentsList.ItemsSource = appointmentsAll;
                    
                    //если слотов нет
                    NoAppointmentsText.Visibility = appointmentsAll.Count == 0 ? Visibility.Visible : Visibility.Hidden;
                }
            }
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
