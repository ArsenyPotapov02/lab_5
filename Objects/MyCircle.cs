using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_5.Objects
{
    public class MyCircle : BaseObject
    {
        public int count = 50;
        public Action<MyCircle> DeleteObj;
        public MyCircle(float x, float y, float angle) : base(x, y, angle)
        {
        }

        public override void Render(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.Yellow), -30, -30, 15, 15);
            g.DrawEllipse(new Pen(Color.Red, 2), -30, -30, 15, 15);
            if (count > 0)
            {
                g.DrawString($"{count}", new Font("Verdana", 6), new SolidBrush(Color.Green), -15, -15);
                count--;
            }
            else
            {
                DeleteObj?.Invoke(this);
            }

        }
        public override GraphicsPath GetGraphicsPath()
        {
            var path = base.GetGraphicsPath();
            path.AddEllipse(-30, -30, 15, 15);
            return path;
        }

    }
}
