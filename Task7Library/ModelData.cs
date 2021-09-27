using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;

namespace Task7Library
{
    [Serializable]
    public class ModelData : IDataErrorInfo
    {
        public ModelData(int n = 2, double p = 0)
        {
            // let my function be F(x) = 10 * (x^2) + p, where p is a parameter
            double h = (double)1/n;
            propX = new double[n + 1];
            propF = new double[n + 1];
            for (int i = 0; i <= n; i++)
            {
                propX[i] = i * h;
                propF[i] = 10 * propX[i] * propX[i] + p;
            }
            propN = n;
            propP = p;
        }

        public int propN { get; set; }
        public double propP { get; set; }
        public double[] propX { get; set; }
        public double[] propF { get; set; }

        public static double pMin = -100.0;
        public static double pMax = 100.0;
        public static double nMin = 2.0;
        public static double nMax = 1000.0;

        public double F(double x)
        {
            double a, b;
            if (x == propX[propN])
                return propF[propN];
            for (int i = 0; i < propN; i++)
            {
                if (x == propX[i])
                    return propF[i];
                else if (x > propX[i] && x < propX[i + 1])
                {
                    a = (propF[i + 1] - propF[i]) / (propX[i + 1] - propX[i]);
                    b = propF[i] - a * propX[i];
                    return a * x + b;
                }
            }
            MessageBox.Show("x cannot be less 0 and greater 1");
            return -1;
        }

        public string Error
        {
            get
            {
                return "Error Text";
            }
        }
        public string this[string property]
        {
            get
            {
                string msg = null;
                switch (property)
                {
                    case "propN":
                        if (propN < nMin || propN > nMax)
                            msg = "N cannot be less 2 and greater 1000";
                        break;
                    case "propP":
                        if (propP < pMin || propP > pMax)
                            msg = "P cannot be less -100 and greater 100";
                        break;
                    default:
                        break;
                }
                return msg;
            }
        }

        public ModelData DeepCopy()
        {
            return new ModelData(propN, propP);
        }

    }
}
