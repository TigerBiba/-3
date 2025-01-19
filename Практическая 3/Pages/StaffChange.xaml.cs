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
    /// Логика взаимодействия для StaffChange.xaml
    /// </summary>
    public partial class StaffChange : Page
    {
        public StaffChange(Staff staff)
        {
            InitializeComponent();
            HospitalProEntities1 db = Helper.GetContext();

            tbSurname.Text = ;
            tbFirstname.Text = staff.firstname;
            tbEmail.Text = staff.email;

            cbSpeciality.Items.Add("Глав врач");
            cbSpeciality.Items.Add("Уборщик");
            cbSpeciality.Items.Add("Медсестра"); 
            cbSpeciality.Items.Add("Администратор");

            cbSpeciality.SelectedIndex = staff.speciality-1;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
