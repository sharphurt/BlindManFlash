using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            DoubleBuffered = true;
            
            InitializeComponent();
        }

        public float Angle;
        public Point Position;

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;

            g.FillRectangle(new SolidBrush(Color.Red), 0, 0, Width, Height);

            var myPath1 = new GraphicsPath();
            var path = new GraphicsPath(
                new[] {new Point(0, 25), new Point(100, 25), new Point(60, 280), new Point(40, 280)},
                new byte[] {1, 1, 1, 1});
            myPath1.AddPath(path, false);

            var myPath2 = new GraphicsPath();
            myPath2.AddClosedCurve(new[] {new Point(5, 30), new Point(50, 0), new Point(95, 30)}, 1);

            var unitedRegion = new Region(new Rectangle(0, 0, Width * 3, Height * 3));

            var translateMatrix = new Matrix();
            translateMatrix.Translate(Width, Height);

            myPath1.Transform(translateMatrix);
            myPath2.Transform(translateMatrix);


            unitedRegion.Exclude(myPath1);
            unitedRegion.Exclude(myPath2);

            unitedRegion.Translate(-Width / 2 - 50 + Position.X, -Height / 2 - 100 + Position.Y);

            var rotationMatrix = new Matrix();
            rotationMatrix.RotateAt(Angle, new PointF(Width / 2f + Position.X, Height + Position.Y));

            unitedRegion.Transform(rotationMatrix);

            e.Graphics.FillRegion(Brushes.Black, unitedRegion);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
                Position += new Size(0, -1);
            if (e.KeyCode == Keys.S)
                Position += new Size(0, 1);
            if (e.KeyCode == Keys.D)
                Position += new Size(1, 0);
            if (e.KeyCode == Keys.A)
                Position += new Size(-1, 0);
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            float x = (e.X - Width / 2) - Position.X;
            float y = (e.Y - Height) - Position.Y;

            var vectorAngle = (float) (Math.Atan(y / x) * 180 / Math.PI);
            
            if (vectorAngle < 0)
                Angle = 90 + vectorAngle;
            else
                Angle = vectorAngle - 90;
            
            Invalidate();
        }
    }
}