using Drawing.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Drawing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private ILog _logger = LogManager.GetLogger(typeof(MainWindow));

        public MainWindow()
        {
            InitializeComponent();
        }
        private Point _initMousePoint; //鼠标起始位置坐标
        private Point _currentMousePoint;    //当前鼠标坐标
        private double _canvasInitTop;       //起始位置canvas坐标
        private double _canvasInitLeft;      //当前位置canvas坐标

        private List<ShapeBase> _diagram = new List<ShapeBase>();   //存储canvas上的图形

        private ShapeBase _selectedShape;   //缓存被选中的图形
        private List<ShapeBase> _selectedShapes = new List<ShapeBase>(); //缓存被选中的图形组
        private System.Windows.Shapes.Rectangle _selectedArea = new System.Windows.Shapes.Rectangle
        {
            Fill = Brushes.Tomato,
            Opacity = 0.5,
            Width = 0.1,
            Height = 0.1
        };

        //画圆
        private void NewCircle_Click(object sender, RoutedEventArgs e)
        {
            CreateNewShape(ShapeType.Circle);
        }

        //画正方形
        private void NewRectangle_Click(object sender, RoutedEventArgs e)
        {
            CreateNewShape(ShapeType.Rectangle);
        }

        private void CalcShapeSelection(Point start, Point end)
        {
            _selectedShapes.Clear();
            foreach (var shape in _diagram)
            {
                var top = (double)shape.Instance.GetValue(Canvas.TopProperty);
                var left = (double)shape.Instance.GetValue(Canvas.LeftProperty);
                if (Math.Abs(end.Y - start.Y) > Math.Abs(top - start.Y)
                 && Math.Abs(end.X - start.X) > Math.Abs(left - start.X))
                {
                    _selectedShapes.Add(shape);
                }
            }
        }

        //画线段
        private void NewLine_Click(object sender, RoutedEventArgs e)
        {
            CreateNewShape(ShapeType.Line);
        }
        private void CreateNewShape(ShapeType type)
        {
            ShapeBase shape = null;
            switch (type)
            {
                case ShapeType.Circle:
                    shape = ShapeFactory.Instance.CreateCircle(80);
                    break;
                case ShapeType.Rectangle:
                    shape = ShapeFactory.Instance.CreateRectangle(100, 200);
                    break;
                case ShapeType.Line:
                    shape = ShapeFactory.Instance.CreateLine(50, 50, 200, 200); 
                    break;
                default:
                    throw new NotImplementedException();
            }

            _canvasInitTop = 20;
            _canvasInitLeft = 20;
            shape.Left = _canvasInitLeft;
            shape.Top = _canvasInitTop;
            DrawOnStage(shape);
        }

        private void DrawOnStage(ShapeBase shape)
        {
            shape.Draw();
            _diagram.Add(shape);
            shape.Instance.SetValue(Canvas.TopProperty, shape.Top);
            shape.Instance.SetValue(Canvas.LeftProperty, shape.Left);

            shape.Instance.MouseLeftButtonDown += ShapeInstance_MouseLeftButtonDown;
            shape.Instance.MouseLeftButtonUp += ShapeInstance_MouseLeftButtonUp;
            shape.Instance.MouseMove += ShapeInstance_MouseMove;

            Stage.Children.Add(shape.Instance);
        }

        //复制
        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedShape != null)
            {

                ShapeBase shape = _selectedShape.Clone();

                shape.Draw();
                _diagram.Add(shape);

                _canvasInitTop = _selectedShape.Top + 10;
                _canvasInitLeft = _selectedShape.Left + 10;
                shape.Instance.SetValue(Canvas.TopProperty, _canvasInitTop);
                shape.Instance.SetValue(Canvas.LeftProperty, _canvasInitLeft);

                shape.Instance.MouseLeftButtonDown += ShapeInstance_MouseLeftButtonDown;
                shape.Instance.MouseLeftButtonUp += ShapeInstance_MouseLeftButtonUp;
                shape.Instance.MouseMove += ShapeInstance_MouseMove;

                shape.Instance.MouseLeftButtonUp += ShapeInstance_MouseLeftButtonUp;

                Stage.Children.Add(shape.Instance);
            }
            else
            {
                Status.Content = "请选择要复制的图形！";
            }

        }
        #region Zoom function

        //鼠标左键松开
        private void ShapeInstance_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _logger.Debug("Shape's mouse left button up event triggled.");
            ShowAreaAndPerimeter();
            if (sender is Shape shape)
            {
                shape.ReleaseMouseCapture();
                shape.StrokeThickness = 2;
                FocusManager.SetFocusedElement(Stage, null);
            }
            if (sender is Shape ishape && !(ishape is System.Windows.Shapes.Line))
            {
                ishape.StrokeThickness = 0;
            }
            if ((sender as Shape) != null)
            {
                _selectedShape = _diagram.Where(s => s.Instance == (Shape)sender).FirstOrDefault();
            }
        }

        //缩小
        private void ZoomIn_Click(object sender, RoutedEventArgs e)
        {
            _logger.Debug("Shape's zoom in method was called.");
            if (_selectedShape != null)
            {
                _selectedShape.ZoomIn(1.1);
                ShowAreaAndPerimeter();
            }
            else
            {
                Status.Content = "请选择图形！";
            }
        }

        //放大
        private void ZoomOut_Click(object sender, RoutedEventArgs e)
        {
            _logger.Debug("Shape's zoom out method was called.");
            if (_selectedShape != null)
            {
                _selectedShape.ZoomOut(0.9);
                ShowAreaAndPerimeter();
            }
            else
            {
                Status.Content = "请选择图形！";
            }
        }
        #endregion

        private void ShowAreaAndPerimeter()
        {
            if (_selectedShape != null)
            {
                SelectedShapeInfo.Text = $"Area:{_selectedShape.Area}, Perimeter:{_selectedShape.GetPerimeter()}";
            }
        }

        #region Drag function

        //鼠标左键点击选中图形
        private void ShapeInstance_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _logger.Debug("Shape's mouse left button down event triggled.");
            if (sender is Shape shape && e.ClickCount == 1)
            {
                shape.Focus();
                shape.CaptureMouse();
                _initMousePoint = e.GetPosition(this);
                _canvasInitLeft = Canvas.GetLeft(shape);
                _canvasInitTop = Canvas.GetTop(shape);
                shape.RaiseEvent(new DragStartedEventArgs(0, 0));
                DrawSelectedSytle(shape);
            }
            e.Handled = true;
        }

        private void DrawSelectedSytle(Shape shape)
        {
            shape.StrokeThickness = 3;
            if (!(shape is System.Windows.Shapes.Line))
            {
                DrawSelectionStyle(shape);
            }
            FocusManager.SetFocusedElement(Stage, shape);
        }

        private static void DrawSelectionStyle(Shape shape)
        {
            var strokeDashArry = new DoubleCollection { 2, 2 };
            shape.StrokeDashArray = strokeDashArry;
        }

        //鼠标移动
        private void ShapeInstance_MouseMove(object sender, MouseEventArgs e)
        {
            _logger.Debug("Shape's mouse move event triggled.");
            if (sender is Shape shape && e.LeftButton == MouseButtonState.Pressed && shape.IsFocused)
            {
                _currentMousePoint = e.GetPosition(this);
                double leftOffset = _currentMousePoint.X - _initMousePoint.X;
                double topOffset = _currentMousePoint.Y - _initMousePoint.Y;

                var selectedShape = _diagram.Where(s => s.Instance == (Shape)sender).FirstOrDefault();
                selectedShape.Left = leftOffset + _canvasInitLeft;
                selectedShape.Top = topOffset + _canvasInitTop;

                shape.SetValue(Canvas.TopProperty, selectedShape.Top);
                shape.SetValue(Canvas.LeftProperty, selectedShape.Left);
            }
        }
        #endregion

        private void Stage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1)
            {
                _initMousePoint = e.GetPosition(Stage);
                var strokeDashArry = new DoubleCollection { 2, 2 };
                if (!Stage.Children.Contains(_selectedArea))
                {
                    Stage.Children.Add(_selectedArea);
                }
                _selectedArea.SetValue(Canvas.TopProperty, _initMousePoint.Y);
                _selectedArea.SetValue(Canvas.LeftProperty, _initMousePoint.X);
            }
        }

        private void Stage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1)
            {
                _currentMousePoint = e.GetPosition(Stage);
                CalcShapeSelection(_initMousePoint, _currentMousePoint);
                foreach (var shape in _selectedShapes)
                {
                    DrawSelectedSytle(shape.Instance);
                }

                if (Stage.Children.Contains(_selectedArea))
                {
                    Stage.Children.Remove(_selectedArea);
                }
            }
        }

        private void Stage_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && _selectedArea != null)
            {
                var current = e.GetPosition(Stage);

                _selectedArea.Width = Math.Abs(current.X - _initMousePoint.X);
                _selectedArea.Height = Math.Abs(current.Y - _initMousePoint.Y);

                if (current.X <= _initMousePoint.X)
                {
                    _selectedArea.SetValue(Canvas.LeftProperty, _initMousePoint.X - _selectedArea.Width);
                }

                if (current.Y <= _initMousePoint.Y)
                {
                    _selectedArea.SetValue(Canvas.TopProperty, _initMousePoint.Y - _selectedArea.Height);
                }
            }
        }
        //组合

        private void Composite_Click(object sender, RoutedEventArgs e)
        {
            CompositeShape compShape = new CompositeShape();
            foreach (var shape in _selectedShapes)
            {
                compShape.Add(shape);
            }
            compShape.Draw();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var saveDialog = new SaveWindow(_diagram);
            saveDialog.ShowDialog();
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            var openDialog = new OpenWindow();
            if (openDialog.ShowDialog() == true)
            {
                ClearStage();
                var diagram = openDialog.SelectedDiagram;

                foreach (var shape in diagram.Shapes)
                {
                    DrawOnStage(shape);
                }
            }
        }

        private void ClearStage()
        {
            _diagram.Clear();
            Stage.Children.Clear();
        }

        private void Opacity_Click(object sender, RoutedEventArgs e)
        {
            if(_selectedShape!= null)
            {
                Drawing.Models.Decorator shapeDecorator = new OpacityDecorator(_selectedShape);
                shapeDecorator.Draw();
            }
            if (_selectedShapes?.Count > 0)
            {
                foreach (var shape in _selectedShapes)
                {
                    Drawing.Models.Decorator shapeDecorator = new OpacityDecorator(shape);
                    shapeDecorator.Draw();
                }
            }
        }
    }
}
