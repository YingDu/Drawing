using Drawing.Models;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
        private Point initMousePoint;
        private Point currentMousePoint;
        private double canvasInitTop;
        private double canvasInitLeft;


        private List<ShapeBase> _diagram = new List<ShapeBase>();

        private ShapeBase _selectedShape;

        private void NewCircle_Click(object sender, RoutedEventArgs e)
        {

            var circle = new Circle(80);

            circle.Draw();
            _diagram.Add(circle);
            var rnd = new Random();
            canvasInitTop = (double)rnd.Next((int)Height / 2);
            canvasInitLeft = (double)rnd.Next((int)Width / 2);
            circle.Instance.SetValue(Canvas.TopProperty, canvasInitTop);
            circle.Instance.SetValue(Canvas.LeftProperty, canvasInitLeft);

            circle.Instance.MouseRightButtonDown += ShapeInstance_MouseRightButtonDown;
            circle.Instance.MouseRightButtonUp += ShapeInstance_MouseRightButtonUp;
            circle.Instance.MouseMove += ShapeInstance_MouseMove;

            circle.Instance.MouseLeftButtonUp += ShapeInstance_MouseLeftButtonUp;

            Stage.Children.Add(circle.Instance);
        }

        private void NewRectangle_Click(object sender, RoutedEventArgs e)
        {
            var rectangle = new Models.Rectangle(100, 100);
            rectangle.Draw();
            _diagram.Add(rectangle);
            var rnd = new Random();
            canvasInitTop = (double)rnd.Next((int)Height);
            canvasInitLeft = (double)rnd.Next((int)(Width));
            rectangle.Instance.SetValue(Canvas.TagProperty, canvasInitTop);
            rectangle.Instance.SetValue(Canvas.LeftProperty, canvasInitLeft);

            rectangle.Instance.MouseRightButtonDown += ShapeInstance_MouseRightButtonDown;
            rectangle.Instance.MouseRightButtonUp += ShapeInstance_MouseRightButtonUp;
            rectangle.Instance.MouseMove += ShapeInstance_MouseMove;

            rectangle.Instance.MouseLeftButtonUp += ShapeInstance_MouseLeftButtonUp;

            Stage.Children.Add(rectangle.Instance);
        }

        #region Zoom function

        private void ShapeInstance_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if ((sender as Shape) != null)
            {
                _selectedShape = _diagram.Where(s => s.Instance == (Shape)sender).FirstOrDefault();
            }
        }

        private void ZoomIn_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedShape != null)
            {
                IShape temp = (IShape)_selectedShape;
                temp.ZoomIn(1.1);
                _selectedShape = null;
            }
            else
            {
                MessageBox.Show("请选择图形！");
            }
        }

        private void ZoomOut_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedShape != null)
            {
                IShape temp = (IShape)_selectedShape;
                temp.ZoomOut(0.9);
            }
            else
            {
                MessageBox.Show("请选择图形！");
            }
        }
        #endregion

        #region Drag function

        private void ShapeInstance_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Shape shape && e.ClickCount == 1)
            {
                shape.Focus();
                shape.CaptureMouse();
                initMousePoint = e.GetPosition(this);
                canvasInitLeft = Canvas.GetLeft(shape);
                canvasInitTop = Canvas.GetTop(shape);
                shape.RaiseEvent(new DragStartedEventArgs(0, 0));
            }
            e.Handled = true;
        }

        private void ShapeInstance_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Shape shape)
            {
                shape.ReleaseMouseCapture();
            }
        }

        private void ShapeInstance_MouseMove(object sender, MouseEventArgs e)
        {
            if (sender is Shape shape && shape.IsMouseCaptured)
            {
                shape.RaiseEvent(new DragDeltaEventArgs(0, 0));
                currentMousePoint = e.GetPosition(this);
                double leftOffset = currentMousePoint.X - initMousePoint.X;
                double topOffset = currentMousePoint.Y - initMousePoint.Y;
                shape.SetValue(Canvas.TopProperty, topOffset + canvasInitTop);
                shape.SetValue(Canvas.LeftProperty, leftOffset + canvasInitLeft);
            }
        }

        #endregion
    }
}
