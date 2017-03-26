using Drawing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
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
            _shapeFactory = ShapeFactory.Instance("");
            
        }
        private ShapeFactory _shapeFactory = null;
        private Point initMousePoint; //鼠标起始位置坐标
        private Point currentMousePoint;    //当前鼠标坐标
        private double canvasInitTop;       //起始位置canvas坐标
        private double canvasInitLeft;      //当前位置canvas坐标

        private List<ShapeBase> _diagram = new List<ShapeBase>();   //存储canvas上的图形

        private ShapeBase _selectedShape;   //存储被选中的图形

        //画圆
        private void NewCircle_Click(object sender, RoutedEventArgs e)
        {
            CreateNewShape(ShapeType.Circle);
        }

        private void CreateNewShape(ShapeType type)
        {
            ShapeBase shape = null;
            switch (type)
            {
                case ShapeType.Circle:
                    shape = _shapeFactory.MakeCircle(80);
                    break;
                case ShapeType.Rectangle:
                    shape = _shapeFactory.MakeRectangle(100, 200);
                    break;
                default:
                    throw new NotImplementedException();
            }
            
            shape.Draw();
            _diagram.Add(shape);
            var rnd = new Random();
            canvasInitTop = 0;
            canvasInitLeft = 0;
            shape.Instance.SetValue(Canvas.TopProperty, canvasInitTop);
            shape.Instance.SetValue(Canvas.LeftProperty, canvasInitLeft);

            shape.Instance.MouseLeftButtonDown += ShapeInstance_MouseLeftButtonDown;
            shape.Instance.MouseLeftButtonUp += ShapeInstance_MouseLeftButtonUp;
            shape.Instance.MouseMove += ShapeInstance_MouseMove;

            shape.Instance.MouseLeftButtonUp += ShapeInstance_MouseLeftButtonUp;

            Stage.Children.Add(shape.Instance);
        }

        //画正方形
        private void NewRectangle_Click(object sender, RoutedEventArgs e)
        {
            CreateNewShape(ShapeType.Rectangle);
        }

        #region Zoom function

        //鼠标左键松开
        private void ShapeInstance_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Shape shape)
            {
                shape.ReleaseMouseCapture();
                shape.StrokeThickness = 1;
            }
            if ((sender as Shape) != null)
            {
                _selectedShape = _diagram.Where(s => s.Instance == (Shape)sender).FirstOrDefault();
                
                ShowArea.Text = "面积: " + _selectedShape.Area.ToString();
            }
        }

        //缩小
        private void ZoomIn_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedShape != null)
            {
                _selectedShape.ZoomIn(1.1);
                ShowArea.Text = "面积: " + _selectedShape.Area.ToString();
            }
            else
            {
                MessageBox.Show("请选择图形！");
            }
        }

        //放大
        private void ZoomOut_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedShape != null)
            {
                _selectedShape.ZoomOut(0.9);
                ShowArea.Text = "面积: " + _selectedShape.Area.ToString();
            }
            else
            {
                MessageBox.Show("请选择图形！");
            }
        }
        #endregion

        #region Drag function

        //鼠标左键点击选中图形
        private void ShapeInstance_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Shape shape && e.ClickCount == 1)
            {
                shape.Focus();
                shape.CaptureMouse();
                initMousePoint = e.GetPosition(this);
                canvasInitLeft = Canvas.GetLeft(shape);
                canvasInitTop = Canvas.GetTop(shape);
                shape.RaiseEvent(new DragStartedEventArgs(0, 0));
                shape.StrokeThickness = 3;

                //将选中的图形置于顶层
                Canvas parent = shape.Parent as Canvas;
                var maxZ = parent.Children.OfType<UIElement>()
                    .Where(x => x != shape)
                    .Select(x => Panel.GetZIndex(x))
                    .Max();
                Panel.SetZIndex(shape, maxZ + 1);                         
            }
            e.Handled = true;
        }

        //鼠标移动
        private void ShapeInstance_MouseMove(object sender, MouseEventArgs e)
        {
            if (sender is Shape shape && e.LeftButton==MouseButtonState.Pressed)
            {
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
