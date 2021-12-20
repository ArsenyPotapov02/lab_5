using lab_5.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace lab_5
{
    public partial class Form1 : Form
    {
        MyCircle myRect;
        List<BaseObject> objects = new();
        Player player;
        Marker marker;
        MyCircle myRect_1;
        Random rX = new Random();
       
        public Form1()
        {
            

            InitializeComponent();
            int score = 0;
           
            player = new Player(pbMain.Width / 2, pbMain.Height / 2, 0);
            tb1.Text = $"{score}";
            player.OnOverlap += (p, obj) =>
            {
                txtLog.Text = $"[{DateTime.Now:HH:mm:ss:ff}] Игрок пересекся с {obj}\n" + txtLog.Text;
            };

            // добавил реакцию на пересечение с маркером
            player.OnMarkerOverlap += (m) =>
            {
                objects.Remove(m);
                marker = null;
            };
            marker = new Marker(pbMain.Width / 2 + 50, pbMain.Height / 2 + 50, 0);

            player.OnMyRectangleOverlap += (r) =>
            {
                score = score + 1;
                tb1.Text = $"{score}";
                r.X = pbMain.Width / 2 + rX.Next(-170, 200);
                r.Y = pbMain.Height / 2 + rX.Next(-150, 150);
                r.count = 50;
            };
            if (myRect == null)
            {
                myRect = new MyCircle(pbMain.Width / 2 + rX.Next(-170, 200), pbMain.Height / 2 + rX.Next(-150, 150), 0);
                myRect.DeleteObj += painterBlocks;


            }
            if (myRect_1 == null)
            {
                myRect_1 = new MyCircle(pbMain.Width / 2 + rX.Next(-170, 200), pbMain.Height / 2 + rX.Next(-150, 150), 0);
                myRect_1.DeleteObj += painterBlocks;


            }


            objects.Add(marker);
            objects.Add(player);
            objects.Add(myRect_1);
            objects.Add(myRect);
          
        }

        public void painterBlocks(MyCircle r)
        {
            r.X = pbMain.Width / 2 + rX.Next(-170, 200);
            r.Y = pbMain.Height / 2 + rX.Next(-150, 150);
            r.count = 50;
        }

        private void pbMain_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            g.Clear(Color.White);
            updatePlayer();

            // пересчитываем пересечения
            foreach (var obj in objects.ToList())
            {
                if (obj != player && player.Overlaps(obj, g))
                {
                    player.Overlap(obj);
                    obj.Overlap(player);
                }
            }

            // рендерим объекты
            foreach (var obj in objects)
            {
                g.Transform = obj.GetTransform();
                obj.Render(g);
            }

        }

        public void updatePlayer()
        {
            if (marker != null)
            {
                float dx = marker.X - player.X;
                float dy = marker.Y - player.Y;

                float length = MathF.Sqrt(dx * dx + dy * dy);
                dx /= length;
                dy /= length;

                
                player.vX += dx * 0.5f;
                player.vY += dy * 0.5f;

                // расчитываем угол поворота игрока 
                player.Angle = 90 - MathF.Atan2(player.vX, player.vY) * 180 / MathF.PI;
            }

           
            player.vX += -player.vX * 0.1f;
            player.vY += -player.vY * 0.1f;

            // пересчет позиция игрока с помощью вектора скорости
            player.X += player.vX;
            player.Y += player.vY;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            pbMain.Invalidate();
        }

        private void pbMain_MouseClick(object sender, MouseEventArgs e)
        {
            
            if (marker == null)
            {
                marker = new Marker(0, 0, 0);
                objects.Add(marker); // и главное не забыть пололжить в objects
            }

           
            marker.X = e.X;
            marker.Y = e.Y;
        }

       
      
    }
}
