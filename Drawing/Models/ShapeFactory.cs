﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawing.Models
{
    class ShapeFactory
    {
        static private ShapeFactory _instance = null;
        
        //受保护的构造方法，避免Client直接创建对象
        protected ShapeFactory()
        {

        }

        public virtual ShapeBase MakeCircle(double radius)
        {
            return new Circle(radius);
        }

        public virtual ShapeBase MakeRectangle(double with, double height)
        {
            return new Rectangle(with, height);
        }

        public static ShapeFactory Instance(string FactoryTypeStr)
        {
            if(_instance == null)
            {
                _instance = new ShapeFactory();
            }
            return _instance;
        }
    }
}