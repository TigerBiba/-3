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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Практическая_3.Models;
using Практическая_3.Services;

namespace Практическая_3.Pages
{
    /// <summary>
    /// Логика взаимодействия для TwoAutoPage.xaml
    /// </summary>
    public partial class TwoAutoPage : Page
    {
        private int emailCode;
        private string userRole;
        Login user;

        public TwoAutoPage(string role, Login login, int code)
        {
            InitializeComponent();
            emailCode = code;
            userRole = role;
            user = login;
        }


        private void LoadPage(string role, Login login)
        {
            HospitalProEntities db = Helper.GetContext();

            TimeSpan userTime = (DateTime.Now.TimeOfDay);
            TimeSpan morning = new TimeSpan(8, 0, 0);
            TimeSpan evening = new TimeSpan(19, 0, 0);

            if (userTime < morning && role == "Работник" || userTime > evening && role == "Работник")
            {
                MessageBox.Show("Сейчас время для отдыха");
            }
            else
            {
                switch (role)
                {
                    case "Пациент":
                        Patient patient = db.Patient.FirstOrDefault(x => x.ID_login == login.ID_login);

                        NavigationService.Navigate(new Client(patient));

                        break;
                    case "Работник":
                        Staff staff = db.Staff.FirstOrDefault(x => x.ID_login == login.ID_login);

                        NavigationService.Navigate(new StaffPage(staff));

                        break;
                    case "Админ":
                        Staff admin = db.Staff.FirstOrDefault(x => x.ID_login == login.ID_login);

                        NavigationService.Navigate(new Admin(admin));

                        break;
                }
            }
        }

        private void btnCheckCode_Click(object sender, RoutedEventArgs e)
        {
            if (tbCode.Text == emailCode.ToString())
            {
                MessageBox.Show("Успешный вход в аккаунт");
                LoadPage(userRole, user);
            }
            else
            {
                MessageBox.Show("Неверный код");
            }
        }
    }
}
