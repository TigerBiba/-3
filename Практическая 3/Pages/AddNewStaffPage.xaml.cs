using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

                    staff.ID_staff = currentID;
                    staff.secondname = tbSurname.Text;
                    staff.firstname = tbFirstname.Text;
                    staff.email = tbEmail.Text;
                    staff.ID_login = loginID;
                    staff.ID_departament = 1;
                    staff.work_experience = tbExp.Text;

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

                    db.Staff.Add(staff);
                    db.Login.Add(staffLogin);

                    var context = new ValidationContext(staffLogin);
                    var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
                
                if (!Validator.TryValidateObject(staffLogin, context, results, true))
                {
                    var contextStaffInformation = new ValidationContext(staff);
                    foreach (var error in results)
                    {
                        MessageBox.Show(error.ErrorMessage);
                        return;
                    }

                    if (!Validator.TryValidateObject(staff, contextStaffInformation, results, true))
                    {
                        foreach (var error in results)
                        {
                            MessageBox.Show(error.ErrorMessage);
                            return;
                        }
                    } else if (Validator.TryValidateObject(staff, contextStaffInformation, results, true))
                    {
                        {
                            db.SaveChanges();
                            NavigationService.Navigate(new Admin(staff.firstname, staff.secondname));
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
