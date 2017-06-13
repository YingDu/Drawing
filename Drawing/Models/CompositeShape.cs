using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace Drawing.Models
{
    class CompositeShape : IDrawable, IIteratable<ShapeBase>
    {
        List<ShapeBase> compositeShape = new List<ShapeBase>();

        public int Length => compositeShape == null ? 0 : compositeShape.Count;

        public void Draw()
        {
            foreach (ShapeBase shape in compositeShape)
            {
                shape.Draw();
                shape.Instance.Fill = Brushes.Blue;
            }
        }

        public void Add(ShapeBase shape)
        {
            compositeShape.Add(shape);
        }

        public void Remove(ShapeBase shape)
        {
            compositeShape.Remove(shape);
        }

        public IIterator<ShapeBase> GetIterator()
        {
            return new CompositeShapeIterator(this);
        }

        public ShapeBase GetElement(int index)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index out of range.");
            }
            return compositeShape[index];
        }
    }

    class CompositeShapeIterator : IIterator<ShapeBase>
    {
        private int _index;
        private CompositeShape _compositeShape;

        public CompositeShapeIterator(CompositeShape compositeShape)
        {
            _compositeShape = compositeShape;
            _index = 0;
        }

        public ShapeBase GetCurrent()
        {
            return _compositeShape.GetElement(_index);
        }

        public bool HasNext()
        {
            return _index < _compositeShape.Length;
        }

        public void MoveNext()
        {
            if (_index < _compositeShape.Length)
            {
                _index++;
            }
        }

        public void Rest()
        {
            _index = 0;
        }
    }
}
