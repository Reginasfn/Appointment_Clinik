using Main_project.Models;
using System.Windows;
using System.Windows.Controls;

namespace Main_project.Views
{
    public partial class AddRedactUsers : Page
    {
        private User _currentUser;
        private User _editingUser;
        private bool _isEditMode;
        private List<string> _roles = new List<string> { "Администратор", "Пользователь" };
        public AddRedactUsers(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _isEditMode = false;
            LoadData();
            this.DataContext = this;
        }
        public AddRedactUsers(User userToEdit, User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _editingUser = userToEdit;
            _isEditMode = true;
            LoadData();
            FillUserData();
            this.DataContext = this;
        }
        private void LoadData()
        {
            try
            {
                cmbRole.ItemsSource = _roles;
                if (!_isEditMode)
                {
                    cmbRole.SelectedIndex = 1;
                    dpBirthDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void FillUserData()
        {
            if (_editingUser != null)
            {
                txtSurname.Text = _editingUser.SurnameUsers;
                txtName.Text = _editingUser.NameUsers;
                txtEmail.Text = _editingUser.EmailUsers;
                if (_editingUser.DateBirth.HasValue)
                {
                    dpBirthDate.Text = _editingUser.DateBirth.Value.ToString("dd/MM/yyyy");
                }
                cmbRole.SelectedItem = _editingUser.RoleIdUsers;
                txtPhone.Text = _editingUser.PhoneNumber;
                txtMedicalPolicy.Text = _editingUser.MedicalPolicy;
                txtPassport.Text = _editingUser.PassportNumber;
                btnSave.Content = "Сохранить изменения";
                Title = "Редактирование пользователя";
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
                if (string.IsNullOrWhiteSpace(txtSurname.Text) ||
                    string.IsNullOrWhiteSpace(txtName.Text) ||
                    string.IsNullOrWhiteSpace(txtEmail.Text) ||
                    cmbRole.SelectedItem == null ||
                    string.IsNullOrWhiteSpace(dpBirthDate.Text) ||
                    !IsValidDate(dpBirthDate.Text))
                {
                    MessageBox.Show("Пожалуйста, заполните все обязательные поля корректно!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (!DateTime.TryParseExact(dpBirthDate.Text, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime birthDate))
                {
                    MessageBox.Show("Пожалуйста, введите корректную дату рождения в формате ДД/ММ/ГГГГ!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                using (var db = new DbAppontmentClinikContext())
                {
                    if (_isEditMode)
                    {
                        var user = db.Users.FirstOrDefault(u => u.IdMedCard == _editingUser.IdMedCard);
                        if (user == null)
                        {
                            MessageBox.Show("Пользователь не найден в базе данных!", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        user.SurnameUsers = txtSurname.Text;
                        user.NameUsers = txtName.Text;
                        user.EmailUsers = txtEmail.Text;
                        user.RoleIdUsers = cmbRole.SelectedItem.ToString();
                        user.PhoneNumber = txtPhone.Text;
                        user.MedicalPolicy = txtMedicalPolicy.Text;
                        user.PassportNumber = txtPassport.Text;
                        user.DateBirth = DateOnly.FromDateTime(birthDate);

                        db.SaveChanges();

                        MessageBox.Show("Данные пользователя успешно обновлены!", "Успех",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        var newUser = new User
                        {
                            SurnameUsers = txtSurname.Text,
                            NameUsers = txtName.Text,
                            EmailUsers = txtEmail.Text,
                            RoleIdUsers = cmbRole.SelectedItem.ToString(),
                            PhoneNumber = txtPhone.Text,
                            MedicalPolicy = txtMedicalPolicy.Text,
                            PassportNumber = txtPassport.Text,
                            DateBirth = DateOnly.FromDateTime(birthDate)
                        };
                        db.Users.Add(newUser);
                        db.SaveChanges();
                        MessageBox.Show("Новый пользователь успешно добавлен!", "Успех",
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
        private bool IsValidDate(string dateString)
        {
            return DateTime.TryParseExact(dateString, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out _);
        }
    }
}