using System;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Drawing.Models
{
    class Circle : ShapeBase
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

        public override void Draw()
        {
            _stroke = Brushes.Red;
            if (Instance == null)
            {
                Instance = new Ellipse();
            }

            Instance.Width = 2 * Radius;
            Instance.Height = 2 * Radius;
            Instance.Stroke = _stroke;
            Instance.Fill = Brushes.Tomato;
        }

        protected override void Zoom(double multiple)
        {
            if (multiple <= 0)
            {
                throw new ArgumentOutOfRangeException("缩放倍数必须大于0。");
            }

            Radius = Radius * multiple;
            Draw();
        }
    }
}
