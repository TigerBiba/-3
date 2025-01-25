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
using System.ComponentModel.DataAnnotations;

namespace Практическая_3.Pages
{
    /// <summary>
    /// Логика взаимодействия для StaffChange.xaml
    /// </summary>
    public partial class StaffChange : Page
    {
        private Staff selectedStaff;
        private string newImagePath;
        public StaffChange(Staff staff)
        {
            InitializeComponent();
            HospitalProEntities1 db = Helper.GetContext();

            selectedStaff = staff;

            tbSurname.Text = staff.secondname;
            tbFirstname.Text = staff.firstname;
            tbEmail.Text = staff.email;
            imgStaff.Source = new BitmapImage(new Uri(staff.photo));
            tbExp.Text = staff.work_experience;

            cbSpeciality.Items.Add("Глав врач");
            cbSpeciality.Items.Add("Уборщик");
            cbSpeciality.Items.Add("Медсестра"); 
            cbSpeciality.Items.Add("Администратор");

            cbSpeciality.SelectedIndex = staff.speciality-1;

            var staffLogin = db.Login.FirstOrDefault(x => x.ID_login == staff.ID_login);
            tbLogin.Text = staffLogin.login1.ToString();
            tbPassword.Text = staffLogin.password.ToString();
        
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            HospitalProEntities1 db = Helper.GetContext();

            var selectedStaffInformation = db.Staff.FirstOrDefault(x => x.ID_staff == selectedStaff.ID_staff);
            var selectedStaffLogin = db.Login.FirstOrDefault(x => x.ID_login == selectedStaffInformation.ID_login);

            if (selectedStaffInformation != null)
            {
                string exp = tbExp.Text;
                selectedStaffInformation.secondname = tbSurname.Text;
                selectedStaffInformation.firstname = tbFirstname.Text;
                selectedStaffInformation.email = tbEmail.Text;
                selectedStaffInformation.work_experience = tbExp.Text;

                selectedStaffInformation.speciality = cbSpeciality.SelectedIndex+1;
                if (newImagePath != null)
                {
                    selectedStaffInformation.photo = newImagePath;
                }
                else
                {
                    selectedStaffInformation.photo = selectedStaffInformation.photo;
                }

                selectedStaffLogin.login1 = tbLogin.Text;
                selectedStaffLogin.password = Hash.HashPassword(tbPassword.Text);

                var context = new ValidationContext(selectedStaffLogin);
                var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
                if (Validator.TryValidateObject(selectedStaffLogin, context, results, true))
                {
                    var contextStaffInformation = new ValidationContext(selectedStaffInformation);

                    if (Validator.TryValidateObject(selectedStaffInformation, contextStaffInformation, results, true))
                    {
                        db.SaveChanges();
                        NavigationService.GoBack();
                    }
                }
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
