using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Task6Library;
using System.Globalization;

namespace Task6
{
    public class DataConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo info)
        {
            Activity res = value as Activity;
            string tmp = "Activity: ";
            if (res is Project) 
                tmp = "Project: ";
            else if (res is Consulting) 
                tmp = "Consulting: ";
            if (res != null) 
                return tmp + res.OrgName;
            else 
                return "Error";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo info)
        {
            return value;
        }
    }
}
