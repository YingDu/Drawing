using Drawing.Models;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
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

namespace Drawing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private List<ShapeBase> _diagram = new List<ShapeBase>();

        private async void NewCircle_Click(object sender, RoutedEventArgs e)
        {
            var result = await this.ShowInputAsync("半径录入", "请输入圆的半径");

            if (result == null) //user pressed cancel
                return;

            var radius = double.Parse(result);

            var circle = new Circle(radius);
            circle.Draw();
            _diagram.Add(circle);

            Stage.Children.Add(circle.Instance);
        }

        private void ZoomIn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var shape in _diagram)
            {
                IShape temp = (IShape)shape;
                temp.ZoomIn(1.1);
            }
        }

        private void ZoomOut_Click(object sender, RoutedEventArgs e)
        {
            foreach (var shape in _diagram)
            {
                IShape temp = (IShape)shape;
                temp.ZoomOut(0.9);
            }
        }

        private void NewRectangle_Click(object sender, RoutedEventArgs e)
        {
            //var result = await this.ShowInputAsync("尺寸录入", "请输入矩形的长和宽（英文逗号分隔长和宽）");

            //if (result == null) //user pressed cancel
            //    return;
            //var widthAndHeight = result.Split(',');
            //var width = double.Parse(widthAndHeight[0]);
            //var height = double.Parse(widthAndHeight[1]);

            var rectangle = new Models.Rectangle(200,100);
            rectangle.Draw();
            _diagram.Add(rectangle);
            Stage.Children.Add(rectangle.Instance);


        }
    }
}
