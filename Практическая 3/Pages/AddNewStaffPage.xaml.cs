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
using Xceed;

using Xceed.Words.NET;

namespace Практическая_3.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddNewStaffPage.xaml
    /// </summary>
    public partial class AddNewStaffPage : Page
    {
        private string newImagePath;
        DateTime dtNow = DateTime.Now;

        public AddNewStaffPage()
        {
            InitializeComponent();
            
            HospitalProEntities db = Helper.GetContext();

            cbSpeciality.Items.Add("Глав врач");
            cbSpeciality.Items.Add("Уборщик");
            cbSpeciality.Items.Add("Медсестра");
            cbSpeciality.Items.Add("Администратор");

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            HospitalProEntities db = Helper.GetContext();
            Staff staff = new Staff();

            Login staffLogin = new Login();

            var currentID = db.Staff.OrderByDescending(x => x.ID_staff).First().ID_staff + 1; //ищет id последнего пользователя и прибавляет 1, для нового пользователя

            var loginID = db.Login.OrderByDescending(x => x.ID_login).First().ID_login + 1;

            long series = Convert.ToInt64(tbPassportSeries.Text);
            long nomber = Convert.ToInt64(tbPassportNomber.Text);

            staff.ID_staff = currentID;
            staff.secondname = tbSurname.Text;
            staff.firstname = tbFirstname.Text;
            staff.email = tbEmail.Text;
            staff.ID_login = loginID;
            staff.ID_departament = 1;
            staff.work_experience = tbExp.Text;
            staff.series = series;
            staff.nomber = nomber;
            staff.issued = tbIssues.Text;

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

            

            var context = new ValidationContext(staffLogin);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            var contextStaffInformation = new ValidationContext(staff);

            var loginValid = !Validator.TryValidateObject(staffLogin, context, results, true);
            var staffValid = !Validator.TryValidateObject(staff, contextStaffInformation, results, true);

            
            if (loginValid || staffValid) // валидация, проверка на правильность введённых значений
            {
                StringBuilder sb = new StringBuilder();
                if(tbPassword.Text.ToString() == "") sb.AppendLine("Введите пароль");
                foreach (var error in results)
                {
                    sb.AppendLine(error.ToString());
                }
                MessageBox.Show(sb.ToString());
                return;
            }

            db.Staff.Add(staff);
            db.Login.Add(staffLogin);

            PrintStaffDocs(staff);

            if (Validator.TryValidateObject(staff, contextStaffInformation, results, true) && Validator.TryValidateObject(staffLogin, context, results, true))
            { 
                db.SaveChanges();
                NavigationService.GoBack();
            }

        }                                                  
        private void btnAddImage_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog(); // открывает диалоговое окно для добавление аватарки пользователей
            dialog.DefaultExt = ".png";

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                string filePath = dialog.FileName;

                try
                {
                    filePath.Replace("\\", "\\\\"); //Заменяет слэши в пути т.к. \\ - спец символ
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

        private void PrintStaffDocs(Staff staff)
        {
            try
            {
                HospitalProEntities db = Helper.GetContext();

                string fullNameStaff = (string)staff.firstname + " " + (string)staff.secondname;
                string date = dtNow.ToLongDateString();
                
                var workDate = dtNow.AddDays(1);
                string workDateS = workDate.ToLongDateString();

                string datePro = dtNow.ToShortDateString();

                var staffSpeciality = db.Speciality.FirstOrDefault(x => x.ID_speciality == staff.speciality);
                var addressDepartament = db.Departament.FirstOrDefault(x => x.ID_departament == staff.ID_departament);

                var items = new Dictionary<string, string>()
                {
                    {"<Number>", staff.ID_staff.ToString()},
                    {"<City>", "Новосибирск" },
                    {"<OllDate>", date },
                    {"<Hospital>", "Психическая больница"},
                    {"<HospitalName>", "Цветочки" },
                    {"<Director>", "Хаустова Артёма Сергеевича" },
                    {"<FullNameStaff>", fullNameStaff},
                    {"<Hospitals>", "психиатрическую больницу: Цветочки" },
                    {"<StaffSpeciality>", (string)staffSpeciality.name },
                    {"<AddresDepartament>", (string)addressDepartament.addres },
                    {"<WorkDate>", workDate.ToString()},
                    {"<Chellange>",  "1 месяц"},
                    {"<Salary>", "50000" },
                    {"<SalaryPropis>", "пятьдесят тысяч" },
                    {"<NormDock>", "Какой то крутой документ" },
                    {"<PSNormalDock>", "Доп выплат не будет" },
                    {"<DirectorINN>", "680076212" },
                    {"<DirectorPro>", "Mickey" },
                    {"<GeneralDirector>", "Теслюк Алина Витальевна" },
                    {"<GeneralDirectorSocr>", "Теслюк А. В." },
                    {"<GeneralDirectorSign>", "Настоящая подпись" },
                    {"<Series>", tbPassportSeries.Text },
                    {"<Nomber>",  tbPassportNomber.Text},
                    {"<Issued>", (string)staff.issued },
                    {"<DatePro>",  datePro}
                };

                string staffNameDoc = (string)staff.firstname + (string)staff.secondname;

                using (var doc = DocX.Load("C:\\Users\\Mickey\\source\\repos\\Практическая 3\\Практическая 3\\Files\\blank-trudovogo-dogovora.docx"))
                {
                    foreach (var item in items)
                    {
                        foreach (var paragraph in doc.Paragraphs)
                        {
                            if (paragraph.Text.Contains(item.Key))
                            {
                                paragraph.ReplaceText(item.Key, item.Value);
                            }
                        }
                    }
                    doc.SaveAs($"C:\\Users\\Mickey\\source\\repos\\Практическая 3\\Практическая 3\\Files\\{staffNameDoc}blank-trudovogo-dogovora.docx");
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
