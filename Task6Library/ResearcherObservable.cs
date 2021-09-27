using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Windows;

namespace Task6Library
{
    [Serializable]
    public class ResearcherObservable : ObservableCollection<Activity>, INotifyPropertyChanged
    {
        public ResearcherObservable() { }

        public ResearcherObservable(string name = "Andrew", string surname = "Kinshin")
        {
            Name = name;
            SurName = surname;
            IfChanged = false;
            this.CollectionChanged += CollectionChangedEventHandler;
            orgnsName = new List<string>();
            orgnsName.Add("Org1");
            orgnsName.Add("Org2");
            orgnsName.Add("Org3");
        }

        [field: NonSerializedAttribute()]
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public void CollectionChangedEventHandler(object sender, NotifyCollectionChangedEventArgs e)
        {
            IfChanged = true;
        }

        public string Name { get; set; }
        public string SurName { get; set; }

        private bool ifChanged;
        public bool IfChanged
        {
            get
            {
                return ifChanged;
            }
            set
            {
                ifChanged = value;
                OnPropertyChanged("IfChanged");
                OnPropertyChanged("InfCount");
            }
        }

        private List<string> orgnsName;
        public List<string> OrgnsName
        {
            get
            {
                return orgnsName;
            }
        }

        private int infCount;
        public int InfCount 
        {
            get
            {
                infCount = 0;
                foreach (var i in base.Items)
                {
                    if (i is Project) infCount++;
                }
                return infCount;
            }
        }

        /*public IEnumerable<Project> Projects
        {
            get
            {
                return from x in base.Items
                       where x is Project
                       select x as Project;
            }
        }*/

        public void AddActivity(params Activity[] Activitys)
        {
            foreach (Activity i in Activitys)
            {
                base.Add(i);
            }
        }

        public void RemoveActivityAt(int index)
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
            base.Add(new Activity("Disney", new DateTime(2020, 1, 1), new DateTime(2020, 2, 2)));
            base.Add(new Consulting("DreamWorks", new DateTime(2020, 3, 3), new DateTime(2020, 4, 4), true, 1000));
            base.Add(new Activity("LucasFilm", new DateTime(2020, 5, 5), new DateTime(2020, 6, 6)));
            base.Add(new Project("MosFilm", new DateTime(2020, 7, 7), new DateTime(2020, 8, 8), 500, "NewEra"));
        }

        public void AddDefaultConsulting()
        {
            base.Add(new Consulting());
        }

        public void AddDefaultProject()
        {
            base.Add(new Project());
        }

        public static bool Save(string filename, ResearcherObservable obj)
        {
            bool check = true;
            FileStream fs = null;
            try
            {
                obj.IfChanged = false;
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

        public static bool Load(string filename, ref ResearcherObservable obj)
        {
            bool check = true;
            FileStream fs = null;
            try
            {
                fs = File.OpenRead(filename);
                BinaryFormatter bf = new BinaryFormatter();
                obj = bf.Deserialize(fs) as ResearcherObservable;
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
            string tmp = "";
            foreach (var i in base.Items)
            {
                tmp += "\n" + i.ToString();
            }
            return tmp;
        }

    }
}

