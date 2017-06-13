using Newtonsoft.Json;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Drawing.Models
{
    
    public abstract class ShapeBase: IDrawable
    {
        public double Top { get; set; }

        public double Left { get; set; }

        public abstract double Area { get; } //只读

        public abstract double GetPerimeter();

        public IDrawable Parent { get; set; }

        [JsonIgnore]
        public Shape Instance { get; protected set; } //内部可设置

        protected Brush _stroke;

        public abstract void Draw();

        public abstract ShapeBase Clone();

        /// <summary>
        /// 将图形放大到指定倍数
        /// </summary>
        /// <param name="multiple">放大倍数</param>
        public void ZoomIn(double multiple)
        {
            if (multiple < 1)
            {
                throw new ArgumentOutOfRangeException("放大倍数必须大于1。");
            }
            Zoom(multiple);
            Draw();
        }

        /// <summary>
        /// 将图形缩小到指定倍数
        /// </summary>
        /// <param name="multiple">缩小倍数</param>
        public void ZoomOut(double multiple)
        {
            if (multiple > 1 || multiple <= 0)
            {
                throw new ArgumentOutOfRangeException("缩小倍数必须介于0和1之间。");
            }
            Zoom(multiple);
            Draw();
        }

        protected abstract void Zoom(double multiple);
    }
}
