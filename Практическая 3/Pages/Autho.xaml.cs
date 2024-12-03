using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
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
        
        DispatcherTimer dt = new DispatcherTimer();

        static System.Timers.Timer timer;

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
            NavigationService.Navigate(new Client(null));
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
            tbCapcha.Visibility = Visibility.Hidden;
            tblCaptcha.Visibility = Visibility.Hidden;

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
                        MessageBox.Show("Вы вошли под: " + role);
                        LoadPage(role, user);
                        txtbLogin.Clear();
                        pswbPassword.Clear();
                        tbCapcha.Clear();
                        login1 = null;
                        password = null;
                        click = 0;
                    }
                    else if (isPatient != null)
                    {
                        role = "Пациент";
                        MessageBox.Show("Вы вошли под: " + role);
                        LoadPage(role, user);
                        txtbLogin.Clear();
                        pswbPassword.Clear();
                        tbCapcha.Clear();
                        login1 = null;
                        password = null;
                        click = 0;
                    }
                 }
                else
                {
                    MessageBox.Show("Вы ввели неправильный логин или пароль");
                    txtbLogin.Clear();
                    pswbPassword.Clear();
                    GenerateCapctcha();
                }
             }
             else if (click > 1 && click <= 3)
             {

                 if (user != null && tbCapcha.Text == tblCaptcha.Text)
                 {
                    var isStaff = db.Staff.FirstOrDefault(x => x.ID_login == user.ID_login);
                    var isPatient = db.Patient.FirstOrDefault(x => x.ID_login == user.ID_login);
                    if (isStaff != null)
                    {
                        role = "Работник";
                        MessageBox.Show("Вы вошли под: " + role);
                        LoadPage(role, user);
                    }
                    else if (isPatient != null)
                    {
                        role = "Пациент";
                        MessageBox.Show("Вы вошли под: " + role);
                        LoadPage(role, user);
                    }
                    txtbLogin.Clear();
                    pswbPassword.Clear();
                    tbCapcha.Clear();
                    login1 = null;
                    password = null;
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
                     GenerateCapctcha();
                     tbCapcha.Visibility = Visibility.Visible;
                     tblCaptcha.Visibility = Visibility.Visible;
                 }
             }
            else if(click > 3)
            {
                if (user != null && tbCapcha.Text == tblCaptcha.Text)
                {
                    var isStaff = db.Staff.FirstOrDefault(x => x.ID_login == user.ID_login);
                    var isPatient = db.Patient.FirstOrDefault(x => x.ID_login == user.ID_login);
                    if (isStaff != null)
                    {
                        role = "Работник";
                        MessageBox.Show("Вы вошли под: " + role);
                        LoadPage(role, user);
                    }
                    else if (isPatient != null)
                    {
                        role = "Пациент";
                        MessageBox.Show("Вы вошли под: " + role);
                        LoadPage(role, user);
                    }
                    txtbLogin.Clear();
                    pswbPassword.Clear();
                    tbCapcha.Clear();
                    login1 = null;
                    password = null;
                    click = 0;
                }
                else
                {
                    dt.Interval = TimeSpan.FromSeconds(1);
                    dt.Stop();
                    dt.Tick -= dtTicker;

                    lbTimer.Visibility = Visibility.Visible;
                    dt.Tick += dtTicker;
                    txtbLogin.IsEnabled = false;
                    tbCapcha.IsEnabled = false;
                    pswbPassword.IsEnabled = false;
                    dt.Start();

                    MessageBox.Show("НЕПРАВИЛЬНЫЙ ЛОГИН ИЛИ ПАРОЛЬ!!!, без негатива)");

                    txtbLogin.Clear();
                    pswbPassword.Clear();
                    tbCapcha.Clear();
                    login1 = null;
                    password = null;
                    GenerateCapctcha();
                    tbCapcha.Visibility = Visibility.Visible;
                    tblCaptcha.Visibility = Visibility.Visible;
                }
            }
        }
        private void LoadPage(string role, Login login1)
        {

            switch (role)
            {
                case "Пациент":
                    NavigationService.Navigate(new Client(login1));
                    break;
                case "Работник":
                    NavigationService.Navigate(new Staff(login1));
                    break;
            }
        }

        int? second = 10;
        private void dtTicker(object sender, EventArgs e)
        {
            if (second >= 0)
            {
                lbTimer.Content = ($"Вам осталось ждать: {second.ToString()} сек.");

                second--;
            }
            else
            {
                dt.Stop();
                second = 10;
                lbTimer.Visibility = Visibility.Hidden;
                txtbLogin.IsEnabled = true;
                pswbPassword.IsEnabled = true;
                tbCapcha.IsEnabled = true;
            }
        }
    }
}
