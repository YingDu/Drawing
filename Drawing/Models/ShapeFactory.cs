using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawing.Models
{
    class ShapeFactory
    {
        private static ShapeFactory _instance = null;
        //同步锁
        private static readonly object _synLock = new object();
        //受保护的构造方法，避免Client直接创建对象
        private ShapeFactory(){}

        public virtual ShapeBase CreateCircle(double radius)
        {
            return new Circle(radius);
        }

        public virtual ShapeBase CreateRectangle(double with, double height)
        {
            return new Rectangle(with, height);
        }
        public virtual ShapeBase CreateLine(double x1,double y1,double x2,double y2)
        {
            return new Line(x1, y1, x2, y2);
        }

        public static ShapeFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_synLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new ShapeFactory(); 
                        }
                    }
                }
                return _instance;
            }
        }
    }
}
