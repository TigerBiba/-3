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
using Практическая_3.Pages;
using Практическая_3.Services;

namespace Практическая_3.Pages
{
    /// <summary>
    /// Логика взаимодействия для Autho.xaml
    /// </summary>
    public partial class Autho : Page
    {
        int click;
        string role = null;


        public Autho()
        {
            InitializeComponent();
            click = 0;
        }

        private void txtbLogin_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnEnterGuests_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Client(""));
        }
        private void GenerateCapctcha()
        {
            tbCapcha.Visibility = Visibility.Visible;
            tblCaptcha.Visibility = Visibility.Visible;

            string capctchaText = CaptchaGenerator.GenerateCaptchaText(6);
            tblCaptcha.Text = capctchaText;
            tblCaptcha.TextDecorations = TextDecorations.Strikethrough;
        }


        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            click += 1;
            string login1 = txtbLogin.Text.Trim();
            string password = Hash.HashPassword(pswbPassword.Password.Trim());
            HospitalProEntities1 db = Helper.GetContext();

            var user = db.Login.FirstOrDefault(x => x.login1 == login1 && x.password == password);
            if (click == 1)
             {
                 if (user != null)
                 {
                    var isStaff = db.Staff.FirstOrDefault(x => x.ID_login == user.ID_login);
                    var isPatient = db.Patient.FirstOrDefault(x => x.ID_login == user.ID_login);
                    if (isStaff != null)
                    {
                        role = "Работник";
                        GenerateCapctcha();
                    }
                    else if (isPatient != null)
                    {
                        role = "Пациент";
                        GenerateCapctcha();
                    }
                    else
                    {
                        MessageBox.Show("Вы ввели неправильный логин или пароль");
                        txtbLogin.Clear();
                        pswbPassword.Clear();
                        click = 0;
                    }
                 }
                else
                {
                    MessageBox.Show("Вы ввели ничего не ввели!");
                    click = 0;
                }
             }
             else if (click > 1)
             {
                 if (user != null && tbCapcha.Text == tblCaptcha.Text && role != null)
                 {
                     MessageBox.Show("Вы вошли под: " + role);
                     LoadPage(role, user);
                     txtbLogin.Clear();
                     pswbPassword.Clear();
                     tbCapcha.Clear();
                     login1 = null;
                     password = null;
                     tbCapcha.Visibility = Visibility.Hidden;
                     tblCaptcha.Visibility = Visibility.Hidden;
                     click = 0;
                 }
                 else
                 {
                     MessageBox.Show("Введите данные заново!");
                     txtbLogin.Clear();
                     pswbPassword.Clear();
                     tbCapcha.Clear();
                     login1 = null;
                     password = null;
                     tbCapcha.Visibility = Visibility.Hidden;
                     tblCaptcha.Visibility = Visibility.Hidden;
                     click = 0;
                 }
             }
        }
        private void LoadPage(string role, Login login1)
        {

            switch (role)
            {
                case "Пациент":
                    NavigationService.Navigate(new Client(role));
                    break;
                case "Работник":
                    NavigationService.Navigate(new Staff(role));
                    break;
            }
        }
    }
}
