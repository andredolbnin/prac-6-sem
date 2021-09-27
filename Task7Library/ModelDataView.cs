using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Forms.DataVisualization.Charting;

namespace Task7Library
{
    public class ModelDataView: IDataErrorInfo
    {
        public ObservableModelData Obs;
        public ModelDataView(ObservableModelData param)
        {
            Obs = new ObservableModelData();
            Obs = param;
            chType = "Line";
            floatNumber = 1;
        }

        public string chType { get; set; }
        public int floatNumber { get; set; }

        public void Draw(Chart chart, ModelData selectedDataModel)
        {
            chart.ChartAreas.Clear();
            chart.Legends.Clear();
            chart.Series.Clear();
            chart.BackColor = System.Drawing.Color.LightSteelBlue;
            //1
            chart.ChartAreas.Add(new ChartArea("ChartArea1"));
            chart.ChartAreas[0].AxisX.LabelStyle.Format = "N" + floatNumber;
            chart.Legends.Add(new Legend("Legend"));
            chart.Legends[0].Title = "Value of parameter";
            int count = 0;
            double chosenP = selectedDataModel.propP;
            foreach (var item in Obs)
            {
                if (item.propP > chosenP)
                   continue;
                chart.Series.Add("Series" + count);
                chart.Series[count].LegendText = "P = " + item.propP;
                List<double> x = new List<double>();
                List<double> f = new List<double>();
                for (int i = 0; i <= item.propN; ++i)
                {
                    x.Add(item.propX[i]);
                    f.Add(item.propF[i]);
                }
                chart.Series[count].Points.DataBindXY(x, f);
                chart.Series[count].BorderWidth = 2;
                chart.Series[count].MarkerStyle = MarkerStyle.None;
                if (chType == "Line")
                    chart.Series[count].ChartType = SeriesChartType.Line;
                else
                    chart.Series[count].ChartType = SeriesChartType.Spline;
                ++count;
            }
            //2
            chart.ChartAreas.Add(new ChartArea("ChartArea2"));
            chart.ChartAreas[1].AxisX.LabelStyle.Format = "N" + floatNumber;
            ModelData md = selectedDataModel;
            int modelsCount = Obs.Count;
            List<double> fixX = new List<double>();
            for (int i = 0; i <= md.propN; ++i)
            {
                fixX.Add(md.propX[i]);
            }
            List<double> midF = new List<double>();
            List<double> minF = new List<double>();
            List<double> maxF = new List<double>();
            for (int i = 0; i <= md.propN; ++i)
            {
                List<double> temp = new List<double>();
                temp = Obs.GetF(fixX[i]).ToList();
                double mid = 0, min = temp[0], max = temp[0]; 
                for (int j = 0; j < modelsCount; ++j)
                {
                    mid += temp[j];
                    if (min > temp[j])
                        min = temp[j];
                    if (max < temp[j])
                        max = temp[j];
                }
                midF.Add((double)mid / modelsCount);
                minF.Add(min);
                maxF.Add(max);
            }
            chart.Series.Add("Series" + count);
            chart.Series[count].Points.DataBindXY(fixX, midF);
            chart.Series.Add("Series" + (count + 1));
            chart.Series[count + 1].Points.DataBindXY(fixX, minF);
            chart.Series.Add("Series" + (count + 2));
            chart.Series[count + 2].Points.DataBindXY(fixX, maxF);
            for (int i = 0; i < 3; ++i)
            {
                chart.Series[count].ChartArea = "ChartArea2";
                chart.Series[count].IsVisibleInLegend = false;
                chart.Series[count].BorderWidth = 2;
                chart.Series[count].MarkerStyle = MarkerStyle.Cross;
                chart.Series[count].MarkerBorderWidth = 2;
                if (chType == "Line")
                    chart.Series[count].ChartType = SeriesChartType.Line;
                else
                    chart.Series[count].ChartType = SeriesChartType.Spline;
                for (int j = 0; j < chart.Series[count].Points.Count; ++j)
                    chart.Series[count].Points[j].ToolTip =
                     "x = " + chart.Series[count].Points[j].XValue.ToString("F5") +
                     "\ny = " + chart.Series[count].Points[j].YValues[0].ToString("F5");
                ++count;
            }
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
                    case "floatNumber":
                        if (floatNumber < 1 || floatNumber > 5)
                            msg = "floatNumber cannot be less 1 and greater 5";
                        break;
                    default:
                        break;
                }
                return msg;
            }
        }

    }
}
