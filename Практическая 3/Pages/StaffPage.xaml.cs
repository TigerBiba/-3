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
    /// Логика взаимодействия для StaffPage.xaml
    /// </summary>
    public partial class StaffPage : Page
    {
        public StaffPage(string firstname, string secondname)
        {
            InitializeComponent();
            timeNow(firstname, secondname);
            HospitalProEntities1 db = Helper.GetContext();
        }
        private void timeNow(string firstname, string lastname)
        {
            TimeSpan userTime = (DateTime.Now.TimeOfDay);
            TimeSpan morning = new TimeSpan(10, 0, 0);
            TimeSpan day = new TimeSpan(12, 0, 0);
            TimeSpan evening = new TimeSpan(17, 0, 0);
            TimeSpan deepEvening = new TimeSpan(19, 0, 0);

            if (userTime >= morning && userTime < day)
            {
                tbStaff.Text = ($"Доброе утро, {firstname} {lastname}");
            }
            else if (userTime >= day && userTime < evening)
            {
                tbStaff.Text = ($"Добрый день, {firstname} {lastname}");
            }
            else if (userTime >= evening && userTime < deepEvening)
            {
                tbStaff.Text = ($"Добрый вечер, {firstname} {lastname}");
            }
        }
    }
}
