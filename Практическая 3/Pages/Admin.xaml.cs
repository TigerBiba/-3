﻿using System;
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
    public struct staffStruct
    {
        public string firstname { get; set; }
        public string secondname { get; set; }
        public string speciality { get; set; }
        public string email { get; set; }
        public string photo { get; set; }
    }
    public partial class Admin : Page
    {
        private ObservableCollection<staffStruct> staffList;
        private ObservableCollection<staffStruct> staffListSearch;
        public Admin(string firstname, string lastname)
        {
            InitializeComponent();

            staffList = new ObservableCollection<staffStruct>();
            staffListSearch = new ObservableCollection<staffStruct>();

            AllStaffCard();

            LViewStaff.ItemsSource = staffListSearch;
        }

        private void AllStaffCard()
        {
            HospitalProEntities1 db = new HospitalProEntities1();
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
                };
                staffList.Add(staffStruct);
            }
            staffListSearch = new ObservableCollection<staffStruct>(staffList);
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
    }
}
