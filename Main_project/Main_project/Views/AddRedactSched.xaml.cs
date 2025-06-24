using Main_project.Models;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;

namespace Main_project.Views
{
    public partial class AddRedactSched : Page
    {
        private User _currentUser;
        private Schedule _editingSchedule;
        private bool _isEditMode;

        public AddRedactSched(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _isEditMode = false;
            LoadDoctors();
            InitializeDayWeekComboBox();
            this.DataContext = this;
        }

        public AddRedactSched(Schedule scheduleToEdit, User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _editingSchedule = scheduleToEdit;
            _isEditMode = true;
            LoadDoctors();
            InitializeDayWeekComboBox();
            FillScheduleData();
            this.DataContext = this;
        }

        private void InitializeDayWeekComboBox()
        {
            cmbDayWeek.Items.Clear();
            cmbDayWeek.Items.Add(new ComboBoxItem() { Content = "Пн", Tag = "Пн" });
            cmbDayWeek.Items.Add(new ComboBoxItem() { Content = "Вт", Tag = "Вт" });
            cmbDayWeek.Items.Add(new ComboBoxItem() { Content = "Ср", Tag = "Ср" });
            cmbDayWeek.Items.Add(new ComboBoxItem() { Content = "Чт", Tag = "Чт" });
            cmbDayWeek.Items.Add(new ComboBoxItem() { Content = "Пт", Tag = "Пт" });
            cmbDayWeek.Items.Add(new ComboBoxItem() { Content = "Сб", Tag = "Сб" });
            cmbDayWeek.Items.Add(new ComboBoxItem() { Content = "Вс", Tag = "Вс" });
        }

        private void LoadDoctors()
        {
            try
            {
                using (var db = new DbAppontmentClinikContext())
                {
                    var doctors = db.Doctors
                        .Include(d => d.IdSpecialtyNavigation)
                        .Select(d => new
                        {
                            d.IdDoctor,
                            DisplayText = $"{d.SurnameDoctor} {d.NameDoctor[0]}.{(!string.IsNullOrEmpty(d.PatronymicDoctor) ? d.PatronymicDoctor[0] + "." : "")} ({d.IdSpecialtyNavigation.NameSpecialty})"
                        })
                        .ToList();

                    cmbDoctor.DisplayMemberPath = "DisplayText";
                    cmbDoctor.SelectedValuePath = "IdDoctor";
                    cmbDoctor.ItemsSource = doctors;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке списка врачей: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FillScheduleData()
        {
            if (_editingSchedule != null)
            {
                cmbDoctor.SelectedValue = _editingSchedule.IdDoctor;

                foreach (ComboBoxItem item in cmbDayWeek.Items)
                {
                    if (item.Tag.ToString() == _editingSchedule.DayWeek)
                    {
                        cmbDayWeek.SelectedItem = item;
                        break;
                    }
                }

                timeStart.Text = _editingSchedule.TimeStart.ToString("HH:mm:ss");
                timeEnd.Text = _editingSchedule.TimeEnd.ToString("HH:mm:ss");

                btnSave.Content = "Сохранить изменения";
                Title = "Редактирование графика работы";
            }
        }

        private bool CheckExistingSchedule(int doctorId, string dayOfWeek, int? excludeScheduleId = null)
        {
            using (var db = new DbAppontmentClinikContext())
            {
                var query = db.Schedules
                    .Where(s => s.IdDoctor == doctorId && s.DayWeek == dayOfWeek);

                if (excludeScheduleId.HasValue)
                {
                    query = query.Where(s => s.IdSсhedule != excludeScheduleId.Value);
                }

                return query.Any();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is ClinikMainWindow mainWindow)
            {
                mainWindow.mainframe.Navigate(new Admin_panel(_currentUser));
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbDoctor.SelectedValue == null)
                {
                    MessageBox.Show("Пожалуйста, выберите врача!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (cmbDayWeek.SelectedItem == null)
                {
                    MessageBox.Show("Пожалуйста, выберите день недели!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(timeStart.Text) || string.IsNullOrWhiteSpace(timeEnd.Text))
                {
                    MessageBox.Show("Пожалуйста, укажите время начала и окончания работы!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!TimeSpan.TryParse(timeStart.Text, out TimeSpan startTime) ||
                    !TimeSpan.TryParse(timeEnd.Text, out TimeSpan endTime))
                {
                    MessageBox.Show("Пожалуйста, укажите время в корректном формате (HH:mm:ss)!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (startTime >= endTime)
                {
                    MessageBox.Show("Время окончания должно быть позже времени начала!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                int doctorId = (int)cmbDoctor.SelectedValue;
                string dayOfWeek = ((ComboBoxItem)cmbDayWeek.SelectedItem).Tag.ToString();
                if (!_isEditMode && CheckExistingSchedule(doctorId, dayOfWeek))
                {
                    MessageBox.Show("У выбранного врача уже есть график в этот день недели!\n" +
                                  "Пожалуйста, выберите другой день или отредактируйте существующий график.",
                                  "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                using (var db = new DbAppontmentClinikContext())
                {
                    if (_isEditMode)
                    {
                        if (CheckExistingSchedule(doctorId, dayOfWeek, _editingSchedule.IdSсhedule))
                        {
                            MessageBox.Show("У выбранного врача уже есть график в этот день недели!\n" +
                                          "Пожалуйста, выберите другой день.",
                                          "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }

                        var schedule = db.Schedules.Find(_editingSchedule.IdSсhedule);
                        if (schedule == null)
                        {
                            MessageBox.Show("График работы не найден в базе данных!", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        schedule.IdDoctor = doctorId;
                        schedule.DayWeek = dayOfWeek;
                        schedule.TimeStart = TimeOnly.FromTimeSpan(startTime);
                        schedule.TimeEnd = TimeOnly.FromTimeSpan(endTime);

                        db.SaveChanges();
                        MessageBox.Show("График работы успешно обновлен!", "Успех",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        var newSchedule = new Schedule
                        {
                            IdDoctor = doctorId,
                            DayWeek = dayOfWeek,
                            TimeStart = TimeOnly.FromTimeSpan(startTime),
                            TimeEnd = TimeOnly.FromTimeSpan(endTime)
                        };

                        db.Schedules.Add(newSchedule);
                        db.SaveChanges();
                        MessageBox.Show("Новый график работы успешно добавлен!", "Успех",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    if (Application.Current.MainWindow is ClinikMainWindow mainWindow)
                    {
                        mainWindow.mainframe.Navigate(new Admin_panel(_currentUser));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}