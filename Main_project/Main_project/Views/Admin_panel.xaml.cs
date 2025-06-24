using Main_project.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Main_project.Views
{
    public partial class Admin_panel : Page
    {
        private User _user;
        private Specialty _specialty;
        public int total_items;
        public int total_pages;
        public int current_page = 1;
        public ObservableCollection<Doctor> doctors;
        public ObservableCollection<Appointment> appointment;
        public ObservableCollection<Specialty> specialties;
        public ObservableCollection<Schedule> schedules;
        public List<User> users;
        public Admin_panel(User user)
        {
            InitializeComponent();
            ClinikMainWindow mainWindow = Application.Current.MainWindow as ClinikMainWindow;
            mainWindow.Title = "Администраторская панель";
            _user = user;
            DataContext = user;
            getData();
            LoadData();
        }
        private void getData()
        {
            using var db = new DbAppontmentClinikContext();
            doctors = new ObservableCollection<Doctor>(db.Doctors.ToList());
            users = db.Users.ToList();
            appointment = new ObservableCollection<Appointment>(db.Appointments.ToList());
            specialties = new ObservableCollection<Specialty>(db.Specialties.ToList());
            schedules = new ObservableCollection<Schedule>(db.Schedules.ToList());
        }
        public void LoadDoctors()
        {
            using var db = new DbAppontmentClinikContext();
            if (doctors is not null)
            {
                total_items = doctors.Count();
                total_pages = (int)Math.Ceiling((double)total_items / 10);
                if (current_page > total_pages && total_pages > 0)
                {
                    current_page = total_pages;
                }
                else if (total_pages == 0){ current_page = 1; }
                var doctorsfiltered = doctors.Select(d => new
                {
                    d.IdDoctor,
                    d.IdSpecialty,
                    SpecialtyName = db.Specialties.FirstOrDefault(s => s.IdSpecialty == d.IdSpecialty)?.NameSpecialty,
                    d.SurnameDoctor,
                    d.NameDoctor,
                    d.PatronymicDoctor,
                    d.EmailDoctor,
                    d.PhoneNumberDoctor,
                    d.MedicalExperience,
                    d.CabinetNumber,
                    d.StatusWork,
                    d.IconDoctor
                }).ToList();
                var pagedGames = doctorsfiltered.Skip((current_page - 1) * 10).Take(10).ToList();
                currPageTblock.Text = $"{current_page}";
                datagrid.ItemsSource = pagedGames;
            }
        }
        public void LoadUsers()
        {
            using var db = new DbAppontmentClinikContext();
            if (users is not null)
            {
                total_items = users.Count();
                total_pages = (int)Math.Ceiling((double)total_items / 10);
                if (current_page > total_pages && total_pages > 0)
                {
                    current_page = total_pages;
                }
                else if (total_pages == 0)
                {
                    current_page = 1;
                }
                var usersfiltered = users.Select(g => new
                {
                    g.IdMedCard,
                    g.EmailUsers,
                    g.RoleIdUsers,
                    g.SurnameUsers,
                    g.NameUsers,
                    g.DateBirth,
                    g.MedicalPolicy,
                    g.PassportNumber,
                    g.PhoneNumber,
                }).ToList();
                var pagedGames = usersfiltered.Skip((current_page - 1) * 10).Take(10).ToList();
                currPageTblock.Text = $"{current_page}";
                datagrid.ItemsSource = pagedGames;
            }
        }
        private void LoadAppointment()
        {
            using var db = new DbAppontmentClinikContext();
            if (appointment is not null)
            {
                total_items = appointment.Count();
                total_pages = (int)Math.Ceiling((double)total_items / 10);

                if (current_page > total_pages && total_pages > 0)
                {
                    current_page = total_pages;
                }
                else if (total_pages == 0)
                {
                    current_page = 1;
                }

                var appfiltered = appointment.Select(g => new
                {
                    g.IdDoctor,
                    DoctorName = db.Doctors.FirstOrDefault(d => d.IdDoctor == g.IdDoctor)?.SurnameDoctor + " " +
                               db.Doctors.FirstOrDefault(d => d.IdDoctor == g.IdDoctor)?.NameDoctor.Substring(0, 1) + "." +
                               (string.IsNullOrEmpty(db.Doctors.FirstOrDefault(d => d.IdDoctor == g.IdDoctor)?.PatronymicDoctor) ? "" :
                               db.Doctors.FirstOrDefault(d => d.IdDoctor == g.IdDoctor)?.PatronymicDoctor.Substring(0, 1) + "."),
                    PatientName = db.Users.FirstOrDefault(u => u.IdMedCard == g.IdMedCard)?.SurnameUsers + " " + db.Users.FirstOrDefault(u => u.IdMedCard == g.IdMedCard)?.NameUsers,
                    g.DateAppointment,
                    g.TimeAppointment,
                    g.StatusAppointment
                }).ToList();
                var pagedGames = appfiltered.Skip((current_page - 1) * 10).Take(10).ToList();
                currPageTblock.Text = $"{current_page}";
                datagrid.ItemsSource = pagedGames;
            }
        }
        private void LoadSpecialties()
        {
            using var db = new DbAppontmentClinikContext();
            if (specialties is not null)
            {
                total_items = specialties.Count();
                total_pages = (int)Math.Ceiling((double)total_items / 10);
                if (current_page > total_pages && total_pages > 0)
                {
                    current_page = total_pages;
                }
                else if (total_pages == 0)
                {
                    current_page = 1;
                }
                var specialtiesFiltered = specialties.Select(s => new
                {
                    s.IdSpecialty,
                    s.NameSpecialty,
                    s.TimeAccept,
                    s.IconSpecialty
                }).ToList();
                var pagedItems = specialtiesFiltered.Skip((current_page - 1) * 10).Take(10).ToList();
                currPageTblock.Text = $"{current_page}";
                datagrid.ItemsSource = pagedItems;
            }
        }
        private void LoadSchedules()
        {
            using var db = new DbAppontmentClinikContext();
            if (schedules is not null)
            {
                total_items = schedules.Count();
                total_pages = (int)Math.Ceiling((double)total_items / 10);

                if (current_page > total_pages && total_pages > 0)
                {
                    current_page = total_pages;
                }
                else if (total_pages == 0)
                {
                    current_page = 1;
                }
                var schedulesFiltered = schedules.Select(s => new
                {
                    s.IdSсhedule,
                    s.IdDoctor,
                    DoctorName = db.Doctors.FirstOrDefault(d => d.IdDoctor == s.IdDoctor)?.SurnameDoctor + " " +
                               db.Doctors.FirstOrDefault(d => d.IdDoctor == s.IdDoctor)?.NameDoctor.Substring(0, 1) + "." +
                               (string.IsNullOrEmpty(db.Doctors.FirstOrDefault(d => d.IdDoctor == s.IdDoctor)?.PatronymicDoctor) ? "" :
                               db.Doctors.FirstOrDefault(d => d.IdDoctor == s.IdDoctor)?.PatronymicDoctor.Substring(0, 1) + "."),
                    s.DayWeek,
                    s.TimeStart,
                    s.TimeEnd
                }).ToList();

                var pagedItems = schedulesFiltered.Skip((current_page - 1) * 10).Take(10).ToList();
                currPageTblock.Text = $"{current_page}";
                datagrid.ItemsSource = pagedItems;
            }
        }
        public void LoadData()
        {
            var showButtons = new[] { 0, 1, 3, 4 }.Contains(cboxMode.SelectedIndex);
            if (btnAdd != null)
            {
                btnAdd.Visibility = showButtons ? Visibility.Visible : Visibility.Collapsed;
                btnEdit.Visibility = showButtons ? Visibility.Visible : Visibility.Collapsed;
                btnDelete.Visibility = showButtons ? Visibility.Visible : Visibility.Collapsed;
            }
            switch (cboxMode.SelectedIndex)
            {
                case 0: LoadDoctors(); break;
                case 1: LoadUsers(); break;
                case 2: LoadAppointment(); break;
                case 3: LoadSchedules(); break;
                case 4: LoadSpecialties(); break;
            }
        }
        private void arrowLeft_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (current_page > 1){ current_page--; LoadData(); }
        }
        private void arrowRight_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            int totalpages = 0;
            switch (cboxMode.SelectedIndex)
            {
                case 0: // Врачи
                    totalpages = (int)Math.Ceiling((double)doctors.Count / 10);
                    break;
                case 1: // Пользователи
                    totalpages = (int)Math.Ceiling((double)users.Count / 10);
                    break;
                case 2: // Записи на приём
                    totalpages = (int)Math.Ceiling((double)appointment.Count / 10);
                    break;
                case 3: // График работы
                    totalpages = (int)Math.Ceiling((double)schedules.Count / 10);
                    break;
                case 4: // Специальности
                    totalpages = (int)Math.Ceiling((double)specialties.Count / 10);
                    break;
            }
            if (current_page < totalpages){current_page++; LoadData(); }
        }

        private void cboxMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadData();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is ClinikMainWindow mainWindow)
            {
                switch (cboxMode.SelectedIndex)
                {
                    case 0: // Врачи
                        mainWindow.mainframe.NavigationService.Navigate(new AddRedactDoctors(_user));
                        break;
                    case 1: // Пользователи
                        mainWindow.mainframe.NavigationService.Navigate(new AddRedactUsers(_user));
                        break;
                    case 4: // Специальности
                        mainWindow.mainframe.NavigationService.Navigate(new AddRedactSpecialities(_user));
                        break;
                    case 3: // График работы
                        mainWindow.mainframe.NavigationService.Navigate(new AddRedactSched(_user));
                        break;
                }
            }
        }
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is ClinikMainWindow mainWindow && datagrid.SelectedItem != null)
            {
                try
                {
                    dynamic selectedItem = datagrid.SelectedItem;
                    switch (cboxMode.SelectedIndex)
                    {
                        case 0: // Врачи
                            var doctorIdProperty = selectedItem.GetType().GetProperty("IdDoctor");
                            if (doctorIdProperty != null)
                            {
                                object idValue = doctorIdProperty.GetValue(selectedItem);
                                if (idValue != null && int.TryParse(idValue.ToString(), out int doctorId))
                                {
                                    var doctor = doctors.FirstOrDefault(d => d.IdDoctor == doctorId);
                                    if (doctor != null)
                                    {
                                        mainWindow.mainframe.Navigate(new AddRedactDoctors(doctor, _user));
                                    }
                                    else
                                    {
                                        MessageBox.Show("Врач не найден в базе данных", "Ошибка",
                                            MessageBoxButton.OK, MessageBoxImage.Warning);
                                    }
                                }
                            }
                            break;
                        case 1: // Пользователи
                            var userIdProperty = selectedItem.GetType().GetProperty("IdMedCard");
                            if (userIdProperty != null)
                            {
                                object idValue = userIdProperty.GetValue(selectedItem);
                                if (idValue != null && int.TryParse(idValue.ToString(), out int userId))
                                {
                                    using (var db = new DbAppontmentClinikContext())
                                    {
                                        var user = db.Users.FirstOrDefault(u => u.IdMedCard == userId);
                                        if (user != null)
                                        {
                                            mainWindow.mainframe.Navigate(new AddRedactUsers(user, _user));
                                        }
                                        else
                                        {
                                            MessageBox.Show("Пользователь не найден в базе данных", "Ошибка",
                                                MessageBoxButton.OK, MessageBoxImage.Warning);
                                        }
                                    }
                                }
                            }
                            break;
                        case 4: // Специальности
                            var specialityIdProperty = selectedItem.GetType().GetProperty("IdSpecialty");
                            if (specialityIdProperty != null)
                            {
                                object idValue = specialityIdProperty.GetValue(selectedItem);
                                if (idValue != null && int.TryParse(idValue.ToString(), out int specialityId))
                                {
                                    using (var db = new DbAppontmentClinikContext())
                                    {
                                        var speciality = db.Specialties.FirstOrDefault(s => s.IdSpecialty == specialityId);
                                        if (speciality != null)
                                        {
                                            mainWindow.mainframe.Navigate(new AddRedactSpecialities(speciality, _user));
                                        }
                                        else
                                        {
                                            MessageBox.Show("Специальность не найдена в базе данных", "Ошибка",
                                                MessageBoxButton.OK, MessageBoxImage.Warning);
                                        }
                                    }
                                }
                            }
                            break;
                        case 3: // График работы
                            var scheduleIdProperty = selectedItem.GetType().GetProperty("IdSсhedule");
                            if (scheduleIdProperty != null)
                            {
                                object idValue = scheduleIdProperty.GetValue(selectedItem);
                                if (idValue != null && int.TryParse(idValue.ToString(), out int scheduleId))
                                {
                                    using (var db = new DbAppontmentClinikContext())
                                    {
                                        var schedule = db.Schedules
                                            .Include(s => s.IdDoctorNavigation)
                                            .FirstOrDefault(s => s.IdSсhedule == scheduleId);

                                        if (schedule != null)
                                        {
                                            mainWindow.mainframe.Navigate(new AddRedactSched(schedule, _user));
                                        }
                                        else
                                        {
                                            MessageBox.Show("График работы не найден в базе данных", "Ошибка",
                                                MessageBoxButton.OK, MessageBoxImage.Warning);
                                        }
                                    }
                                }
                            }
                            break;
                        default:
                            MessageBox.Show("Режим редактирования не поддерживается для этого типа данных", "Информация",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при редактировании: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
