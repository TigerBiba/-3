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
    /// Логика взаимодействия для AddNewStaffPage.xaml
    /// </summary>
    public partial class AddNewStaffPage : Page
    {
        private Staff selectedStaff;
        private string newImagePath;

        public AddNewStaffPage()
        {
            InitializeComponent();
            
            HospitalProEntities1 db = Helper.GetContext();

            cbSpeciality.Items.Add("Глав врач");
            cbSpeciality.Items.Add("Уборщик");
            cbSpeciality.Items.Add("Медсестра");
            cbSpeciality.Items.Add("Администратор");

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            HospitalProEntities1 db = Helper.GetContext();

            Staff staff = new Staff();

            Login staffLogin = new Login();

            var currentID = db.Staff.OrderByDescending(x => x.ID_staff).First().ID_staff + 1;

            var loginID = db.Login.OrderByDescending(x => x.ID_login).First().ID_login + 1;


                if (tbSurname.Text != null && tbFirstname.Text != null && tbEmail.Text != null && tbLogin.Text != null && tbPassword.Text != null)
                {
                    staff.ID_staff = currentID;
                    staff.secondname = tbSurname.Text;
                    staff.firstname = tbFirstname.Text;
                    staff.email = tbEmail.Text;
                    staff.ID_staff = loginID;
                    staff.ID_departament = 1;

                    staff.speciality = cbSpeciality.SelectedIndex + 1;
                    if (newImagePath != null)
                    {
                        staff.photo = newImagePath;
                    }
                    else
                    {
                        staff.photo = null;
                    }

                    staffLogin.login1 = tbLogin.Text;
                    staffLogin.password = Hash.HashPassword(tbPassword.Text);

                    db.SaveChanges();

                    NavigationService.Navigate(new Admin(staff.firstname, staff.secondname));
                }
                else
                {
                    MessageBox.Show("Ошибка, все поля должны быть заполнены");
                }
        }

        private void btnAddImage_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.DefaultExt = ".png";

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                string filePath = dialog.FileName;

                try
                {
                    filePath.Replace("\\", "\\\\");
                    imgStaff.Source = new BitmapImage(new Uri(filePath));
                    newImagePath = filePath;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении фото: {ex.Message}");
                }
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            tbFirstname.Text = "";
            tbSurname.Text = "";
            tbLogin.Text = "";
            tbPassword.Text = "";
            tbEmail.Text = "";

            cbSpeciality.SelectedIndex = 1;
        }
        
    }
}
