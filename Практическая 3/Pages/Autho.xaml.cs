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
    public partial class Autho : Page
    {
        int click;
        string role = null;
        string firstname = null;
        string secondname = null;
        int speciality;
        
        DispatcherTimer dt = new DispatcherTimer();

        static System.Timers.Timer timer;

        public Autho()
        {
            InitializeComponent();
            click = 0;
        }

        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            click += 1;
            string login1 = txtbLogin.Text.Trim();
            string password = Hash.HashPassword(pswbPassword.Password.Trim());
            HospitalProEntities1 db = Helper.GetContext();

            captchaVisibility(false);

            var user = db.Login.FirstOrDefault(x => x.login1 == login1 && x.password == password);
            if (click == 1)
            {
                 if (user != null)
                 {
                    findUser(login1, password);
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
                    findUser(login1, password);
                 }
                 else
                 {
                    MessageBox.Show("Введите данные заново!");

                    Clear(login1, password, clickZero: false);

                    GenerateCapctcha();

                    captchaVisibility(true);
                 }
            }
            else if(click > 3)
            {
                if (user != null && tbCapcha.Text == tblCaptcha.Text)
                {
                    findUser(login1, password);
                }
                else
                {
                    dt.Interval = TimeSpan.FromSeconds(1);
                    dt.Stop();
                    dt.Tick -= dtTicker;

                    lbTimer.Visibility = Visibility.Visible;
                    dt.Tick += dtTicker;

                    Enabled(isEnabled: false);

                    dt.Start();

                    MessageBox.Show("НЕПРАВИЛЬНЫЙ ЛОГИН ИЛИ ПАРОЛЬ!!!, без негатива)");

                    Clear(login1, password, clickZero: false);

                    GenerateCapctcha();
                    captchaVisibility(true);
                }
            }
        }
        private void LoadPage(string role, string firstname, string lastname)
        {
            TimeSpan userTime = (DateTime.Now.TimeOfDay);
            TimeSpan morning = new TimeSpan(10, 0, 0);
            TimeSpan evening = new TimeSpan(19, 0, 0);

            if (userTime < morning && role == "Работник" || userTime > evening && role == "Работник")
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
                        NavigationService.Navigate(new StaffPage(firstname, lastname));
                        break;
                    case "Админ":
                        NavigationService.Navigate(new Admin(firstname, lastname));
                        break;
                }
            }
        }
        private void GenerateCapctcha()
        {
            captchaVisibility(true);

            string capctchaText = CaptchaGenerator.GenerateCaptchaText(6);
            tblCaptcha.Text = capctchaText;
            tblCaptcha.TextDecorations = TextDecorations.Strikethrough;
        }

        int second = 10;
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

                Enabled(isEnabled: true);

                click = 0;

                captchaVisibility(false);
            }
        }
        public void Clear(string login1, string password, bool clickZero)
        {
            if (clickZero == true)
            {
                txtbLogin.Clear();
                pswbPassword.Clear();
                tbCapcha.Clear();
                login1 = null;
                password = null;
                click = 0;
            }
            else if (clickZero == false)
            {
                txtbLogin.Clear();
                pswbPassword.Clear();
                tbCapcha.Clear();
                login1 = null;
                password = null;
            }
        }

        public void Enabled(bool isEnabled)
        {
            if (isEnabled == true)
            {
                txtbLogin.IsEnabled = true;
                pswbPassword.IsEnabled = true;
                tbCapcha.IsEnabled = true;
                btnEnter.IsEnabled = true;
                btnEnterGuests.IsEnabled = true;
            }
            else if (isEnabled == false)
            {
                txtbLogin.IsEnabled = false;
                tbCapcha.IsEnabled = false;
                pswbPassword.IsEnabled = false;
                btnEnter.IsEnabled = false;
                btnEnterGuests.IsEnabled = false;
            }
        }

        public void findUser(string login1, string password) 
        {
            HospitalProEntities1 db = Helper.GetContext();

            var user = db.Login.FirstOrDefault(x => x.login1 == login1 && x.password == password);

            var isStaff = db.Staff.FirstOrDefault(x => x.ID_login == user.ID_login);
            var isPatient = db.Patient.FirstOrDefault(x => x.ID_login == user.ID_login);
            if (isStaff != null)
            {
                speciality = isStaff.speciality;
                if (speciality == 4)
                {
                    role = "Админ";
                    firstname = isStaff.firstname;
                    secondname = isStaff.secondname;
                    LoadPage(role, firstname, secondname);
                }
                else 
                {
                    role = "Работник";
                    firstname = isStaff.firstname;
                    secondname = isStaff.secondname;
                    LoadPage(role, firstname, secondname);
                }
            }
            else if (isPatient != null)
            {
                role = "Пациент";
                firstname = isPatient.firstname;
                secondname = isPatient.secondname;
                LoadPage(role, firstname, secondname);
            }

            Clear(login1, password, true);
        }
        private void captchaVisibility(bool captchaVisible)
        {
            if (captchaVisible == true)
            {
                tbCapcha.Visibility = Visibility.Visible;
                tblCaptcha.Visibility = Visibility.Visible;
            }
            else if (captchaVisible == false)
            {
                tbCapcha.Visibility = Visibility.Hidden;
                tblCaptcha.Visibility = Visibility.Hidden;
            }
        }

        private void btnEnterGuests_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Guest());
        }

        private void txtbLogin_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
