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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Main_project.Scripts;
using System.IO.Packaging;
using System.Text.RegularExpressions;

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
            bookAppointmentButton.IsEnabled = false;
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

                if (currDoctor == null) appointmentCalendar.IsEnabled = false; //если врач не выбран

                NoAppointmentsText.Visibility = Visibility.Visible;

                using var db = new DbAppontmentClinikContext();
                var scheduleDays = db.Schedules.Where(s => s.IdDoctor == currDoctor.IdDoctor).Select(sc => sc.DayWeek).ToList();

                for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    string dayOfWeekString = date.ToString("ddd", new CultureInfo("ru-RU"));
                    dayOfWeekString = dayOfWeekString[..1].ToUpper() + dayOfWeekString[1..].ToLower();

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

        private void AppointmentCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
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
                dayOfWeek = char.ToUpper(dayOfWeek[0]) + dayOfWeek[1..].ToLower();

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

        private static List<Schedule> GenerateTimeApp(DateTime startTime, DateTime endTime, TimeSpan interval, List<TimeOnly> bookedTimes)
        {
            var appList = new List<Schedule>();
            if (startTime >= endTime) return appList;
            if (interval <= TimeSpan.Zero) return appList;

            //генерация слотов с интервалом и проверкой на занятость
            for (var i = startTime; i <= endTime; i += interval)
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

        private void AppointmentCalendar_DisplayDateChanged(object sender, CalendarDateChangedEventArgs e)
        {
            if (appointmentCalendar.DisplayDate.Month != DateTime.Now.Month || appointmentCalendar.DisplayDate.Year != DateTime.Now.Year)
            {
                appointmentCalendar.DisplayDate = DateTime.Now;
            }
        }

        private async void AvailableAppointmentsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (availableAppointmentsList.SelectedItem != null && mainScrollViewer != null)
            {
                // 1. Делаем information_user_grid видимым
                information_user_grid.Visibility = Visibility.Visible;

                // 2. Откладываем остальной код до завершения отрисовки и добавляем небольшую задержку
                await Task.Delay(50); // Задержка в 50 миллисекунд

                Dispatcher.Invoke(() =>
                {
                    // 3. Останавливаем текущую анимацию (если есть)
                    mainScrollViewer.BeginAnimation(ScrollViewerBehavior.VerticalOffsetProperty, null);

                    // 4. Вычисляем целевую позицию
                    double targetOffset = mainScrollViewer.VerticalOffset + 550;
                    targetOffset = Math.Min(targetOffset, mainScrollViewer.ScrollableHeight);

                    // 5. Создаем новую анимацию
                    DoubleAnimation animation = new()
                    {
                        To = targetOffset,
                        Duration = TimeSpan.FromMilliseconds(800), // Оптимальное время
                        EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut },
                        FillBehavior = FillBehavior.Stop // Важно!
                    };

                    // 6. Обработчик завершения анимации
                    animation.Completed += (s, _) =>
                    {
                        mainScrollViewer.BeginAnimation(ScrollViewerBehavior.VerticalOffsetProperty, null);
                        mainScrollViewer.ScrollToVerticalOffset(targetOffset);
                    };

                    // 7. Запускаем анимацию
                    mainScrollViewer.BeginAnimation(ScrollViewerBehavior.VerticalOffsetProperty, animation);

                }, DispatcherPriority.Render);
            }
        }

        private string _lastVerificationCode;
        private void code_button_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateTxtBox())
            {
                return;
            }

            string name = CapitalizeFirstLetter(name_user_txtbx.Text.Trim());
            string surname = CapitalizeFirstLetter(surname_user_txtbx.Text.Trim());
            DateOnly date = FormatDate(date_user_txtbx.Text.Trim());
            string phone = new string(phone_user_txtbx.Text.Trim().Where(char.IsDigit).ToArray());
            string email = email_user_txtbx.Text.Trim().ToLower();

            // Активируем/деактивируем кнопку
            try
            {
                // 1. Генерируем случайный код
                string verificationCode = Email_code.GenerateCode();

                // 2. Создаем сообщение с кодом
                List<string> emailContent = Email_code.GenerateVerificateMessage(DateTime.Now, verificationCode);

                // 4. Сохраняем код для последующей проверки (например, в поле класса)
                this._lastVerificationCode = verificationCode;

                Email_code.SendMessage(email, emailContent[0], emailContent[1]);
                MessageBox.Show($"Код подтверждения отправлен на {email_user_txtbx.Text}\n");
                bookAppointmentButton.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при отправке кода: {ex.Message}");
            }
        }

        private bool ValidateTxtBox()
        {
            if (string.IsNullOrWhiteSpace(name_user_txtbx.Text) || string.IsNullOrWhiteSpace(surname_user_txtbx.Text) || string.IsNullOrWhiteSpace(date_user_txtbx.Text) || string.IsNullOrWhiteSpace(phone_user_txtbx.Text) || string.IsNullOrWhiteSpace(email_user_txtbx.Text))
            {
                return false;
            }

            if (date_user_txtbx.Text.Length < 10) { MessageBox.Show("Запишите корректную дату рождения"); return false; }
            if (new string(phone_user_txtbx.Text.Trim().Where(char.IsDigit).ToArray()).Length < 11) { MessageBox.Show("Запишите корректный номер телефона"); return false; }
            if (name_user_txtbx.Text.Length < 3) { MessageBox.Show("Запишите корректное имя"); return false; }
            if (surname_user_txtbx.Text.Length < 2) { MessageBox.Show("Запишите корректную фамилию"); return false; }
            if (email_user_txtbx.Text.Length < 8) { MessageBox.Show("Запишите корректную эл. почту"); return false; }
            if (!IsValidDateFormat(date_user_txtbx.Text))
            {
                MessageBox.Show("Некорректный формат даты!");
                return false;
            }
            return true;

        }
        public static bool IsValidDateFormat(string input)
        {
            return Regex.IsMatch(input, @"^\d{1,2}\.\d{1,2}\.\d{4}$");
        }

        private void BookAppointmentButton_Click(object sender, RoutedEventArgs e)
        {
            string entered_code = code_txtbx.Text;

            if (Verify(entered_code))
            { 
                using var db = new DbAppontmentClinikContext();
                var check_user = db.Users.Where(us => us.EmailUsers == email_user_txtbx.Text).FirstOrDefault();
                string oms = oms_user_txtbx.Text;
                string passport = passport_user_txtbx.Text.Replace("_", "").Replace(" ", "");
                if (check_user == null) // если его нет
                {
                    if (string.IsNullOrWhiteSpace(oms) || string.IsNullOrWhiteSpace(passport))
                    {
                        MessageBox.Show("Необходимо заполнить мед.полис и данные паспорта для дальнейшей записи и прикрепления к клинике", "Заполните поля", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    else
                    {
                        var newUser = new User
                        {
                            EmailUsers = email_user_txtbx.Text,
                            RoleIdUsers = "Пользователь",
                            SurnameUsers = surname_user_txtbx.Text,
                            NameUsers = name_user_txtbx.Text,
                            DateBirth = FormatDate(date_user_txtbx.Text.Trim()),
                            MedicalPolicy = oms,
                            PassportNumber = passport,
                            PhoneNumber = phone_user_txtbx.Text
                        };

                        try
                        {
                            db.Users.Add(newUser);
                            db.SaveChanges();

                            var c = db.Users.Where(s => s.EmailUsers == newUser.EmailUsers).FirstOrDefault();
                            List<string> emailContent = Email_code.GenerateRegMessage(DateTime.Now, name_user_txtbx.Text, c.IdMedCard.ToString());
                            Email_code.SendMessage(email_user_txtbx.Text, emailContent[0], emailContent[1]);
                            return;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Ошибка при регистрации: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                else // если он есть
                {
                    if(string.IsNullOrWhiteSpace(oms) && string.IsNullOrWhiteSpace(passport)) //если пустые поля, то продолжаем
                    {
                        MessageBox.Show($"{check_user.NameUsers}, вы успешно записались на приём!\n\nВаш номер мед.карты: {check_user.IdMedCard}");
                    }
                    else if(string.IsNullOrWhiteSpace(oms))//если поле с мед полисом пустое, то обновляем паспорт
                    {
                        if(check_user.PassportNumber != passport) { check_user.PassportNumber = passport; MessageBox.Show($"Паспорт был успешно обновлён.\n\n{check_user.NameUsers}, вы успешно записались на приём!\n\nВаш номер мед.карты: {check_user.IdMedCard}"); }
                        else { MessageBox.Show($"{check_user.NameUsers}, вы успешно записались на приём!\n\nВаш номер мед.карты: {check_user.IdMedCard}"); }
                        
                    }
                    else // обновляем мед полис
                    {
                        if(check_user.MedicalPolicy != oms)
                        {
                            check_user.MedicalPolicy = oms;
                            MessageBox.Show($"Медицинский полис был успешно обновлён.\n\n{check_user.NameUsers}, вы успешно записались на приём!\n\nВаш номер мед.карты: {check_user.IdMedCard}"); 
                        }
                        else { MessageBox.Show($"{check_user.NameUsers}, вы успешно записались на приём!\n\nВаш номер мед.карты: {check_user.IdMedCard}"); }
                    }
                }
            }
            else { MessageBox.Show("Неверный или просроченный код", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private bool Verify(string user_code)
        {
            return !string.IsNullOrEmpty(_lastVerificationCode) && _lastVerificationCode == user_code;
        }

        private void name_surname_TextChanged(object sender, TextChangedEventArgs e)
        {
            string name = name_user_txtbx.Text;
            string surname = surname_user_txtbx.Text;
            if (!string.IsNullOrEmpty(name) || !string.IsNullOrEmpty(surname))
            {
                // Авто форматирование первой буквы
                name_user_txtbx.Text = CapitalizeFirstLetter(name);
                surname_user_txtbx.Text = CapitalizeFirstLetter(surname);
                name_user_txtbx.CaretIndex = name.Length; // Установка курсора в конец
                surname_user_txtbx.CaretIndex = surname.Length; // Установка курсора в конец
            }

        }
        private string CapitalizeFirstLetter(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            return char.ToUpper(input[0]) + input.Substring(1).ToLower();
        }
        private DateOnly FormatDate(string dateInput)
        {
            return DateOnly.Parse(dateInput); // или более сложная логика форматирования
        }

        private void NoLetter_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.All(char.IsDigit);
        }

        private void email_user_txtbx_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[а-яА-ЯёЁ]");
            e.Handled = regex.IsMatch(e.Text);
        }
    }

    public static class ScrollViewerBehavior
    {
        public static readonly DependencyProperty VerticalOffsetProperty =
            DependencyProperty.RegisterAttached("VerticalOffset", typeof(double), typeof(ScrollViewerBehavior),
            new UIPropertyMetadata(0.0, OnVerticalOffsetChanged));

        public static void SetVerticalOffset(FrameworkElement target, double value) =>
            target.SetValue(VerticalOffsetProperty, value);

        public static double GetVerticalOffset(FrameworkElement target) =>
            (double)target.GetValue(VerticalOffsetProperty);

        private static void OnVerticalOffsetChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            if (target is ScrollViewer scrollViewer)
                scrollViewer.ScrollToVerticalOffset((double)e.NewValue);
        }
    }
}
