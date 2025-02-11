using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public struct drugStruct 
    {
        public string name { get; set; }
        public string dosage { get; set; }
        public string date_of_manufactured { get; set; }
        public int ID_drug { get; set; }
    }
    /// <summary>
    /// Логика взаимодействия для AllDrugs.xaml
    /// </summary>
    public partial class AllDrugs : Page
    {
        private ObservableCollection<drugStruct> drugList; // все пользователи которых в последствии надо отобразить
        private ObservableCollection<drugStruct> drugListSearch; //Карточки пользователей которые надо отобразить
        public AllDrugs(Staff administrator)
        {
            InitializeComponent();

            drugList = new ObservableCollection<drugStruct>();
            drugListSearch = new ObservableCollection<drugStruct>();
        }

        private void AllDrugsCard()
        {
            HospitalProEntities1 db = Helper.GetContext();
            var drugs = Helper.GetContext().Drugs.ToList();

            foreach (var drug in drugs)
            {
                drugStruct drugStruct = new drugStruct()
                {
                    name = drug.name,
                    dosage = drug.dosage.ToString() + " мг.",
                    date_of_manufactured = "Дата " + drug.date_of_manufactured.ToString(),
                    ID_drug = drug.ID_drug
                };
                drugList.Add(drugStruct);
            }
            drugListSearch = new ObservableCollection<drugStruct>(drugList);
        }
        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                drugList.Clear();

                LViewStaff.ItemsSource = null;

                AllDrugsCard();

                LViewStaff.ItemsSource = drugListSearch;
            }
        }

        private void btnPrintDocs(object sender, RoutedEventArgs e)
        {
            FlowDocument doc = flowDocumentReader.Document;

            if (doc == null)
            {
                MessageBox.Show("Документ не найден");
                return;
            }
            else
            {
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    IDocumentPaginatorSource idpSource = doc;
                    printDialog.PrintDocument(idpSource.DocumentPaginator, "Список всех лекарств которые используются в больницах");
                }
            }
        }

        private void btnNewDrug_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
