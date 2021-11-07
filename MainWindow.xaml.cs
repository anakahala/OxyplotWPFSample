using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace OxyplotWPFSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// PlotModel
        /// </summary>
        public OxyPlot.PlotModel PlotModel { get; set; } = new OxyPlot.PlotModel();

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            this.CreatePlotModel();
        }

        /// <summary>
        /// 
        /// </summary>
        private void CreatePlotModel()
        {
            // グラフのタイトル（設定しなければ表示しない）
            this.PlotModel.Title = "Sample Chart";

            // グラフのデータを作成
            var dataList = this.CreateDataList();

            // 横軸（時間）
            var xAxis = new OxyPlot.Axes.DateTimeAxis()
            {
                Position = OxyPlot.Axes.AxisPosition.Bottom, // メモリの表示位置をBottom（下）に設定
                Minimum = OxyPlot.Axes.DateTimeAxis.ToDouble(dataList.Min(v => v.MeasureDateTime)), // メモリの最小値（データから最小値を取得して設定）
                Maximum = OxyPlot.Axes.DateTimeAxis.ToDouble(dataList.Max(v => v.MeasureDateTime)), // メモリの最大値（データから最大値を取得して設定）
                StringFormat = "HH", // 横軸のメモリは時間（0-23）で表示する
                MajorStep = 1.0 / 24 * 2, // 大きいメモリの単位（2時間）
                MinorStep = 1.0 / 24, // 小さいメモリの単位（1時間）
            };
            this.PlotModel.Axes.Add(xAxis);

            // 縦軸（数値）
            var yAxis = new OxyPlot.Axes.LinearAxis()
            {
                Position = OxyPlot.Axes.AxisPosition.Left,
                Minimum = 0.0,
                Maximum = 10.0,
                MajorStep = 5,
                MinorStep = 1,
                MajorGridlineStyle = OxyPlot.LineStyle.Solid,
                MinorGridlineStyle = OxyPlot.LineStyle.Dash,
            };
            this.PlotModel.Axes.Add(yAxis);

            // グラフ線
            var lineSeries = new OxyPlot.Series.LineSeries()
            {
                //Color = OxyPlot.OxyColors.Green,
                //MarkerFill = OxyPlot.OxyColors.Blue,
                //MarkerType = OxyPlot.MarkerType.Square,
                ItemsSource = dataList,
                DataFieldX = nameof(OxyPlotData.MeasureDateTime),
                DataFieldY = nameof(OxyPlotData.Value),
            };
            this.PlotModel.Series.Add(lineSeries);

            // グラフを更新
            this.PlotModel.InvalidatePlot(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<OxyPlotData> CreateDataList()
        {
            var dataList = new ObservableCollection<OxyPlotData>();
            var baseDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, DateTime.Now.Minute, DateTime.Now.Second);

            for(int i = 0; i <= 23; i++)
            {
                var data = new OxyPlotData()
                {
                    MeasureDateTime = baseDateTime.AddHours(i),
                    Value = new Random().Next(0, 10)
                };
                dataList.Add(data);
            }

            return dataList;
        }
    }
}
