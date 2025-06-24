using Main_project.Models;
using Main_project.Scripts;
using System.Windows;
using System.Windows.Controls;
namespace Main_project.Views
{
    public partial class AdminAuth : Page
    {
        private string _lastVerificationCode;
        User currUser;
        public AdminAuth()
        {
            InitializeComponent();
            ClinikMainWindow mainWindow = Application.Current.MainWindow as ClinikMainWindow;
            mainWindow.Title = "Вход в админ-панель";
        }
        private void code_button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(email_txtbx.Text) || !email_txtbx.Text.Contains("@"))
            {
                MessageBox.Show("Введите корректный email адрес");
                return;
            }
            using var db = new DbAppontmentClinikContext();
            var admin = db.Users.FirstOrDefault(u => u.EmailUsers == email_txtbx.Text && u.RoleIdUsers == "Администратор");
            if (admin == null)
            {
                MessageBox.Show("У вас недостаточно прав для доступа к администраторской панели");
                ClinikMainWindow mainWindow = Application.Current.MainWindow as ClinikMainWindow;
                mainWindow.mainframe.NavigationService.Navigate(new SpecialtiesPage());
                return;
            }
            try
            {
                string verificationCode = Email_code.GenerateCode();
                List<string> emailContent = Email_code.GenerateVerificateMessageAdmin(DateTime.Now, verificationCode);
                this._lastVerificationCode = verificationCode;
                Email_code.SendMessage(email_txtbx.Text, emailContent[0], emailContent[1]);
                MessageBox.Show($"Код подтверждения отправлен на {email_txtbx.Text}\n");
                enter_button.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при отправке кода: {ex.Message}");
            }
        }

        private void enter_button_Click(object sender, RoutedEventArgs e)
        {
            //if (!Verify(code_txtbx.Text))
            //{
            //    MessageBox.Show("Неверный или просроченный код", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return;
            //}
            //else
            //{
                ClinikMainWindow mainWindow = Application.Current.MainWindow as ClinikMainWindow;
                mainWindow.mainframe.NavigationService.Navigate(new Admin_panel(currUser));
            //}
        }
        private bool Verify(string user_code)
        {
            return !string.IsNullOrEmpty(_lastVerificationCode) && _lastVerificationCode == user_code;
        }
    }
}
