using System;
using System.Windows.Media;

namespace Drawing.Models
{
    class Rectangle : ShapeBase
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public override double Area
        {
            get
            {
                return Width * Height;
            }
        }

        public override double Perimeter
        {
            get
            {
                return 2 * (Width + Height);
            }
        }


        public Rectangle(double width, double height)
        {
            Width = width;
            Height = height;
        }

        public override void Draw()
        {
            _stroke = Brushes.Red;
            if (Instance == null)
            {
                Instance = new System.Windows.Shapes.Rectangle();
            }
            Instance.Width = Width;
            Instance.Height = Height;
            Instance.Stroke = _stroke;
            Instance.Fill = Brushes.Tomato;
        }

        protected override void Zoom(double multiple)
        {
            if(multiple <= 0)
            {
                throw new ArgumentOutOfRangeException("缩放倍数必须大于0。");
            }
            Width = Width * multiple;
            Height = Height * multiple;
            Draw();
        }
    }
}
