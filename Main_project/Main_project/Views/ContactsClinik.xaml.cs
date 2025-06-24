using System.Windows;
using System.Windows.Controls;

namespace Main_project.Views
{
    public partial class ContactsClinik : Page
    {
        public ContactsClinik()
        {
            ClinikMainWindow mainWindow = Application.Current.MainWindow as ClinikMainWindow;
            mainWindow.Title = "Контакты клиники";
            InitializeComponent();
        }
    }
}
