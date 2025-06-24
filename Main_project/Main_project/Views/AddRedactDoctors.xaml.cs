using Main_project.Models;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Main_project.Views
{
    public partial class AddRedactDoctors : Page
    {
        private User _user;
        private Doctor _doctor;
        private bool _isEditMode;
        private List<Specialty> _specialties;
        private List<string> _statuses = new List<string>
        {
            "По графику",
            "В отпуске",
            "На больничном",
        };
        public AddRedactDoctors(User user)
        {
            InitializeComponent();
            _user = user;
            _isEditMode = false;
            LoadData();
            this.DataContext = this;
        }
        public AddRedactDoctors(Doctor doctor, User user)
        {
            InitializeComponent();
            _user = user;
            _doctor = doctor;
            _isEditMode = true;
            LoadData();
            FillDoctorData();
            this.DataContext = this;
        }

        private void LoadData()
        {
            try
            {
                using (var db = new DbAppontmentClinikContext())
                {
                    _specialties = db.Specialties.ToList();
                    cmbSpecialty.ItemsSource = _specialties;
                    cmbSpecialty.DisplayMemberPath = "NameSpecialty";
                    cmbSpecialty.SelectedValuePath = "IdSpecialty";
                }
                cmbStatus.ItemsSource = _statuses;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FillDoctorData()
        {
            if (_doctor != null)
            {
                txtSurname.Text = _doctor.SurnameDoctor;
                txtName.Text = _doctor.NameDoctor;
                txtPatronymic.Text = _doctor.PatronymicDoctor;
                cmbSpecialty.SelectedValue = _doctor.IdSpecialty;
                txtEmail.Text = _doctor.EmailDoctor;
                txtPhone.Text = _doctor.PhoneNumberDoctor;
                txtExperience.Text = _doctor.MedicalExperience?.ToString();
                txtCabinet.Text = _doctor.CabinetNumber;
                cmbStatus.SelectedItem = _doctor.StatusWork;
                if (!string.IsNullOrEmpty(_doctor.IconDoctor))
                {
                    string imagePath = _doctor.DisplayIconDoctor;
                    if (File.Exists(imagePath))
                    {
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(imagePath);
                        bitmap.EndInit();
                        doctorImage.Source = bitmap;
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is ClinikMainWindow mainWindow)
            {
                mainWindow.mainframe.Navigate(new Admin_panel(_user));
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtSurname.Text) || string.IsNullOrWhiteSpace(txtName.Text) || cmbSpecialty.SelectedItem == null || string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtPhone.Text))
                {
                    MessageBox.Show("Пожалуйста, заполните все обязательные поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(txtExperience.Text, out int experience) || experience < 0)
                {
                    MessageBox.Show("Пожалуйста, введите корректный стаж работы (целое положительное число)!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                using (var db = new DbAppontmentClinikContext())
                {
                    if (_isEditMode)
                    {
                        Doctor doctor = db.Doctors.FirstOrDefault(d => d.IdDoctor == _doctor.IdDoctor);
                        if (doctor == null)
                        {
                            MessageBox.Show("Врач не найден в базе данных!", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        doctor.SurnameDoctor = txtSurname.Text;
                        doctor.NameDoctor = txtName.Text;
                        doctor.PatronymicDoctor = txtPatronymic.Text;
                        doctor.IdSpecialty = (int)cmbSpecialty.SelectedValue;
                        doctor.EmailDoctor = txtEmail.Text;
                        doctor.PhoneNumberDoctor = txtPhone.Text;
                        doctor.MedicalExperience = experience;
                        doctor.CabinetNumber = txtCabinet.Text;
                        doctor.StatusWork = cmbStatus.SelectedItem?.ToString();
                        db.SaveChanges();
                        MessageBox.Show("Данные врача успешно обновлены!", "Успех",MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        var newdoctor = new Doctor
                        {
                            SurnameDoctor = txtSurname.Text,
                            NameDoctor = txtName.Text,
                            PatronymicDoctor = txtPatronymic.Text,
                            IdSpecialty = (int)cmbSpecialty.SelectedValue,
                            EmailDoctor = txtEmail.Text,
                            PhoneNumberDoctor = txtPhone.Text,
                            MedicalExperience = experience,
                            CabinetNumber = txtCabinet.Text,
                            StatusWork = cmbStatus.SelectedItem?.ToString(),
                            IconDoctor = "default_doctor.png",
                        };
                        db.Doctors.Add(newdoctor);
                        db.SaveChanges();
                        MessageBox.Show("Новый врач успешно добавлен!", "Успех",MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    if (Application.Current.MainWindow is ClinikMainWindow mainWindow)
                    {
                        mainWindow.mainframe.Navigate(new Admin_panel(_user));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}", "Ошибка",MessageBoxButton.OK, MessageBoxImage.Error);
            }
        
        }
    }
}