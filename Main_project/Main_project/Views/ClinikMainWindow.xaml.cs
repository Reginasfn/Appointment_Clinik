using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Main_project.Views
{
    /// <summary>
    /// Логика взаимодействия для ClinikMainWindow.xaml
    /// </summary>
    public partial class ClinikMainWindow : Window
    {
        public ClinikMainWindow()
        {
            InitializeComponent();
            mainframe.NavigationService.Navigate(new SpecialtiesPage());
        }

        private void Click_aboutClinik_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            mainframe.NavigationService.Navigate(new AboutClinik());
        }

        private void Click_contactsClinik_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            mainframe.NavigationService.Navigate(new ContactsClinik());
        }

        private void Click_appointmentClinik_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            mainframe.NavigationService.Navigate(new SpecialtiesPage());
        }

        private void mainframe_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

        }
    }
}
