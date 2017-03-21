using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Drawing.Models
{
    class Rectangle : ShapeBase, IShape
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

        public void Draw()
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


        public void ZoomIn(double multiple)
        {
            if (multiple < 1)
            {
                throw new ArgumentOutOfRangeException("放大倍数必须大于1。");
            }
            Width = Width * multiple;
            Height = Height * multiple;
            Draw();
        }

        public void ZoomOut(double multiple)
        {
            if(multiple > 1 || multiple <= 0)
            {
                throw new ArgumentOutOfRangeException("缩小倍数必须小于1。");
            }
            Width = Width * multiple;
            Height = Height * multiple;
            Draw();
        }
    }
}
