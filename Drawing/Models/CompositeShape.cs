using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Drawing.Models
{
    class CompositeShape : ShapeBase
    {
        List<ShapeBase> compositeshape = new List<ShapeBase>();
        public override double Area => throw new NotImplementedException();
        public GeometryGroup GeometryGroup { get; private set; } = new GeometryGroup();

        public override ShapeBase Clone()
        {
            throw new NotImplementedException();
        }

        public override void Draw()
        {
            foreach (ShapeBase shape in compositeshape)
            {
                shape.Draw();
                shape.Instance.Fill = Brushes.Blue;
            }
        }

        public override double GetPerimeter()
        {
            throw new NotImplementedException();
        }

        protected override void Zoom(double multiple)
        {
            throw new NotImplementedException();
        }

        public void Add(ShapeBase shape)
        {
            compositeshape.Add(shape);
           
        }

        public void Remove(ShapeBase shape)
        {
            compositeshape.Remove(shape);
        }
    }
}
