using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task6Library
{
    [Serializable]
    public class Project : Activity, IDeepCopy
    {
        private int staffCount;
        public int StaffCount
        {
            get
            {
                return staffCount;
            }
            set
            {
                if (value != staffCount)
                    staffCount = value;
            }
        }

        private string resName;
        public string ResName
        {
            get
            {
                return resName;
            }
            set
            {
                if (value != resName)
                    resName = value;
            }
        }

        public Project()
        {
            staffCount = 1000;
            resName = "Project";
        }
        public Project(string param1, DateTime param2, DateTime param3, int param4, string param5) : base(param1, param2, param3)
        {
            staffCount = param4;
            resName = param5;
        }
        public override string ToString()
        {
            return "Project: " + OrgName.ToString() + " " + DtTm[0].ToString() + " " + DtTm[1].ToString() + " " + StaffCount.ToString() + " " + ResName;
        }

        public override object DeepCopy()
        {
            return new Project(OrgName, DtTm[0], DtTm[1], StaffCount, ResName);
        }
    }
}


