using Newtonsoft.Json;
using System;
using System.Windows.Media;

namespace Drawing.Models
{
    class Line : ShapeBase
    {
     
        public double X1 { get; private set; }
        public double Y1 { get; private set; }
        public double X2 { get; private set; }
        public double Y2 { get; private set; }

        [JsonConstructor]
        public Line(double x1,double y1,double x2,double y2)
        {
            X1 = x1;
            X2 = x2;
            Y1 = y1;
            Y2 = y2;
        }
        public override void Draw()
        {
            
            _stroke = Brushes.Red;
            if (Instance == null)
            {
                Instance = new System.Windows.Shapes.Line();
              
            }
            System.Windows.Shapes.Line myline = (System.Windows.Shapes.Line)Instance;
            myline.X1 = X1;
            myline.X2 = X2;
            myline.Y1 = Y1;
            myline.Y2 = Y2;
            myline.Stroke = _stroke;
            myline.Fill = Brushes.Tomato;
            myline.StrokeThickness = 2;
            


        }
        public override double Area { get { return 0; } }

        public override double GetPerimeter()
        {
            return 0;
        }

        public override ShapeBase Clone()
        {
            Line cloned = this.MemberwiseClone() as Line;
            cloned.Instance = new System.Windows.Shapes.Line();
            System.Windows.Shapes.Line myline = (System.Windows.Shapes.Line)Instance;
            myline.X1 = X1;
            myline.X2 = X2;
            myline.Y1 = Y1;
            myline.Y2 = Y2;
            myline.Stroke = _stroke;
            myline.Fill = Brushes.Tomato;
            myline.StrokeThickness = 2;
            return cloned as Line;
        }


        protected override void Zoom(double multiple)
        {
            if (multiple <= 0)
            {
                throw new ArgumentOutOfRangeException("缩放倍数必须大于0。");
            }

            
            X2 = X2 * multiple;
            Y2 = Y2 * multiple;
            Draw();
        }
    }
}
