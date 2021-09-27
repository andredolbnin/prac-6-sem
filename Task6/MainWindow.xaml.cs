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
using Task6Library;
using Microsoft.Win32;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Task6
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        ResearcherObservable ResObs = new ResearcherObservable("Andrew", "Kinshin");
        Project proj = new Project();

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = ResObs;

            DataTemplate templ = this.TryFindResource("key_for_projects") as DataTemplate;
            if (templ != null)
                ListBox2.ItemTemplate = templ;

            ComboBox1.ItemsSource = ResObs.OrgnsName;

            //
            Binding bind1 = get_bind("OrgName");
            ComboBox1.SetBinding(ComboBox.TextProperty, bind1);

            Binding bind2 = get_bind("DtTm[0]");
            DatePicker1.SetBinding(DatePicker.SelectedDateProperty, bind2);

            Binding bind3 = get_bind("DtTm[1]");
            DatePicker2.SetBinding(DatePicker.SelectedDateProperty, bind3);

            Binding bind4 = get_bind("ResName");
            TextBox3_cust.SetBinding(TextBox.TextProperty, bind4);

            Binding bind5 = get_bind("StaffCount");
            TextBox4_cust.SetBinding(TextBox.TextProperty, bind5);
        }

        Binding get_bind(string src)
        {
            Binding bind = new Binding();
            bind.Source = proj;
            bind.Path = new PropertyPath(src);
            //bind.Mode = BindingMode.TwoWay;
            bind.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            return bind;
        }

        private void AddDefs(object sender, RoutedEventArgs e)
        {
            ResObs.AddDefaults();
        }

        private void AddDefProj_Click(object sender, RoutedEventArgs e)
        {
            ResObs.AddDefaultProject();
        }

        private void AddCustProj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ResObs.AddActivity((Project)proj.DeepCopy());
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AddDefCons_Click(object sender, RoutedEventArgs e)
        {
            ResObs.AddDefaultConsulting();
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {            
            ResObs.RemoveActivityAt(ListBox1.SelectedIndex);
        }

        private void SaveDialog()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == true)
                ResearcherObservable.Save(sfd.FileName, ResObs);
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            if (ResObs.IfChanged == true &&
                MessageBox.Show("Do you want to save info?", "Attention", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    SaveDialog();
            ResObs = new ResearcherObservable("Andrew", "Kinshin");
            DataContext = ResObs;
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                //1
                if (ResObs.IfChanged == true &&
                    MessageBox.Show("Do you want to save info?", "Attention", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        SaveDialog();
                //2
                ResearcherObservable.Load(ofd.FileName, ref ResObs);
                //3
                DataContext = ResObs;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveDialog();
        }

        private void Closed_Click(object sender, EventArgs e)
        {
            if (ResObs.IfChanged == true &&
                MessageBox.Show("Do you want to save info?", "Attention", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    SaveDialog();
        }

        private void FilterProjects(object sender, FilterEventArgs e)
        {
            Activity res = e.Item as Activity;
            if (res != null && res is Project)
                e.Accepted = true;
            else
                e.Accepted = false;
        }

        private void RB_Template_Click(object sender, RoutedEventArgs e)
        {
            DataTemplate templ = this.TryFindResource("key_RB_Template") as DataTemplate;
            if (templ != null)
                ListBox1.ItemTemplate = templ;
        }

        private void RB_Default_Click(object sender, RoutedEventArgs e)
        { 
            ListBox1.ItemTemplate = null; 
        }
    }
}
