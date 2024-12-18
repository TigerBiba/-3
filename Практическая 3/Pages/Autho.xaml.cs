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
        string firstname = null;
        string secondname = null;
        
        DispatcherTimer dt = new DispatcherTimer();

        TimeSpan now = new TimeSpan();

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
            NavigationService.Navigate(new Guest());
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
                        firstname = isStaff.firstname;
                        secondname = isStaff.secondname;

                        LoadPage(role, firstname, secondname);
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
                        firstname = isPatient.firstname;
                        secondname = isPatient.secondname;

                        LoadPage(role, firstname, secondname);
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
                        firstname = isStaff.firstname;
                        secondname = isStaff.secondname;
                        LoadPage(role, firstname, secondname);
                    }
                    else if (isPatient != null)
                    {
                        role = "Пациент";
                        firstname = isPatient.firstname;
                        secondname = isPatient.secondname;
                        LoadPage(role, firstname, secondname);
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
                        firstname = isStaff.firstname;
                        secondname = isStaff.secondname;
                        LoadPage(role, firstname, secondname);
                    }
                    else if (isPatient != null)
                    {
                        role = "Пациент";
                        firstname = isPatient.firstname;
                        secondname = isPatient.secondname;
                        LoadPage(role, firstname, secondname);
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
        private void LoadPage(string role, string firstname, string lastname)
        {
            TimeSpan userTime = (DateTime.Now.TimeOfDay);
            TimeSpan morning = new TimeSpan(10, 0, 0);
            TimeSpan evening = new TimeSpan(19, 0, 0);

            if (userTime < morning || userTime > evening && role == "Работник")
            {
                MessageBox.Show("Сейчас время для отдыха");
            }
            else if (firstname != null && lastname != null)
            {
                switch (role)
                {
                    case "Пациент":
                        NavigationService.Navigate(new Client(firstname, lastname));
                        break;
                    case "Работник":
                        NavigationService.Navigate(new Staff(firstname, lastname));
                        break;
                }
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
