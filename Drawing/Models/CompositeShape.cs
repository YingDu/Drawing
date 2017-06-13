using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace Drawing.Models
{
    class CompositeShape : IDrawable
    {
        List<ShapeBase> compositeshape = new List<ShapeBase>();
        public GeometryGroup GeometryGroup { get; private set; } = new GeometryGroup();

        public void Draw()
        {
            foreach (ShapeBase shape in compositeshape)
            {
                shape.Draw();
                shape.Instance.Fill = Brushes.Blue;
            }
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
