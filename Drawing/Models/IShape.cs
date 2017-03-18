using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawing.Models
{
    interface IShape
    {
        /// <summary>
        /// 绘制图形
        /// </summary>
        void Draw();

        /// <summary>
        /// 移动图形
        /// </summary>
        /// <param name="offsetX">横坐标偏移量</param>
        /// <param name="offsetY">纵坐标偏移量</param>
        void Move(double offsetX, double offsetY);

        /// <summary>
        /// 将图形放大到指定倍数
        /// </summary>
        /// <param name="multiple">放大倍数</param>
        void ZoomIn(double multiple);

        /// <summary>
        /// 将图形缩小到指定倍数
        /// </summary>
        /// <param name="multiple">缩小倍数</param>
        void ZoomOut(double multiple);
    }
}
