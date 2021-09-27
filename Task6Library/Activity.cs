using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task6Library
{
    public interface IDeepCopy
    {
        object DeepCopy();
    }

    [Serializable]
    public class Activity : IDeepCopy 
    {
        private string orgname;
        public string OrgName
        {
            get
            {
                return orgname;
            }
            set
            {
                if (value != orgname)
                    orgname = value;
            }
        }

        public DateTime[] DtTm { get; set; }

        public Activity(string param1 = "Organisation", DateTime param2 = default(DateTime), DateTime param3 = default(DateTime))
        {
            orgname = param1;
            DtTm = new DateTime[2];
            DtTm[0] = param2;
            DtTm[1] = param3;
        }

        public override string ToString()
        {
            return "Activity: " + OrgName.ToString() + " " + DtTm[0].ToString() + " " + DtTm[1].ToString();
        }

        public virtual object DeepCopy()
        {
            return new Activity(OrgName, DtTm[0], DtTm[1]);
        }

    }
}


