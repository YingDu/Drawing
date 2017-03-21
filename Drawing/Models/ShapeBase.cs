using System.Windows.Media;
using System.Windows.Shapes;

namespace Drawing.Models
{
    public abstract class ShapeBase
    {
        public double Top { get; set; }

        public double Left { get; set; }

        public abstract double Area { get; } //只读

        public abstract double Perimeter { get; }

        public Shape Instance { get; protected set; } //内部可设置

        protected Brush _stroke;

         
    }
}
