using Main_project.Models;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Main_project.Views
{
    public partial class AddRedactSpecialities : Page
    {
        private User _currentUser;
        private Specialty _editingSpecialty;
        private bool _isEditMode;
        public AddRedactSpecialities(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _isEditMode = false;
            this.DataContext = this;
        }
        public AddRedactSpecialities(Specialty specialtyToEdit, User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _editingSpecialty = specialtyToEdit;
            _isEditMode = true;
            FillSpecialtyData();
            this.DataContext = this;
        }
        private void FillSpecialtyData()
        {
            if (_editingSpecialty != null)
            {
                txtName.Text = _editingSpecialty.NameSpecialty;
                txtTimeAccept.Text = _editingSpecialty.TimeAccept?.ToString();

                btnSave.Content = "Сохранить изменения";
                Title = "Редактирование специальности";
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
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("Пожалуйста, укажите название специальности!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (!int.TryParse(txtTimeAccept.Text, out int timeAccept) || timeAccept <= 0)
                {
                    MessageBox.Show("Пожалуйста, укажите корректное время приема (положительное число)!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                using (var db = new DbAppontmentClinikContext())
                {
                    if (_isEditMode)
                    {
                        var specialty = db.Specialties.Find(_editingSpecialty.IdSpecialty);
                        if (specialty == null)
                        {
                            MessageBox.Show("Специальность не найдена в базе данных!", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        specialty.NameSpecialty = txtName.Text;
                        specialty.TimeAccept = timeAccept;
                        specialty.IconSpecialty = null;
                        db.SaveChanges();
                        MessageBox.Show("Данные специальности успешно обновлены!", "Успех",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        var newSpecialty = new Specialty
                        {
                            NameSpecialty = txtName.Text,
                            TimeAccept = timeAccept,
                            IconSpecialty = null
                        };
                        db.Specialties.Add(newSpecialty);
                        db.SaveChanges();
                        MessageBox.Show("Новая специальность успешно добавлена!", "Успех",
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