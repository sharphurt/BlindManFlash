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
        public float Angle;
        public Point Position = new Point(0, 0);
        public float ScaleFactor = 0.7f;
        
        public Form1()
        {
            DoubleBuffered = true;
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;

            g.FillRectangle(new SolidBrush(Color.Red), 0, 0, Width, Height);

            var path = new GraphicsPath();
            path.AddArc(0, 0, 100, 25, 180, 180);
            path.AddLines(new[] {new Point(100, 25), new Point(60, 280), new Point(40, 280), new Point(0, 25)});
            
            var ellipsePath = new GraphicsPath();
            ellipsePath.AddEllipse(new Rectangle(new Point(50 - 100, 280 - 80), new Size(200, 200)));
            
            var unitedRegion = new Region(new Rectangle(0, 0, Width, Height));
            
            var translateMatrix = new Matrix();
            translateMatrix.Translate(Position.X - 50, Position.Y - 300);
            
            var rotationMatrix = new Matrix();
            rotationMatrix.RotateAt(Angle, new PointF(Position.X, Position.Y));
            
            var scaleMatrix = new Matrix();
            scaleMatrix.Scale(ScaleFactor, ScaleFactor);
            
            ApplyTransformMatrices(path, translateMatrix, rotationMatrix, scaleMatrix);
            ApplyTransformMatrices(ellipsePath, translateMatrix, scaleMatrix);
            
            unitedRegion.Exclude(path);
            unitedRegion.Exclude(ellipsePath);
            
            e.Graphics.FillRegion(Brushes.Black, unitedRegion);
        }

        private void ApplyTransformMatrices(GraphicsPath target, params Matrix[] matrices) =>
            matrices.ToList().ForEach(target.Transform);

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
                Position += new Size(0, -10);
            if (e.KeyCode == Keys.S)
                Position += new Size(0, 10);
            if (e.KeyCode == Keys.D)
                Position += new Size(10, 0);
            if (e.KeyCode == Keys.A)
                Position += new Size(-10, 0);
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            float x = e.X - Position.X;
            float y = e.Y - Position.Y;

            Angle = (float) (Math.Atan2(y, x) * 180 / Math.PI) + 90;
            Invalidate();
        }
    }
}