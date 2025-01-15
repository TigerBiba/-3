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
    /// <summary>
    /// Логика взаимодействия для Admin.xaml
    /// </summary>
    public partial class Admin : Page
    {
        public Admin(string fiertname, string lastname)
        {
            InitializeComponent();
            this.Loaded += Admin_Loaded;
        }

        private void Admin_Loaded(object sender, RoutedEventArgs e)
        {
            LoadStaff();
        }

        private void LoadStaff()
        {
            HospitalProEntities1 db = Helper.GetContext();

            db.Staff.Load();

            LViewStaff.ItemsSource = db.Staff.Local;
        }
    }
}
