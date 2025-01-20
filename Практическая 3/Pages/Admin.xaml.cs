using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /*public struct StaffStruct
    {
        public int ID_staff { get; set; }
        public int speciality { get; set; }
        public int work_experience { get; set; }
        public int ID_departament { get; set; }
        public string firstname { get; set; }
        public string secondname { get; set; }
        public string photo { get; set; }
        public string email { get; set; }
    }*/

    public partial class Admin : Page
    {
        int click;
        public Admin(string firstname, string lastname)
        {
            InitializeComponent();

            var staff = Helper.GetContext().Staff.ToList();
            LViewStaff.ItemsSource = staff;
            LViewStaff.SelectedItem = null;
        }

        private void btnChangeUser_Click(object sender, RoutedEventArgs e)
        {
            if (LViewStaff.SelectedItem is Staff selectedStaff)
                {
                    NavigationService.Navigate(new StaffChange(selectedStaff));
                }
        }

        private void btnNewStaff_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddNewStaffPage());
        }
    }
}
