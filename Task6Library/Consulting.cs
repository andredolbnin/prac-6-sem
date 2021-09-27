using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task6Library
{
    [Serializable]
    public class Consulting : Activity, IDeepCopy, IComparable<Consulting>
    {
        private bool isInter;
        public bool IsInter
        {
            get
            {
                return isInter;
            }
            set
            {
                if (value != isInter)
                    isInter = value;
            }
        }

        private double vol;
        public double Vol
        {
            get
            {
                return vol;
            }
            set
            {
                if (value != vol)
                    vol = value;
            }
        }
        public Consulting()
        {
            isInter = false;
            vol = 100.0;
        }
        public Consulting(string param1, DateTime param2, DateTime param3, bool param4, double param5)
            : base(param1, param2, param3)
        {
            isInter = param4;
            vol = param5;
        }

        public override string ToString()
        {
            return "Consulting: " + OrgName + " " + DtTm[0] + " " + DtTm[1] + " " + IsInter + " " + Vol;
        }

        public override object DeepCopy()
        {
            return new Consulting(OrgName, DtTm[0], DtTm[1], IsInter, Vol);
        }

        public int CompareTo(Consulting other)
        {
            return this.Vol.CompareTo(other.Vol);
        }
    }
}
