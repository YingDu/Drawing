using System;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Drawing.Models
{
    class Circle : ShapeBase, IShape
    {
        public double Radius { get; private set; }

        public override double Area
        {
            get
            {
                return Math.PI * Math.Pow(Radius, 2);
            }
        }

        public override double Perimeter
        {
            get
            {
                return 2 * Math.PI * Radius;
            }
        }

        public Circle(double radius)
        {
            Radius = radius;
        }

        public void Draw()
        {
            _stroke = Brushes.Red;
            if (Instance == null)
            {
                Instance = new Ellipse();
            }
            Instance.Width = 2 * Radius;
            Instance.Height = 2 * Radius;
            Instance.Stroke = _stroke;
        }

        public void Move(double offsetX, double offsetY)
        {
            throw new NotImplementedException();
        }

        public void ZoomIn(double multiple)
        {
            if(multiple < 1)
            {
                throw new ArgumentOutOfRangeException("放大倍数必须大于1。");
            }
            Radius = Radius * multiple;
            Draw();
        }

        public void ZoomOut(double multiple)
        {
            if (multiple > 1 || multiple <= 0)
            {
                throw new ArgumentOutOfRangeException("缩小倍数必须介于0和1之间。");
            }
            Radius = Radius * multiple;
            Draw();
        }
    }
}
