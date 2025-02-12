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
    /// Логика взаимодействия для ChangePasswordPage.xaml
    /// </summary>
    public partial class ChangePasswordPage : Page
    {
        private int code;
        public ChangePasswordPage()
        {
            InitializeComponent();
        }

        private void btnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string userEmail = tbEmail.Text;

                var dbContext = new HospitalProEntities();
                var emailService = new EmailService("smtp.mail.ru", 587, "elonmuskpro@mail.ru", "yKpwiCcg6Dhtibb4dbu5");

                Random rnd = new Random();
                code = rnd.Next(100000, 999999);

                var userService = new UserService(dbContext, emailService);

                userService.RequestPasswordReset(userEmail, code);

                Swipe();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex}");
            }
        }

        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            if (code.ToString() == tbCode.Text)
            {
                HospitalProEntities db = Helper.GetContext();

                var patient = db.Patient.FirstOrDefault(x => x.email == tbEmail.Text);
                var staff = db.Staff.FirstOrDefault(x => x.email == tbEmail.Text);

                if (patient != null)
                {
                    var patientLogin = db.Login.FirstOrDefault(x => x.ID_login == patient.ID_login);

                    patientLogin.password = Hash.HashPassword(tbNewPassword.Text);
                }
                else if (staff != null)
                {
                    var staffLogin = db.Login.FirstOrDefault(x => x.ID_login == staff.ID_login);

                    staffLogin.password = Hash.HashPassword(tbNewPassword.Text);
                }
                db.SaveChanges();
                NavigationService.Navigate(new Autho());
            }
            else 
            {
                MessageBox.Show("Неправильный код");
            }
        }
        private void Swipe()
        {
            tbEmail.Visibility = Visibility.Hidden;
            txtbEmail1.Visibility = Visibility.Hidden;
            btnChangePassword.Visibility = Visibility.Hidden;
            txtbNewPassword.Visibility = Visibility.Visible;
            tbNewPassword.Visibility = Visibility.Visible;
            tbCode.Visibility = Visibility.Visible;
            txtbCode.Visibility = Visibility.Visible;
            btnAccept.Visibility = Visibility.Visible;
        }
    }
}
