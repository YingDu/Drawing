using Newtonsoft.Json;
using System;
using System.Windows.Controls;
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

        public override double GetPerimeter()
        {
            return 2 * Math.PI * Radius;
        }

        [JsonConstructor]
        public Circle(double radius)
        {
            Radius = radius;
        }

        public Circle(Circle target)
        {
            Radius = target.Radius;
            Left = target.Left;
            Top = target.Top;

            Instance = new Ellipse()
            {
                Width = target.Instance.Width,
                Height = target.Instance.Height,
                Stroke = target.Instance.Stroke,
                Fill = target.Instance.Fill
            };
            Instance.SetValue(Canvas.TopProperty, target.Instance.GetValue(Canvas.TopProperty));
            Instance.SetValue(Canvas.LeftProperty, target.Instance.GetValue(Canvas.LeftProperty));
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

        public override ShapeBase Clone()
        {
            return new Circle(this);
        }
    }
}
