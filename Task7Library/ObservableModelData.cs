using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Specialized;

namespace Task7Library
{
    [Serializable]
    public class ObservableModelData : ObservableCollection<ModelData>
    {
        public ObservableModelData() 
        {
            types = new List<string>();
            types.Add("Line");
            types.Add("Spline");
            isChanged = false;
            this.CollectionChanged += CollectionChangedEventHandler;
            this.AddDefaults();  // comment to stop adding defaults
        }
        public bool isChanged { get; set; }

        private List<string> types;
        public List<string> Types
        {
            get
            {
                return types;
            }
        }

        public void Add_ModelData(ModelData modelData)
        {
            base.Add(modelData);
            return;
        }

        public void Remove_At(int index)
        {
            try
            {
                base.RemoveAt(index);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void AddDefaults()
        {
            Add_ModelData(new ModelData(2, 5.0));
            Add_ModelData(new ModelData(2, 10.0));
        }

        public IEnumerable<double> GetF(double x) 
        {
            var query = from i in base.Items
                        select i.F(x);
            return query;
        }

        public static bool Save(string filename, ObservableModelData obj)
        {
            bool check = true;
            FileStream fs = null;
            try
            {
                obj.isChanged = false;
                fs = File.Open(filename, FileMode.OpenOrCreate);
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, obj);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                check = false;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
            return check;
        }

        public static bool Load(string filename, ref ObservableModelData obj)
        {
            bool check = true;
            FileStream fs = null;
            try
            {
                fs = File.OpenRead(filename);
                BinaryFormatter bf = new BinaryFormatter();
                obj = bf.Deserialize(fs) as ObservableModelData;
                obj.CollectionChanged += obj.CollectionChangedEventHandler;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                check = false;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
            return check;
        }

        public override string ToString()
        {
            string str = "";
            foreach (var i in base.Items)
                str += "\n" + i.ToString();
            return str;
        }

        public void CollectionChangedEventHandler(object sender, NotifyCollectionChangedEventArgs e)
        {
            isChanged = true;
        }
    }
}
