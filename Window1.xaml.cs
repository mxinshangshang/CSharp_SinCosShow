using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Windows.Threading;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay.PointMarkers;

namespace Simulation
{
    
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
   /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        // 动态存储坐标
        private ObservableDataSource<Point> dataSource1 = new ObservableDataSource<Point>();
        private ObservableDataSource<Point> dataSource2 = new ObservableDataSource<Point>();
        // 定时器
        private DispatcherTimer timer = new DispatcherTimer();
        
        private LineGraph graphSin = new LineGraph();
        private LineGraph graphCos = new LineGraph();

        // 构造坐标
        Point pointSin = new Point();
        Point pointCos = new Point();

        // 绘图刷新标志
        bool newGraph = true;

        // X轴
        private uint i1 = 0;
        private uint i2 = 0;

        public Window1()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            timer.Interval = TimeSpan.FromMilliseconds(1);
            timer.Tick += SinPlot;
            plotter.Viewport.FitToView();
        }

        private void SinPlot(object sender, EventArgs e)
        {
            // axis x
            double x1 = i1++;
            double x2 = (i2 += 2);

            // axis y
            double y1 = Math.Sin(x1 * Math.PI / 180);
            double y2 = Math.Cos(x2 * Math.PI / 180);

            // 更新Point
            pointSin.X = x1;
            pointSin.Y = y1;
            pointCos.X = x2;
            pointCos.Y = y2;

            // 追加至Plot
            dataSource1.AppendAsync(base.Dispatcher, pointSin);
            dataSource2.AppendAsync(base.Dispatcher, pointCos);
        }

        private void btStart_Click(object sender, RoutedEventArgs e)
        {
            if (newGraph)
            {
                graphSin = plotter.AddLineGraph(dataSource1, Colors.DeepSkyBlue, 2, "Sin");
                graphCos = plotter.AddLineGraph(dataSource2, Colors.DeepPink, 2, "Cos");
                newGraph = false;
            }
            timer.IsEnabled = true;
            btReset.IsEnabled = false;
        }

        private void btStop_Click(object sender, RoutedEventArgs e)
        {
            timer.IsEnabled = false;
            btReset.IsEnabled = true;
        }

        private void btReset_Click(object sender, RoutedEventArgs e)
        {
            i1 = 0;
            i2 = 0;
            newGraph = true;
            //plotter.Children.RemoveAll(typeof(LineGraph));
            plotter.Children.Remove(graphSin);
            plotter.Children.Remove(graphCos);
        }
    }
}
