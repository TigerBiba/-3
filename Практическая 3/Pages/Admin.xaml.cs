using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// <summary>
    /// Логика взаимодействия для Admin.xaml
    /// </summary>
    public struct staffStruct
    {
        public string firstname { get; set; }
        public string secondname { get; set; }
        public string speciality { get; set; }
        public string email { get; set; }
        public string photo { get; set; }
        public int ID_staff { get; set; }
    }
    public partial class Admin : Page
    {
        private ObservableCollection<staffStruct> staffList; // все пользователи которых в последствии надо отобразить
        private ObservableCollection<staffStruct> staffListSearch; //Карточки пользователей которые надо отобразить
        public Admin(Staff admin)
        {
            InitializeComponent();

            staffList = new ObservableCollection<staffStruct>();
            staffListSearch = new ObservableCollection<staffStruct>();

        }

        private void AllStaffCard()
        {
            HospitalProEntities1 db = Helper.GetContext();
            var staff = Helper.GetContext().Staff.ToList();

            foreach (var user in staff)
            {
                string role = null;

                switch(user.speciality)
                {
                    case 1:
                        role = "Глав врач";
                        break;
                    case 2:
                        role = "Уборщик";
                        break;
                    case 3:
                        role = "Медсестра";
                        break;
                    case 4:
                        role = "Администратор";
                        break;
                }

                staffStruct staffStruct = new staffStruct()
                {
                    firstname = user.firstname,
                    secondname = user.secondname,
                    speciality = role,
                    email = user.email,
                    photo = user.photo,
                    ID_staff = user.ID_staff,
                };
                staffList.Add(staffStruct);
            }
            staffListSearch = new ObservableCollection<staffStruct>(staffList);
        }

        private void btnChangeUser_Click(object sender, RoutedEventArgs e)
        {
            HospitalProEntities1 db = Helper.GetContext();

            Console.WriteLine(LViewStaff.SelectedItem);
            if (LViewStaff.SelectedItem is staffStruct selectedStaff)
                {
                    Staff staff = db.Staff.FirstOrDefault(x => x.ID_staff == selectedStaff.ID_staff);

                    NavigationService.Navigate(new StaffChange(staff));
                }
        }

        private void btnNewStaff_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddNewStaffPage());
        }

        private void tbFind_TextChanged(object sender, TextChangedEventArgs e)
        {
            FindStaff();
        }

        private void cbSpeciality_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FindStaff();
        }

        private void FindStaff()
        {
            staffListSearch.Clear();

            string selectedSpeciality = (cbSpeciality.SelectedItem as ComboBoxItem)?.Content?.ToString();

            foreach (var staff in staffList)
            {
                //ищет все схождения с условиями: нулевой текст бокс, или если найдено вхождениев staff
                bool matchesSearch = string.IsNullOrEmpty(tbFind.Text) || staff.firstname.Contains(tbFind.Text) || staff.secondname.Contains(tbFind.Text);
                bool mathesSpeciality = string.IsNullOrEmpty(selectedSpeciality) || selectedSpeciality == staff.speciality;

                if (matchesSearch && mathesSpeciality)
                {
                    staffListSearch.Add(staff);
                }
            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            tbFind.Text = "";
            cbSpeciality.SelectedIndex = -1;

            LViewStaff.ItemsSource = staffListSearch;
        }
        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                staffList.Clear();

                LViewStaff.ItemsSource = null;

                AllStaffCard();

                LViewStaff.ItemsSource = staffListSearch;
            }
        }
    }
}
