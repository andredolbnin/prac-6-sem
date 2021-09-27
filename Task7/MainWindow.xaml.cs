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
using Task7Library;
using Microsoft.Win32;
using System.Windows.Forms.DataVisualization.Charting;

namespace Task7
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        Chart chart = new Chart();

        public static RoutedCommand AddModelCommand = new RoutedCommand("AddModelCommand", typeof(Task7.MainWindow));
        public static RoutedCommand DrawCommand = new RoutedCommand("DrawCommand", typeof(Task7.MainWindow));
        static ObservableModelData Obs = new ObservableModelData();
        ModelData MD = new ModelData();
        ModelDataView MDView = new ModelDataView(Obs);

        public MainWindow()
        {
            InitializeComponent();
            WFHost.Child = chart;
            this.DataContext = Obs; 

            DataTemplate templ = this.TryFindResource("p_template") as DataTemplate;
            if (templ != null)
                ListBox1.ItemTemplate = templ;

            Binding bnd1 = getBnd("propN", 0);
            TextBoxN.SetBinding(TextBox.TextProperty, bnd1);

            Binding bnd2 = getBnd("propP", 0);
            TextBoxP.SetBinding(TextBox.TextProperty, bnd2);

            ComboBox1.ItemsSource = Obs.Types;

            Binding bnd3 = getBnd("chType", 1);
            ComboBox1.SetBinding(ComboBox.TextProperty, bnd3);

            Binding bnd4 = getBnd("floatNumber", 1);
            TextBox1.SetBinding(TextBox.TextProperty, bnd4);
        }

        private Binding getBnd(string str, int i)
        {
            Binding bnd = new Binding();
            if (i == 0)
                bnd.Source = MD;
            else
                bnd.Source = MDView;
            bnd.Path = new PropertyPath(str);
            bnd.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            bnd.ValidatesOnDataErrors = true;
            return bnd;
        }

        private void SaveDialog()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == true)
                ObservableModelData.Save(sfd.FileName, Obs);
        }

        private void NewCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (Obs.isChanged == true && MessageBox.Show("Do you want to save info?", "Attention", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                SaveDialog();
            Obs = new ObservableModelData();
            DataContext = Obs;
            MDView = new ModelDataView(Obs);

            Binding bnd3 = getBnd("chType", 1);
            ComboBox1.SetBinding(ComboBox.TextProperty, bnd3);

            Binding bnd4 = getBnd("floatNumber", 1);
            TextBox1.SetBinding(TextBox.TextProperty, bnd4);
        }

        private void OpenCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                //1
                if (Obs.isChanged == true && MessageBox.Show("Do you want to save info?", "Attention", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    SaveDialog();
                //2
                ObservableModelData.Load(ofd.FileName, ref Obs);
                //3
                DataContext = Obs;
                MDView = new ModelDataView(Obs);

                Binding bnd3 = getBnd("chType", 1);
                ComboBox1.SetBinding(ComboBox.TextProperty, bnd3);

                Binding bnd4 = getBnd("floatNumber", 1);
                TextBox1.SetBinding(TextBox.TextProperty, bnd4);
            }
        }

        private void CanSaveCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            if (Obs.isChanged == true)
                e.CanExecute = true;
            else
                e.CanExecute = false;
        }

        private void SaveCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            SaveDialog();
        }

        private void CanDeleteCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ListBox1.SelectedIndex == -1)
                e.CanExecute = false;
            else
                e.CanExecute = true;
        }

        private void DeleteCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            Obs.Remove_At(ListBox1.SelectedIndex);
        }

        private void CanAddCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            if (Grid1 != null)
            {
                foreach (FrameworkElement child in Grid1.Children)
                {
                    if (Validation.GetHasError(child) == true)
                    {
                        e.CanExecute = false;
                        return;
                    }
                    e.CanExecute = true;
                }
            }
            else 
                e.CanExecute = false;
        }

        private void AddCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                Obs.Add_ModelData(MD.DeepCopy());
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void B1_Click(object sender, RoutedEventArgs e) //added for convenience
        {
            if (Validation.GetHasError(TextBoxN) == true)
            {
                string str = "";
                foreach (var i in Validation.GetErrors(TextBoxN))
                    str += i.ErrorContent.ToString() + "\n";
                MessageBox.Show(str);
            }
            else
                MessageBox.Show("This N is allowable");
        }

        private void B2_Click(object sender, RoutedEventArgs e) //added for convenience
        {
            if (Validation.GetHasError(TextBoxP) == true)
            {
                string str = "";
                foreach (var i in Validation.GetErrors(TextBoxP))
                    str += i.ErrorContent.ToString() + "\n";
                MessageBox.Show(str);
            }
            else
                MessageBox.Show("This P is allowable");
        }

        private void CanDrawCommandHandler(object sender, CanExecuteRoutedEventArgs e) 
        {
            if (ListBox1.SelectedIndex == -1)
            {
                e.CanExecute = false;
                return;
            } 
            if (Grid2 != null)
            {
                foreach (FrameworkElement child in Grid2.Children)
                {
                    if (Validation.GetHasError(child) == true)
                    {
                        e.CanExecute = false;
                        return;
                    }
                    e.CanExecute = true;
                }
            }
            else
                e.CanExecute = false;
        }

        private void DrawCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            ModelData md = Obs[ListBox1.SelectedIndex];
            MDView.Draw(chart, md);
        }

        private void B3_Click(object sender, RoutedEventArgs e) //added for convenience
        {
            if (Validation.GetHasError(TextBox1) == true)
            {
                string str = "";
                foreach (var i in Validation.GetErrors(TextBox1))
                    str += i.ErrorContent.ToString() + "\n";
                MessageBox.Show(str);
            }
            else
                MessageBox.Show("This floatNumber is allowable");
        }

        private void Closed_Click(object sender, EventArgs e)
        {
            if (Obs.isChanged == true && MessageBox.Show("Do you want to save info?", "Attention", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    SaveDialog();
        }
    }
}
